﻿using Emgu.CV;
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

        private sitk.ParameterMap EditedMapRigid { get; set; } = null;
        private sitk.ParameterMap EditedMapNonRigid { get; set; } = null;

        private uint LargestImageWidth { get; set; } = 0;
        private uint LargestImageHeight { get; set; } = 0;

        // msgs
        private string loaded = "Loaded and resized image {0}.\n";
        private string segmented = "Created mask for particle and start registration. \n";
        private string registration = "Registration done. Time consumed: {0}. For output log see {1}.\n";

        public RegistrationForm(List<string> filenamesToRegistrate)
        {
            InitializeComponent();
            InitializeButtons();
            ToRegistrate = filenamesToRegistrate;

            InitializeBackgroundWorker();
        }

        private void InitializeButtons()
        {
            radioButtonFirstFromStack.Checked = true;
            radioButtonNoMask.Checked = true;
            radioButtonTranslation.Checked = true;
            radioButtonAdvancedBspline.Checked = true;
            radioButtonNoPenalties.Checked = true;
            buttonCancel.Enabled = false;
            buttonEvaluation.Enabled = false;
            buttonSegmentationInnerstructures.Enabled = false;
            buttonCancelNonRigidReg.Enabled = false;
            buttonEvaluateNonRigidReg.Enabled = false;
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorkerRigid.WorkerReportsProgress = true;
            backgroundWorkerRigid.WorkerSupportsCancellation = true;
            backgroundWorkerRigid.DoWork += new DoWorkEventHandler(backgroundWorkerRigid_DoWork);
            backgroundWorkerRigid.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerRigid_ProgressChanged);
            backgroundWorkerRigid.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerRigid_RunWorkerCompleted);

            backgroundWorkerNonRigid.WorkerReportsProgress = true;
            backgroundWorkerNonRigid.WorkerSupportsCancellation = true;
            backgroundWorkerNonRigid.DoWork += new DoWorkEventHandler(backgroundWorkerNonRigid_DoWork);
            backgroundWorkerNonRigid.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerNonRigid_ProgressChanged);
            backgroundWorkerNonRigid.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerNonRigid_RunWorkerCompleted);
        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            InitializeRigidParameterMap();
            InitializeNonRigidParameterMap();

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
            Cursor.Current = Cursors.WaitCursor;
        }

        private void buttonStartRegistration_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerRigid.IsBusy != true)
            {
                textBoxConsoleRigid.Clear();
                progressBarRigid.Value = 0;
                buttonStartRegistration.Enabled = false;
                buttonCancel.Enabled = true;
                backgroundWorkerRigid.RunWorkerAsync();
            }

        }

        private void backgroundWorkerRigid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

        private void backgroundWorkerRigid_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBarRigid.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                AppendLine(textBoxConsoleRigid, e.UserState as string);
            }
        }

        public void AppendLine(TextBox source, string value)
        {
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }

        private void backgroundWorkerRigid_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            float percentagePerIteration = 100 / ToRegistrate.Count;
            int percentage = 0;

            using (StringWriter stringWriter = new StringWriter())
            {
                Directory.CreateDirectory(Path.Combine(ApplicationContext.OutputPath, RegistrationParametersRigid.SubDirectory));
                Console.SetOut(stringWriter);

                string filenameReference = ToRegistrate.First();
                RegistrationParametersRigid.FixedImageFilename = filenameReference;
                // resize fixed image
                sitk.Image refImage = ReadWriteUtils.ReadITKImageFromFile(filenameReference);
                sitk.Image refResized = ImageUtils.ResizeImage(refImage, LargestImageWidth, LargestImageHeight);
                ReadWriteUtils.WriteSitkImage(refResized,
                    Path.Combine(ApplicationContext.OutputPath, RegistrationParametersRigid.SubDirectory, Path.GetFileName(filenameReference)));
                worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.40), string.Format("Loaded fixed image {0}.\n", filenameReference));

                sitk.Image fixedMaskFull = GetWholeParticleMask(filenameReference, RegistrationParametersRigid);
                worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.50), "Created mask for fixed image.\n");

                int i = 0;
                foreach (string imageFilename in ToRegistrate)
                {
                    i++;
                    if (imageFilename == filenameReference) continue;

                    // resize moving image
                    sitk.Image movImage = ReadWriteUtils.ReadITKImageFromFile(imageFilename);
                    sitk.Image movResized = ImageUtils.ResizeImage(movImage, LargestImageWidth, LargestImageHeight);
                    ReadWriteUtils.WriteSitkImage(movResized, imageFilename);
                    worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.1), string.Format(loaded, imageFilename));

                    // mask particle
                    sitk.Image movingMaskFull = GetWholeParticleMask(imageFilename, RegistrationParametersRigid);
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
                    WriteTransform(imageFilename, transformparams, RegistrationParametersRigid, i, isBinary: true);

                    stopWatch.Stop();
                    string elapsed = string.Format("[{0}m {1}s]", stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds);
                    worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.8), string.Format(registration, elapsed, "output.log"));
                    movingMaskFull.Dispose();
                }

                worker.ReportProgress(100, "Registration of all images done.\n");
            }
        }

        private void backgroundWorkerNonRigid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                buttonEvaluateNonRigidReg.Enabled = true;
            }

            // enable disable buttons
            buttonStartNonRigidRegistration.Enabled = true;
            buttonCancelNonRigidReg.Enabled = false;
        }

        private void backgroundWorkerNonRigid_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBarNonRigid.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                AppendLine(this.textBoxConsoleNonRigid, e.UserState as string);
            }
        }

        private void backgroundWorkerNonRigid_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            float percentagePerIteration = 100 / ToRegistrate.Count;
            int percentage = 0;

            using (StringWriter stringWriter = new StringWriter())
            {
                Directory.CreateDirectory(Path.Combine(ApplicationContext.OutputPath, RegistrationParametersNonRigid.SubDirectory)); // change reg type
                Console.SetOut(stringWriter);

                string filenameReference = ToRegistrate.First();
                RegistrationParametersNonRigid.FixedImageFilename = filenameReference;
                // resize fixed image
                sitk.Image refImage = ReadWriteUtils.ReadITKImageFromFile(filenameReference);
                sitk.Image refResized = ImageUtils.ResizeImage(refImage, LargestImageWidth, LargestImageHeight);
                ReadWriteUtils.WriteSitkImage(refResized,
                    Path.Combine(ApplicationContext.OutputPath, RegistrationParametersNonRigid.SubDirectory, Path.GetFileName(filenameReference)));
                worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.40), string.Format("Loaded fixed image {0}.\n", filenameReference));

                worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.50), "Created mask for fixed image.\n");

                int i = 0;
                foreach (string imageFilename in ToRegistrate)
                {
                    i++;
                    if (imageFilename == filenameReference) continue;

                    // resize moving image
                    sitk.Image movImage = ReadWriteUtils.ReadITKImageFromFile(imageFilename);
                    sitk.Image movResized = ImageUtils.ResizeImage(movImage, LargestImageWidth, LargestImageHeight);
                    ReadWriteUtils.WriteSitkImage(movResized, imageFilename);
                    worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.1), string.Format(loaded, imageFilename));

                    // mask particle
                    worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.1), string.Format(segmented, imageFilename));

                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    // masked registration selection
                    sitk.VectorOfParameterMap transformparams = PerformNonRigidRegistration(refResized, movResized, imageFilename, iteration: i);
                    WriteTransform(imageFilename, transformparams, RegistrationParametersNonRigid, i, isBinary: true);

                    stopWatch.Stop();
                    string elapsed = string.Format("[{0}m {1}s]", stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds);
                    worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.8), string.Format(registration, elapsed, "output.log"));
                }

                worker.ReportProgress(100, "Registration of all images done.\n");
            }
        }

        private sitk.VectorOfParameterMap RunRegistration(Func<sitk.Image, sitk.Image, sitk.Image, sitk.Image, sitk.VectorOfParameterMap> registrationFunc, 
            sitk.Image fixedImage, sitk.Image movingImage, sitk.Image fixedMask = null, sitk.Image movingMask = null)
        {
            sitk.VectorOfParameterMap transformparams = null;
            if (checkBoxMaskRegistration.Checked)
            {
                transformparams = registrationFunc(fixedMask, movingMask, null, null);
            }
            else
            {
                if (radioButtonNoMask.Checked)
                    transformparams = registrationFunc(fixedImage, movingImage, null, null);
                else if (radioButtonOnlyFixedMask.Checked)
                    transformparams = registrationFunc(fixedImage, movingImage, fixedMask, null);
                else if (radioButtonOnlyMovingMask.Checked)
                    transformparams = registrationFunc(fixedImage, movingImage, null, movingMask);
                else
                    transformparams = registrationFunc(fixedImage, movingImage, fixedMask, movingMask);
            }
            return transformparams;
        }

        private sitk.VectorOfParameterMap PerformRigidRegistration(sitk.Image fixedImage, sitk.Image movingImage,
            sitk.Image fixedMask = null, sitk.Image movingMask = null)
        {
            RigidRegistration regRigid = new RigidRegistration(fixedImage, movingImage, EditedMapRigid, RegistrationParametersRigid);
            regRigid.SetDefaultParameterMap(RegistrationParametersRigid.RegistrationType, RegistrationParametersRigid.NumberOfResolutions);

            if (fixedMask != null)
            {
                regRigid.SetFixedMask(fixedMask);
            }

            if (movingMask != null)
            {
                regRigid.SetMovingMask(movingMask);
            }

            regRigid.Execute();
            sitk.VectorOfParameterMap transformparams = regRigid.GetTransformationParameterMap();
            regRigid.Dispose();

            return transformparams;
        }

        private sitk.VectorOfParameterMap PerformNonRigidRegistration(sitk.Image fixedImage, sitk.Image movingImage, string movingImageFilename,
            sitk.Image fixedMask = null, sitk.Image movingMask = null, int iteration = -1)
        {
            if (radioButtonTransformRigidity.Checked)
            {
                string coefficientMapFilename = GetInnerStructureSegmentations(movingImageFilename, RegistrationParametersNonRigid, iteration);
                RegistrationParametersNonRigid.CoefficientMapFilename = coefficientMapFilename;
            }

            // TODO
            // make this generic for rigid and non rigid registration
            // registration can be handled through the registration parameters!
            NonRigidRegistration nonRigidRegistration = new NonRigidRegistration(fixedImage, movingImage, EditedMapNonRigid, RegistrationParametersNonRigid);
            //nonRigidRegistration.SetDefaultParameterMap(RegistrationParametersNonRigid.RegistrationType, RegistrationParametersNonRigid.NumberOfResolutions);

            if (fixedMask != null)
            {
                nonRigidRegistration.SetFixedMask(fixedMask);
            }

            if (movingMask != null)
            {
                nonRigidRegistration.SetMovingMask(movingMask);
            }

            nonRigidRegistration.Execute();
            sitk.VectorOfParameterMap transformparams = nonRigidRegistration.GetTransformationParameterMap();
            nonRigidRegistration.Dispose();

            return transformparams;
        }

        private sitk.Image GetWholeParticleMask(string filename, RegistrationParameters parameters, int iteration = -1)
        {
            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile(filename);

            WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, SegmentationParametersWholeParticle);
            segImage.Execute();
            Bitmap bmp = segImage.GetOutput().Bitmap;
            segImage.Dispose();
            image.Dispose();

            string outputFilename = ReadWriteUtils.GetOutputDirectory(parameters, iteration) + "\\mask_" + Path.GetFileName(filename);
            ReadWriteUtils.WriteBitmapAsPng(bmp, outputFilename);
            bmp.Dispose();

            sitk.Image mask = ReadWriteUtils.ReadITKImageFromFile(outputFilename);//ImageUtils.GetITKImageFromBitmap(bmp);

            return mask;
        }

        private string GetInnerStructureSegmentations(string filename, RegistrationParameters parameters, int iteration = -1)
        {
            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile(filename);

            WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, SegmentationParametersWholeParticle);
            segImage.Execute();
            Image<Gray, byte> wholeMask = segImage.GetOutput().Clone();
            segImage.Dispose();

            InnerTissueSegmentation innerSegImage = new InnerTissueSegmentation(image, wholeMask, SegmentationParametersInnerStructures);
            innerSegImage.Execute();

            string filenameCoefficientMap = ReadWriteUtils.GetOutputDirectory(parameters, iteration) + "\\coefficientMap.png";
            ReadWriteUtils.WriteUMatToFile(filenameCoefficientMap, innerSegImage.GetOutput().FirstOrDefault());

            sitk.Image img = ReadWriteUtils.ReadITKImageFromFile(filenameCoefficientMap);
            sitk.CastImageFilter castFilter = new sitk.CastImageFilter();
            castFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkFloat32);
            img = castFilter.Execute(img);
            sitk.RescaleIntensityImageFilter filter = new sitk.RescaleIntensityImageFilter();
            filter.SetOutputMinimum(0.0);
            filter.SetOutputMaximum(1.0);
            sitk.Image coefficientMap = filter.Execute(img);

            filenameCoefficientMap = ReadWriteUtils.GetOutputDirectory(parameters, iteration) + "\\coefficientMap.mhd";
            ReadWriteUtils.WriteSitkImage(coefficientMap, filenameCoefficientMap);
            /*//UMat coefficientMap = innerSegImage.GetCoefficientMatrix();
            string filenameCoefficientMap = Path.Combine(ApplicationContext.OutputPath, RegistrationParametersNonRigid.SubDirectory) + "\\coefficientMap.png";
            //ReadWriteUtils.WriteUMatToFile(filenameCoefficientMap, coefficientMap);*/
            return filenameCoefficientMap;
        }

        /// <summary>
        /// Method executes transform for a given moving image and the calculated transform parameters and writes it to disk.
        /// </summary>
        /// <param name="filename">filename of the image to transform</param>
        /// <param name="transformParams">transform parameters</param>
        /// <param name="parameters">registration parameters</param>
        /// <param name="isBinary">flag for binary registration (will set interpolation order to zero)</param>
        private void WriteTransform(string filename, sitk.VectorOfParameterMap transformParams, RegistrationParameters parameters, int iteration, bool isBinary = false)
        {
            string resultFilename = ReadWriteUtils.GetOutputDirectory(parameters, iteration) + "\\" + Path.GetFileNameWithoutExtension(filename) + ".png";
            // add transform parameter map to registration parameters
            parameters.TransformationParameterMap.Add(resultFilename, transformParams);
            // read moving image from file
            sitk.Image movingImageToTransform = ReadWriteUtils.ReadITKImageFromFile(filename, sitk.PixelIDValueEnum.sitkVectorUInt8);
            // initialize transform instance
            TransformRGB trans = new TransformRGB(movingImageToTransform, transformParams,
                Path.Combine(ApplicationContext.OutputPath, parameters.SubDirectory));

            if (isBinary)
            {
                // for binary reg set interpolation order to zero
                trans.SetInterpolationOrder(0);
            }

            trans.Execute();      
            trans.WriteTransformedImage(resultFilename);

            // write deformation field
            sitk.Image deformationField = trans.GetDeformationField();
            string filenameDeformationField = ReadWriteUtils.GetOutputDirectory(parameters, iteration) + "\\deformationField.mhd";
            ReadWriteUtils.WriteSitkImage(deformationField, filenameDeformationField);
            trans.Dispose();
            movingImageToTransform.Dispose();
        }

        private void buttonEditParameters_Click(object sender, EventArgs e)
        {
            InitializeRigidParameterMap();

            using (EditParametersForm paramForm = new EditParametersForm(EditedMapRigid))
            {
                DialogResult result = paramForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RegistrationParametersRigid.ParamMapToUse = paramForm.Parametermap;
                }
            }
        }

        private void InitializeRigidParameterMap()
        {
            if (EditedMapRigid == null)
            {
                
                using (RigidRegistration reg = new RigidRegistration(RegistrationParametersRigid))
                {
                    EditedMapRigid = reg.GetParameterMap();
                }

                if (EditedMapRigid == null)
                {
                    EditedMapRigid = RegistrationUtils.GetDefaultParameterMap(RegistrationParametersRigid.RegistrationType);
                }
            }
        }

        private void buttonSegmentationParams_Click(object sender, EventArgs e)
        {
            SegmentationParamsRigid();
        }

        /// <summary>
        /// Reads in first image from stack and starts segmentation form rigid to specify segmentation params.
        /// </summary>
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

        /// <summary>
        /// Reads in first image from stack and starts segmentation non rigid form to specify segmentation params.
        /// </summary>
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
            if (backgroundWorkerRigid.WorkerSupportsCancellation == true)
            {
                backgroundWorkerRigid.CancelAsync();
            }
        }

        private void buttonEvaluation_Click(object sender, EventArgs e)
        {
            using (EvaluationForm form = new EvaluationForm(RegistrationParametersRigid))
            {
                form.ShowDialog();
            }
        }

        private void checkBoxMaskRegistration_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMaskRegistration.Checked)
            {
                //RegistrationParametersRigid.
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
            EditedMapRigid = null;
            if (radioButtonTranslation.Checked)
            {
                SetRegistrationParameterMap(RegistrationParametersRigid, RegistrationDefaultParameters.translation, EditedMapRigid);
            }
            else if (radioButtonSimilarity.Checked)
            {
                SetRegistrationParameterMap(RegistrationParametersRigid, RegistrationDefaultParameters.similarity, EditedMapRigid);
            }
            else if (radioButtonRigid.Checked)
            {
                SetRegistrationParameterMap(RegistrationParametersRigid, RegistrationDefaultParameters.rigid, EditedMapRigid);
            }
            else if (radioButtonAffine.Checked)
            {
                SetRegistrationParameterMap(RegistrationParametersRigid, RegistrationDefaultParameters.affine, EditedMapRigid);
            }
            RegistrationParametersRigid.SubDirectory = RegistrationParametersRigid.RegistrationType + "_" + DateTime.Now.ToShortDateString();
        }

        private void RegistrationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void radioButtonAdvancedBspline_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNonRigidRegistrationType();
        }

        private void radioButtonBsplineDiffusion_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNonRigidRegistrationType();
        }

        private void radioButtonKernelSpline_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNonRigidRegistrationType();
        }

        private void radioButtonSplineRecursive_CheckedChanged(object sender, EventArgs e)
        {
            UpdateNonRigidRegistrationType();
        }

        private void UpdateNonRigidRegistrationType()
        {
            EditedMapNonRigid = null;
            if (radioButtonAdvancedBspline.Checked)
            {
                SetRegistrationParameterMap(RegistrationParametersNonRigid, RegistrationDefaultParameters.bspline, EditedMapNonRigid);
            }
            else if (radioButtonBsplineDiffusion.Checked)
            {
                SetRegistrationParameterMap(RegistrationParametersNonRigid, RegistrationDefaultParameters.diffusion, EditedMapNonRigid);
            }
            else if (radioButtonKernelSpline.Checked)
            {
                SetRegistrationParameterMap(RegistrationParametersNonRigid, RegistrationDefaultParameters.spline, EditedMapNonRigid);
            }
            else if (radioButtonSplineRecursive.Checked)
            {
                SetRegistrationParameterMap(RegistrationParametersNonRigid, RegistrationDefaultParameters.recursive, EditedMapNonRigid);
            }
            RegistrationParametersNonRigid.SubDirectory = RegistrationParametersNonRigid.RegistrationType + "_" + DateTime.Now.ToShortDateString();
        }

        private void SetRegistrationParameterMap(RegistrationParameters parameters, RegistrationDefaultParameters defaultParams, sitk.ParameterMap map)
        {
            parameters.RegistrationType = defaultParams;
            map = RegistrationUtils.GetDefaultParameterMap(defaultParams);
        }

        private void buttonEditParamsNonRigid_Click(object sender, EventArgs e)
        {
            InitializeNonRigidParameterMap();

            using (EditParametersForm paramForm = new EditParametersForm(EditedMapNonRigid))
            {
                DialogResult result = paramForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RegistrationParametersNonRigid.ParamMapToUse = paramForm.Parametermap;
                }
            }
        }

        private void InitializeNonRigidParameterMap()
        {
            if (EditedMapNonRigid == null)
            {
                using (NonRigidRegistration reg = new NonRigidRegistration(RegistrationParametersNonRigid))
                {
                    EditedMapNonRigid = reg.GetParameterMap();
                }

                if (EditedMapNonRigid == null)
                {
                    EditedMapNonRigid = RegistrationUtils.GetDefaultParameterMap(RegistrationParametersNonRigid.RegistrationType);
                }
            }
        }

        private void buttonStartNonRigidRegistration_Click(object sender, EventArgs e)
        {
            if (!backgroundWorkerNonRigid.IsBusy)
            {
                textBoxConsoleNonRigid.Clear();
                progressBarNonRigid.Value = 0;
                buttonStartNonRigidRegistration.Enabled = false;
                buttonCancelNonRigidReg.Enabled = true;
                backgroundWorkerNonRigid.RunWorkerAsync();
            }
        }

        private void radioButtonNoPenalties_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePenalty();
        }

        private void UpdatePenalty()
        {
            if(radioButtonNoPenalties.Checked)
            {
                RegistrationParametersNonRigid.Penaltyterm = PenaltyTerm.None;
            } else if(radioButtonTransformRigidity.Checked)
            {
                RegistrationParametersNonRigid.SetTransformPenaltyTerm();
            } else if (radioButtonBendEnergy.Checked)
            {
                RegistrationParametersNonRigid.SetTransformBendingEnergy();
            } else if (radioButtonDistancePreserving.Checked)
            {
                RegistrationParametersNonRigid.SetDistancePreservingRigidityPenalty();
            }
        }

        private void radioButtonTransformRigidity_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePenalty();
            if (radioButtonTransformRigidity.Checked)
            {
                buttonSegmentationInnerstructures.Enabled = true;
            }
        }

        private void radioButtonBendEnergy_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePenalty();
        }

        private void radioButtonDistancePreserving_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePenalty();
        }

        private void buttonEvaluateNonRigidReg_Click(object sender, EventArgs e)
        {
            using(EvaluationForm form = new EvaluationForm(RegistrationParametersNonRigid))
            {
                form.ShowDialog();
            }
        }
    }
}
