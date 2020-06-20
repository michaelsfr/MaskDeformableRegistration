using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using MaskedDeformableRegistrationApp.Utils;

namespace MaskedDeformableRegistrationApp.Segmentation
{
    class WholeTissueSegmentation : ISegmentation<Image<Gray, byte>>
    {
        private Image<Bgr, byte> image;
        private Image<Gray, byte> mask;

        private int maxContourSize;

        public WholeTissueSegmentation(Image<Bgr, byte> image, int maxContourSize)
        {
            Console.WriteLine("Max contour size: " + maxContourSize);
            this.image = image.Clone();
            this.maxContourSize = maxContourSize;
        }

        public void Execute()
        {
            if(image != null)
            {
                UMat temp = SegmentationUtils.IncreaseContrast(image.ToUMat(), 5.0, 8);
                temp = SegmentationUtils.Blur(temp, 3, 1.0);
                temp = SegmentationUtils.Sharp(temp);
                //UMat h = SegmentationUtils.GetColorChannel(temp, ColorConversion.Bgr2Hsv, 0);
                //UMat s = SegmentationUtils.GetColorChannel(temp, ColorConversion.Bgr2Hsv, 1);
                UMat v = SegmentationUtils.GetColorChannel(temp, ColorConversion.Bgr2Hsv, 2);

                //UMat otsu_h = SegmentationUtils.ThresholdOtsu(h);
                //UMat otsu_s = SegmentationUtils.ThresholdOtsu(s);
                UMat otsu_v = SegmentationUtils.ThresholdOtsu(v);
                CvInvoke.BitwiseNot(otsu_v, otsu_v);

                UMat closed = SegmentationUtils.Closing(otsu_v, 5);

                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                VectorOfVectorOfPoint contoursRelevant = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(closed, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

                for(int i = 0; i < contours.Size; i++)
                {
                    double area = CvInvoke.ContourArea(contours[i]);
                    // todo: how to determine biggest contour/s // throw away contours that are not relevant
                    //if(area > maxContourSize)
                    //{
                        Console.WriteLine(area);
                        contoursRelevant.Push(contours[i]);
                    //}
                }

                CvInvoke.DrawContours(image, contoursRelevant, -1, new MCvScalar(0, 0, 255));

                mask = new Image<Gray, Byte>(image.Width, image.Height, new Gray(0.0));
                CvInvoke.DrawContours(mask, contoursRelevant, -1, new MCvScalar(255.0), thickness: -1);

                //ImageViewer.Show(mask, "V-channel");

            }
        }

        public Image<Gray, byte> GetOutput()
        {
            return mask;
        }

        public void Dispose()
        {
            image.Dispose();
            mask.Dispose();
        }

    }
}
