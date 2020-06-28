using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
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
    }
}
