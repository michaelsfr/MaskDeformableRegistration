using MaskedDeformableRegistrationApp.Registration;
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
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Forms
{
    public partial class EvaluationForm : Form
    {
        private RegistrationParameters registrationParameters;

        private string filenameFixedPointSet;
        private string filenameMovingPointSet;

        string movingImage = null;

        public EvaluationForm(RegistrationParameters parameters)
        {
            registrationParameters = parameters;

            InitializeComponent();
        }

        private void EvaluationForm_Load(object sender, EventArgs e)
        {
            comboBoxMoving.DataSource = registrationParameters.TransformationParameterMap.Keys.ToList();
            comboBoxMoving.Select(0, 1);
        }

        private void buttonChooseFixedPS_Click(object sender, EventArgs e)
        {
            openFileDialogFixed.Filter = "Point set (*.pts) | *.pts";
            //openFileDialogFixed.InitialDirectory = 
            DialogResult result = openFileDialogFixed.ShowDialog();

            if(result == DialogResult.OK)
            {
                string filename = openFileDialogFixed.FileName;
                textBoxFixedPointSet.Text = filename;
                filenameFixedPointSet = filename;
            }
        }

        private void buttonChooseMovPS_Click(object sender, EventArgs e)
        {
            openFileDialogMoving.Filter = "Point set (*.pts) | *.pts";
            DialogResult result = openFileDialogMoving.ShowDialog();

            if(result == DialogResult.OK)
            {
                string filename = openFileDialogMoving.FileName;
                textBoxMovingPointSet.Text = filename;
                filenameMovingPointSet = filename;
            }
        }

        private void buttonCalcMetrics_Click(object sender, EventArgs e)
        {
            string key = comboBoxMoving.SelectedValue.ToString();
            sitk.VectorOfParameterMap map = registrationParameters.TransformationParameterMap[key];
            registrationParameters.FixedImagePointSetFilename = filenameFixedPointSet;

            VisualizationEvaluationUtils.TransfromPointSet(map, key, registrationParameters);
        }

    }
}
