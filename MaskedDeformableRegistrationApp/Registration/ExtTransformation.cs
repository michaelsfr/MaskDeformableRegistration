using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
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
        private double DefaultPixelValue = 0;

        public ExtTransformation(List<string> filenamesTransformParameters, List<string> imagesToTransform)
        {
            TransformParameters = filenamesTransformParameters;
            TransformImages = imagesToTransform;
        }

        public void ComposeTransformsParameters(bool compose)
        {
            ComposeTransforms = compose;
        }

        public void SetInterpolationType(Interpolator type, int order = 3)
        {
            Interpolator = type;
            Order = order;
        }

        public void SetDefaultPixelValue(double pixelValue)
        {
            DefaultPixelValue = pixelValue;
        }

        public void StartTransformation()
        {
            sitk.VectorOfParameterMap parameterMaps = PrepareTransformationParameters();

            foreach (string imageFilename in TransformImages)
            {
                using(sitk.Image img = ReadWriteUtils.ReadITKImageFromFile(imageFilename))
                {
                    List<sitk.Image> channels = TransformationUtils.SplitColorChannels(img);

                    foreach(sitk.Image channel in channels)
                    {
                        sitk.Image resultImage = ExecuteTransform(parameterMaps, channel);

                    }
                }
            }

        }

        private static sitk.Image ExecuteTransform(sitk.VectorOfParameterMap parameterMaps, sitk.Image channel)
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

        private sitk.VectorOfParameterMap PrepareTransformationParameters()
        {
            sitk.VectorOfParameterMap resultMap = new sitk.VectorOfParameterMap();

            sitk.TransformixImageFilter transformix = new sitk.TransformixImageFilter();

            foreach (string parameterFilename in TransformParameters)
            {
                resultMap.Add(transformix.ReadParameterFile(parameterFilename));
            }

            transformix.Dispose();

            // TODO: modify list

            return resultMap;
        }

        public void Dispose()
        {
            // TODO
        }
    }
}
