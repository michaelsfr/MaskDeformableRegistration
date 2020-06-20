using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaskedDeformableRegistrationApp.Forms
{
    public partial class WSIHistoObject : UserControl
    {
        public string wsiImagePath;  // path of the source WSI
        public Rectangle objRectangle;  // bbox in WSI coordinates
        public int NrOfRotations { get; set; } = 0;  // how often the image has been rotated by 90 degrees CW
        public Boolean HorizontalFlip { get; set; } = false;
        public int resolutionLevel = 0;  // on what resolution level the object has been segmented

        public WSIHistoObject()
        {
            InitializeComponent();
        }

        public void PictureBox_Rotate90FlipNone()
        {
            Image flipImage = this.pictureBox1.Image;
            flipImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            this.pictureBox1.Image = flipImage;
            NrOfRotations++;
        }

        public void PictureBox_FlipHorizontal()
        {
            Image flipImage = this.pictureBox1.Image;
            flipImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            this.pictureBox1.Image = flipImage;
            if (HorizontalFlip == false)
                HorizontalFlip = true;
            else
                HorizontalFlip = false;
        }

        /// <summary>
        /// set thumbnail image
        /// </summary>
        /// <param name="img">source image that is used for creating the thumbnail</param>
        /// <param name="resolutionLevel">on what resolution level the object has been segmented</param>
        public void PictureBox_SetImage(Image img, int resolutionLevel)
        {
            // save pointer to old image so it can disposed afterwards
            Image oldImage = this.pictureBox1.Image;
            // resize image to save memory and set picturebox image
            //double ratioWidth = (double)img.Width / (double)img.Height;
            //double ratioHeight = (double)img.Height / (double)img.Width;
            //int newWidth = img.Width > img.Height ? pictureBox1.Width : (int)(ratioWidth * pictureBox1.Width);
            //int newHeight = img.Width > img.Height ? (int)(ratioHeight * pictureBox1.Height) : pictureBox1.Height;
            //double thumbnailFactor = (double)img.Width / this.pictureBox1.Size.Width;
            //this.pictureBox1.Image = new Bitmap(img, new Size(newWidth, newHeight));
            double thumbnailFactor = (double)img.Width / this.pictureBox1.Size.Width;
            this.pictureBox1.Image = new Bitmap(img,
                new Size((int)((double)img.Width / thumbnailFactor), (int)((double)img.Height / thumbnailFactor)));
            // dispose old image
            if (oldImage != null) oldImage.Dispose();
            this.NrOfRotations = 0;
            this.resolutionLevel = resolutionLevel;
        }

        private void buttonAdaptSize_Click(object sender, EventArgs e)
        {
            // TODO
            /*FormWSIBoundingBoxSelector frm = new FormWSIBoundingBoxSelector(this.wsiImagePath, this.objRectangle);
            // show dialog to correct the bbox
            DialogResult res = frm.ShowDialog();
            if (res == DialogResult.OK)
            {
                // get selected rectangle
                this.objRectangle = frm.GetSelectedRectangle();
                try
                {
                    // update thumbnail
                    using (VMscope.InteropCore.ImageStreaming.IStreamingImage image = VMscope.VirtualSlideAccess.Sdk.GetImage(wsiImagePath))
                    {
                        int layerFactor = (int)(Math.Pow(2.0, (double)this.resolutionLevel));
                        Bitmap bmp = image.GetImagePart(this.objRectangle.X, this.objRectangle.Y, this.objRectangle.Width, this.objRectangle.Height,
                            (int)(this.objRectangle.Width / layerFactor), (int)(this.objRectangle.Height / layerFactor));
                        PictureBox_SetImage(bmp, this.resolutionLevel);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }*/
        }

        private void buttonRotate_Click(object sender, EventArgs e)
        {
            this.PictureBox_Rotate90FlipNone();
        }

        private void buttonFlip_Click(object sender, EventArgs e)
        {
            this.PictureBox_FlipHorizontal();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            // TODO
        }

        /// <summary>
        /// add DragDrop handler
        /// </summary>
        /// <param name="handler">handler to add</param>
        public void PictureBox_AddDragDropHandler(DragEventHandler handler)
        {
            this.pictureBox1.DragDrop += handler;
        }

        /// <summary>
        /// add DragOver handler
        /// </summary>
        /// <param name="handler">handler object to add</param>
        public void PictureBox_AddDragOverHandler(DragEventHandler handler)
        {
            this.pictureBox1.DragOver += handler;
        }

        /// <summary>
        /// add MouseDown handler
        /// </summary>
        /// <param name="handler">handler object to add</param>
        public void PictureBox_AddMouseDownHandler(MouseEventHandler handler)
        {
            this.pictureBox1.MouseDown += handler;
        }
    }
}
