﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp.WSIExtraction
{
    class Extraction
    {
        public static List<Rectangle> ExtractObjects(Image<Bgr, byte> image, double threshold, double minContourSize, out Image<Bgr, byte> image_debug)
        {
            double[] average_background_color = new double[] { 0, 0, 0 };
            List<Rectangle> contours = new List<Rectangle>();
            Image<Gray, Byte> gray = new Image<Gray, byte>(image.Size);
            image_debug = new Image<Bgr, byte>(image.Size);

            CvInvoke.CvtColor(image, gray, ColorConversion.Bgr2Gray);
            CvInvoke.Threshold(gray, gray, threshold, 255, ThresholdType.BinaryInv);
            gray = gray.Erode(2);
            gray = gray.Dilate(7);
            gray = FillHoles(gray);
            CvInvoke.CvtColor(gray, image_debug, ColorConversion.Gray2Bgr);

            using (VectorOfVectorOfPoint contoursVector = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(gray, contoursVector, null, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);
                for (int i = 0; i < contoursVector.Size; i++)
                {
                    double area = CvInvoke.ContourArea(contoursVector[i]);

                    if (area > minContourSize)
                    {
                        Rectangle boundingRec = CvInvoke.BoundingRectangle(contoursVector[i]);
                        contours.Add(boundingRec);
                        image_debug.Draw(boundingRec, new Bgr(0.0, 0.0, 255.0), 2);
                    }
                }
            }

            gray.Dispose();
            // todo: calculate background color?

            return contours;
        }

        private static Image<Gray, byte> FillHoles(Image<Gray, byte> image)
        {
            var resultImage = image.CopyBlank();

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(image, contours, null, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);
                CvInvoke.DrawContours(resultImage, contours, -1, new MCvScalar(255.0), thickness: -1);
            }

            return resultImage;
        }

        public static Image<Bgr, byte> ExtractImageFromWSI(string filename, int resolution)
        {
            using (VMscope.InteropCore.ImageStreaming.IStreamingImage image = VMscope.VirtualSlideAccess.Sdk.GetImage(filename))
            {
                int layerFactor = (int)(Math.Pow(2.0, (double)resolution));
                Bitmap bmp = image.GetImagePart(0, 0, image.Size.Width, image.Size.Height, (int)(image.Size.Width / layerFactor), (int)(image.Size.Height / layerFactor));

                Image<Bgr, byte> m = new Image<Bgr, byte>(bmp);
                bmp.Dispose();
                return m;
            }
        }
    }
}