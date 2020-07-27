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

        /// <summary>
        /// Create a difference image of two given images for a specific color channel.
        /// </summary>
        /// <param name="img01">image 1</param>
        /// <param name="img02">image 2</param>
        /// <param name="channel">color channel</param>
        /// <returns>difference image</returns>
        public static sitk.Image GetTotalDifferenceImage(sitk.Image img01, sitk.Image img02, uint channel = 0)
        {
            sitk.VectorIndexSelectionCastImageFilter channelFilter = new sitk.VectorIndexSelectionCastImageFilter();
            channelFilter.SetIndex(channel);
            sitk.Image ch01 = channelFilter.Execute(img01);
            sitk.VectorIndexSelectionCastImageFilter channelFilter2 = new sitk.VectorIndexSelectionCastImageFilter();
            channelFilter2.SetIndex(channel);
            sitk.Image ch02 = channelFilter2.Execute(img02);
            sitk.SubtractImageFilter subtractImageFilter = new sitk.SubtractImageFilter();
            sitk.Image imgResult = subtractImageFilter.Execute(ch01, ch02);
            return imgResult;
        }

        /// <summary>
        /// Create a checkerboard image of two corresponding images. 
        /// </summary>
        /// <param name="img01">image 1</param>
        /// <param name="img02">image 2</param>
        /// <param name="size">grid size of the checker board</param>
        /// <returns>checker board image</returns>
        public static sitk.Image GetCheckerBoard(sitk.Image img01, sitk.Image img02, uint size = 0)
        {
            uint width = img01.GetWidth() > img02.GetWidth() ? img01.GetWidth() : img02.GetWidth();
            uint height = img01.GetHeight() > img02.GetHeight() ? img01.GetHeight() : img02.GetHeight();
            //Console.WriteLine(string.Format("width: Img01 [{0}] - Img02 [{1}]", img01.GetWidth(), img02.GetWidth()));
            //Console.WriteLine(string.Format("height: Img01 [{0}] - Img02 [{1}]", img01.GetHeight(), img02.GetHeight()));
            //Console.WriteLine(string.Format("pixel type: Img01 [{0}] - Img02 [{1}]", img01.GetPixelIDTypeAsString(), img02.GetPixelIDTypeAsString()));

            sitk.Image reference = ImageUtils.ResizeImage(img01, width, height, sitk.PixelIDValueEnum.sitkFloat32);
            sitk.Image transformed = ImageUtils.ResizeImage(img02, width, height, sitk.PixelIDValueEnum.sitkFloat32);

            sitk.CheckerBoardImageFilter checkerBoard = new sitk.CheckerBoardImageFilter();

            if (size != 0)
            {
                sitk.VectorUInt32 vec = new sitk.VectorUInt32();
                vec.Add(size);
                vec.Add(size);
                checkerBoard.SetCheckerPattern(vec);
            }

            sitk.Image temp = checkerBoard.Execute(reference, transformed);
            sitk.CastImageFilter castImageFilter = new sitk.CastImageFilter();
            castImageFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkUInt8);
            sitk.Image result = castImageFilter.Execute(temp);
            temp.Dispose();
            return result;
        }

        public static sitk.Image GetCheckerBoardV2(string fImg01, string fImg02, uint size = 0)
        {
            sitk.Image img01 = ReadWriteUtils.ReadITKImageFromFile(fImg01);
            sitk.Image img02 = ReadWriteUtils.ReadITKImageFromFile(fImg02);

            uint width = img01.GetWidth() > img02.GetWidth() ? img01.GetWidth() : img02.GetWidth();
            uint height = img01.GetHeight() > img02.GetHeight() ? img01.GetHeight() : img02.GetHeight();

            sitk.Image reference = ImageUtils.ResizeImage(img01, width, height);
            sitk.Image transformed = ImageUtils.ResizeImage(img02, width, height);

            sitk.CheckerBoardImageFilter checkerBoard = new sitk.CheckerBoardImageFilter();
            if (size != 0)
            {
                sitk.VectorUInt32 vec = new sitk.VectorUInt32();
                vec.Add(size);
                vec.Add(size);
                checkerBoard.SetCheckerPattern(vec);
            }
            sitk.Image result = checkerBoard.Execute(reference, transformed);
            img01.Dispose();
            img02.Dispose();
            reference.Dispose();
            transformed.Dispose();
            return result;
        }

        /// <summary>
        /// Create a displacement field image by the transformation parameters.
        /// (Save as .mhd afterwards)
        /// </summary>
        /// <param name="image">image</param>
        /// <param name="transform">transform parameters</param>
        /// <returns>displacement field image</returns>
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

        /// <summary>
        /// Draws a histogram as Bitmap
        /// </summary>
        /// <param name="maxVal">max value of intensity range</param>
        /// <param name="width">width of the bitmap</param>
        /// <param name="height">height of the bitmap</param>
        /// <param name="histData">histogram data from opencv</param>
        /// <returns>histogram</returns>
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

        /// <summary>
        /// Create a label map image for givben image.
        /// </summary>
        /// <param name="img">image</param>
        /// <returns>label map</returns>
        public static sitk.Image CreateLabelMapImage(sitk.Image img)
        {
            sitk.BinaryImageToLabelMapFilter binaryImageToLabel = new sitk.BinaryImageToLabelMapFilter();
            sitk.Image temp = binaryImageToLabel.Execute(img);

            sitk.LabelMapToLabelImageFilter labelMapToLabel = new sitk.LabelMapToLabelImageFilter();
            sitk.Image result = labelMapToLabel.Execute(temp);

            temp.Dispose();
            return result;
        }

        /// <summary>
        /// Get a label statistics filter for image.
        /// </summary>
        /// <param name="img">image</param>
        /// <param name="labelImage">label image</param>
        /// <returns>statistics filter</returns>
        public static sitk.LabelStatisticsImageFilter GetLabelStatisticsForImage(sitk.Image img, sitk.Image labelImage)
        {
            sitk.LabelStatisticsImageFilter labelStatisticsImageFilter = new sitk.LabelStatisticsImageFilter();
            labelStatisticsImageFilter.Execute(img, labelImage);
            return labelStatisticsImageFilter;
        }

        /// <summary>
        /// Get an overlap measure image filter for given masks.
        /// </summary>
        /// <param name="mask01">mask</param>
        /// <param name="mask02">corresponding mask</param>
        /// <returns>overlap meassure filter</returns>
        public static sitk.LabelOverlapMeasuresImageFilter GetOverlapImageFilter(sitk.Image mask01, sitk.Image mask02)
        {
            sitk.LabelOverlapMeasuresImageFilter overlapFilter = new sitk.LabelOverlapMeasuresImageFilter();
            overlapFilter.Execute(mask01, mask02);
            return overlapFilter;
        }

        /// <summary>
        /// Transform a pointset for given transform parameters.
        /// </summary>
        /// <param name="transformParameters">transform params</param>
        /// <param name="parameters">registration params</param>
        /// <returns>filename of transformed point set</returns>
        public static string TransfromPointSet(List<sitk.VectorOfParameterMap> transformParameters, RegistrationParameters parameters)
        {
            sitk.TransformixImageFilter transformix = null;
            try
            {
                transformix = new sitk.TransformixImageFilter();
                transformix.SetTransformParameterMap(transformParameters.First());
                if (transformParameters.Count > 1)
                {
                    for (int i = 1; i < transformParameters.Count; i++)
                    {
                        var vectorParameterMap = transformParameters[i];
                        foreach (var paramMap in vectorParameterMap.AsEnumerable())
                        {
                            transformix.AddTransformParameterMap(paramMap);
                        }
                    }
                }
                transformix.SetFixedPointSetFileName(parameters.MovingImagePointSetFilename);
                transformix.SetOutputDirectory(ReadWriteUtils.GetOutputDirectory(parameters));
                sitk.Image image = transformix.Execute();
                string output = ReadWriteUtils.GetOutputDirectory(parameters) + "\\outputpoints.txt";
                image.Dispose();
                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            } finally
            {
                transformix.Dispose();
            }
        }

        /// <summary>
        /// Returns a registration error object with tre / fle, std and absolute error.
        /// </summary>
        /// <param name="coordPoints01">point set</param>
        /// <param name="coordPoints02">corresponding point set</param>
        /// <returns>registration error object</returns>
        public static RegistrationError GetRegistrationError(List<CoordPoint> coordPoints01, List<CoordPoint> coordPoints02)
        {
            if (!IsCorrespondingList(coordPoints01, coordPoints02))
            {
                return null;
            }

            List<double> distances = new List<double>();
            for (int i = 0; i < coordPoints01.Count; i++)
            {
                double dist = EuclideanDistance(coordPoints01.ElementAt(i), coordPoints02.ElementAt(i));
                Console.WriteLine(string.Format("Distance between ({0},{1}) and ({2},{3}): {4}",
                    coordPoints01.ElementAt(i).X, coordPoints01.ElementAt(i).Y,
                    coordPoints02.ElementAt(i).X, coordPoints02.ElementAt(i).Y, dist));
                distances.Add(dist);
            }

            RegistrationError result = new RegistrationError();
            result.AbsoluteRegistrationError = distances.Sum(val => Math.Abs(val));
            result.MaximumRegistrationError = distances.Max();
            result.MeanRegistrationError = result.AbsoluteRegistrationError / distances.Count;
            double sumOfSquares = distances.Select(val => Math.Pow((Math.Abs(val) - result.MeanRegistrationError), 2)).Sum();
            Console.WriteLine(sumOfSquares);
            result.StdDevRegistrationError = Math.Sqrt(sumOfSquares / distances.Count);

            return result;
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
            for (int i = 0; i < coords01.Count; i++)
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
        /// Calculate euclidean distance for a pair od coordinates.
        /// </summary>
        /// <param name="pt01">first coordinate pair</param>
        /// <param name="pt02">second coordinate pair</param>
        /// <returns>euclidean distance of given points</returns>
        private static double EuclideanDistance(CoordPoint pt01, CoordPoint pt02)
        {
            return Math.Sqrt((Math.Pow((pt01.X - pt02.X), 2) + Math.Pow((pt01.Y - pt02.Y), 2)));
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

    /// <summary>
    /// Class representing the target / fiducial registration error.
    /// </summary>
    public class RegistrationError
    {
        public RegistrationError() { }

        public RegistrationError(double meanRE, double stdRE, double maxRE, double absRE)
        {
            MeanRegistrationError = meanRE;
            StdDevRegistrationError = stdRE;
            MaximumRegistrationError = maxRE;
            AbsoluteRegistrationError = absRE;
        }

        public double MeanRegistrationError;
        public double StdDevRegistrationError;
        public double MaximumRegistrationError;
        public double AbsoluteRegistrationError;

        public override string ToString()
        {
            return string.Format("Mean Registration Error: [{0}] \nStdDevRegistrationError: [{1}] \nMaximumRegistrationError: [{2}] \nAbsoluteRegistrationError: [{3}]",
                MeanRegistrationError, StdDevRegistrationError, MaximumRegistrationError, AbsoluteRegistrationError);
        }
    }
}
