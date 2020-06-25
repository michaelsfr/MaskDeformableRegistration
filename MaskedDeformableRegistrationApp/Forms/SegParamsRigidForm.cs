using Emgu.CV;
using Emgu.CV.Structure;
using MaskedDeformableRegistrationApp.Segmentation;
using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaskedDeformableRegistrationApp.Forms
{
    public partial class SegParamsRigidForm : Form
    {
        public SegmentationParameters segmentationParameters;

        private Image<Bgr, byte> image;

        public SegParamsRigidForm(Image<Bgr, byte> image, SegmentationParameters parameters)
        {
            InitializeComponent();

            this.segmentationParameters = parameters;
            this.image = image;

            InitializeButtons();
        }

        private void InitializeButtons()
        {
            textBoxMin.Enabled = false;
            textBoxMax.Enabled = false;
            radioButtonOtsu.Checked = true;
            trackBar1.Enabled = false;

            splitContainer2.SplitterDistance = this.Height / 2;
            splitContainer1.IsSplitterFixed = true;

            trackBar1.Minimum = 0;
            trackBar1.Maximum = 255;
            trackBar1.Value = segmentationParameters.Threshold;
            labelThreshold.Text = segmentationParameters.Threshold.ToString();

            int[] channels = { 1, 2, 3 };
            comboBoxChannel.DataSource = channels;
            comboBoxChannel.SelectedIndex = segmentationParameters.Channel;
            comboBoxColorspace.DataSource = Enum.GetValues(typeof(ColorSpace));
            comboBoxColorspace.SelectedIndex = (int)segmentationParameters.Colorspace;

            pictureBoxOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxMask.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxOriginal.Image = image.Bitmap;
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if(image != null)
            {
                WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, ImageUtils.GetPercentualImagePixelCount(image, 0.3f));
                segImage.Execute();
                Image<Gray, byte> mask = segImage.GetOutput().Clone();
                segImage.Dispose();

                pictureBoxMask.Image = mask.Bitmap;
            }
        }

        private void buttonSaveParameters_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void checkBoxContourSize_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxContourSize.Checked)
            {
                textBoxMin.Enabled = true;
                textBoxMax.Enabled = true;
            }
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
    }
}
