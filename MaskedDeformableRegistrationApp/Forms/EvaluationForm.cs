using MaskedDeformableRegistrationApp.Registration;
using MaskedDeformableRegistrationApp.Segmentation;
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
            InitializeComboBoxes();
        }

        private void InitializeComboBoxes()
        {
            var comboBoxSource = registrationParameters.TransformationParameterMap.Keys.Select(key => new KeyValuePair<string, string>(key, Path.GetFileName(key))).ToList();
            comboBoxMoving.DataSource = comboBoxSource;
            comboBoxMoving.Select(0, 1);
            comboBoxMoving.DisplayMember = "value";
            comboBoxMoving.ValueMember = "key";
            comboBoxMovingImage.DataSource = comboBoxSource;
            comboBoxMovingImage.Select(0, 1);
            comboBoxMovingImage.DisplayMember = "value";
            comboBoxMovingImage.ValueMember = "key";
            comboBoxSegmentationParams.Items.Insert(0, "Whole Particle Segmentation");
            comboBoxSegmentationParams.Items.Insert(1, "Inner Structure Segmentation");
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

            string filenameOutputPoints = VisualizationEvaluationUtils.TransfromPointSet(map, key, registrationParameters);
            var tuple = ReadWriteUtils.ReadFixedAndTransformedPointSets(filenameOutputPoints);

            double absTRE = VisualizationEvaluationUtils.CalculateAbsoluteTargetRegistrationError(tuple.Item1, tuple.Item2);
            double meanTRE = VisualizationEvaluationUtils.CalculateMeanTargetRegistrationError(tuple.Item1, tuple.Item2);

            labelAbsDiff.Text = absTRE.ToString();
            labelMeanDiff.Text = meanTRE.ToString();
        }

        private void buttonCalcCoef_Click(object sender, EventArgs e)
        {
            string fixedImageFilename = registrationParameters.FixedImageFilename;
            string movingImageFilename = comboBoxMovingImage.SelectedValue.ToString();
            bool isInnerSeg = comboBoxSegmentationParams.SelectedIndex == 1;

            // read images
            var img01 = ReadWriteUtils.ReadOpenCVImageFromFile(fixedImageFilename);
            var img02 = ReadWriteUtils.ReadOpenCVImageFromFile(movingImageFilename);

            // whole particle seg
            WholeTissueSegmentation seg01 = new WholeTissueSegmentation(img01, registrationParameters.WholeTissueSeg);
            seg01.Execute();
            var mask01w = seg01.GetOutput().Clone();
            seg01.Dispose();
            string mask01wFn = registrationParameters.OutputDirectory + "\\mask01w.png";
            ReadWriteUtils.WriteUMatToFile(mask01wFn, mask01w.ToUMat());

            WholeTissueSegmentation seg02 = new WholeTissueSegmentation(img02, registrationParameters.WholeTissueSeg);
            seg02.Execute();
            var mask02w = seg02.GetOutput().Clone();
            seg02.Dispose();
            string mask02wFn = registrationParameters.OutputDirectory + "\\mask02w.png";
            ReadWriteUtils.WriteUMatToFile(mask02wFn, mask02w.ToUMat());

            sitk.LabelOverlapMeasuresImageFilter overlapFilter = null;
            if (isInnerSeg)
            {
                // inner structure seg


            } else
            {
                sitk.Image sImg01 = ReadWriteUtils.ReadITKImageFromFile(mask01wFn);
                sitk.Image sImg02 = ReadWriteUtils.ReadITKImageFromFile(mask02wFn);
                overlapFilter = VisualizationEvaluationUtils.GetOverlapImageFilter(sImg01, sImg02);

                Console.WriteLine("Dice coefficient: " + overlapFilter.GetDiceCoefficient());
                Console.WriteLine("False negative error: " + overlapFilter.GetFalseNegativeError());
                Console.WriteLine("False positive error: " + overlapFilter.GetFalsePositiveError());
                Console.WriteLine("JaccaFrd coefficient: " + overlapFilter.GetJaccardCoefficient());
                Console.WriteLine("Mean overlap: " + overlapFilter.GetMeanOverlap());
            }
        }

        private void buttonDiffImage_Click(object sender, EventArgs e)
        {
            string fixedImageFilename = registrationParameters.FixedImageFilename;
            string movingImageFilename = comboBoxMovingImage.SelectedValue.ToString();

            // read images
            var img01 = ReadWriteUtils.ReadITKImageFromFile(fixedImageFilename);
            var img02 = ReadWriteUtils.ReadITKImageFromFile(movingImageFilename);
            var difference = VisualizationEvaluationUtils.GetTotalDifferenceImage(img01, img02);
            ReadWriteUtils.WriteSitkImage(difference, registrationParameters.OutputDirectory + "\\difference.png");
        }

        private void buttonCheckerBoard_Click(object sender, EventArgs e)
        {
            string fixedImageFilename = registrationParameters.FixedImageFilename;
            string movingImageFilename = comboBoxMovingImage.SelectedValue.ToString();

            // read images
            var img01 = ReadWriteUtils.ReadITKImageFromFile(fixedImageFilename);
            var img02 = ReadWriteUtils.ReadITKImageFromFile(movingImageFilename);
            var checkerboard = VisualizationEvaluationUtils.GetCheckerBoard(img01, img02);
            ReadWriteUtils.WriteSitkImage(checkerboard, registrationParameters.OutputDirectory + "\\checkerboard.png");
        }
    }
}
