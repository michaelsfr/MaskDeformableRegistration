using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using MaskedDeformableRegistrationApp.Utils;
using SharpAccessory.Imaging.Filters;
using SharpAccessory.Imaging.Processors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp.Segmentation
{
    public static class SegmentationUtils
    {
        // cluster colors / intensities
        public static Bgr[] _clusterColors = new Bgr[] 
        {
            new Bgr(0,0,255),
            new Bgr(0, 255, 0),
            new Bgr(255, 100, 100),
            new Bgr(255,0,255),
        };

        public static Gray[] _clusterColorsGray = new Gray[]
        {
            new Gray(255.0),
            new Gray(192.0),
            new Gray(128.0),
            new Gray(64.0)
        };

        /// <summary>
        /// Enhance contrast of an image given as UMat.
        /// </summary>
        /// <param name="input">input image</param>
        /// <param name="clipLimit">clip limit (default is 5)</param>
        /// <param name="tileGridSize">tile size (default is 8)</param>
        /// <returns>image with enhanced contrast</returns>
        public static UMat IncreaseContrast(UMat input, double clipLimit = 5.0, int tileGridSize = 8)
        {
            UMat lab = new UMat(), cl = new UMat(), result = new UMat();
            CvInvoke.CvtColor(input, lab, ColorConversion.Bgr2Lab);
            UMat[] splitLAB = lab.Split();
            CvInvoke.CLAHE(splitLAB[0], clipLimit, new Size(tileGridSize, tileGridSize), cl);
            CvInvoke.Merge(new VectorOfUMat(cl, splitLAB[1], splitLAB[2]), result);
            lab.Dispose();
            cl.Dispose();
            return result;
        }

        /// <summary>
        /// Blur image.
        /// </summary>
        /// <param name="input">input image</param>
        /// <param name="kernelSize">kernel size for blurring</param>
        /// <param name="sigma">sigma</param>
        /// <returns>blurred image</returns>
        public static UMat Blur(UMat input, int kernelSize, double sigma)
        {
            UMat temp = new UMat();
            CvInvoke.GaussianBlur(input, temp, new Size(kernelSize, kernelSize), sigma);
            input.Dispose();
            return temp;
        }

        /// <summary>
        /// Sharp image with default values.
        /// </summary>
        /// <param name="input">input image</param>
        /// <returns>sharpened image</returns>
        public static UMat Sharp(UMat input)
        {
            UMat temp = new UMat();
            CvInvoke.GaussianBlur(input, temp, new Size(0, 0), 3);
            CvInvoke.AddWeighted(input, 1.5, temp, -0.5, 0, temp);
            input.Dispose();
            return temp;
        }

        /// <summary>
        /// Get a color channel for a given color space and channel
        /// </summary>
        /// <param name="colorSpace">color space / model</param>
        /// <param name="image">input image</param>
        /// <param name="channel">channel</param>
        /// <returns>color channel as grayscale image</returns>
        public static UMat GetColorChannelAsUMat(ColorSpace colorSpace, Image<Bgr, byte> image, int channel)
        {
            using(Image<Bgr, byte> imageCopy = image.Clone())
            {
                if ((int)colorSpace >= 0 && (int)colorSpace <= 7)
                {
                    int cs = (int)colorSpace;
                    return GetColorDeconvolutedChannelAsUMat(imageCopy.Bitmap, (ColorDeconvolution.KnownStain)cs, channel);
                }
                else
                {
                    ColorConversion cc = ColorConversion.Bgr2Bgra;
                    switch (colorSpace)
                    {
                        case ColorSpace.HLS: cc = ColorConversion.Bgr2Hls; break;
                        case ColorSpace.HSV: cc = ColorConversion.Bgr2Hsv; break;
                        case ColorSpace.LAB: cc = ColorConversion.Bgr2Lab; break;
                        case ColorSpace.LUV: cc = ColorConversion.Bgr2Luv; break;
                        case ColorSpace.RGB: cc = ColorConversion.Bgr2Rgb; break;
                    }

                    return GetColorChannel(imageCopy.ToUMat(), cc, channel);
                }
            }
        }

        /// <summary>
        /// Get color channel of a known stain.
        /// </summary>
        /// <param name="image">input image</param>
        /// <param name="stain">known stain</param>
        /// <param name="channel">channel of the known stain</param>
        /// <returns>color channel</returns>
        public static UMat GetColorDeconvolutedChannelAsUMat(Bitmap image, ColorDeconvolution.KnownStain stain,int channel)
        {
            Bitmap copy = (Bitmap)image.Clone();
            Bitmap bitmap = null;
            UMat umat = null;

            try
            {
                bitmap = ColorDeconvolution(copy, stain, channel);
                umat = new Image<Gray, byte>(bitmap).ToUMat();
            } catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } finally
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
            }

            return umat;
        }

        /// <summary>
        /// Get default color model and channel from openCV.
        /// </summary>
        /// <param name="input">input image</param>
        /// <param name="type">color model / color conversion type</param>
        /// <param name="channel">color channel</param>
        /// <returns>extracted color channel</returns>
        public static UMat GetColorChannel(UMat input, ColorConversion type, int channel)
        {
            UMat channels = new UMat();
            CvInvoke.CvtColor(input, channels, type);
            if(channels.NumberOfChannels > channel)
            {
                return channels.Split()[channel];
            }
            return null;
        }

        /// <summary>
        /// Convert image to color space of known stain.
        /// </summary>
        /// <param name="source">Source image.</param>
        /// <param name="stain">Known stain.</param>
        /// <param name="channel">Channel of stain. Takes int 0-2.</param>
        /// <returns>Color deconvoluted image.</returns>
        public static Bitmap ColorDeconvolution(Bitmap source, ColorDeconvolution.KnownStain stain, int channel)
        {
            using (var image = source.Clone() as Bitmap)
            {
                BitmapProcessor bitmapProcessor = new BitmapProcessor(source);
                ColorDeconvolution colorDeconvolution = new ColorDeconvolution();
                GrayscaleProcessor gpDeconvoluted = null;
                if(channel == 0)
                {
                    gpDeconvoluted = colorDeconvolution.Get1stStain(bitmapProcessor, stain);
                } else if (channel == 1)
                {
                    gpDeconvoluted = colorDeconvolution.Get2ndStain(bitmapProcessor, stain);
                } else if (channel == 2)
                {
                    gpDeconvoluted = colorDeconvolution.Get3rdStain(bitmapProcessor, stain);
                } else
                {
                    return null;
                }

                Bitmap result = gpDeconvoluted.Bitmap.Clone() as Bitmap;
                gpDeconvoluted.Dispose();
                return result;
            }
        }

        /// <summary>
        /// Do Otsu thresholding on grayscale UMat.
        /// </summary>
        /// <param name="input">grayscale image</param>
        /// <param name="maskWhitePixels">flag to specify if white pixels should be masked</param>
        /// <returns>thresholded image</returns>
        public static UMat ThresholdOtsu(UMat input, bool maskWhitePixels = false)
        {
            UMat result = new UMat();
            //CvInvoke.Threshold(input, result, 0, 255, ThresholdType.Otsu);
            double otsu_threshold = CvInvoke.Threshold(input, result, 0, 255, ThresholdType.Otsu | ThresholdType.Binary);

            if (maskWhitePixels)
            {
                CvInvoke.Threshold(input, result, otsu_threshold, 255, ThresholdType.BinaryInv);
            }

            input.Dispose();
            return result;
        }

        /// <summary>
        /// Apply a threshold segmentation to a grayscale image.
        /// </summary>
        /// <param name="input">grayscale image input</param>
        /// <param name="threshold">threshold</param>
        /// <returns>thresholded image</returns>
        public static UMat Threshold(UMat input, int threshold)
        {
            UMat result = new UMat();
            CvInvoke.Threshold(input, result, threshold, 255, ThresholdType.Binary);
            input.Dispose();
            return result;
        }


        /// <summary>
        /// Apply morphological operation closing on image.
        /// Closing is erosion followed by dilation with the same structuring element.
        /// </summary>
        /// <param name="input">input image</param>
        /// <param name="kernelSize">size of the structuring element</param>
        /// <returns>modified image</returns>
        public static UMat Closing(UMat input, int kernelSize)
        {
            UMat temp = new UMat();
            UMat result = new UMat();
            var element = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(kernelSize, kernelSize), new Point(-1, -1));
            CvInvoke.Erode(input, temp, element, new Point(-1, -1), 1, BorderType.Default, default(MCvScalar));
            CvInvoke.Dilate(temp, result, element, new Point(-1, -1), 1, BorderType.Default, default(MCvScalar));
            return result;
        }

        /// <summary>
        /// Apply morphological operation opening on image.
        /// Opening is dilation followed by erosion with the same structuring element.
        /// </summary>
        /// <param name="input">input image</param>
        /// <param name="kernelSize">size of the structuring element</param>
        /// <returns>modified image</returns>
        public static UMat Opening(UMat input, int kernelSize)
        {
            UMat temp = new UMat();
            UMat result = new UMat();
            var element = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(kernelSize, kernelSize), new Point(-1, -1));
            CvInvoke.Dilate(input, temp, element, new Point(-1, -1), 1, BorderType.Default, default(MCvScalar));
            CvInvoke.Erode(temp, result, element, new Point(-1, -1), 1, BorderType.Default, default(MCvScalar));
            return result;
        }

        /// <summary>
        /// Apply morphological operation erosion on image.
        /// </summary>
        /// <param name="input">input image</param>
        /// <param name="kernelSize">kernel size</param>
        /// <returns>eroded image</returns>
        public static UMat Erode(UMat input, int kernelSize)
        {
            UMat result = new UMat();
            var element = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(kernelSize, kernelSize), new Point(-1, -1));
            CvInvoke.Erode(input, result, element, new Point(-1, -1), 1, BorderType.Default, default(MCvScalar));
            return result;
        }

        /// <summary>
        /// Apply morphological operation dilation on image.
        /// </summary>
        /// <param name="input">input image</param>
        /// <param name="kernelSize">kernel size</param>
        /// <returns>dilated image</returns>
        public static UMat Dilate(UMat input, int kernelSize)
        {
            UMat result = new UMat();
            var element = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(kernelSize, kernelSize), new Point(-1, -1));
            CvInvoke.Dilate(input, result, element, new Point(-1, -1), 1, BorderType.Default, default(MCvScalar));
            return result;
        }

        /// <summary>
        /// Generic method to apply k-Means-Clustering on an opencv image of unknown type.
        /// </summary>
        /// <typeparam name="T">color space type of the image</typeparam>
        /// <typeparam name="V">pixel data type</typeparam>
        /// <typeparam name="K">color / intensities datatype</typeparam>
        /// <param name="image">input image</param>
        /// <param name="k">amount of clusters k</param>
        /// <param name="colors">array of colors used to represent the clusters</param>
        /// <returns>clustered image</returns>
        public static Image<T, V> KMeansClustering<T, V, K>(Image<T, K> image, int k, T[] colors) where T : struct, IColor where K : new() where V : new()
        {
            if (k > 4)
                return null;

            Image<Bgr, float> src = image.Convert<Bgr, float>();

            Matrix<float> samples = new Matrix<float>(src.Rows * src.Cols, 1, 3);
            Matrix<int> finalClusters = new Matrix<int>(src.Rows * src.Cols, 1);

            for (int y = 0; y < src.Rows; y++)
            {
                for (int x = 0; x < src.Cols; x++)
                {
                    samples.Data[y + x * src.Rows, 0] = (float)src[y, x].Blue;
                    samples.Data[y + x * src.Rows, 1] = (float)src[y, x].Green;
                    samples.Data[y + x * src.Rows, 2] = (float)src[y, x].Red;
                }
            }

            MCvTermCriteria criteria = new MCvTermCriteria(50, 0.5);
            criteria.Type = TermCritType.Iter | TermCritType.Eps;

            Matrix<Single> centers = new Matrix<Single>(k, src.Rows * src.Cols);
            double compactness = CvInvoke.Kmeans(samples, k, finalClusters, criteria, 3, KMeansInitType.RandomCenters);

            Image<T, V> kMeansImage = new Image<T, V>(src.Size);
            for (int y = 0; y < src.Rows; y++)
            {
                for (int x = 0; x < src.Cols; x++)
                {
                    PointF p = new PointF(x, y);
                    kMeansImage.Draw(new CircleF(p, 1.0f), colors[finalClusters[y + x * src.Rows, 0]], 1);
                }
            }
            // Debug
            ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "kmeans_new.png", kMeansImage.ToUMat());
            return kMeansImage;
        }
    }
}
