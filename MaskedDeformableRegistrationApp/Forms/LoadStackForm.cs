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
    public partial class LoadStackForm : Form
    {
        private List<string> Filenames { get; set; } = new List<string>();
        private string Stackname { get; set; } = null;

        private const string _dialogFilterStack = "json files (*.json)|*.json";
        private const string _dialogFilterImages = "image files (*.png)|*.png|(*.mhd)|*.mhd|(*.tif)|*.tif|(*.bmp)|*.bmp";

        private readonly string _initDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public LoadStackForm()
        {
            InitializeComponent();
        }

        private void radioButtonLoadStack_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null)
            {
                if(rb == this.radioButtonLoadStack)
                {
                    this.textBox1.Enabled = true;
                    this.buttonChooseStack.Enabled = true;
                    this.textBox2.Enabled = false;
                    this.buttonChoosePNGs.Enabled = false;
                }
            }
        }

        private void radioButtonLoadPNGs_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null)
            {
                if (rb == this.radioButtonLoadPNGs)
                {
                    this.textBox1.Enabled = false;
                    this.buttonChooseStack.Enabled = false;
                    this.textBox2.Enabled = true;
                    this.buttonChoosePNGs.Enabled = true;
                }
            }
        }

        private void buttonChooseStack_Click(object sender, EventArgs e)
        {
            if(Filenames.Count > 0)
            {
                Filenames.Clear();
            }

            openFileDialogStack.Filter = _dialogFilterStack;
            openFileDialogStack.InitialDirectory = _initDir;
            openFileDialogStack.FileName = "";
            DialogResult result = openFileDialogStack.ShowDialog();

            if (result == DialogResult.OK)
            {
                Stackname = openFileDialogStack.FileName;
                if(Stackname != null)
                {
                    textBox1.Text = Stackname;

                    WSIExtraction.Stack stack = ReadWriteUtils.DeserializeObjectFromJSON<WSIExtraction.Stack>(Stackname);
                    foreach (WSIExtraction.Slice slice in stack.Section)
                    {
                        Filenames.Add(Path.Combine(Path.GetDirectoryName(Stackname), slice.Path));
                    }
                }
                textBox1.Text = Stackname;
                EnableButtons();
            }
        }

        private void buttonChoosePNGs_Click(object sender, EventArgs e)
        {
            if (Filenames.Count > 0)
            {
                Filenames.Clear();
            }

            openFileDialogImages.Multiselect = true;
            openFileDialogImages.Filter = _dialogFilterImages;
            openFileDialogImages.InitialDirectory = _initDir;
            openFileDialogImages.FileName = "";
            DialogResult result = openFileDialogImages.ShowDialog();

            if (result == DialogResult.OK)
            {
                foreach (string path in openFileDialogImages.FileNames)
                {
                    Filenames.Add(path);
                }
            }
            textBox2.Text = string.Join("; ", Filenames);
            EnableButtons();
        }

        private void EnableButtons()
        {
            this.buttonProceed.Enabled = textBox1.Text != "" || textBox2.Text != "";
        }

        private void buttonProceed_Click(object sender, EventArgs e)
        {
            using(RegistrationForm form = new RegistrationForm(Filenames))
            {
                this.Hide();
                form.ShowDialog();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadStackForm_Load(object sender, EventArgs e)
        {
            this.buttonProceed.Enabled = false;
        }

        private void LoadStackForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            Application.Exit();
        }
    }
}
