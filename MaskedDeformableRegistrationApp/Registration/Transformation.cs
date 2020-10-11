using MaskedDeformableRegistrationApp.Utils;
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

        private List<sitk.VectorOfParameterMap> parameterMaps = null;
        private sitk.InterpolatorEnum interpolationType = sitk.InterpolatorEnum.sitkLinear;

        private RegistrationParameters registrationParameters = null;

        private int interpolationOrder = -1;

        public TransformRGB(sitk.Image movingImage, List<sitk.VectorOfParameterMap> parameterMaps, RegistrationParameters parameters)
        {
            this.movingImage = movingImage;
            this.parameterMaps = parameterMaps;
            this.registrationParameters = parameters;

            transformix = new sitk.TransformixImageFilter();
        }

        /// <summary>
        /// Execute transformation of rgb image.
        /// </summary>
        public void Execute()
        {
            string outputDir = ReadWriteUtils.GetOutputDirectory(registrationParameters, registrationParameters.Iteration);
            if (Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // split rgb channels
            sitk.VectorIndexSelectionCastImageFilter rgbVector = new sitk.VectorIndexSelectionCastImageFilter();
            sitk.Image redChannel = rgbVector.Execute(movingImage, 0, sitk.PixelIDValueEnum.sitkFloat32);
            sitk.Image greenChannel = rgbVector.Execute(movingImage, 1, sitk.PixelIDValueEnum.sitkFloat32);
            sitk.Image blueChannel = rgbVector.Execute(movingImage, 2, sitk.PixelIDValueEnum.sitkFloat32);

            foreach (var parameter in parameterMaps[0])
            {
                parameter["DefaultPixelValue"][0] = "255.0";
                parameter["ResultImagePixelType"][0] = "short";

                if (parameter.ContainsKey("UseBinaryFormatForTransformationParameters"))
                    parameter.Remove("UseBinaryFormatForTransformationParameters");

                if (interpolationOrder != -1)
                {
                    parameter["FinalBSplineInterpolationOrder"][0] = interpolationOrder.ToString();
                }
                else
                {
                    parameter["FinalBSplineInterpolationOrder"][0] = "3";
                }

                if (parameter["Transform"][0] == "SimilarityTransform" 
                    || registrationParameters.RegistrationDefaultParams == RegistrationDefaultParameters.similarity)
                {
                    sitk.VectorString vec = new sitk.VectorString();
                    vec.Add((registrationParameters.LargestImageWidth / 2).ToString()); // fixed image width / 2
                    vec.Add((registrationParameters.LargestImageHeight / 2).ToString()); // fixed image height / 2
                    parameter.Add("CenterOfRotationPoint", vec);
                }
            }

            // initialize transformix
            transformix.SetOutputDirectory(outputDir);
            transformix.SetTransformParameterMap(parameterMaps.First()[0]);
            // add further transform parameters
            if (parameterMaps[0].Count > 1)
            {
                for (int i = 1; i < parameterMaps[0].Count; i++)
                {
                    var vectorParameterMap = parameterMaps[0][i];
                    transformix.AddTransformParameterMap(vectorParameterMap);
                }
            }

            transformix.ComputeDeformationFieldOn();
            transformix.LogToFileOn();

            if (registrationParameters.ComputeJaccobian)
            {
                transformix.ComputeSpatialJacobianOn();
                transformix.ComputeDeterminantOfSpatialJacobianOn();
            }

            transformix.PrintParameterMap();

            // red
            transformix.SetMovingImage(redChannel);
            sitk.Image resultRedChannel = transformix.Execute();
            resultRedChannel = TransformationUtils.InterpolateImage(resultRedChannel, sitk.InterpolatorEnum.sitkBSplineResamplerOrder3, sitk.PixelIDValueEnum.sitkUInt8);
            //ReadWriteUtils.WriteSitkImageWithPreCast(resultRedChannel, registrationParameters.OutputDirectory + "\\red_channel.png");

            // green
            transformix.SetMovingImage(greenChannel);
            sitk.Image resultGreenChannel = transformix.Execute();
            resultGreenChannel = TransformationUtils.InterpolateImage(resultGreenChannel, sitk.InterpolatorEnum.sitkBSplineResamplerOrder3, sitk.PixelIDValueEnum.sitkUInt8);
            //ReadWriteUtils.WriteSitkImageWithPreCast(resultGreenChannel, registrationParameters.OutputDirectory + "\\green_channel.png");

            // blue
            transformix.SetMovingImage(blueChannel);
            sitk.Image resultBlueChannel = transformix.Execute();
            resultBlueChannel = TransformationUtils.InterpolateImage(resultBlueChannel, sitk.InterpolatorEnum.sitkBSplineResamplerOrder3, sitk.PixelIDValueEnum.sitkUInt8);
            //ReadWriteUtils.WriteSitkImageWithPreCast(resultBlueChannel, registrationParameters.OutputDirectory + "\\blue_channel.png");

            // compose image channels
            sitk.VectorOfImage vectorImages = new sitk.VectorOfImage();
            vectorImages.Add(resultRedChannel);
            vectorImages.Add(resultGreenChannel);
            vectorImages.Add(resultBlueChannel);
            sitk.ComposeImageFilter composeImageFilter = new sitk.ComposeImageFilter();
            sitk.Image composedImage = composeImageFilter.Execute(vectorImages);

            if (registrationParameters.Type == RegistrationType.NonRigid)
            {
                transformedImage = TransformationUtils.InterpolateImage(composedImage, sitk.InterpolatorEnum.sitkBSplineResamplerOrder5, composedImage.GetPixelID());
            }
            else
            {
                transformedImage = composedImage;
            }

            transformedImage = composedImage;

            // interpolation of output image (needs to be improved -> currently theres a little loss in quality)
            // possible solution: interpolate grayscale images and compose afterwards
            /*sitk.ExpandImageFilter expandImageFilter = new sitk.ExpandImageFilter();
            expandImageFilter.SetInterpolator(sitk.InterpolatorEnum.sitkLinear);
            transformedImage = expandImageFilter.Execute(composedImage);*/

            //transformedImage = composedImage;
        }

        /// <summary>
        /// Add vector of parameter maps to transformix.
        /// </summary>
        /// <param name="vectorOfParametermap">vector of parameter maps</param>
        public void AddVectorOfParameterMap(sitk.VectorOfParameterMap vectorOfParametermap)
        {
            foreach (sitk.ParameterMap map in vectorOfParametermap.AsEnumerable())
            {
                transformix = transformix.AddTransformParameterMap(map);
            }
        }

        /// <summary>
        /// Get transformed image.
        /// </summary>
        /// <returns></returns>
        public sitk.Image GetOutput()
        {
            return this.transformedImage;
        }

        /// <summary>
        /// Get deformation field.
        /// </summary>
        /// <returns></returns>
        public sitk.Image GetDeformationField()
        {
            if(transformix != null)
            {
                return transformix.GetDeformationField();
            }
            return null;
        }

        /// <summary>
        /// Set interpolation type manually.
        /// </summary>
        /// <param name="interpolationType">interpolation method</param>
        public void SetInterpolationType(sitk.InterpolatorEnum interpolationType)
        {
            this.interpolationType = interpolationType;
        }

        public void SetInterpolationOrder(int order)
        {
            this.interpolationOrder = order;
        }

        /// <summary>
        /// Write transformed image to disk.
        /// </summary>
        /// <param name="imagename">image filename</param>
        public void WriteTransformedImage(string imagename)
        {
            sitk.CastImageFilter castImageFilter = new sitk.CastImageFilter();
            castImageFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkVectorUInt8);
            sitk.Image temp = castImageFilter.Execute(transformedImage);

            sitk.ImageFileWriter writer = new sitk.ImageFileWriter();
            writer.SetFileName(Path.Combine(registrationParameters.OutputDirectory, imagename));
            writer.Execute(temp);
        }

        /// <summary>
        /// Dispose used instances.
        /// </summary>
        public void Dispose()
        {
            transformix.Dispose();
            movingImage.Dispose();
            transformedImage.Dispose();
        }
    }
}
