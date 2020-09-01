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
    public class ExtTransformation
    {
        private List<string> TransformParameters;
        private List<string> TransformImages;

        private bool ComposeTransforms = true;
        private Interpolator Interpolator = Interpolator.LinearInterpolation;
        private int Order = 3;
        private double DefaultPixelValue = 0.0;

        // Constructor
        public ExtTransformation(List<string> filenamesTransformParameters, List<string> imagesToTransform)
        {
            TransformParameters = filenamesTransformParameters;
            TransformImages = imagesToTransform;
        }

        /// <summary>
        /// Set compose transform type.
        /// true --> compose
        /// false --> add
        /// </summary>
        /// <param name="compose">true for compose</param>
        public void ComposeTransformsParameters(bool compose)
        {
            ComposeTransforms = compose;
        }

        /// <summary>
        /// Set interpolation type and order.
        /// Order is needed when interpolation type is bspline.
        /// </summary>
        /// <param name="type">interpolation type</param>
        /// <param name="order">order (default is 3)</param>
        public void SetInterpolationType(Interpolator type, int order = 3)
        {
            Interpolator = type;
            Order = order;
        }

        /// <summary>
        /// Set default pixel type. Default is 0.0.
        /// </summary>
        /// <param name="pixelValue">default pixel value</param>
        public void SetDefaultPixelValue(double pixelValue)
        {
            DefaultPixelValue = pixelValue;
        }

        /// <summary>
        /// Start transformation process.
        /// </summary>
        public void StartTransformation()
        {
            sitk.VectorOfParameterMap parameterMaps = PrepareTransformationParameters();

            foreach (string imageFilename in TransformImages)
            {
                using(sitk.Image img = ReadWriteUtils.ReadITKImageFromFile(imageFilename))
                {
                    List<sitk.Image> channels = TransformationUtils.SplitColorChannels(img);
                    List<sitk.Image> transformedChannels = new List<sitk.Image>();
                    foreach (sitk.Image channel in channels)
                    {
                        transformedChannels.Add(ExecuteTransform(parameterMaps, channel));
                        channel.Dispose();
                    }

                    sitk.VectorOfImage vectorImages = new sitk.VectorOfImage();
                    foreach (sitk.Image tChannel in transformedChannels)
                    {
                        vectorImages.Add(TransformationUtils.InterpolateImage(tChannel, GetInterpolatorEnum(), sitk.PixelIDValueEnum.sitkUInt8));
                    }
                    sitk.ComposeImageFilter composeImageFilter = new sitk.ComposeImageFilter();
                    sitk.Image composedImage = composeImageFilter.Execute(vectorImages);

                    TransformationUtils.WriteTransformedImage(composedImage, ApplicationContext.OutputPath + "\\" + Path.GetFileNameWithoutExtension(imageFilename) + "_transformed.png");
                }
            }

        }

        /// <summary>
        /// Get interpolation type as sitk.InterpolationEnum.
        /// </summary>
        /// <returns>interpolation type</returns>
        private sitk.InterpolatorEnum GetInterpolatorEnum()
        {
            if (Interpolator == Interpolator.LinearInterpolation)
            {
                return sitk.InterpolatorEnum.sitkLinear;
            }
            else if (Interpolator == Interpolator.NearestNighbour)
            {
                return sitk.InterpolatorEnum.sitkNearestNeighbor;
            } else
            {
                switch (Order)
                {
                    case 0: return sitk.InterpolatorEnum.sitkBSplineResampler;
                    case 1: return sitk.InterpolatorEnum.sitkBSplineResamplerOrder1;
                    case 2: return sitk.InterpolatorEnum.sitkBSplineResamplerOrder2;
                    case 3: return sitk.InterpolatorEnum.sitkBSplineResamplerOrder3;
                    case 4: return sitk.InterpolatorEnum.sitkBSplineResamplerOrder4;
                    case 5: return sitk.InterpolatorEnum.sitkBSplineResamplerOrder5;
                    default: return sitk.InterpolatorEnum.sitkBSpline;
                }
            }
        }

        /// <summary>
        /// Execute actual transformation.
        /// </summary>
        /// <param name="parameterMaps">parameter maps</param>
        /// <param name="channel">image channel as grayscale image</param>
        /// <returns></returns>
        private sitk.Image ExecuteTransform(sitk.VectorOfParameterMap parameterMaps, sitk.Image channel)
        {

            using (sitk.TransformixImageFilter transformix = new sitk.TransformixImageFilter())
            {
                transformix.SetTransformParameterMap(parameterMaps);
                transformix.SetMovingImage(channel);
                transformix.SetLogToConsole(true);
                transformix.SetOutputDirectory(ApplicationContext.OutputPath);
                return transformix.Execute();
            }
        }

        /// <summary>
        /// Prepare transformation parameters and set initial transform if more than one parameter file.
        /// </summary>
        /// <returns>transformation parameter maps</returns>
        private sitk.VectorOfParameterMap PrepareTransformationParameters()
        {
            sitk.VectorOfParameterMap resultMap = new sitk.VectorOfParameterMap();

            using (sitk.TransformixImageFilter transformix = new sitk.TransformixImageFilter())
            {
                foreach (string parameterFilename in TransformParameters)
                {
                    resultMap.Add(transformix.ReadParameterFile(parameterFilename));
                }
            }

            // TODO: modify list

            return resultMap;
        }
    }
}
