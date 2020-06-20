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

        private ColorSpace colorSpace = ColorSpace.HSV;
        private int channel = 1;

        public InnerTissueSegmentation(Image<Bgr, byte> image, Image<Gray, byte> particleMask)
        {
            this.image = image;
            this.particleMask = particleMask;
        }

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
                UMat uMatChannel = GetColorChannel();

                Image<Gray, byte> newImage = new Image<Gray, byte>(uMatChannel.Bitmap);
                //SegmentationUtils.KMeansClustering(newImage.Clone(), 3);
                SegmentationUtils.KMeansClustering<Gray, byte, byte>(newImage.Clone(), 3, SegmentationUtils.clusterColorsGray);

                UMat otsu_s = SegmentationUtils.ThresholdOtsu(uMatChannel);
                UMat opened = SegmentationUtils.Opening(otsu_s, 5);
                UMat dilated = SegmentationUtils.Dilate(opened, 5);
                ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "dilated.png", dilated);
                // invert mask
                UMat inverted = new UMat();
                CvInvoke.BitwiseNot(dilated, inverted);
                //VisualizationUtils.ShowUMat(inverted, "S-Otsu");

                ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "bevor_contours.png", inverted);

                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                VectorOfVectorOfPoint contoursRelevant = new VectorOfVectorOfPoint();

                CvInvoke.FindContours(inverted, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

                for (int i = 0; i < contours.Size; i++)
                {
                    double area = CvInvoke.ContourArea(contours[i]);
                    if (area > 15000)
                    {
                        Image<Gray, byte> tempMask = new Image<Gray, byte>(image.Width, image.Height, new Gray(0.0));
                        tempMask.Draw(contours[i].ToArray(), new Gray(255.0), -1);

                        using (VectorOfMat vm = new VectorOfMat())
                        {
                            vm.Push(image);
                            UMat hist = new UMat();
                            int[] channel = new int[] { 0 };
                            int[] histSize = new int[] { 32 };
                            float[] range = new float[] { 0.0f, 256.0f };
                            CvInvoke.CalcHist(vm, channel, tempMask, hist, histSize, range, false);
                            Console.WriteLine(hist.Data);
                            double min = 0.0, max = 0.0;
                            Point maxP = new Point(), minP = new Point();
                            CvInvoke.MinMaxLoc(hist, ref min, ref max, ref minP, ref maxP);
                            Console.WriteLine("[ min: " + min + " | max: " + max + " | minP: " + minP.ToString() + " | " + maxP.ToString());
                            Console.WriteLine("##########");

                            // todo: calculate out
                            if(max > 10000)
                            {
                                //Bitmap histogram = VisualizationUtils.DrawHistogram(max, 0, 255, hist.Bytes);
                                
                                contoursRelevant.Push(contours[i]);
                            }
                        }
                        tempMask.Dispose();
                    }
                }

                CvInvoke.DrawContours(image, contoursRelevant, -1, new MCvScalar(0, 255, 0));
                //ImageViewer.Show(image, "masked img");

                Image<Gray, byte> mask = new Image<Gray, byte>(image.Width, image.Height, new Gray(0.0));
                CvInvoke.DrawContours(mask, contoursRelevant, -1, new MCvScalar(255.0), thickness: -1);
                UMat convertedMask = new UMat();
                mask.ToUMat().ConvertTo(convertedMask, DepthType.Cv32F);

                Image<Gray, byte> mask2 = new Image<Gray, byte>(image.Width, image.Height, new Gray(0.0));
                CvInvoke.Subtract(particleMask, mask, mask2);
                UMat convertedMask2 = new UMat();
                mask2.ToUMat().ConvertTo(convertedMask2, DepthType.Cv32F);

                masks.Add(convertedMask);
                masks.Add(convertedMask2);
            }
        }

        public UMat GetColorChannel()
        {
            if((int)colorSpace >= 0 && (int)colorSpace <= 7)
            {
                int cs = (int)colorSpace;
                return SegmentationUtils.GetColorDeconvolutedChannelAsUMat(image.Bitmap, (ColorDeconvolution.KnownStain)cs, channel);
            } else
            {
                ColorConversion cc = ColorConversion.Bgr2Bgra;
                switch(colorSpace)
                {
                    case ColorSpace.HLS: cc = ColorConversion.Bgr2Hls; break;
                    case ColorSpace.HSV: cc = ColorConversion.Bgr2Hsv; break;
                    case ColorSpace.LAB: cc = ColorConversion.Bgr2Lab; break;
                    case ColorSpace.LUV: cc = ColorConversion.Bgr2Luv; break;
                    case ColorSpace.RGB: cc = ColorConversion.Bgr2Rgb; break;
                }

                return SegmentationUtils.GetColorChannel(image.ToUMat(), cc, channel);
            }
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
