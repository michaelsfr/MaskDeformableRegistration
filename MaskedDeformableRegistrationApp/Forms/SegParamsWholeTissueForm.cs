using Emgu.CV;
using Emgu.CV.Structure;
using MaskedDeformableRegistrationApp.Segmentation;
using MaskedDeformableRegistrationApp.Utils;
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
    public partial class SegParamsWholeTissueForm : Form
    {
        public SegmentationParameters segmentationParameters;

        private string selectedFile;
        private List<string> filenames;

        private Image<Bgr, byte> image;
        private Image<Gray, byte> mask;

        public SegParamsWholeTissueForm(List<string> imageFilnames, SegmentationParameters parameters)
        {
            InitializeComponent();

            this.segmentationParameters = parameters;
            this.filenames = imageFilnames;
            this.selectedFile = imageFilnames.FirstOrDefault();
            this.image = ReadWriteUtils.ReadOpenCVImageFromFile<Bgr, byte>(this.selectedFile);

            InitializeButtons();
        }

        private void InitializeButtons()
        {
            ToolTip toolTip1 = new ToolTip();
            numericUpDownMin.Enabled = false;
            numericUpDownMin.Minimum = 0;
            toolTip1.SetToolTip(numericUpDownMin, "Leave 0 to not specify a lower bound.");
            numericUpDownMax.Enabled = false;
            numericUpDownMax.Maximum = ImageUtils.GetImagePixelCount(image);
            toolTip1.SetToolTip(numericUpDownMax, "Leave 0 to not specify an upper bound.");
            radioButtonOtsu.Checked = true;
            trackBar1.Enabled = false;

            splitContainer2.SplitterDistance = this.Height / 2;
            splitContainer1.IsSplitterFixed = true;

            trackBar1.ValueChanged -= trackBar1_ValueChanged;
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 255;
            trackBar1.Value = segmentationParameters.Threshold;
            labelThreshold.Text = segmentationParameters.Threshold.ToString();
            trackBar1.ValueChanged += trackBar1_ValueChanged;

            int[] channels = { 1, 2, 3 };
            comboBoxChannel.DataSource = channels;
            comboBoxChannel.SelectedIndex = segmentationParameters.Channel;
            comboBoxColorspace.DataSource = Enum.GetValues(typeof(ColorSpace));
            comboBoxColorspace.SelectedIndex = (int)segmentationParameters.Colorspace;

            pictureBoxOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxMask.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxOriginal.Image = image.Bitmap;

            buttonPreviousSlice.Enabled = false;
            labelFilename.Text = Path.GetFileNameWithoutExtension(selectedFile);
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if(mask != null)
            {
                mask.Dispose();
            }

            if(image != null)
            {
                Cursor.Current = Cursors.WaitCursor;

                SetSegmentationParameters();

                WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, segmentationParameters);
                segImage.Execute();
                mask = segImage.GetOutput().Clone();
                segImage.Dispose();

                pictureBoxMask.Image = mask.Bitmap;

                Cursor.Current = Cursors.Default;
            }
        }

        private void SetSegmentationParameters()
        {
            segmentationParameters = new SegmentationParameters();
            if (checkBoxContourSize.Checked)
            {
                segmentationParameters.ManualContourSizeRestriction = true;
                segmentationParameters.MinContourSize = (int)numericUpDownMin.Value;
                segmentationParameters.MaxContourSize = (int)numericUpDownMax.Value;
            }
            if (radioButtonThresholdManually.Checked)
            {
                segmentationParameters.UseOtsu = false;
                segmentationParameters.Threshold = trackBar1.Value;
            }
            segmentationParameters.Channel = comboBoxChannel.SelectedIndex;
            int index = comboBoxColorspace.SelectedIndex;
            segmentationParameters.Colorspace = (ColorSpace)index;
        }

        private void buttonSaveParameters_Click(object sender, EventArgs e)
        {
            SetSegmentationParameters();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkBoxContourSize_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownMin.Enabled = checkBoxContourSize.Checked;
            numericUpDownMax.Enabled = checkBoxContourSize.Checked;
        }

        private void radioButtonThresholdManually_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButtonThresholdManually.Checked)
            {
                trackBar1.Enabled = true;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            labelThreshold.Text = trackBar1.Value.ToString();
        }

        private void buttonPreviousSlice_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int index = filenames.IndexOf(selectedFile) - 1;

            selectedFile = filenames.ElementAt(index);
            image.Dispose();
            image = ReadWriteUtils.ReadOpenCVImageFromFile<Bgr, byte>(selectedFile);

            ReloadForm();

            buttonPreviousSlice.Enabled = index > 0;
            buttonNextSlice.Enabled = index < filenames.Count - 1;
            Cursor.Current = Cursors.Default;
        }

        private void buttonNextSlice_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int index = filenames.IndexOf(selectedFile) + 1;

            selectedFile = filenames.ElementAt(index);
            image.Dispose();
            image = ReadWriteUtils.ReadOpenCVImageFromFile<Bgr, byte>(selectedFile);

            ReloadForm();

            buttonPreviousSlice.Enabled = index > 0;
            buttonNextSlice.Enabled = index < filenames.Count - 1;
            Cursor.Current = Cursors.Default;
        }

        private void ReloadForm()
        {
            numericUpDownMax.Maximum = ImageUtils.GetImagePixelCount(image);
            splitContainer2.SplitterDistance = this.Height / 2;
            splitContainer1.IsSplitterFixed = true;
            pictureBoxOriginal.Image = image.Bitmap;
            labelFilename.Text = Path.GetFileNameWithoutExtension(selectedFile);
        }

        private void buttonSaveMask_Click(object sender, EventArgs e)
        {
            if (mask != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = ApplicationContext.OutputPath;
                saveFileDialog.FileName = "mask_" + Path.GetFileNameWithoutExtension(selectedFile) + ".png";
                DialogResult result = saveFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string fn = saveFileDialog.FileName;
                    mask.Save(fn);
                    Cursor.Current = Cursors.Default;
                }
            }
        }
    }
}
