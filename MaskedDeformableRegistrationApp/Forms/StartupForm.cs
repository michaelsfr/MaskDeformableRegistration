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
    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
        }

        private void buttonWSIExtraction_Click(object sender, EventArgs e)
        {
            using(LoadWSIForm form = new LoadWSIForm())
            {
                this.Hide();
                form.ShowDialog();
            }
        }

        private void buttonExistingStack_Click(object sender, EventArgs e)
        {
            using(LoadStackForm form = new LoadStackForm())
            {
                this.Hide();
                form.ShowDialog();
            }
        }
    }
}
