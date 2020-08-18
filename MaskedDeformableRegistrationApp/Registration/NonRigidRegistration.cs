using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaskedDeformableRegistrationApp.Utils;
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Registration
{
    internal class NonRigidRegistration : RegInitialization
    {
        public NonRigidRegistration(sitk.Image fixedImage, sitk.Image movingImage, RegistrationParameters parameters) : base(parameters)
        {
            // cast images to from pixel type uint to float
            sitk.CastImageFilter castImageFilter = new sitk.CastImageFilter();
            castImageFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkVectorFloat32);
            sitk.Image vector1 = castImageFilter.Execute(fixedImage);
            sitk.Image vector2 = castImageFilter.Execute(movingImage);

            sitk.VectorIndexSelectionCastImageFilter vectorFilter = new sitk.VectorIndexSelectionCastImageFilter();
            sitk.Image tempImage1 = vectorFilter.Execute(vector1, 0, sitk.PixelIDValueEnum.sitkFloat32);
            sitk.Image tempImage2 = vectorFilter.Execute(vector2, 0, sitk.PixelIDValueEnum.sitkFloat32);

            this.fixedImage = tempImage1;
            this.movingImage = tempImage2;

            // initiate elastix and set default registration params
            elastix = new sitk.ElastixImageFilter();
            if (parameterMap == null)
            {
                parameterMap = RegistrationUtils.GetDefaultParameterMap(parameters.RegistrationDefaultParams);
            }

            // coefficient map is used for penalty term
            if (parameters.CoefficientMapFilename != null && parameters.NonRigidOptions == MaskedNonRigidRegistrationOptions.BsplineWithPenaltyTermAndCoefficientMap)
            {
                RegistrationUtils.ChangeOrAddParamIfNotExist(ref parameterMap, "DilateRigidityImages", RegistrationUtils.GetVectorString("true"));
                RegistrationUtils.ChangeOrAddParamIfNotExist(ref parameterMap, "FixedRigidityImageName", RegistrationUtils.GetVectorString(parameters.CoefficientMapFilename));
            }

            // coefficient map is used for diffuse registration
            if (parameters.CoefficientMapFilename != null && parameters.NonRigidOptions == MaskedNonRigidRegistrationOptions.DiffuseRegistration)
            {
                RegistrationUtils.ChangeOrAddParamIfNotExist(ref parameterMap, "UseMovingSegmentation", RegistrationUtils.GetVectorString("true"));
                RegistrationUtils.ChangeOrAddParamIfNotExist(ref parameterMap, "MovingSegmentationFileName", RegistrationUtils.GetVectorString(parameters.CoefficientMapFilename));
            }

            // set output dir and log file
            outputDirectory = Path.Combine(registrationParameters.OutputDirectory, registrationParameters.Iteration.ToString());
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            elastix.SetOutputDirectory(outputDirectory);
            elastix.SetLogFileName(outputDirectory + registrationParameters.ElastixLogFileName);
            elastix.LogToConsoleOn();

            // set non rigid parameters
            //base.SetGeneralParameters();
        }

        public NonRigidRegistration(RegistrationParameters parameters) : base(parameters)
        {
            elastix = new sitk.ElastixImageFilter();
            parameterMap = RegistrationUtils.GetDefaultParameterMap(parameters.RegistrationDefaultParams);
            //base.SetGeneralParameters();
        }

        public override object Execute()
        {
            if (fixedImage != null && movingImage != null)
            {
                /*// initialize non rigid 
                sitk.BSplineTransformInitializerFilter bSplineTransformInitializer = new sitk.BSplineTransformInitializerFilter();
                // todo: calculate mesh size here
                Console.WriteLine(bSplineTransformInitializer.GetTransformDomainMeshSize().ToString());
                bSplineTransformInitializer.Execute(fixedImage);*/

                // set fixed and moving mask if defined
                if(this.fixedMask != null)
                {
                    elastix.SetFixedMask(this.fixedMask);
                    AddParameter(Constants.cFixedImagePyramid, ImagePyramid.FixedSmoothingImagePyramid);
                }

                if(this.movingMask != null)
                {
                    elastix.SetMovingMask(this.movingMask);
                    AddParameter(Constants.cMovingImagePyramid, ImagePyramid.MovingSmoothingImagePyramid);
                }

                elastix.SetParameterMap(parameterMap);

                // set fixed and moving images
                elastix.AddFixedImage(fixedImage);
                elastix.AddMovingImage(movingImage);
                elastix.WriteParameterFile(parameterMap, Path.Combine(outputDirectory, "parameters-non-rigid.txt"));

                try
                {
                    transformedImage = elastix.Execute();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occurred during registration: ");
                    Console.WriteLine(ex);
                    return ex;
                }
            }
            return null;
        }
    }
}
