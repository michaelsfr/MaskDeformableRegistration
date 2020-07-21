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
            buttonCancel.Enabled = false;
            buttonEvaluation.Enabled = false;
            buttonSegmentationInnerstructures.Enabled = true;
            buttonCancelNonRigidReg.Enabled = false;
            buttonEvaluateNonRigidReg.Enabled = false;
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
            if (EditedMapRigid == null)
            {

                using (RigidRegistration reg = new RigidRegistration(RegistrationParametersRigid))
                {
                    EditedMapRigid = reg.GetParameterMap();
                }

                if (EditedMapRigid == null)
                {
                    EditedMapRigid = RegistrationUtils.GetDefaultParameterMap(RegistrationParametersRigid.RegistrationDefaultParams);
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
                    EditedMapNonRigid = RegistrationUtils.GetDefaultParameterMap(RegistrationParametersNonRigid.RegistrationDefaultParams);
                }
            }
        }

        #endregion

        #region Events

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

        private void buttonSegmentationParams_Click(object sender, EventArgs e)
        {
            SegmentationParamsInnerStructures();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerRigid.WorkerSupportsCancellation == true)
            {
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

        private void radioButtonTranslation_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRigidRegistrationType();
        }

        private void buttonSegmentationInnerstructures_Click(object sender, EventArgs e)
        {
            SegmentationParamsWholeTissue();
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

        private void radioButtonTransformRigidity_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePenalty();
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
            using (EvaluationForm form = new EvaluationForm(RegistrationParametersNonRigid))
            {
                form.ShowDialog();
            }
        }

        private void buttonCancelNonRigidReg_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerNonRigid.WorkerSupportsCancellation == true)
            {
                backgroundWorkerNonRigid.CancelAsync();

                progressBarNonRigid.Value = 0;
                AppendLine(textBoxConsoleNonRigid, "Registration cancelled.");
                buttonStartNonRigidRegistration.Enabled = true;
                buttonCancelNonRigidReg.Enabled = false;
            }
            // clean up?
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
        private void SegmentationParamsInnerStructures()
        {
            Cursor.Current = Cursors.WaitCursor;
            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile<Bgr, byte>(ImageStackToRegister.FirstOrDefault());

            using (SegParamsRigidForm form = new SegParamsRigidForm(image, RegistrationParametersRigid.InnerStructuresSegParams))
            {
                Cursor.Current = Cursors.Default;
                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RegistrationParametersRigid.InnerStructuresSegParams = form.segmentationParameters;
                    RegistrationParametersNonRigid.InnerStructuresSegParams = form.segmentationParameters;
                    //SegmentationParametersInnerStructures = form.segmentationParameters;
                }
            }
        }

        /// <summary>
        /// Reads in first image from stack and starts segmentation non rigid form to specify segmentation params.
        /// </summary>
        private void SegmentationParamsWholeTissue()
        {
            Cursor.Current = Cursors.WaitCursor;

            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile<Bgr, byte>(ImageStackToRegister.FirstOrDefault());

            WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, RegistrationParametersRigid.WholeTissueSegParams);
            segImage.Execute();
            Image<Gray, byte> mask = segImage.GetOutput().Clone();
            segImage.Dispose();

            using (SegParamsNonRigidForm form = new SegParamsNonRigidForm(image, mask, RegistrationParametersRigid.InnerStructuresSegParams))
            {
                Cursor.Current = Cursors.Default;
                DialogResult result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RegistrationParametersRigid.WholeTissueSegParams = form.segmentationParameters;
                    RegistrationParametersNonRigid.WholeTissueSegParams = form.segmentationParameters;
                    //SegmentationParametersInnerStructures = form.segmentationParameters;
                }
            }
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
            RegistrationParametersRigid.SubDirectory = RegistrationParametersRigid.RegistrationDefaultParams + "_" + DateTime.Now.ToShortDateString();
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
            RegistrationParametersNonRigid.SubDirectory = RegistrationParametersNonRigid.RegistrationDefaultParams + "_" + DateTime.Now.ToShortDateString();
        }

        private void SetRegistrationParameterMap(RegistrationParameters parameters, RegistrationDefaultParameters defaultParams, sitk.ParameterMap map)
        {
            parameters.RegistrationDefaultParams = defaultParams;
            map = RegistrationUtils.GetDefaultParameterMap(defaultParams);
        }

        private void UpdatePenalty()
        {
            if (radioButtonNoPenalties.Checked)
            {
                RegistrationParametersNonRigid.Penaltyterm = PenaltyTerm.None;
            }
            else if (radioButtonTransformRigidity.Checked)
            {
                RegistrationParametersNonRigid.SetTransformPenaltyTerm();
            }
            else if (radioButtonBendEnergy.Checked)
            {
                RegistrationParametersNonRigid.SetTransformBendingEnergy();
            }
            else if (radioButtonDistancePreserving.Checked)
            {
                RegistrationParametersNonRigid.SetDistancePreservingRigidityPenalty();
            }
        }

        #endregion        
    }
}
