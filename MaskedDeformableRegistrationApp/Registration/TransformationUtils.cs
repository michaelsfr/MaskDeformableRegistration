using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Registration
{
    public static class TransformationUtils
    {
        /// <summary>
        /// Split color channels of a rgb image and return a list with channels as sitk.Image.
        /// list[0] = red channel
        /// list[1] = blue channel
        /// list[2] = green channel
        /// </summary>
        /// <param name="img">color image (rgb)</param>
        /// <returns>list with three color channels</returns>
        public static List<sitk.Image> SplitColorChannels(sitk.Image img)
        {
            if (img.GetNumberOfComponentsPerPixel() >= 3)
            {
                List<sitk.Image> result = new List<sitk.Image>();
                sitk.VectorIndexSelectionCastImageFilter rgbVector = new sitk.VectorIndexSelectionCastImageFilter();
                result.Add(rgbVector.Execute(img, 0, sitk.PixelIDValueEnum.sitkFloat32));
                result.Add(rgbVector.Execute(img, 1, sitk.PixelIDValueEnum.sitkFloat32));
                result.Add(rgbVector.Execute(img, 2, sitk.PixelIDValueEnum.sitkFloat32));
                return result;
            } else
            {
                return null;
            }
        }

        /// <summary>
        /// Get given color channel as a grayscale image.
        /// </summary>
        /// <param name="img">image</param>
        /// <param name="channel">color channel (r, g, b)</param>
        /// <returns>grayscale image</returns>
        public static sitk.Image GetColorChannelAsImage(sitk.Image img, ColorChannel channel)
        {
            sitk.Image result = null;
            sitk.VectorIndexSelectionCastImageFilter rgbVector = new sitk.VectorIndexSelectionCastImageFilter();

            switch (channel)
            {
                case ColorChannel.R:
                    result = rgbVector.Execute(img, 0, sitk.PixelIDValueEnum.sitkFloat32); break;
                case ColorChannel.G:
                    result = rgbVector.Execute(img, 1, sitk.PixelIDValueEnum.sitkFloat32); break;
                case ColorChannel.B:
                    result = rgbVector.Execute(img, 2, sitk.PixelIDValueEnum.sitkFloat32); break;
            }

            return result == null? img : result;
        }

        /// <summary>
        /// Interpolate image by interpolation type and output pixel type.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="interpolator"></param>
        /// <param name="pixelIDValueEnum"></param>
        /// <returns>interpolated image</returns>
        public static sitk.Image InterpolateImage(sitk.Image img, sitk.InterpolatorEnum interpolator, sitk.PixelIDValueEnum pixelIDValueEnum, double defaultPixelType = 0.0)
        {
            sitk.ResampleImageFilter resampleImageFilter = new sitk.ResampleImageFilter();
            resampleImageFilter.SetSize(img.GetSize());
            resampleImageFilter.SetOutputOrigin(img.GetOrigin());
            resampleImageFilter.SetOutputDirection(img.GetDirection());
            resampleImageFilter.SetOutputSpacing(img.GetSpacing());
            resampleImageFilter.SetInterpolator(interpolator);
            resampleImageFilter.SetOutputPixelType(pixelIDValueEnum);
            resampleImageFilter.SetDefaultPixelValue(defaultPixelType);
            return resampleImageFilter.Execute(img);
        }

        /// <summary>
        /// Write transformed file to disk.
        /// </summary>
        /// <param name="imagename">filename</param>
        public static void WriteTransformedImage(sitk.Image img, string fullFilename)
        {
            sitk.CastImageFilter castImageFilter = new sitk.CastImageFilter();
            castImageFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkVectorUInt8);
            sitk.Image temp = castImageFilter.Execute(img);

            sitk.ImageFileWriter writer = new sitk.ImageFileWriter();
            writer.SetFileName(fullFilename);
            writer.Execute(temp);
        }
    }
}
