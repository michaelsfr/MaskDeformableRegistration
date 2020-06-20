using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Forms
{
    public partial class EditParametersForm : Form
    {
        public sitk.ParameterMap Parametermap { get; set; }

        public EditParametersForm(sitk.ParameterMap map)
        {
            InitializeComponent();

            Parametermap = map;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string[] parameters = richTextBox1.Text.Split('\n');

            foreach(var param in parameters)
            {
                string[] splitted = param.Split(' ');
                sitk.VectorString vec = GetVectorFromString(param);
                if(vec != null)
                {
                    if (Parametermap.ContainsKey(splitted[0]))
                    {
                        Parametermap[splitted[0]] = vec;
                    }
                    else
                    {
                        Parametermap.Add(splitted[0], vec);
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private sitk.VectorString GetVectorFromString(string parameter)
        {
            try
            {
                string[] values = parameter.Split('(')[1].Split(')')[0].Split(';');

                sitk.VectorString vec = new sitk.VectorString();
                foreach (string value in values)
                {
                    value.Replace(" ", "");
                    vec.Add(value);
                }
                return vec;
            } catch (Exception)
            {
                return null;
            }
        }

        private void EditParametersForm_Load(object sender, EventArgs e)
        {
            foreach(var param in Parametermap.AsEnumerable())
            {
                string values = String.Join(";", param.Value.AsEnumerable());
                richTextBox1.AppendText(param.Key + " ( " + values + " )\n");
            }
        }
    }
}
