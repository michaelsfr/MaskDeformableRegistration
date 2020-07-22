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
    public partial class BitmapViewer : Form
    {
        private Bitmap Bitmap { get; set; }

        public BitmapViewer(Bitmap bitmap)
        {
            InitializeComponent();

            this.Bitmap = bitmap;
        }

        private void BitmapViewer_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = Bitmap;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}
