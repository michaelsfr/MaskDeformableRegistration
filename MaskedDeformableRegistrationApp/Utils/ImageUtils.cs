using Emgu.CV;
using Emgu.CV.Structure;
using sitk = itk.simple;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Util;

namespace MaskedDeformableRegistrationApp.Utils
{
    public class ImageUtils
    {
        /// <summary>
        /// Method returns the count of pixels for a specific percentage in perspective to the whole image size.
        /// </summary>
        /// <typeparam name="T">generic image type</typeparam>
        /// <param name="image">image</param>
        /// <param name="percentage">percentage as a float value</param>
        /// <returns></returns>
        public static int GetPercentualImagePixelCount<T>(T image, float percentage)
        {
            if(percentage >= 1.0)
            {
                return GetImagePixelCount(image);
            } else if (percentage <= 0)
            {
                return 0;
            } else
            {
                return (int)(GetImagePixelCount(image) * percentage);
            }
        }

        /// <summary>
        /// Method returns the count of all pixels for an image.
        /// </summary>
        /// <typeparam name="T">generic image type</typeparam>
        /// <param name="image">image</param>
        /// <returns>amount of pixels</returns>
        public static int GetImagePixelCount<T>(T image)
        {
            Size size = new Size();
            if (typeof(T) == typeof(Image<Bgr, byte>))
            {
                Image<Bgr, byte> temp = (Image<Bgr, byte>)(object)image;
                size = temp.Size;
            } else if (typeof(T) == typeof(Image<Gray, byte>))
            {
                Image<Gray, byte> temp = (Image<Gray, byte>)(object)image;
                size = temp.Size;
            } else if (typeof(T) == typeof(UMat))
            {
                UMat temp = (UMat)(object)image;
                size = temp.Size;
            } 

            if(size.IsEmpty)
            {
                return -1;
            } else
            {
                return size.Height * size.Width;
            }   
        }

        /// <summary>
        /// Resize image by new width and height.
        /// </summary>
        /// <param name="image">input image</param>
        /// <param name="newWidth">width</param>
        /// <param name="newHeight">height</param>
        /// <returns>resized image</returns>
        public static sitk.Image ResizeImage(sitk.Image image, uint newWidth, uint newHeight)
        {
            if (image.GetWidth() == newWidth && image.GetHeight() == newHeight)
                return image;

            sitk.VectorUInt32 vec = new sitk.VectorUInt32();
            vec.Add(newWidth);
            vec.Add(newHeight);

            sitk.ResampleImageFilter resampleFilter = new sitk.ResampleImageFilter();
            resampleFilter.SetSize(vec);
            resampleFilter.SetOutputOrigin(image.GetOrigin());
            resampleFilter.SetOutputDirection(image.GetDirection());
            resampleFilter.SetOutputSpacing(image.GetSpacing());
            resampleFilter.SetOutputPixelType(image.GetPixelID());
            resampleFilter.SetDefaultPixelValue(255.0);
            sitk.Image resultImage = resampleFilter.Execute(image);

            return resultImage;
        }

        /// <summary>
        /// Cast bitmap to sitk image.
        /// </summary>
        /// <param name="image">bitmap</param>
        /// <returns>sitk image</returns>
        public static sitk.Image GetITKImageFromBitmap(Bitmap image)
        {
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed/*image.PixelFormat*/);
            int bitmapByteSize = bitmapData.Stride * image.Height;
            byte[] bitmapByteArray = new byte[bitmapByteSize];
            uint width = (uint)image.Width;
            uint height = (uint)image.Height;

            Marshal.Copy(bitmapData.Scan0, bitmapByteArray, 0, bitmapByteSize);
            image.UnlockBits(bitmapData);

            sitk.Image sitkImage = new sitk.Image(width, height, sitk.PixelIDValueEnum.sitkUInt8);
            IntPtr sitkImageBuffer = sitkImage.GetBufferAsUInt8();

            Marshal.Copy(bitmapByteArray, 0, sitkImageBuffer, bitmapByteSize);
            return sitkImage;
        }

        /// <summary>
        /// Resize image by width and height and set output pixel type.
        /// </summary>
        /// <param name="img">input image</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="pixelType">output pixel type</param>
        /// <returns>resampled image</returns>
        public static sitk.Image ResizeImage(sitk.Image img, uint width, uint height, sitk.PixelIDValueEnum pixelType)
        {
            sitk.VectorUInt32 vec = new sitk.VectorUInt32();
            vec.Add(width);
            vec.Add(height);

            sitk.ResampleImageFilter resampleFilter = new sitk.ResampleImageFilter();
            resampleFilter.SetSize(vec);
            resampleFilter.SetOutputOrigin(img.GetOrigin());
            resampleFilter.SetOutputDirection(img.GetDirection());
            resampleFilter.SetOutputSpacing(img.GetSpacing());
            resampleFilter.SetOutputPixelType(pixelType);
            resampleFilter.SetDefaultPixelValue(255.0);
            sitk.Image resultImage = resampleFilter.Execute(img);
            img.Dispose();

            return resultImage;
        }

        /// <summary>
        /// Resize image depending on a reference image.
        /// </summary>
        /// <param name="toResize">image</param>
        /// <param name="referenceImage">reference image</param>
        /// <returns>resampled image</returns>
        public static sitk.Image ResizeImage(sitk.Image toResize, sitk.Image referenceImage)
        {
            if (toResize.GetWidth() == referenceImage.GetWidth() && toResize.GetHeight() == referenceImage.GetHeight())
                return toResize;

            uint width = toResize.GetWidth() < referenceImage.GetWidth() ? referenceImage.GetWidth() : toResize.GetWidth();
            uint height = toResize.GetHeight() < referenceImage.GetHeight() ? referenceImage.GetHeight() : toResize.GetHeight();
            sitk.VectorUInt32 vec = new sitk.VectorUInt32();
            vec.Add(width);
            vec.Add(height);

            sitk.ResampleImageFilter resampleFilter = new sitk.ResampleImageFilter();
            resampleFilter.SetSize(vec);
            resampleFilter.SetOutputOrigin(toResize.GetOrigin());
            resampleFilter.SetOutputDirection(toResize.GetDirection());
            resampleFilter.SetOutputSpacing(toResize.GetSpacing());
            resampleFilter.SetOutputPixelType(referenceImage.GetPixelID());
            resampleFilter.SetDefaultPixelValue(255.0);
            sitk.Image resultImage = resampleFilter.Execute(toResize);

            return resultImage;
        }

        /// <summary>
        /// Calculate histogram for given vector of matrices, allowing masking.
        /// </summary>
        /// <param name="vm">matrices</param>
        /// <param name="tempMask">mask</param>
        /// <returns>histogram as umat</returns>
        public static UMat CalculateHistogram(VectorOfMat vm, Image<Gray, byte> tempMask)
        {
            UMat hist = new UMat();
            int[] channel = new int[] { 0 };
            int[] histSize = new int[] { 32 };
            float[] range = new float[] { 0.0f, 256.0f };
            CvInvoke.CalcHist(vm, channel, tempMask, hist, histSize, range, false);
            return hist;
        }

        /// <summary>
        /// Resize image an get image transform.
        /// </summary>
        /// <param name="image">image</param>
        /// <param name="newWidth">width</param>
        /// <param name="newHeight">height</param>
        /// <param name="transform">out param transform</param>
        /// <returns>resampled image</returns>
        public static sitk.Image ResizeImage(sitk.Image image, uint newWidth, uint newHeight, out sitk.Transform transform)
        {
            transform = null;
            if (image.GetWidth() == newWidth && image.GetHeight() == newHeight)
                return image;

            sitk.VectorUInt32 vec = new sitk.VectorUInt32();
            vec.Add(newWidth);
            vec.Add(newHeight);

            sitk.ResampleImageFilter resampleFilter = new sitk.ResampleImageFilter();
            resampleFilter.SetSize(vec);
            resampleFilter.SetOutputOrigin(image.GetOrigin());
            resampleFilter.SetOutputDirection(image.GetDirection());
            resampleFilter.SetOutputSpacing(image.GetSpacing());
            resampleFilter.SetOutputPixelType(image.GetPixelID());
            resampleFilter.SetDefaultPixelValue(255.0);
            sitk.Image resultImage = resampleFilter.Execute(image);
            transform = resampleFilter.GetTransform();

            return resultImage;
        }

        /// <summary>
        /// Resample image output pixel type.
        /// </summary>
        /// <param name="img">image</param>
        /// <param name="pixelIDValue">output pixel type</param>
        /// <returns>sitk image</returns>
        public static sitk.Image ResampleImage(sitk.Image img, sitk.PixelIDValueEnum pixelIDValue)
        {
            sitk.ResampleImageFilter resampleImageFilter = new sitk.ResampleImageFilter();
            resampleImageFilter.SetOutputPixelType(pixelIDValue);
            resampleImageFilter.SetOutputOrigin(img.GetOrigin());
            resampleImageFilter.SetOutputDirection(img.GetDirection());
            resampleImageFilter.SetOutputSpacing(img.GetSpacing());
            return resampleImageFilter.Execute(img);
        }
        
        /// <summary>
        /// Binarize image by a lower and upper threshold.
        /// </summary>
        /// <param name="img">image</param>
        /// <param name="lowerT">lower threshold</param>
        /// <param name="upperT">upper threshold</param>
        /// <returns></returns>
        public static sitk.Image Binarize(sitk.Image img, int lowerT = 127, int upperT = 255)
        {
            sitk.BinaryThresholdImageFilter binaryFilter1 = new sitk.BinaryThresholdImageFilter();
            binaryFilter1.SetLowerThreshold(lowerT);
            binaryFilter1.SetUpperThreshold(upperT);
            binaryFilter1.SetInsideValue(1);
            binaryFilter1.SetOutsideValue(0);
            return binaryFilter1.Execute(img);
        }

        /// <summary>
        /// Cast image to float 32.
        /// </summary>
        /// <param name="img">image</param>
        /// <returns>casted image</returns>
        public static sitk.Image CastImageToFloat32(sitk.Image img)
        {
            sitk.CastImageFilter castImageFilter = new sitk.CastImageFilter();
            castImageFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkVectorFloat32);
            sitk.Image vector = castImageFilter.Execute(img);

            sitk.VectorIndexSelectionCastImageFilter vectorFilter = new sitk.VectorIndexSelectionCastImageFilter();
            sitk.Image tempImage = vectorFilter.Execute(vector, 0, sitk.PixelIDValueEnum.sitkFloat32);

            castImageFilter.Dispose();
            vector.Dispose();

            return tempImage;
        }
    }
}
