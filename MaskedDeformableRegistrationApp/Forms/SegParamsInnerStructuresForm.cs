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
    public partial class SegParamsInnerStructuresForm : Form
    {
        private Image<Bgr, byte> image = null;
        private Image<Gray, byte> mask = null;

        public SegmentationParameters segmentationParameters;

        public SegParamsInnerStructuresForm(Image<Bgr, byte> image, Image<Gray, byte> mask, SegmentationParameters parameters)
        {
            InitializeComponent();
            this.image = image;
            this.mask = mask;
            this.segmentationParameters = parameters;
        }

        private void SegmentationParameterForm_Load(object sender, EventArgs e)
        {
            radioButtonOtsu.Checked = true;

            splitContainer2.SplitterDistance = this.Height / 2;
            splitContainer3.SplitterDistance = splitContainer2.Width / 2;
            splitContainer4.SplitterDistance = splitContainer2.Width / 2;
            splitContainer1.IsSplitterFixed = true;

            comboBoxChannel.SelectedIndexChanged -= comboBoxChannel_SelectedIndexChanged;
            comboBoxColorspace.SelectedIndexChanged -= comboBoxChannel_SelectedIndexChanged;
            int[] channels = { 1, 2, 3 };
            comboBoxChannel.DataSource = channels;
            comboBoxChannel.SelectedIndex = segmentationParameters.Channel;
            comboBoxColorspace.DataSource = Enum.GetValues(typeof(ColorSpace));
            comboBoxColorspace.SelectedIndex = (int)segmentationParameters.Colorspace;
            comboBoxChannel.SelectedIndexChanged += comboBoxChannel_SelectedIndexChanged;
            comboBoxColorspace.SelectedIndexChanged += comboBoxChannel_SelectedIndexChanged;

            trackBar1.ValueChanged -= trackBar1_ValueChanged;
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 255;
            trackBar1.Value = segmentationParameters.Threshold;
            labelThreshold.Text = segmentationParameters.Threshold.ToString();
            trackBar1.ValueChanged += trackBar1_ValueChanged;

            radioButtonOtsu.CheckedChanged -= radioButtonOtsu_CheckedChanged;
            radioButtonThresholdManually.CheckedChanged -= radioButtonThresholdManually_CheckedChanged;
            radioButtonOtsu.Checked = segmentationParameters.UseOtsu;
            radioButtonThresholdManually.Checked = !segmentationParameters.UseOtsu;
            radioButtonOtsu.CheckedChanged += radioButtonOtsu_CheckedChanged;
            radioButtonThresholdManually.CheckedChanged += radioButtonThresholdManually_CheckedChanged;

            checkBoxKmeans.CheckedChanged -= checkBoxKmeans_CheckedChanged;
            checkBoxKmeans.Checked = segmentationParameters.UseKmeans;
            checkBoxKmeans.CheckedChanged += checkBoxKmeans_CheckedChanged;

            pictureBoxOriginal.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxColorChannel.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSegmentation1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSegmentation2.SizeMode = PictureBoxSizeMode.Zoom;

            pictureBoxOriginal.Image = image.Bitmap;
        }

        private void splitContainer2_SizeChanged(object sender, EventArgs e)
        {
            splitContainer2.SplitterDistance = this.Height / 2;
        }

        private void splitContainer3_SizeChanged(object sender, EventArgs e)
        {
            splitContainer3.SplitterDistance = splitContainer2.Width / 2;
        }

        private void splitContainer4_SizeChanged(object sender, EventArgs e)
        {
            splitContainer4.SplitterDistance = splitContainer2.Width / 2;
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if(image != null && mask != null)
            {
                Cursor.Current = Cursors.WaitCursor;

                InnerTissueSegmentation seg = new InnerTissueSegmentation(image.Clone(), mask.Clone(), segmentationParameters);
                seg.SetColorSpace(segmentationParameters.Colorspace);
                seg.SetChannel(segmentationParameters.Channel);
                seg.Execute();
                List<UMat> result = seg.GetOutput();
                UMat a = new UMat();
                result[0].Clone().ConvertTo(a, Emgu.CV.CvEnum.DepthType.Cv8U);
                UMat b = new UMat();
                result[1].Clone().ConvertTo(b, Emgu.CV.CvEnum.DepthType.Cv8U);
                seg.Dispose();

                Cursor.Current = Cursors.Default;

                if (pictureBoxSegmentation1.Image != null)
                {
                    this.Invoke(new MethodInvoker(delegate () {
                        pictureBoxSegmentation1.Image.Dispose();
                        pictureBoxSegmentation1.Image = null;
                    }));
                }
                pictureBoxSegmentation1.Image = a.Bitmap;

                if (pictureBoxSegmentation2.Image != null)
                {
                    this.Invoke(new MethodInvoker(delegate () {
                        pictureBoxSegmentation2.Image.Dispose();
                        pictureBoxSegmentation2.Image = null;
                    }));
                }
                pictureBoxSegmentation2.Image = b.Bitmap;
            }
        }

        private void buttonSaveParameters_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void comboBoxColorspace_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBoxColorspace.SelectedIndex;
            segmentationParameters.Colorspace = (ColorSpace)index;
        }

        private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            segmentationParameters.Channel = comboBoxChannel.SelectedIndex;
        }

        private void buttonPreviewColorSpace_Click(object sender, EventArgs e)
        {
            using(Image<Bgr, byte> copy = image.Clone())
            {
                var imageColorChannel = SegmentationUtils.GetColorChannelAsUMat(segmentationParameters.Colorspace, copy, segmentationParameters.Channel);
                if(imageColorChannel != null)
                {
                    if (pictureBoxColorChannel.Image != null)
                    {
                        this.Invoke(new MethodInvoker(delegate () {
                            pictureBoxColorChannel.Image.Dispose();
                            pictureBoxColorChannel.Image = null;
                        }));
                    }
                    pictureBoxColorChannel.Image = imageColorChannel.Bitmap;
                }
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            segmentationParameters.Threshold = trackBar1.Value;
            labelThreshold.Text = segmentationParameters.Threshold.ToString();
        }

        private void radioButtonOtsu_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb == this.radioButtonOtsu)
            {
                this.trackBar1.Enabled = false;
                segmentationParameters.UseOtsu = true;
            }
        }

        private void radioButtonThresholdManually_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb == this.radioButtonThresholdManually)
            {
                this.trackBar1.Enabled = true;
                segmentationParameters.UseOtsu = false;
            }
        }

        private void checkBoxKmeans_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            segmentationParameters.UseKmeans = cb.Checked;
        }
    }
}
