﻿using System;
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

        public InnerTissueSegmentation(Image<Bgr, byte> image, Image<Gray, byte> particleMask, SegmentationParameters segmentationParameters)
        {
            this.image = image;
            this.particleMask = particleMask;
            this.segmentationParameters = segmentationParameters;
        }

        /// <summary>
        /// Execute segmentation process of inner structures.
        /// </summary>
        public void Execute()
        {
            if (image != null)
            {
                Image<Bgr, byte> maskedImage = new Image<Bgr, byte>(image.Width, image.Height, new Bgr(255, 255, 255));
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

        /// <summary>
        /// Kmeans segmentation of given image.
        /// 1. Do a kmeans clustering
        /// 2. Find contours
        /// 3. Classification of clusters (?)
        /// </summary>
        /// <param name="uMatChannel"></param>
        private void KmeansSegmentation(UMat uMatChannel)
        {
            if(false)
            {
                Image<Bgr, byte> newImage = new Image<Bgr, byte>(uMatChannel.Bitmap);
                Image<Bgr, byte> clusteredImage = SegmentationUtils.KMeansClustering<Bgr, byte, byte>(newImage.Clone(), 3, SegmentationUtils._clusterColors);

                ReadWriteUtils.WriteUMatToFile(@"D:\testdata\_temp\kmeans.png", clusteredImage.ToUMat());

                // TODO
                // Find contours
                // Classificate contours
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Standard segmentation of given image.
        /// 1. Thresholding
        /// 2. Morphological operations
        /// 3. Find contours
        /// 4. Calculate coefficients and classificate contours
        /// </summary>
        /// <param name="uMatChannel">image in UMat format</param>
        private void StandardSegmentation(UMat uMatChannel)
        {
            // thresholding
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

            // morphological operations (todo: make kernel generic or depending on image size)
            UMat opened = SegmentationUtils.Opening(thresholded, 5);
            UMat dilated = SegmentationUtils.Dilate(opened, 5);
            //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\dilated.png", dilated);
            UMat inverted = new UMat();
            CvInvoke.BitwiseNot(dilated, inverted);
            //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\inverted.png", inverted);

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            VectorOfVectorOfPoint contoursRelevant = new VectorOfVectorOfPoint();

            CvInvoke.FindContours(inverted, contours, null, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);

            int nonRelevantPixelSize = ImageUtils.GetPercentualImagePixelCount<UMat>(inverted, 0.005f);
            Console.WriteLine("Non relevant pixel size: " + nonRelevantPixelSize);
            Console.WriteLine("Number of contours: " + contours.Size);

            ClassifyContours(uMatChannel, ref contours, ref contoursRelevant, nonRelevantPixelSize);

            // debug: show image with found contours
            //CvInvoke.DrawContours(image, contoursRelevant, -1, new MCvScalar(0, 255, 0));
            //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\image_with_contours.png", image.ToUMat());

            // creat mask with rigid structures
            Image<Gray, byte> mask = new Image<Gray, byte>(image.Width, image.Height, new Gray(0.0));
            CvInvoke.DrawContours(mask, contoursRelevant, -1, new MCvScalar(255.0), thickness: -1);
            //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\mask.png", mask.ToUMat());
            UMat inv1 = new UMat();
            CvInvoke.BitwiseNot(mask, inv1);
            UMat convertedMask = new UMat();
            inv1.ConvertTo(convertedMask, DepthType.Cv32F);

            // create mask with non rigid structures
            Image<Gray, byte> convMask = new Image<Gray, byte>(convertedMask.Bitmap);
            Image<Gray, byte> mask2 = new Image<Gray, byte>(image.Width, image.Height, new Gray(255.0));
            CvInvoke.Subtract(particleMask, convMask, mask2);
            //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\INV_mask.png", mask2.ToUMat());
            UMat inv2 = new UMat();
            //CvInvoke.BitwiseNot(mask2, inv2);
            CvInvoke.BitwiseAnd(mask2, mask, inv2);
            inv2 = SegmentationUtils.Dilate(inv2, 10);
            UMat convertedMask2 = new UMat();
            inv2.ConvertTo(convertedMask2, DepthType.Cv32F);

            //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\_mask1.png", convertedMask);
            //ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "\\_mask2.png", convertedMask2);

            // add masks to list
            masks.Add(convertedMask);
            masks.Add(convertedMask2);
        }

        /// <summary>
        /// Classify contours by size and variance of intensities.
        /// </summary>
        /// <param name="uMatChannel">original image</param>
        /// <param name="contours">all contours</param>
        /// <param name="contoursRelevant">relevant contours</param>
        /// <param name="nonRelevantPixelSize">non relevant contour pixel size</param>
        private void ClassifyContours(UMat uMatChannel, ref VectorOfVectorOfPoint contours, ref VectorOfVectorOfPoint contoursRelevant, int nonRelevantPixelSize)
        {
            Dictionary<int, double> variances = new Dictionary<int, double>();
            for (int i = 0; i < contours.Size; i++)
            {
                double area = CvInvoke.ContourArea(contours[i]);

                if(area > nonRelevantPixelSize)
                {
                    double variance = CalculateVariance(uMatChannel, contours[i]);
                    variances.Add(i, variance);
                }               
            }

            // set percentile to 1.0 to account all contours
            double variancePercentile = GetPercentile(variances.Select(it => it.Value).ToArray(), 0.85);

            for (int i = 0; i < variances.Count; i++)
            {
                if (variances.ElementAt(i).Value < variancePercentile)
                {
                    contoursRelevant.Push(contours[variances.ElementAt(i).Key]);
                }
            }
        }

        /// <summary>
        /// Calculate variance of a contour.
        /// </summary>
        /// <param name="uMatChannel">original image</param>
        /// <param name="contour">contour as a vector of points</param>
        /// <returns>variance</returns>
        private double CalculateVariance(UMat uMatChannel, VectorOfPoint contour)
        {
            using (Image<Gray, byte> tempMask = new Image<Gray, byte>(image.Width, image.Height, new Gray(0.0)))
            {
                tempMask.Draw(contour.ToArray(), new Gray(255.0), -1);
                MCvScalar mean = new MCvScalar();
                MCvScalar stdDev = new MCvScalar();

                using (UMat img1 = uMatChannel.Clone())
                using (UMat img2 = tempMask.ToUMat().Clone())
                {
                    CvInvoke.MeanStdDev(img1, ref mean, ref stdDev, img2);
                }

                /*double min = 0.0, max = 0.0;
                Point maxP = new Point(), minP = new Point();
                CvInvoke.MinMaxLoc(hist, ref min, ref max, ref minP, ref maxP);
                Console.WriteLine("[ min: " + min + " | max: " + max + " | minP: " + minP.ToString() + " | " + maxP.ToString());*/

                double variance = Math.Sqrt(stdDev.V0);
                Console.WriteLine("Mean: " + mean.V0 + " | StdDev: " + stdDev.V0 + " | Var: " + Math.Sqrt(stdDev.V0));
                return variance;
            }
        }

        /// <summary>
        /// Calculates a pecentile of a given list of doubles
        /// </summary>
        /// <param name="sequence">list of doubles</param>
        /// <param name="percentile">percentile --> value between 0 and 1</param>
        /// <returns></returns>
        private double GetPercentile(IEnumerable<double> sequence, double percentile)
        {
            if(percentile >= 0 && percentile <= 1)
            {
                double[] dArray = sequence.ToArray();
                Array.Sort(dArray);

                if (dArray.Length > 0)
                {
                    double realIndex = percentile * (dArray.Length - 1);
                    int index = (int)realIndex;
                    double frac = realIndex - index;

                    if (index + 1 < dArray.Length)
                    {
                        return dArray[index] * (1 - frac) + dArray[index + 1] * frac;
                    }
                    else
                    {
                        return dArray[index];
                    }
                }
            }
            return -1;
        }


        public List<UMat> GetOutput()
        {
            return masks;
        }

        /// <summary>
        /// Create a coefficient map of crearted segmentations.
        /// Coefficient map will hold values between 0 and 1 for each segments.
        /// </summary>
        /// <returns>coefficient map as umat</returns>
        public UMat GetCoefficientMatrix()
        {
            UMat mat = null;
            double sc = 0.5;
            foreach(UMat m in masks)
            {
                Console.WriteLine("SC " + sc);
                if (mat == null)
                {
                    mat = new UMat(m.Size, DepthType.Cv32F, m.NumberOfChannels);
                    mat.SetTo(new MCvScalar(0.0));
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

        /// <summary>
        /// Dispose all globally used instances of image.
        /// </summary>
        public void Dispose()
        {
            image.Dispose();
            particleMask.Dispose();
            masks.ForEach(it => it.Dispose());
            masks.Clear();
        }
    }
}
