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
    public partial class SegmentationParameterForm : Form
    {
        private Image<Bgr, byte> image = null;
        private Image<Gray, byte> mask = null;

        private ColorSpace DefaultColorSpace { get; set; } = ColorSpace.HSV;
        private int Channel { get; set; } = 1;

        public SegmentationParameterForm(Image<Bgr, byte> image, Image<Gray, byte> mask)
        {
            InitializeComponent();
            this.image = image;
            this.mask = mask;
        }

        private void splitContainer2_SizeChanged(object sender, EventArgs e)
        {
            splitContainer2.SplitterDistance = this.Height / 3;
        }

        private void splitContainer3_SizeChanged(object sender, EventArgs e)
        {
            splitContainer3.SplitterDistance = this.Height / 3;
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if(image != null && mask != null)
            {
                InnerTissueSegmentation seg = new InnerTissueSegmentation(image.Clone(), mask.Clone());
                seg.SetColorSpace(DefaultColorSpace);
                seg.SetChannel(Channel);
                seg.Execute();
                List<UMat> result = seg.GetOutput();
                Bitmap a = result[0].Clone().Bitmap;
                Bitmap b = result[1].Clone().Bitmap;
                seg.Dispose();

                if(pictureBoxSegmentation1.Image != null)
                {
                    this.Invoke(new MethodInvoker(delegate () {
                        pictureBoxSegmentation1.Image.Dispose();
                        pictureBoxSegmentation1.Image = null;
                    }));
                }
                pictureBoxSegmentation1.Image = a;

                if (pictureBoxSegmentation2.Image != null)
                {
                    this.Invoke(new MethodInvoker(delegate () {
                        pictureBoxSegmentation2.Image.Dispose();
                        pictureBoxSegmentation2.Image = null;
                    }));
                }
                pictureBoxSegmentation2.Image = b;
            }
        }

        private void buttonSaveParameters_Click(object sender, EventArgs e)
        {
            
        }

        private void SegmentationParameterForm_Load(object sender, EventArgs e)
        {
            int[] channels = { 1, 2, 3 };
            comboBoxChannel.DataSource = channels;
            comboBoxChannel.SelectedIndex = Channel;
            comboBoxColorspace.DataSource = Enum.GetValues(typeof(ColorSpace));
            comboBoxColorspace.SelectedIndex = (int)DefaultColorSpace;

            pictureBoxOriginal.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxSegmentation1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxSegmentation2.SizeMode = PictureBoxSizeMode.StretchImage;

            Bitmap toDisplay = ImageUtils.ResizeImage(image.Bitmap, pictureBoxOriginal.Width, pictureBoxOriginal.Height);
            pictureBoxOriginal.Image = image.Bitmap;
        }

        private void comboBoxColorspace_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBoxColorspace.SelectedIndex;
            DefaultColorSpace = (ColorSpace)index;
        }

        private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Channel = comboBoxChannel.SelectedIndex;
        }
    }
}
