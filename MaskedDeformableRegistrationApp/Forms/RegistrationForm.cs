using Emgu.CV;
using Emgu.CV.Structure;
using MaskedDeformableRegistrationApp.Registration;
using MaskedDeformableRegistrationApp.Segmentation;
using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Forms
{
    public partial class RegistrationForm : Form
    {
        private List<string> ToRegistrate { get; set; }

        private SegmentationParameters SegmentationParameters { get; set; } = new SegmentationParameters();
        private RegistrationParameters RegistrationParameters { get; set; } = new RegistrationParameters();

        private uint LargestImageWidth { get; set; } = 0;
        private uint LargestImageHeight { get; set; } = 0;

        public RegistrationForm(List<string> filenamesToRegistrate)
        {
            InitializeComponent();

            ToRegistrate = filenamesToRegistrate;
            InitializeBackgroundWorker();
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {

            foreach(string filename in ToRegistrate)
            {
                using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    using (Image img = Image.FromStream(stream: file,
                                                        useEmbeddedColorManagement: false,
                                                        validateImageData: false))
                    {
                        uint width = (uint)img.PhysicalDimension.Width;
                        uint height = (uint)img.PhysicalDimension.Height;

                        if(LargestImageWidth < width)
                        {
                            LargestImageWidth = width;
                        }

                        if(LargestImageHeight < height)
                        {
                            LargestImageHeight = height;
                        }

                        img.Dispose();
                    }
                }
            }
        }

        private void buttonStartRegistration_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                buttonStartRegistration.Enabled = false;
                buttonCancel.Enabled = true;
                backgroundWorker1.RunWorkerAsync();
            }
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Registration cancelled");
            }
            else
            {
                MessageBox.Show("Registration done. See results and elastix log in result directory.");
            }

            // enable disable buttons
            buttonStartRegistration.Enabled = true;
            buttonCancel.Enabled = false;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            if(e.UserState != null)
            {
                this.textBox1.AppendText(e.UserState as string);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string loaded = "Loaded and resized image {0}.\n";
            string segmented = "Created mask for particle and start registration. \n";
            string registration = "Registration done. Time consumed: {0}. For output log see {1}.\n";
            BackgroundWorker worker = sender as BackgroundWorker;
            float percentagePerIteration = 100/ToRegistrate.Count;
            int percentage = 0;

            using (StringWriter stringWriter = new StringWriter())
            {
                Directory.CreateDirectory(Path.Combine(ApplicationContext.OutputPath, RegistrationParameters.RegistrationType.ToString()));
                Console.SetOut(stringWriter);

                string filenameReference = ToRegistrate.First();
                // resize fixed image
                sitk.Image refImage = ReadWriteUtils.ReadITKImageFromFile(filenameReference);
                sitk.Image resized = ImageUtils.ResizeImage(refImage, LargestImageWidth, LargestImageHeight);
                ReadWriteUtils.WriteSitkImageAsPng(resized,
                    Path.Combine(ApplicationContext.OutputPath, RegistrationParameters.RegistrationType.ToString(), Path.GetFileName(filenameReference)));
                worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.40), string.Format("Loaded fixed image {0}.\n", filenameReference));

                sitk.Image fixedMaskFull = GetWholeParticleMask(filenameReference);
                worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.50), "Created mask for fixed image.\n");

                foreach (string imageFilename in ToRegistrate)
                {
                    if (imageFilename == filenameReference) continue;

                    // resize moving image
                    sitk.Image movImage = ReadWriteUtils.ReadITKImageFromFile(imageFilename);
                    sitk.Image movResized = ImageUtils.ResizeImage(movImage, LargestImageWidth, LargestImageHeight);
                    ReadWriteUtils.WriteSitkImageAsPng(movResized, imageFilename);
                    worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.1), string.Format(loaded, imageFilename));

                    // mask particle
                    sitk.Image movingMaskFull = GetWholeParticleMask(imageFilename);
                    worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.1), string.Format(segmented, imageFilename));

                    // registration
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    sitk.VectorOfParameterMap transformparams = PerformRigidRegistration(fixedMaskFull, movingMaskFull);
                    WriteTransform(imageFilename, transformparams, isBinary: true);
                    stopWatch.Stop();
                    string elapsed = string.Format("[{0}m {1}s]", stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds);
                    worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.8), string.Format(registration, elapsed, "output.log"));
                    movingMaskFull.Dispose();
                }

                worker.ReportProgress(100, "Registration of all images done.\n");
            }
        }

        private sitk.VectorOfParameterMap PerformRigidRegistration(sitk.Image fixedImage, sitk.Image movingImage,
            sitk.Image fixedMask = null, sitk.Image movingMask = null)
        {
            RigidRegistration regRigid = new RigidRegistration(fixedImage, movingImage, 
                Path.Combine(ApplicationContext.OutputPath, RegistrationParameters.RegistrationType.ToString()));
            regRigid.SetDefaultParameterMap(RegistrationParameters.RegistrationType, RegistrationParameters.NumberOfResolutions);
            regRigid.SetSimilarityMetric(RegistrationParameters.Metric);

            if(fixedMask != null)
            {
                regRigid.SetFixedMask(fixedMask);
            }

            if(movingMask != null)
            {
                regRigid.SetMovingMask(movingMask);
            }

            regRigid.Execute();
            sitk.VectorOfParameterMap transformparams = regRigid.GetTransformationFile();
            regRigid.Dispose();

            return transformparams;
        }

        private sitk.Image GetWholeParticleMask(string filename)
        {
            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile(filename);

            WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, ImageUtils.GetPercentualImagePixelCount(image, 0.3f));
            segImage.Execute();
            Bitmap bmp = segImage.GetOutput().Bitmap;
            segImage.Dispose();

            ReadWriteUtils.WriteBitmapAsPng(bmp, 
                Path.Combine(ApplicationContext.OutputPath, RegistrationParameters.RegistrationType.ToString(), ("mask_" +Path.GetFileName(filename))));

            sitk.Image mask = ImageUtils.GetITKImageFromBitmap(bmp);
            image.Dispose();

            return mask;
        }

        private void GetInnerStructureSegmentations(string filename)
        {
            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile(filename);
            
            //todo

            image.Dispose();
        }

        private void WriteTransform(string filename, sitk.VectorOfParameterMap transformParams, bool isBinary = false)
        {
            sitk.Image movingImageToTransform = ReadWriteUtils.ReadITKImageFromFile(filename, sitk.PixelIDValueEnum.sitkVectorUInt8);
            TransformRGB trans = new TransformRGB(movingImageToTransform, transformParams, 
                Path.Combine(ApplicationContext.OutputPath, RegistrationParameters.RegistrationType.ToString()));

            if(isBinary) {
                // for binary reg set interpolation order to zero
                trans.SetInterpolationOrder(0);
            }

            trans.Execute();
            string resultFilename = Path.GetFileNameWithoutExtension(filename) + ".png";
            trans.WriteTransformedImage(resultFilename);
            trans.Dispose();
            movingImageToTransform.Dispose();
        }

        private void buttonEditParameters_Click(object sender, EventArgs e)
        {
            sitk.ParameterMap map = RegistrationUtils.GetDefaultParameterMap(RegistrationParameters.RegistrationType);
            using (EditParametersForm paramForm = new EditParametersForm(map))
            {
                DialogResult result = paramForm.ShowDialog();

                if(result == DialogResult.OK)
                {
                    RegistrationParameters.ParamMapToUse = paramForm.Parametermap;
                }
            }
        }

        private void buttonSegmentationParams_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile(ToRegistrate.FirstOrDefault());

            WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, ImageUtils.GetPercentualImagePixelCount(image, 0.3f));
            segImage.Execute();
            Image<Gray, byte> mask = segImage.GetOutput().Clone();
            segImage.Dispose();

            using (SegmentationParameterForm form = new SegmentationParameterForm(image, mask, SegmentationParameters)) 
            {
                Cursor.Current = Cursors.Default;
                DialogResult result = form.ShowDialog();

                if(result == DialogResult.OK)
                {
                    SegmentationParameters = form.segmentationParameters;
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // clean up?
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                backgroundWorker1.CancelAsync();
            }
        }
    }
}
