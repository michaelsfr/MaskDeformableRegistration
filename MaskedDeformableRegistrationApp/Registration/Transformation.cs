using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Registration
{
    public class TransformRGB
    {
        private sitk.TransformixImageFilter transformix = null;

        private sitk.Image movingImage = null;
        private sitk.Image transformedImage = null;

        private sitk.VectorOfParameterMap parameterMaps = null;
        private sitk.InterpolatorEnum interpolationType = sitk.InterpolatorEnum.sitkLinear;

        private string outputDirectory = null;

        private int interpolationOrder = -1; 

        public TransformRGB(sitk.Image movingImage, sitk.VectorOfParameterMap parameterMaps, string outputDirectory)
        {
            this.movingImage = movingImage;
            this.parameterMaps = parameterMaps;
            this.outputDirectory = outputDirectory;

            transformix = new sitk.TransformixImageFilter();
        }

        public void Execute()
        {
            if(Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // split rgb channels
            sitk.VectorIndexSelectionCastImageFilter rgbVector = new sitk.VectorIndexSelectionCastImageFilter();
            sitk.Image redChannel = rgbVector.Execute(movingImage, 0, sitk.PixelIDValueEnum.sitkVectorUInt8);
            sitk.Image greenChannel = rgbVector.Execute(movingImage, 1, sitk.PixelIDValueEnum.sitkVectorUInt8);
            sitk.Image blueChannel = rgbVector.Execute(movingImage, 2, sitk.PixelIDValueEnum.sitkVectorUInt8);

            foreach(var parameter in parameterMaps.AsEnumerable())
            {
                // will not be overwritten at the moment? todo

                // resize moving image and set default pixels to white
                uint mWidth = movingImage.GetWidth();
                uint mHeight = movingImage.GetHeight();
                uint pWidth = Convert.ToUInt32(parameter["Size"][0]);
                uint pHeight = Convert.ToUInt32(parameter["Size"][1]);
                parameter["Size"][0] = mWidth > pWidth ? mWidth.ToString() : pWidth.ToString();
                parameter["Size"][1] = mHeight > pHeight ? mHeight.ToString() : pHeight.ToString();
                parameter["DefaultPixelValue"][0] = "255.0";
                
                if(interpolationOrder != -1)
                {
                    parameter["FinalBSplineInterpolationOrder"][0] = interpolationOrder.ToString();
                }
            }
            // initialize transformix
            transformix.SetOutputDirectory(outputDirectory);
            transformix.SetTransformParameterMap(parameterMaps);
            transformix.ComputeDeformationFieldOn();

            // red
            transformix.SetMovingImage(redChannel);
            sitk.Image resultRedChannel = transformix.Execute();
            //sitk.ExpandImageFilter expandImageFilterRed = new sitk.ExpandImageFilter();
            //expandImageFilterRed.SetInterpolator(interpolationType);
            //resultRedChannel = expandImageFilterRed.Execute(resultRedChannel);

            // green
            transformix.SetMovingImage(greenChannel);
            sitk.Image resultGreenChannel = transformix.Execute();
            //sitk.ExpandImageFilter expandImageFilterGreen = new sitk.ExpandImageFilter();
            //expandImageFilterGreen.SetInterpolator(interpolationType);
            //resultGreenChannel = expandImageFilterGreen.Execute(resultGreenChannel);

            // blue
            transformix.SetMovingImage(blueChannel);
            sitk.Image resultBlueChannel = transformix.Execute();
            //sitk.ExpandImageFilter expandImageFilterBlue = new sitk.ExpandImageFilter();
            //expandImageFilterBlue.SetInterpolator(interpolationType);
            //resultBlueChannel = expandImageFilterBlue.Execute(resultBlueChannel);

            // compose image channels
            sitk.VectorOfImage vectorImages = new sitk.VectorOfImage();
            vectorImages.Add(resultRedChannel);
            vectorImages.Add(resultGreenChannel);
            vectorImages.Add(resultBlueChannel);
            sitk.ComposeImageFilter composeImageFilter = new sitk.ComposeImageFilter();
            sitk.Image composedImage = composeImageFilter.Execute(vectorImages);

            // interpolation of output image (needs to be improved -> currently theres a little loss in quality)
            // possible solution: interpolate grayscale images and compose afterwards
            /*sitk.ExpandImageFilter expandImageFilter = new sitk.ExpandImageFilter();
            expandImageFilter.SetInterpolator(sitk.InterpolatorEnum.sitkBSplineResamplerOrder3);
            transformedImage = expandImageFilter.Execute(composedImage);*/

            transformedImage = composedImage;
        }

        public sitk.Image GetOutput()
        {
            return this.transformedImage;
        }

        public sitk.Image GetDeformationField()
        {
            if(transformix != null)
            {
                return transformix.GetDeformationField();
            }
            return null;
        }

        public void SetInterpolationType(sitk.InterpolatorEnum interpolationType)
        {
            this.interpolationType = interpolationType;
        }

        public void SetInterpolationOrder(int order)
        {
            this.interpolationOrder = order;
        }

        public void WriteTransformedImage(string imagename)
        {
            sitk.CastImageFilter castImageFilter = new sitk.CastImageFilter();
            castImageFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkVectorUInt8);
            sitk.Image temp = castImageFilter.Execute(transformedImage);

            sitk.ImageFileWriter writer = new sitk.ImageFileWriter();
            writer.SetFileName(Path.Combine(outputDirectory, imagename));
            writer.Execute(temp);
        }

        public void Dispose()
        {
            transformix.Dispose();
            movingImage.Dispose();
            transformedImage.Dispose();
        }
    }
}
