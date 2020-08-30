using System;
using System.Collections.Generic;
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
            if (img.GetDimension() == 3)
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
    }
}
