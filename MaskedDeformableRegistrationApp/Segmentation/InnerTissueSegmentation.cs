using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using MaskedDeformableRegistrationApp.Utils;
using SharpAccessory.Imaging.Filters;

namespace MaskedDeformableRegistrationApp.Segmentation
{
    class InnerTissueSegmentation : ISegmentation<List<UMat>>
    {
        private Image<Bgr, byte> image;
        private Image<Gray, byte> particleMask;
        private List<UMat> masks = new List<UMat>();

        SegmentationParameters segmentationParameters = null;
        private ColorSpace colorSpace = ColorSpace.HaematoxylinDAB;
        private int channel = 2;

        public InnerTissueSegmentation(Image<Bgr, byte> image, Image<Gray, byte> particleMask, SegmentationParameters segmentationParameters)
        {
            this.image = image;
            this.particleMask = particleMask;
            this.segmentationParameters = segmentationParameters;
        }

        public void Execute()
        {
            if (image != null)
            {
                Image<Bgr, byte> maskedImage = new Image<Bgr, byte>(image.Width, image.Height, new Bgr(255, 255, 255));
                ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\BIG_mask.png", maskedImage.ToUMat());
                CvInvoke.cvCopy(image, maskedImage, particleMask);

                Console.WriteLine("Image dimensions: " + image.Width + " / " + image.Height);
                Console.WriteLine("Masked Image dimensions: " + maskedImage.Width + " / " + maskedImage.Height);

                UMat temp = SegmentationUtils.IncreaseContrast(maskedImage.ToUMat(), 5.0, 8);
                temp = SegmentationUtils.Blur(temp, 3, 1.0);
                temp = SegmentationUtils.Sharp(temp);
                Image<Bgr, byte> tempImage = new Image<Bgr, byte>(temp.Bitmap);
                UMat uMatChannel = SegmentationUtils.GetColorChannelAsUMat(segmentationParameters.Colorspace, tempImage, segmentationParameters.Channel);

                if (segmentationParameters.UseKmeans)
                {
                    KmeansSegmentation(uMatChannel);
                }
                else
                {
                    StandardSegmentation(uMatChannel);
                }
            }
        }

        private void KmeansSegmentation(UMat uMatChannel)
        {
            Image<Gray, byte> newImage = new Image<Gray, byte>(uMatChannel.Bitmap);
            //SegmentationUtils.KMeansClustering(newImage.Clone(), 3);
            SegmentationUtils.KMeansClustering<Gray, byte, byte>(newImage.Clone(), 3, SegmentationUtils.clusterColorsGray);
            //throw new NotImplementedException();
        }

        private void StandardSegmentation(UMat uMatChannel)
        {
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
            

            UMat opened = SegmentationUtils.Opening(thresholded, 5);
            UMat dilated = SegmentationUtils.Dilate(opened, 5);
            ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\dilated.png", dilated);
            UMat inverted = new UMat();
            CvInvoke.BitwiseNot(dilated, inverted);
            ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\inverted.png", inverted);

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            VectorOfVectorOfPoint contoursRelevant = new VectorOfVectorOfPoint();

            CvInvoke.FindContours(inverted, contours, null, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);

            int nonRelevantPixelSize = ImageUtils.GetPercentualImagePixelCount<UMat>(inverted, 0.005f);
            Console.WriteLine("Non relevant pixel size: " + nonRelevantPixelSize);
            Console.WriteLine("Number of contours: " + contours.Size);

            for (int i = 0; i < contours.Size; i++)
            {
                double area = CvInvoke.ContourArea(contours[i]);
             
                if (area > nonRelevantPixelSize)
                {
                    Console.WriteLine("Contour (" + i + "): " + area);

                    Image<Gray, byte> tempMask = new Image<Gray, byte>(image.Width, image.Height, new Gray(0.0));
                    tempMask.Draw(contours[i].ToArray(), new Gray(255.0), -1);

                    using (VectorOfMat vm = new VectorOfMat())
                    {
                        vm.Push(image);

                        //UMat hist = ImageUtils.CalculateHistogram(vm, tempMask);
                        
                        MCvScalar mean = new MCvScalar();
                        MCvScalar stdDev = new MCvScalar();
                        using (UMat img1 = uMatChannel.Clone())
                        using (UMat img2 = tempMask.ToUMat().Clone())
                        {
                            CvInvoke.MeanStdDev(img1, ref mean, ref stdDev, img2);
                        }
                        double variance = Math.Sqrt(stdDev.V0);
                        Console.WriteLine("Mean: " + mean.V0 + " | StdDev: " + stdDev.V0 + " | Var: " + Math.Sqrt(stdDev.V0));

                        /*double min = 0.0, max = 0.0;
                        Point maxP = new Point(), minP = new Point();
                        CvInvoke.MinMaxLoc(hist, ref min, ref max, ref minP, ref maxP);
                        Console.WriteLine("[ min: " + min + " | max: " + max + " | minP: " + minP.ToString() + " | " + maxP.ToString());*/

                        // todo: when is variance to high?
                        if (variance < 10)
                        {
                            contoursRelevant.Push(contours[i]);
                        }
                    }
                    tempMask.Dispose();
                }
            }

            //CvInvoke.DrawContours(image, contoursRelevant, -1, new MCvScalar(0, 255, 0));
            //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\image_with_contours.png", image.ToUMat());

            Image<Gray, byte> mask = new Image<Gray, byte>(image.Width, image.Height, new Gray(0.0));
            CvInvoke.DrawContours(mask, contoursRelevant, -1, new MCvScalar(255.0), thickness: -1);
            //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\mask1_not_converted.png", mask.ToUMat());
            UMat convertedMask = new UMat();
            mask.ToUMat().ConvertTo(convertedMask, DepthType.Cv32F);
            //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\mask1_converted.png", convertedMask);


            Image<Gray, byte> mask2 = new Image<Gray, byte>(image.Width, image.Height, new Gray(0.0));
            CvInvoke.Subtract(particleMask, mask, mask2);
            //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\mask2_not_converted.png", mask2.ToUMat());
            UMat convertedMask2 = new UMat();
            mask2.ToUMat().ConvertTo(convertedMask2, DepthType.Cv32F);
            //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\mask2_converted.png", convertedMask2);

            masks.Add(convertedMask);
            masks.Add(convertedMask2);
        }

        public void SetColorSpace(ColorSpace colorSpace)
        {
            this.colorSpace = colorSpace;
        }

        public void SetChannel(int channel)
        {
            this.channel = channel;
        }

        public List<UMat> GetOutput()
        {
            return masks;
        }

        public UMat GetCoefficientMatrix()
        {
            UMat mat = null;
            double sc = 0.0;
            foreach(UMat m in masks)
            {
                Console.WriteLine("SC " + sc);
                if (mat == null)
                {
                    mat = new UMat(m.Size, DepthType.Cv32F, m.NumberOfChannels);
                    mat.SetTo(new MCvScalar(1.0));
                }
                
                UMat nMat = new UMat();
                m.ConvertTo(nMat, DepthType.Cv8U);

                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(nMat, contours, null, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);
                CvInvoke.DrawContours(mat, contours, -1, new MCvScalar(sc), thickness: -1);
                sc += 0.5;
            }

            return mat;
        }

        public void Dispose()
        {
            image.Dispose();
            particleMask.Dispose();
            masks.ForEach(it => it.Dispose());
            masks.Clear();
        }
    }
}
