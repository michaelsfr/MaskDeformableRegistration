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
    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
        }

        private void buttonWSIExtraction_Click(object sender, EventArgs e)
        {
            LoadWSIForm form = new LoadWSIForm();
            this.Hide();
            form.ShowDialog();
        }

        private void buttonExistingStack_Click(object sender, EventArgs e)
        {
            LoadStackForm form = new LoadStackForm();
            this.Hide();
            form.ShowDialog();
        }

        private void EnableButtons()
        {
            if(textBox1.Text != null && textBox1.Text != "" && Directory.Exists(textBox1.Text))
            {
                bool dirExists = Directory.Exists(textBox1.Text);
                buttonExistingStack.Enabled = dirExists;
                buttonWSIExtraction.Enabled = dirExists;
            }
        }

        private void buttonChoosePath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyDocuments;

            if(result == DialogResult.OK)
            {
                if(Directory.Exists(folderBrowserDialog1.SelectedPath))
                {
                    ApplicationContext.OutputPath = folderBrowserDialog1.SelectedPath;
                    textBox1.Text = folderBrowserDialog1.SelectedPath;
                }
                EnableButtons();
            }
        }

        private void StartupForm_Load(object sender, EventArgs e)
        {
            buttonExistingStack.Enabled = false;
            buttonWSIExtraction.Enabled = false;
        }
    }
}
