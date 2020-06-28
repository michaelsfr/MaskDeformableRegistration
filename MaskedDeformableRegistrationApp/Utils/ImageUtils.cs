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

        public static Bitmap GetImageFromITK(itk.simple.Image image)
        {
            // todo
            return null;
        }

        public static sitk.Image GetSITKImageFromOpenCV(Image<Bgr, byte> image)
        {
            return GetITKImageFromBitmap(image.Bitmap);
        }

        public static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            /*var resized = image.Bitmap.Clone();
            var brush = new SolidBrush(Color.Black);
            float scale = Math.Min(width / image.Width, height / image.Height);

            var bmp = new Bitmap((int)width, (int)height);
            var graph = Graphics.FromImage(bmp);

            // interpolation increases quality of image
            //graph.InterpolationMode = InterpolationMode.High;
            //graph.CompositingQuality = CompositingQuality.HighQuality;
            //graph.SmoothingMode = SmoothingMode.AntiAlias;

            var scaleWidth = (int)(image.Width * scale);
            var scaleHeight = (int)(image.Height * scale);

            graph.FillRectangle(brush, new RectangleF(0, 0, width, height));
            graph.DrawImage(resized, ((int)width - scaleWidth) / 2, ((int)height - scaleHeight) / 2, scaleWidth, scaleHeight);*/

            Bitmap resized = new Bitmap(image, new Size(width, height));
            return resized;
        }

        public static sitk.Image GetITKImageFromBitmap(Bitmap image)
        {
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            int bitmapByteSize = bitmapData.Stride * image.Height;
            byte[] bitmapByteArray = new byte[bitmapByteSize];

            Marshal.Copy(bitmapData.Scan0, bitmapByteArray, 0, bitmapByteSize);
            image.UnlockBits(bitmapData);

            sitk.Image sitkImage = new sitk.Image((uint)image.Width, (uint)image.Height, sitk.PixelIDValueEnum.sitkUInt8);
            IntPtr sitkImageBuffer = sitkImage.GetBufferAsUInt8();

            Marshal.Copy(bitmapByteArray, 0, sitkImageBuffer, bitmapByteSize);
            return sitkImage;
        }

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

        public static UMat CalculateHistogram(VectorOfMat vm, Image<Gray, byte> tempMask)
        {
            UMat hist = new UMat();
            int[] channel = new int[] { 0 };
            int[] histSize = new int[] { 32 };
            float[] range = new float[] { 0.0f, 256.0f };
            CvInvoke.CalcHist(vm, channel, tempMask, hist, histSize, range, false);
            return hist;
        }

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

        public static sitk.Image ResampleImage(sitk.Image img, sitk.PixelIDValueEnum pixelIDValue)
        {
            sitk.ResampleImageFilter resampleImageFilter = new sitk.ResampleImageFilter();
            resampleImageFilter.SetOutputPixelType(pixelIDValue);
            resampleImageFilter.SetOutputOrigin(img.GetOrigin());
            resampleImageFilter.SetOutputDirection(img.GetDirection());
            resampleImageFilter.SetOutputSpacing(img.GetSpacing());
            return resampleImageFilter.Execute(img);
        }
    }
}
