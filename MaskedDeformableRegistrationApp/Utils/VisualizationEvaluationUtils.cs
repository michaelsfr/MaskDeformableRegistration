using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using MaskedDeformableRegistrationApp.Registration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Utils
{
    public static class VisualizationEvaluationUtils
    {
        public static void ShowUMat(UMat mat, string text = "")
        {
            Image<Bgr, byte> toConv = mat.ToImage<Bgr, byte>();
            ImageViewer.Show(toConv, text);
        }

        public static sitk.Image GetTotalDifferenceImage(sitk.Image img01, sitk.Image img02)
        {
            // todo: split channels and do subtraction only for grayscale image?
            sitk.SubtractImageFilter subtractImageFilter = new sitk.SubtractImageFilter();
            sitk.Image imgResult = subtractImageFilter.Execute(img01, img02);
            return imgResult;
        }

        public static sitk.Image GetCheckerBoard(sitk.Image img01, sitk.Image img02, uint size = 0)
        {
            Console.WriteLine(string.Format("width: Img01 [{0}] - Img02 [{1}]", img01.GetWidth(), img02.GetWidth()));
            Console.WriteLine(string.Format("height: Img01 [{0}] - Img02 [{1}]", img01.GetHeight(), img02.GetHeight()));
            Console.WriteLine(string.Format("pixel type: Img01 [{0}] - Img02 [{1}]", img01.GetPixelIDTypeAsString(), img02.GetPixelIDTypeAsString()));       

            sitk.CastImageFilter castImageFilter = new sitk.CastImageFilter();
            castImageFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkVectorFloat32);
            sitk.Image castedImage = castImageFilter.Execute(img02);

            sitk.Image tempImage = ImageUtils.ResizeImage(castedImage, img01);

            sitk.CheckerBoardImageFilter checkerBoard = new sitk.CheckerBoardImageFilter();

            if (size != 0)
            {
                sitk.VectorUInt32 vec = new sitk.VectorUInt32();
                vec.Add(size);
                vec.Add(size);
                checkerBoard.SetCheckerPattern(vec);
            }

            sitk.Image result = checkerBoard.Execute(img01, tempImage);
            return result;
        }

        public static sitk.Image GetDisplacementFieldFromTransformation(sitk.Image image, sitk.Transform transform)
        {
            sitk.TransformToDisplacementFieldFilter filter = new sitk.TransformToDisplacementFieldFilter();
            filter.SetReferenceImage(image);
            filter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkUInt8);
            filter.SetOutputOrigin(image.GetOrigin());
            filter.SetOutputDirection(image.GetDirection());
            filter.SetOutputSpacing(image.GetSpacing());
            return filter.Execute(transform);
        }

        public static Bitmap DrawHistogram(double maxVal, int width, int height, byte[] histData)
        {
            Bitmap histo = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(histo);
            g.Clear(SystemColors.Window);
            Pen penGray = new Pen(Brushes.DarkGray);

            for (var i = 0; i < histData.GetLength(0); i++)
            {
                var val = (float)histData.GetValue(i);
                val = (float)(val * (maxVal != 0 ? height / maxVal : 0.0));

                Point s = new Point(i, height);
                Point e = new Point(i, height - (int)val);
                g.DrawLine(penGray, s, e);
            }
            return histo;
        }

        public static sitk.Image CreateLabelMapImage(sitk.Image img)
        {
            sitk.BinaryImageToLabelMapFilter binaryImageToLabel = new sitk.BinaryImageToLabelMapFilter();
            sitk.Image temp = binaryImageToLabel.Execute(img);

            sitk.LabelMapToLabelImageFilter labelMapToLabel = new sitk.LabelMapToLabelImageFilter();
            sitk.Image result = labelMapToLabel.Execute(temp);

            temp.Dispose();
            return result;
        }

        public static sitk.LabelStatisticsImageFilter GetLabelStatisticsForImage(sitk.Image img, sitk.Image labelImage)
        {
            sitk.LabelStatisticsImageFilter labelStatisticsImageFilter = new sitk.LabelStatisticsImageFilter();
            labelStatisticsImageFilter.Execute(img, labelImage);
            return labelStatisticsImageFilter;
        }

        public static string TransfromPointSet(sitk.VectorOfParameterMap transformParameters, string movImage, RegistrationParameters parameters)
        {
            sitk.Image movingImage = ReadWriteUtils.ReadITKImageFromFile(movImage , sitk.PixelIDValueEnum.sitkFloat32);
            sitk.TransformixImageFilter transformix = null;
            try
            {
                transformix = new sitk.TransformixImageFilter();
                transformix.SetTransformParameterMap(transformParameters);
                transformix.SetMovingImage(movingImage);
                transformix.SetFixedPointSetFileName(parameters.FixedImagePointSetFilename);
                transformix.SetOutputDirectory(ReadWriteUtils.GetOutputDirectory(parameters));
                sitk.Image image = transformix.Execute();
                string output = ReadWriteUtils.GetOutputDirectory(parameters) + "\\points.mhd";
                //ReadWriteUtils.WriteSitkImage(image, output);
                image.Dispose();
                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// Calculates the mean TRE between given landmarks.
        /// </summary>
        /// <param name="coords01">list of expected points</param>
        /// <param name="coords02">list of transformed points</param>
        /// <returns>mean TRE</returns>
        public static double CalculateMeanTargetRegistrationError(List<CoordPoint> coords01, List<CoordPoint> coords02)
        {
            if (IsCorrespondingList(coords01, coords02))
            {
                double sum = GetEuclideanSum(coords01, coords02);
                return (sum / coords01.Count);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Calculates the absolute TRE between given landmarks.
        /// </summary>
        /// <param name="coords01">list of expected points</param>
        /// <param name="coords02">list of transformed points</param>
        /// <returns>absolute TRE</returns>
        public static double CalculateAbsoluteTargetRegistrationError(List<CoordPoint> coords01, List<CoordPoint> coords02)
        {
            if(IsCorrespondingList(coords01, coords02))
            {
                double sum = GetEuclideanSum(coords01, coords02);
                return sum;
            }
            else
            {
                return 0;
            }
        }

        private static double GetEuclideanSum(List<CoordPoint> coords01, List<CoordPoint> coords02)
        {
            double sum = 0;
            for (int i = 0; i <= coords01.Count; i++)
            {
                CoordPoint p1 = coords01.ElementAt(i);
                CoordPoint p2 = coords02.ElementAt(i);
                sum += Math.Abs(EuclideanDistance(p1.X, p1.Y, p2.X, p2.Y));
            }

            return sum;
        }

        /// <summary>
        /// Method checks if list are not null, not empty and have the same length.
        /// </summary>
        /// <param name="coords01">list a</param>
        /// <param name="coords02">list b</param>
        /// <returns>true if lists are corresponding</returns>
        private static bool IsCorrespondingList(List<CoordPoint> coords01, List<CoordPoint> coords02)
        {
            if ((coords01 != null && coords01.Count > 0) || (coords02 == null && coords02.Count > 0))
            {
                if (coords01.Count == coords02.Count) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Calculatesthe euclidean distance for given pair of points.
        /// </summary>
        /// <param name="x1">x1</param>
        /// <param name="y1">y1</param>
        /// <param name="x2">x2</param>
        /// <param name="y2">y2</param>
        /// <returns>distance between point (x1,y1) and (x2,y2)</returns>
        private static double EuclideanDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
        }
    }

    public class CoordPoint
    {
        public CoordPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
