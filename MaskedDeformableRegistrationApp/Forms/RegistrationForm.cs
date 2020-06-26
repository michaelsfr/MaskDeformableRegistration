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

        private SegmentationParameters SegmentationParametersWholeParticle { get; set; } = new SegmentationParameters();
        private SegmentationParameters SegmentationParametersInnerStructures { get; set; } = new SegmentationParameters();
        private RegistrationParameters RegistrationParametersRigid { get; set; } = RegistrationParameters.GetRigidRegistrationParameters();
        private RegistrationParameters RegistrationParametersNonRigid { get; set; } = RegistrationParameters.GetNonRigidRegistrationParameters();

        private uint LargestImageWidth { get; set; } = 0;
        private uint LargestImageHeight { get; set; } = 0;

        public RegistrationForm(List<string> filenamesToRegistrate)
        {
            InitializeComponent();

            ToRegistrate = filenamesToRegistrate;
            InitializeBackgroundWorker();
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            radioButtonFirstFromStack.Checked = true;
            radioButtonTranslation.Checked = true;
            radioButtonAdvancedBspline.Checked = true;
            radioButtonNoPenalties.Checked = true;
            buttonCancel.Enabled = false;
            buttonEvaluation.Enabled = false;
            buttonSegmentationInnerstructures.Enabled = false;
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
            foreach (string filename in ToRegistrate)
            {
                using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    using (Image img = Image.FromStream(stream: file,
                                                        useEmbeddedColorManagement: false,
                                                        validateImageData: false))
                    {
                        uint width = (uint)img.PhysicalDimension.Width;
                        uint height = (uint)img.PhysicalDimension.Height;

                        if (LargestImageWidth < width)
                        {
                            LargestImageWidth = width;
                        }

                        if (LargestImageHeight < height)
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
            if (e.Error != null)
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
                buttonEvaluation.Enabled = true;
            }

            // enable disable buttons
            buttonStartRegistration.Enabled = true;
            buttonCancel.Enabled = false;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            if (e.UserState != null)
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
            float percentagePerIteration = 100 / ToRegistrate.Count;
            int percentage = 0;

            using (StringWriter stringWriter = new StringWriter())
            {
                Directory.CreateDirectory(Path.Combine(ApplicationContext.OutputPath, RegistrationParametersRigid.RegistrationType.ToString()));
                Console.SetOut(stringWriter);

                string filenameReference = ToRegistrate.First();
                // resize fixed image
                sitk.Image refImage = ReadWriteUtils.ReadITKImageFromFile(filenameReference);
                sitk.Image refResized = ImageUtils.ResizeImage(refImage, LargestImageWidth, LargestImageHeight);
                ReadWriteUtils.WriteSitkImageAsPng(refResized,
                    Path.Combine(ApplicationContext.OutputPath, RegistrationParametersRigid.RegistrationType.ToString(), Path.GetFileName(filenameReference)));
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

                    // masked registration selection
                    sitk.VectorOfParameterMap transformparams = null;
                    if (checkBoxMaskRegistration.Checked)
                    {
                        transformparams = PerformRigidRegistration(fixedMaskFull, movingMaskFull);
                    }
                    else
                    {
                        if (radioButtonNoMask.Checked)
                            transformparams = PerformRigidRegistration(refResized, movResized);
                        else if (radioButtonOnlyFixedMask.Checked)
                            transformparams = PerformRigidRegistration(refResized, movResized, fixedMask: fixedMaskFull);
                        else if (radioButtonOnlyMovingMask.Checked)
                            transformparams = PerformRigidRegistration(refResized, movResized, movingMask: movingMaskFull);
                        else
                            transformparams = PerformRigidRegistration(refResized, movResized, fixedMask: fixedMaskFull, movingMask: movingMaskFull);
                    }
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
            RigidRegistration regRigid = new RigidRegistration(fixedImage, movingImage, RegistrationParametersRigid);
            regRigid.SetDefaultParameterMap(RegistrationParametersRigid.RegistrationType, RegistrationParametersRigid.NumberOfResolutions);
            regRigid.SetSimilarityMetric(RegistrationParametersRigid.Metric);

            if (fixedMask != null)
            {
                regRigid.SetFixedMask(fixedMask);
            }

            if (movingMask != null)
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

            WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, SegmentationParametersWholeParticle);
            segImage.Execute();
            Bitmap bmp = segImage.GetOutput().Bitmap;
            segImage.Dispose();

            ReadWriteUtils.WriteBitmapAsPng(bmp,
                Path.Combine(ApplicationContext.OutputPath, RegistrationParametersRigid.RegistrationType.ToString(), ("mask_" + Path.GetFileName(filename))));

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
                Path.Combine(ApplicationContext.OutputPath, RegistrationParametersRigid.RegistrationType.ToString()));

            if (isBinary)
            {
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
            sitk.ParameterMap map = RegistrationUtils.GetDefaultParameterMap(RegistrationParametersRigid.RegistrationType);
            using (EditParametersForm paramForm = new EditParametersForm(map))
            {
                DialogResult result = paramForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RegistrationParametersRigid.ParamMapToUse = paramForm.Parametermap;
                }
            }
        }

        private void buttonSegmentationParams_Click(object sender, EventArgs e)
        {
            SegmentationParamsRigid();
        }

        private void SegmentationParamsRigid()
        {
            Cursor.Current = Cursors.WaitCursor;
            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile(ToRegistrate.FirstOrDefault());

            using (SegParamsRigidForm form = new SegParamsRigidForm(image, SegmentationParametersInnerStructures))
            {
                Cursor.Current = Cursors.Default;
                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    SegmentationParametersWholeParticle = form.segmentationParameters;
                }
            }

        }

        private void SegmentationParamsNonRigid()
        {
            Cursor.Current = Cursors.WaitCursor;

            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile(ToRegistrate.FirstOrDefault());

            WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, SegmentationParametersWholeParticle);
            segImage.Execute();
            Image<Gray, byte> mask = segImage.GetOutput().Clone();
            segImage.Dispose();

            using (SegParamsNonRigidForm form = new SegParamsNonRigidForm(image, mask, SegmentationParametersInnerStructures))
            {
                Cursor.Current = Cursors.Default;
                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    SegmentationParametersInnerStructures = form.segmentationParameters;
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

        private void buttonEvaluation_Click(object sender, EventArgs e)
        {
            // TODO
            // 1. Show metrics
            // 2. Show TRE (reg error)
            // 3. Show checkerboard
            // 4. Show subtraction image
        }

        private void checkBoxMaskRegistration_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMaskRegistration.Checked)
            {
                //RegistrationParametersRigid.
            }
        }

        private void radioButtonTransformRigidity_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTransformRigidity.Checked)
            {
                buttonSegmentationInnerstructures.Enabled = true;
            }
        }

        private void radioButtonTranslation_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRigidRegistrationType();
        }

        private void buttonSegmentationInnerstructures_Click(object sender, EventArgs e)
        {
            SegmentationParamsNonRigid();
        }

        private void radioButtonSimilarity_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRigidRegistrationType();
        }

        private void radioButtonRigid_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRigidRegistrationType();
        }

        private void radioButtonAffine_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRigidRegistrationType();
        }

        private void UpdateRigidRegistrationType()
        {
            if (radioButtonTranslation.Checked)
            {
                RegistrationParametersRigid.RegistrationType = RegistrationDefaultParameters.translation;
            }
            else if (radioButtonSimilarity.Checked)
            {
                RegistrationParametersRigid.RegistrationType = RegistrationDefaultParameters.similarity;
            }
            else if (radioButtonRigid.Checked)
            {
                RegistrationParametersRigid.RegistrationType = RegistrationDefaultParameters.rigid;
            }
            else if (radioButtonAffine.Checked)
            {
                RegistrationParametersRigid.RegistrationType = RegistrationDefaultParameters.affine;
            }
        }

        private void RegistrationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            Application.Exit();
        }
    }
}
