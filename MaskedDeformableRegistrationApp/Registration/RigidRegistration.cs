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
    class RigidRegistration : RegInitialization
    {
        public RigidRegistration(sitk.Image fixedImage, sitk.Image movingImage, RegistrationParameters parameters) : base(parameters)
        {
            sitk.CastImageFilter castImageFilter = new sitk.CastImageFilter();
            castImageFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkVectorFloat32);
            sitk.Image vector1 = castImageFilter.Execute(fixedImage);
            sitk.Image vector2 = castImageFilter.Execute(movingImage);

            sitk.VectorIndexSelectionCastImageFilter vectorFilter = new sitk.VectorIndexSelectionCastImageFilter();
            sitk.Image tempImage1 = vectorFilter.Execute(vector1, 0, sitk.PixelIDValueEnum.sitkFloat32);
            sitk.Image tempImage2 = vectorFilter.Execute(vector2, 0, sitk.PixelIDValueEnum.sitkFloat32);

            this.fixedImage = tempImage1;
            this.movingImage = tempImage2;
            this.registrationParameters = parameters;

            vector1.Dispose();
            vector2.Dispose();
            vectorFilter.Dispose();
            
            elastix = new sitk.ElastixImageFilter();
            parameterMap = RegistrationUtils.GetDefaultParameterMap(parameters.RegistrationType);

            // set output dir
            outputDirectory = Path.Combine(ApplicationContext.OutputPath, registrationParameters.SubDirectory);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            elastix.SetOutputDirectory(outputDirectory);
            elastix.SetLogFileName(Path.Combine(outputDirectory, registrationParameters.ElastixLogFileName));

            base.SetGeneralParameters();
        }

        public RigidRegistration(RegistrationParameters parameters) : base(parameters)
        {
            elastix = new sitk.ElastixImageFilter();
            parameterMap = RegistrationUtils.GetDefaultParameterMap(parameters.RegistrationType);
            base.SetGeneralParameters();

        }

        public override void Execute()
        {
            if(fixedImage != null && movingImage != null)
            {
                // set image masks
                if (fixedMask != null)
                {
                    elastix.SetFixedMask(fixedMask);
                    AddParameter(Constants.cFixedImagePyramid, ImagePyramid.FixedSmoothingImagePyramid);
                }

                if (movingMask != null)
                {
                    elastix.SetMovingMask(movingMask);
                    AddParameter(Constants.cMovingImagePyramid, ImagePyramid.MovingSmoothingImagePyramid);
                }

                // set parameter vector here?
                elastix.SetParameterMap(parameterMap);

                // set fixed and moving images
                elastix.AddFixedImage(fixedImage);
                elastix.AddMovingImage(movingImage);
                elastix.WriteParameterFile(parameterMap, Path.Combine(outputDirectory, "parameters.txt"));

                try
                {
                    transformedImage = elastix.Execute();
                } catch(Exception ex)
                {
                    Console.WriteLine("Exception occurred during registration: ");
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
