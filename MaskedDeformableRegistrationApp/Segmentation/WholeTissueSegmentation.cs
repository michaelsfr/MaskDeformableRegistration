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

        private SegmentationParameters segmentationParameters;

        public WholeTissueSegmentation(Image<Bgr, byte> image, SegmentationParameters parameters)
        { 
            this.image = image.Clone();
            this.segmentationParameters = parameters;
        }

        public void Execute()
        {
            if(image != null)
            {
                UMat temp = SegmentationUtils.IncreaseContrast(image.ToUMat(), 5.0, 8);
                temp = SegmentationUtils.Blur(temp, 3, 1.0);
                temp = SegmentationUtils.Sharp(temp);
                Image<Bgr, byte> tempImage = new Image<Bgr, byte>(temp.Bitmap);
                UMat uMatChannel = SegmentationUtils.GetColorChannelAsUMat(segmentationParameters.Colorspace, tempImage, segmentationParameters.Channel);

                UMat thresholded = null;
                using (UMat copy = uMatChannel.Clone())
                {
                    if (segmentationParameters.UseOtsu)
                    {
                        thresholded = SegmentationUtils.ThresholdOtsu(copy);
                    }
                    else
                    {
                        thresholded = SegmentationUtils.Threshold(copy, segmentationParameters.Threshold);
                    }
                }

                CvInvoke.BitwiseNot(thresholded, thresholded);

                UMat closed = SegmentationUtils.Closing(thresholded, 5);
                UMat dilated = SegmentationUtils.Dilate(closed, 10);
                //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\dilated.png", dilated);

                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                VectorOfVectorOfPoint contoursRelevant = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(dilated, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

                int autoMaxPixelSize = ImageUtils.GetPercentualImagePixelCount(image, 0.03f);
                for (int i = 0; i < contours.Size; i++)
                {
                    double area = CvInvoke.ContourArea(contours[i]);

                    if (IsContourRelevant(area, autoMaxPixelSize, segmentationParameters))
                    {
                        contoursRelevant.Push(contours[i]);
                    }
                }

                CvInvoke.DrawContours(image, contoursRelevant, -1, new MCvScalar(0, 0, 255));

                mask = new Image<Gray, Byte>(image.Width, image.Height, new Gray(0.0));
                CvInvoke.DrawContours(mask, contoursRelevant, -1, new MCvScalar(255.0), thickness: -1);
            }
        }

        private bool IsContourRelevant(double area, int autoMaxPixelSize, SegmentationParameters segmentationParameters)
        {
            if (segmentationParameters.ManualContourSizeRestriction)
            {
                int minContour = segmentationParameters.MinContourSize;
                int maxContour = segmentationParameters.MaxContourSize;

                if (minContour == 0 && maxContour == 0)
                    return area > autoMaxPixelSize;
                else if (minContour != 0 && maxContour != 0)
                    return (minContour < area) && (area < maxContour);
                else if (minContour == 0)
                    return area < maxContour;
                else
                    return area > minContour;
            }
            else
            {
                return area > autoMaxPixelSize;
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
