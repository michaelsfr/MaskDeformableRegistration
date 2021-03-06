﻿using Emgu.CV;
using Emgu.CV.Structure;
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
            numericUpDownCheckerSize.Minimum = 1;
            numericUpDownCheckerSize.Maximum = 1000;
            numericUpDownCheckerSize.Value = 50;
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
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string key = comboBoxMoving.SelectedValue.ToString();
                List<sitk.VectorOfParameterMap> map = registrationParameters.TransformationParameterMap[key];
                /*string parametersFilename = registrationParameters.OutputDirectory + "\\1\\TransformParameters.0.txt";
                List<sitk.VectorOfParameterMap> map = new List<sitk.VectorOfParameterMap>();
                map.Add(InvertTransformParameters(parametersFilename));*/

                registrationParameters.MovingImagePointSetFilename = filenameMovingPointSet;
                registrationParameters.FixedImagePointSetFilename = filenameFixedPointSet;

                string filenameOutputPoints = VisualizationEvaluationUtils.TransfromPointSet(map, registrationParameters);
                var movingPointsDict = ReadWriteUtils.ReadFixedPointSet(filenameMovingPointSet).Values.ToList();
                var transformedPointsDict = ReadWriteUtils.ReadTransformedPointSets(filenameOutputPoints).Values.ToList();

                RegistrationError registrationError = VisualizationEvaluationUtils.GetRegistrationError(movingPointsDict, transformedPointsDict);

                labelMeanDiff.Text = registrationError.MeanRegistrationError.ToString("0.###");
                labelStdDev.Text = registrationError.StdDevRegistrationError.ToString("0.###");
                labelMax.Text = registrationError.MaximumRegistrationError.ToString("0.###");
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            } 

            Cursor.Current = Cursors.Default;
        }

        private sitk.VectorOfParameterMap InvertTransformParameters(string parameterFilename)
        {
            sitk.Image fixedImage = ReadWriteUtils.ReadITKImageFromFile(registrationParameters.FixedImageFilename, sitk.PixelIDValueEnum.sitkFloat32);
            sitk.ElastixImageFilter elastix = null;
            try
            {
                // elastix manual 6.1.6: DisplacementMagnitudePenalty
                elastix = new sitk.ElastixImageFilter();
                elastix.SetInitialTransformParameterFileName(parameterFilename);
                elastix.SetParameterMap(sitk.SimpleITK.GetDefaultParameterMap("rigid"));
                elastix.SetFixedImage(fixedImage);
                elastix.SetMovingImage(fixedImage);
                elastix.SetParameter("HowToCombineTransforms", "Compose");
                elastix.SetParameter("Metric", "DisplacementMagnitudePenalty");
                elastix.SetParameter("NumberOfResolutions", "1");
                elastix.Execute();
                return elastix.GetTransformParameterMap();

                /*sitk.TransformixImageFilter transformix = new sitk.TransformixImageFilter();
                transformix.SetTransformParameterMap(elastix.GetTransformParameterMap());
                transformix.SetTransformParameter("InitialTransformParametersFileName", "NoInitialTransform");
                transformix.Execute();*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            } finally
            {
                elastix.Dispose();
                fixedImage.Dispose();
            }
        }

        private void TransformPointSet(sitk.Transform transform, ref List<CoordPoint> pointSetDict)
        {
            if(transform != null)
            {
                foreach(CoordPoint point in pointSetDict)
                {
                    sitk.VectorDouble vec = new sitk.VectorDouble();
                    vec.Add(point.X);
                    vec.Add(point.Y);
                    vec.Add(0);
                    sitk.VectorDouble resultVec = transform.TransformPoint(vec);
                    point.X = resultVec[0];
                    point.Y = resultVec[1];
                }
            }
        }

        private void buttonCalcCoef_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string fixedImageFilename = registrationParameters.FixedImageFilename;
            string movingImageFilename = comboBoxMovingImage.SelectedValue.ToString();
            bool isInnerSeg = comboBoxSegmentationParams.SelectedIndex == 1;

            // read images
            Image<Bgr, byte> img01 = ReadWriteUtils.ReadOpenCVImageFromFile<Bgr, byte>(fixedImageFilename);
            Image<Bgr, byte> img02 = ReadWriteUtils.ReadOpenCVImageFromFile<Bgr, byte>(movingImageFilename);

            // whole particle seg
            Image<Gray, byte> mask01w;
            string mask01wFn = DoWholeTissueSegmentation(img01, out mask01w, "\\mask01w.png");

            Image<Gray, byte> mask02w;
            string mask02wFn = DoWholeTissueSegmentation(img02, out mask02w, "\\mask02w.png");

            sitk.LabelOverlapMeasuresImageFilter overlapFilter = null;
            if (isInnerSeg)
            {
                string mask01iFn = DoInnerTissueSegmentation(img01, mask01w, "\\mask01i.png");
                string mask02iFn = DoInnerTissueSegmentation(img02, mask02w, "\\mask02i.png");

                overlapFilter = GetOverlapImageFilter(mask01iFn, mask02iFn);

                DeleteFile(mask01iFn);
                DeleteFile(mask02iFn);
            }
            else
            {
                overlapFilter = GetOverlapImageFilter(mask01wFn, mask02wFn);
            }

            if (overlapFilter != null)
            {
                double diceCoef = overlapFilter.GetDiceCoefficient();
                double falseNegative = overlapFilter.GetFalseNegativeError();
                double falsePositive = overlapFilter.GetFalsePositiveError();
                double jaccard = overlapFilter.GetJaccardCoefficient();
                double meanOverlap = overlapFilter.GetMeanOverlap();
                double unionOverlap = overlapFilter.GetUnionOverlap();

                labelDice.Text = diceCoef.ToString("0.###");
                labelJacc.Text = jaccard.ToString("0.###");
                labelfalseNegPos.Text = string.Format("{0} / {1}", falseNegative.ToString("0.###"), falsePositive.ToString("0.###"));
                labelMeanOverlap.Text = meanOverlap.ToString("0.###");
                labelUnionOverlap.Text = unionOverlap.ToString("0.###");
            }

            DeleteFile(mask01wFn);
            DeleteFile(mask02wFn);

            Cursor.Current = Cursors.Default;
        }

        private string DoWholeTissueSegmentation(Image<Bgr, byte> img, out Image<Gray, byte> mask, string filename)
        {
            WholeTissueSegmentation seg = new WholeTissueSegmentation(img, registrationParameters.WholeTissueSegParams);
            seg.Execute();
            mask = seg.GetOutput().Clone();
            seg.Dispose();
            string filepathMask = registrationParameters.OutputDirectory + filename;
            ReadWriteUtils.WriteUMatToFile(filepathMask, mask.ToUMat());
            return filepathMask;
        }

        private string DoInnerTissueSegmentation(Image<Bgr, byte> img, Image<Gray, byte> mask, string filename)
        {
            InnerTissueSegmentation seg = new InnerTissueSegmentation(img.Clone(), mask.Clone(), registrationParameters.InnerStructuresSegParams);
            seg.Execute();
            UMat innerMask = seg.GetOutput()[0].Clone();
            seg.Dispose();
            string filepathResult = registrationParameters.OutputDirectory + filename;
            ReadWriteUtils.WriteUMatToFile(filepathResult, innerMask);
            return filepathResult;
        }

        private static sitk.LabelOverlapMeasuresImageFilter GetOverlapImageFilter(string mask01iFn, string mask02iFn)
        {
            sitk.LabelOverlapMeasuresImageFilter overlapFilter;
            sitk.Image sImg01 = ReadWriteUtils.ReadITKImageFromFile(mask01iFn);
            sitk.Image sImg02 = ReadWriteUtils.ReadITKImageFromFile(mask02iFn);
            overlapFilter = VisualizationEvaluationUtils.GetOverlapImageFilter(sImg01, sImg02);
            sImg01.Dispose();
            sImg02.Dispose();
            return overlapFilter;
        }

        private void DeleteFile(string filename)
        {
            if(File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        private void buttonDiffImage_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string fixedImageFilename = registrationParameters.FixedImageFilename;
            string movingImageFilename = comboBoxMovingImage.SelectedValue.ToString();

            // read images
            var img01 = ReadWriteUtils.ReadITKImageFromFile(fixedImageFilename);
            var img02 = ReadWriteUtils.ReadITKImageFromFile(movingImageFilename);
            var difference = VisualizationEvaluationUtils.GetTotalDifferenceImage(img01, img02);
            ReadWriteUtils.WriteSitkImage(difference, registrationParameters.OutputDirectory + "\\difference.png");
            img01.Dispose();
            img02.Dispose();
            difference.Dispose();

            Cursor.Current = Cursors.Default;
        }

        private void buttonCheckerBoard_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string fixedImageFilename = registrationParameters.FixedImageFilename;
            string movingImageFilename = comboBoxMovingImage.SelectedValue.ToString();
            uint size = (uint)numericUpDownCheckerSize.Value;

            // read images
            //var img01 = ReadWriteUtils.ReadITKImageFromFile(fixedImageFilename);
            //var img02 = ReadWriteUtils.ReadITKImageFromFile(movingImageFilename);
            var checkerboard = VisualizationEvaluationUtils.GetCheckerBoardV2(fixedImageFilename, movingImageFilename, size);
            ReadWriteUtils.WriteSitkImage(checkerboard, registrationParameters.OutputDirectory + "\\checkerboard.png");
            //img01.Dispose();
            //img02.Dispose();
            checkerboard.Dispose();

            Cursor.Current = Cursors.Default;
        }

        private void buttonTransformFixed_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string key = comboBoxMoving.SelectedValue.ToString();
                List<sitk.VectorOfParameterMap> map = registrationParameters.TransformationParameterMap[key];

                VisualizationEvaluationUtils.TransfromPointSet(map, filenameFixedPointSet, ApplicationContext.OutputPath);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            } 

            Cursor.Current = Cursors.Default;
        }
    }
}
