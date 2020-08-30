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
        #region Initialization

        private List<string> ImageStackToRegister { get; set; }

        private RegistrationParameters RegistrationParametersRigid { get; set; } = RegistrationParameters.GetRigidRegistrationParameters();
        private RegistrationParameters RegistrationParametersNonRigid { get; set; } = RegistrationParameters.GetNonRigidRegistrationParameters();

        private uint LargestImageWidth { get; set; } = 0;
        private uint LargestImageHeight { get; set; } = 0;

        private List<string> ParameterFilenames = new List<string>();

        public RegistrationForm(List<string> filenamesToRegistrate)
        {
            InitializeComponent();
            InitializeButtons();
            ImageStackToRegister = filenamesToRegistrate;

            InitializeBackgroundWorker();
        }

        private void InitializeButtons()
        {
            radioButtonFirstFromStack.Checked = true;
            radioButtonNoMask.Checked = true;
            radioButtonTranslation.Checked = true;
            radioButtonAdvancedBspline.Checked = true;
            radioButtonNoPenalties.Checked = true;
            radioButtonWholeTissue.Checked = true;
            radioButtonRigidNoMasking.Checked = true;
            buttonCancel.Enabled = false;
            buttonEvaluation.Enabled = false;
            buttonSegmentationInnerstructures.Enabled = true;
            buttonCancelNonRigidReg.Enabled = false;
            buttonEvaluateNonRigidReg.Enabled = false;
            checkBoxUseCoefficientmap.Enabled = false;
            buttonLoadMasks.Enabled = false;
            labelOrder.Enabled = false;
            comboBoxInterpolationOrder.Enabled = false;

            comboBoxInterpolationOrder.Items.AddRange(new object[] { 0, 1, 2, 3, 4, 5 });
            comboBoxInterpolationOrder.SelectedIndex = 3;
            radioButtonCompose.Checked = true;

            numericUpDownDefaultPixelValue.Minimum = 0;
            numericUpDownDefaultPixelValue.Maximum = 255;
            numericUpDownDefaultPixelValue.Value = 0;
        }

        /// <summary>
        /// Initialization of both background workers for rigid and non rigid registration.
        /// </summary>
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

            // set default registration parameter maps
            InitializeRigidParameterMap();
            InitializeNonRigidParameterMap();

            DetermineLargestImageDimensions();
            Cursor.Current = Cursors.WaitCursor;
        }

        private void InitializeRigidParameterMap()
        {
            if (RegistrationParametersRigid.ParamMapToUse == null)
            {

                using (RigidRegistration reg = new RigidRegistration(RegistrationParametersRigid))
                {
                    RegistrationParametersRigid.ParamMapToUse = reg.GetParameterMap();
                }

                if (RegistrationParametersRigid.ParamMapToUse == null)
                {
                    RegistrationParametersRigid.ParamMapToUse = RegistrationUtils.GetDefaultParameterMap(RegistrationParametersRigid.RegistrationDefaultParams);
                }
            }
        }


        private void InitializeNonRigidParameterMap()
        {
            if (RegistrationParametersNonRigid.ParamMapToUse == null)
            {
                using (NonRigidRegistration reg = new NonRigidRegistration(RegistrationParametersNonRigid))
                {
                    RegistrationParametersNonRigid.ParamMapToUse = reg.GetParameterMap();
                }

                if (RegistrationParametersNonRigid.ParamMapToUse == null)
                {
                    RegistrationParametersNonRigid.ParamMapToUse = RegistrationUtils.GetDefaultParameterMap(RegistrationParametersNonRigid.RegistrationDefaultParams);
                }
            }
        }

        #endregion

        #region Events

        private void buttonStartRegistration_Click(object sender, EventArgs e)
        {
            GatherRigidParameters();
            if (backgroundWorkerRigid.IsBusy != true)
            {
                textBoxConsoleRigid.Clear();
                progressBarRigid.Value = 0;
                buttonStartRegistration.Enabled = false;
                buttonCancel.Enabled = true;
                backgroundWorkerRigid.RunWorkerAsync();
            }

        }

        private void buttonEditParameters_Click(object sender, EventArgs e)
        {
            InitializeRigidParameterMap();

            using (EditParametersForm paramForm = new EditParametersForm(RegistrationParametersRigid.ParamMapToUse))
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
            SegmentationParamsWholeTissue();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerRigid.WorkerSupportsCancellation == true)
            {
                // todo: kill process
                backgroundWorkerRigid.CancelAsync();

                progressBarRigid.Value = 0;
                AppendLine(textBoxConsoleRigid, "Registration cancelled.");
                buttonStartRegistration.Enabled = true;
                buttonCancel.Enabled = false;
            }
            // clean up?
        }

        private void buttonEvaluation_Click(object sender, EventArgs e)
        {
            using (EvaluationForm form = new EvaluationForm(RegistrationParametersRigid))
            {
                form.ShowDialog();
            }
        }

        private void buttonSegmentationInnerstructures_Click(object sender, EventArgs e)
        {
            SegmentationParamsInnerStructures();
        }

        private void RegistrationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void buttonEditParamsNonRigid_Click(object sender, EventArgs e)
        {
            InitializeNonRigidParameterMap();

            using (EditParametersForm paramForm = new EditParametersForm(RegistrationParametersNonRigid.ParamMapToUse))
            {
                DialogResult result = paramForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RegistrationParametersNonRigid.ParamMapToUse = paramForm.Parametermap;
                }
            }
        }

        private void buttonStartNonRigidRegistration_Click(object sender, EventArgs e)
        {
            GatherNonRigidParameters();
            if (!backgroundWorkerNonRigid.IsBusy)
            {
                textBoxConsoleNonRigid.Clear();
                progressBarNonRigid.Value = 0;
                buttonStartNonRigidRegistration.Enabled = false;
                buttonCancelNonRigidReg.Enabled = true;
                backgroundWorkerNonRigid.RunWorkerAsync();
            }
        }

        private void buttonEvaluateNonRigidReg_Click(object sender, EventArgs e)
        {
            using (EvaluationForm form = new EvaluationForm(RegistrationParametersNonRigid))
            {
                form.ShowDialog();
            }
        }

        private void buttonCancelNonRigidReg_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerNonRigid.WorkerSupportsCancellation == true)
            {
                // todo: kill process
                backgroundWorkerNonRigid.CancelAsync();

                progressBarNonRigid.Value = 0;
                AppendLine(textBoxConsoleNonRigid, "Registration cancelled.");
                buttonStartNonRigidRegistration.Enabled = true;
                buttonCancelNonRigidReg.Enabled = false;
            }
            // clean up?
        }

        private void radioButtonTranslation_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTranslation.Checked)
            {
                RegistrationParametersRigid.ParamMapToUse = null;
                RegistrationParametersRigid.RegistrationDefaultParams = RegistrationDefaultParameters.translation;
            }
        }

        private void radioButtonSimilarity_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSimilarity.Checked)
            {
                RegistrationParametersRigid.ParamMapToUse = null;
                RegistrationParametersRigid.RegistrationDefaultParams = RegistrationDefaultParameters.similarity;
            }
        }

        private void radioButtonRigid_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRigid.Checked)
            {
                RegistrationParametersRigid.ParamMapToUse = null;
                RegistrationParametersRigid.RegistrationDefaultParams = RegistrationDefaultParameters.rigid;
            }
        }

        private void radioButtonAffine_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAffine.Checked)
            {
                RegistrationParametersRigid.ParamMapToUse = null;
                RegistrationParametersRigid.RegistrationDefaultParams = RegistrationDefaultParameters.affine;
            }
        }

        #endregion

        #region BackgroundWorker

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

        private void backgroundWorkerRigid_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            RegistrationController controller = new RegistrationController(RegistrationParametersRigid, worker);
            controller.RunRegistration(ImageStackToRegister);
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
            RegistrationController controller = new RegistrationController(RegistrationParametersNonRigid, worker);
            controller.RunRegistration(ImageStackToRegister);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines the largest width and height of all images in image stack.
        /// Images of stack will be resized to these dimensions.
        /// </summary>
        private void DetermineLargestImageDimensions()
        {
            foreach (string filename in ImageStackToRegister)
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
            RegistrationParametersRigid.LargestImageWidth = RegistrationParametersNonRigid.LargestImageWidth = LargestImageWidth;
            RegistrationParametersRigid.LargestImageHeight = RegistrationParametersNonRigid.LargestImageHeight = LargestImageHeight;
        }

        /// <summary>
        /// Apppend line to a given text box.
        /// </summary>
        /// <param name="source">text box</param>
        /// <param name="value">line as string</param>
        public void AppendLine(TextBox source, string value)
        {
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }

        /// <summary>
        /// Reads in first image from stack and starts segmentation form rigid to specify segmentation params.
        /// </summary>
        private void SegmentationParamsWholeTissue()
        {
            Cursor.Current = Cursors.WaitCursor;
            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile<Bgr, byte>(ImageStackToRegister.FirstOrDefault());

            using (SegParamsWholeTissueForm form = new SegParamsWholeTissueForm(image, RegistrationParametersRigid.WholeTissueSegParams))
            {
                Cursor.Current = Cursors.Default;
                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RegistrationParametersRigid.WholeTissueSegParams = (SegmentationParameters)form.segmentationParameters.Clone();
                    RegistrationParametersNonRigid.WholeTissueSegParams = (SegmentationParameters)form.segmentationParameters.Clone();
                }
            }
        }

        /// <summary>
        /// Reads in first image from stack and starts segmentation non rigid form to specify segmentation params.
        /// </summary>
        private void SegmentationParamsInnerStructures()
        {
            Cursor.Current = Cursors.WaitCursor;

            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile<Bgr, byte>(ImageStackToRegister.FirstOrDefault());

            WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, RegistrationParametersRigid.WholeTissueSegParams);
            segImage.Execute();
            Image<Gray, byte> mask = segImage.GetOutput().Clone();
            segImage.Dispose();

            using (SegParamsInnerStructuresForm form = new SegParamsInnerStructuresForm(image, mask, RegistrationParametersNonRigid.InnerStructuresSegParams))
            {
                Cursor.Current = Cursors.Default;
                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RegistrationParametersRigid.InnerStructuresSegParams = (SegmentationParameters)form.segmentationParameters.Clone();
                    RegistrationParametersNonRigid.InnerStructuresSegParams = (SegmentationParameters)form.segmentationParameters.Clone();
                }
            }
        }

        private void GatherRigidParameters()
        {
            GatherGeneralParameters(RegistrationParametersRigid);
            UpdateRigidRegistrationType(RegistrationParametersRigid);
            UpdateRigidMaskOptions(RegistrationParametersRigid);
        }

        private void GatherNonRigidParameters()
        {
            GatherGeneralParameters(RegistrationParametersNonRigid);
            UpdateNonRigidRegistrationType(RegistrationParametersNonRigid);
            //UpdatePenalty(RegistrationParametersNonRigid);
            UpdateMiscOptions(RegistrationParametersNonRigid);
        }

        private void GatherGeneralParameters(RegistrationParameters parameters)
        {
            parameters.Order = GetRegistrationOrder();
            UpdateGeneralMaskOptions(parameters);
        }

        private void UpdateGeneralMaskOptions(RegistrationParameters parameters)
        {
            if (radioButtonOnlyFixedMask.Checked)
            {
                parameters.UseFixedMask = true;
                parameters.UseMovingMask = false;
            } else if (radioButtonOnlyMovingMask.Checked)
            {
                parameters.UseFixedMask = false;
                parameters.UseMovingMask = true;
            } else if (radioButtonFixedAndMovingMask.Checked)
            {
                parameters.UseFixedMask = true;
                parameters.UseMovingMask = true;
            } else
            {
                parameters.UseFixedMask = false;
                parameters.UseMovingMask = false;
            }

            if (radioButtonWholeTissue.Checked)
            {
                parameters.UseInnerStructuresSegmentation = false;
            } else if (radioButtonInnerStructures.Checked)
            {
                parameters.UseInnerStructuresSegmentation = true;
            }
        }

        private void UpdateRigidMaskOptions(RegistrationParameters parameters)
        {
            if (radioButtonRigidNoMasking.Checked)
            {
                parameters.RigidOptions = MaskedRigidRegistrationOptions.None;
            } else if (radioButtonMaskWhole.Checked)
            {
                parameters.RigidOptions = MaskedRigidRegistrationOptions.BinaryRegistrationWholeTissue;
                parameters.UseInnerStructuresSegmentation = false;
            } else if (radioButtonMaskInner.Checked)
            {
                parameters.RigidOptions = MaskedRigidRegistrationOptions.BinaryRegistrationInnerStructures;
                parameters.UseInnerStructuresSegmentation = true;
            } else if (radioButtonComponent.Checked)
            {
                parameters.RigidOptions = MaskedRigidRegistrationOptions.ComponentwiseRegistration;
            }
        }

        private void UpdateMiscOptions(RegistrationParameters registrationParametersNonRigid)
        {
            if (checkBoxJaccobian.Checked)
            {
                registrationParametersNonRigid.ComputeJaccobian = true;
            } else if (checkBoxUseCoefficientmap.Checked) {

            }
        }

        private RegistrationOrder GetRegistrationOrder()
        {
            RegistrationOrder order = RegistrationOrder.FirstInStackIsReference;
            if (radioButtonLastInStack.Checked)
            {
                order = RegistrationOrder.LastInStackIsReference;
            } else if (radioButtonMiddleStack.Checked)
            {
                order = RegistrationOrder.MedianIsReference;
            } else if (radioButtonUsePrevInStack.Checked)
            {
                order = RegistrationOrder.PreviousIsReference;
            }
            return order;
        }

        private void UpdateRigidRegistrationType(RegistrationParameters parameters)
        {
            if(parameters.ParamMapToUse == null)
            {
                if (radioButtonTranslation.Checked)
                {
                    SetRegistrationParameterMap(parameters, RegistrationDefaultParameters.translation);
                }
                else if (radioButtonSimilarity.Checked)
                {
                    SetRegistrationParameterMap(parameters, RegistrationDefaultParameters.similarity);
                }
                else if (radioButtonRigid.Checked)
                {
                    SetRegistrationParameterMap(parameters, RegistrationDefaultParameters.rigid);
                }
                else if (radioButtonAffine.Checked)
                {
                    SetRegistrationParameterMap(parameters, RegistrationDefaultParameters.affine);
                }
            }
            parameters.SubDirectory = parameters.RegistrationDefaultParams + "_" + DateTime.Now.ToShortDateString();
        }

        private void UpdateNonRigidRegistrationType(RegistrationParameters parameters)
        {
            if(parameters.ParamMapToUse == null)
            {
                if (radioButtonAdvancedBspline.Checked)
                {
                    SetRegistrationParameterMap(parameters, RegistrationDefaultParameters.bspline);
                }
                else if (radioButtonBsplineDiffusion.Checked)
                {
                    SetRegistrationParameterMap(parameters, RegistrationDefaultParameters.diffusion);
                    parameters.NonRigidOptions = MaskedNonRigidRegistrationOptions.DiffuseRegistration;
                }
                else if (radioButtonKernelSpline.Checked)
                {
                    SetRegistrationParameterMap(parameters, RegistrationDefaultParameters.spline);
                }
                else if (radioButtonSplineRecursive.Checked)
                {
                    SetRegistrationParameterMap(parameters, RegistrationDefaultParameters.recursive);
                }
            }
            parameters.SubDirectory = parameters.RegistrationDefaultParams + "_" + DateTime.Now.ToShortDateString();
        }

        private void SetRegistrationParameterMap(RegistrationParameters parameters, RegistrationDefaultParameters defaultParams)
        {
            parameters.RegistrationDefaultParams = defaultParams;
            parameters.ParamMapToUse = RegistrationUtils.GetDefaultParameterMap(defaultParams);
        }

        private void UpdatePenalty(RegistrationParameters parameters)
        {
            if (radioButtonNoPenalties.Checked)
            {
                parameters.Penaltyterm = PenaltyTerm.None;
            }
            else if (radioButtonTransformRigidity.Checked)
            {
                parameters.SetTransformPenaltyTerm();
            }
            else if (radioButtonBendEnergy.Checked)
            {
                parameters.SetTransformBendingEnergy();
            }
            else if (radioButtonDistancePreserving.Checked)
            {
                parameters.SetDistancePreservingRigidityPenalty();
            }
        }

        #endregion

        private void radioButtonTransformRigidity_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxUseCoefficientmap.Enabled = radioButtonTransformRigidity.Checked;
            if (RegistrationParametersNonRigid != null && RegistrationParametersNonRigid.ParamMapToUse != null)
            {
                RegistrationUtils.AddTransformRigidityPenaltyToParamMap(RegistrationParametersNonRigid.ParamMapToUse);
            }
        }

        private void radioButtonNoPenalties_CheckedChanged(object sender, EventArgs e)
        {
            if (RegistrationParametersNonRigid != null && RegistrationParametersNonRigid.ParamMapToUse != null)
            {
                RegistrationUtils.RemovePenaltyTerm(RegistrationParametersNonRigid.ParamMapToUse);
            }
        }

        private void radioButtonBendEnergy_CheckedChanged(object sender, EventArgs e)
        {
            if (RegistrationParametersNonRigid != null && RegistrationParametersNonRigid.ParamMapToUse != null)
            {
                RegistrationUtils.AddBendingEnergyPenaltyToParamMap(RegistrationParametersNonRigid.ParamMapToUse);
            }
        }

        private void radioButtonDistancePreserving_CheckedChanged(object sender, EventArgs e)
        {
            if (RegistrationParametersNonRigid != null && RegistrationParametersNonRigid.ParamMapToUse != null)
            {
                RegistrationUtils.AddDistancePreservingRigidityPenaltyToParamMap(RegistrationParametersNonRigid.ParamMapToUse);
            }
        }

        private void checkBoxUseCoefficientmap_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxUseCoefficientmap.Checked)
            {
                RegistrationParametersNonRigid.UseInnerStructuresSegmentation = true;
                RegistrationParametersNonRigid.NonRigidOptions = MaskedNonRigidRegistrationOptions.BsplineWithPenaltyTermAndCoefficientMap;
            } else
            {
                RegistrationParametersNonRigid.UseInnerStructuresSegmentation = false;
                RegistrationParametersNonRigid.NonRigidOptions = MaskedNonRigidRegistrationOptions.BsplineWithPenaltyTerm;
            }
            
        }

        private void radioButtonBsplineDiffusion_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonBsplineDiffusion.Checked)
            {
                RegistrationParametersNonRigid.NonRigidOptions = MaskedNonRigidRegistrationOptions.DiffuseRegistration;
                RegistrationParametersNonRigid.ParamMapToUse = RegistrationUtils.GetDefaultParameterMap(RegistrationDefaultParameters.diffusion);
            }
        }

        private void radioButtonAdvancedBspline_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonPreDefMasks_CheckedChanged(object sender, EventArgs e)
        {
            buttonLoadMasks.Enabled = radioButtonPreDefMasks.Checked;
        }

        private void buttonLoadMasks_Click(object sender, EventArgs e)
        {
            using (LoadMaskForm form = new LoadMaskForm(parametersR: RegistrationParametersRigid, parametersNR: RegistrationParametersNonRigid))
            {
                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RegistrationParametersRigid = form.registrationParametersRigid;
                    RegistrationParametersNonRigid = form.registrationParametersNonRigid;
                }
            }
        }

        private void radioButtonBSpline_CheckedChanged(object sender, EventArgs e)
        {
            labelOrder.Enabled = radioButtonBSpline.Checked;
            comboBoxInterpolationOrder.Enabled = radioButtonBSpline.Checked;
        }

        private void buttonChooseParamFiles_Click(object sender, EventArgs e)
        {
            if(ParameterFilenames.Count > 0)
            {
                ParameterFilenames.Clear();
            }

            openFileDialogTransformParam.Multiselect = true;
            openFileDialogTransformParam.Filter = "parameter files(*.txt)| *.txt";
            openFileDialogTransformParam.InitialDirectory = ApplicationContext.OutputPath;
            openFileDialogTransformParam.FileName = "";
            DialogResult result = openFileDialogTransformParam.ShowDialog();

            if (result == DialogResult.OK)
            {
                foreach (string path in openFileDialogTransformParam.FileNames)
                {
                    ParameterFilenames.Add(path);
                }
            }

            textBoxTransformParams.Text = string.Join(", ", ParameterFilenames.Select(it => Path.GetFileName(it).ToArray()));
        }

        private void buttonTransform_Click(object sender, EventArgs e)
        {
            // remove fixed image from list
            List<string> movingImages = ImageStackToRegister.Skip(1).ToList();
            TransformationController controller = new TransformationController(ParameterFilenames, movingImages);
            controller.ComposeTransformsParameters(radioButtonCompose.Checked);
            if (radioButtonLinear.Checked) controller.SetInterpolationType(Interpolator.LinearInterpolation);
            if (radioButtonNN.Checked) controller.SetInterpolationType(Interpolator.NearestNighbour);
            if (radioButtonBSpline.Checked) controller.SetInterpolationType(Interpolator.BSplineInterpolation, (int)comboBoxInterpolationOrder.SelectedValue);
            controller.SetDefaultPixelValue((double)numericUpDownDefaultPixelValue.Value);
            Cursor.Current = Cursors.WaitCursor;
            controller.StartTransformation();
            Cursor.Current = Cursors.Default;
        }
    }
}
