using MaskedDeformableRegistrationApp.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaskedDeformableRegistrationApp.Forms
{
    public partial class LoadMaskForm : Form
    {
        private string FixedMaskFilename = null;
        private List<string> MovingMasksFilenames = new List<string>();
        private List<string> MovingImageCoefficientMaps = new List<string>();

        private const string _dialogFilterImages = "image files (*.png)|*.png|(*.mhd)|*.mhd|(*.tif)|*.tif|(*.bmp)|*.bmp";

        private readonly string _initDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public RegistrationParameters registrationParametersRigid = null;
        public RegistrationParameters registrationParametersNonRigid = null;

        public LoadMaskForm(RegistrationParameters parametersR, RegistrationParameters parametersNR)
        {
            registrationParametersRigid = parametersR;
            registrationParametersNonRigid = parametersNR;
            InitializeComponent();
        }

        private void checkBoxFixedMask_CheckedChanged(object sender, EventArgs e)
        {
            textBoxFixedMask.Enabled = checkBoxFixedMask.Checked;
            buttonChooseFixedMask.Enabled = checkBoxFixedMask.Checked;
        }

        private void checkBoxMovingMasks_CheckedChanged(object sender, EventArgs e)
        {
            textBoxMovingMasks.Enabled = checkBoxMovingMasks.Checked;
            buttonChooseMovingMasks.Enabled = checkBoxMovingMasks.Checked;
        }

        private void checkBoxCoefficientMaps_CheckedChanged(object sender, EventArgs e)
        {
            textBoxCoefficientMaps.Enabled = checkBoxCoefficientMaps.Checked;
            buttonChooseCoefficientMaps.Enabled = checkBoxCoefficientMaps.Checked;
        }

        private void buttonChooseFixedMask_Click(object sender, EventArgs e)
        {
            openFileDialogFixedMask.Filter = _dialogFilterImages;
            openFileDialogFixedMask.InitialDirectory = _initDir;
            openFileDialogFixedMask.Multiselect = false;
            DialogResult result = openFileDialogFixedMask.ShowDialog();

            if (result == DialogResult.OK)
            {
                FixedMaskFilename = openFileDialogFixedMask.FileName;
                if (FixedMaskFilename != null)
                {
                    textBoxFixedMask.Text = FixedMaskFilename;
                }
                buttonSave.Enabled = true;
            }
        }

        private void buttonChooseMovingMasks_Click(object sender, EventArgs e)
        {
            openFileDialogMovingMasks.Filter = _dialogFilterImages;
            openFileDialogMovingMasks.InitialDirectory = _initDir;
            openFileDialogMovingMasks.Multiselect = true;
            DialogResult result = openFileDialogMovingMasks.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (MovingMasksFilenames.Count > 0)
                {
                    MovingMasksFilenames.Clear();
                }

                foreach(string path in openFileDialogMovingMasks.FileNames)
                {
                    MovingMasksFilenames.Add(path);
                }
                FillTexgtBox(textBoxMovingMasks, MovingMasksFilenames);
                buttonSave.Enabled = true;
            }
        }

        private void buttonChooseCoefficientMaps_Click(object sender, EventArgs e)
        {
            openFileDialogCoefficientMaps.Filter = _dialogFilterImages;
            openFileDialogCoefficientMaps.InitialDirectory = _initDir;
            openFileDialogCoefficientMaps.Multiselect = true;
            DialogResult result = openFileDialogCoefficientMaps.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (MovingImageCoefficientMaps.Count > 0)
                {
                    MovingImageCoefficientMaps.Clear();
                }

                foreach (string path in openFileDialogCoefficientMaps.FileNames)
                {
                    MovingImageCoefficientMaps.Add(path);
                }
                FillTexgtBox(textBoxCoefficientMaps, MovingImageCoefficientMaps);
                buttonSave.Enabled = true;
            }
        }

        private void FillTexgtBox(TextBox textBox, List<string> listOfStringas)
        {
            foreach (string str in listOfStringas)
            {
                textBox.AppendText(str + "; ");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            PopulateLoadingParameters(registrationParametersRigid);
            PopulateLoadingParameters(registrationParametersNonRigid);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void PopulateLoadingParameters(RegistrationParameters registrationParameters)
        {
            if (FixedMaskFilename != null && File.Exists(FixedMaskFilename))
            {
                registrationParameters.UseFixedMaskFromDisk = true;
                registrationParameters.FixedMaskFromDisk = FixedMaskFilename;
            }
            if (MovingMasksFilenames.Count > 0)
            {
                bool filesExist = true;
                MovingMasksFilenames.ForEach(it => filesExist &= File.Exists(it));
                if (filesExist)
                {
                    registrationParameters.UseMovingMasksFromDisk = true;
                    registrationParameters.MovingMasksFromDisk = MovingMasksFilenames;
                }
            }
            if (MovingImageCoefficientMaps.Count > 0)
            {
                bool filesExist = true;
                MovingImageCoefficientMaps.ForEach(it => filesExist &= File.Exists(it));
                if (filesExist)
                {
                    registrationParameters.UseCoefficientMapsFromDisk = true;
                    registrationParameters.CoefficientMapsFromDisk = MovingImageCoefficientMaps;
                }
            }
        }

        private void LoadMaskForm_Load(object sender, EventArgs e)
        {
            checkBoxFixedMask.Checked = true;
            buttonSave.Enabled = false;
            buttonChooseMovingMasks.Enabled = false;
            textBoxMovingMasks.Enabled = false;
            buttonChooseCoefficientMaps.Enabled = false;
            textBoxCoefficientMaps.Enabled = false;
        }
    }
}
