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
            this.fixedImage = CastImage(fixedImage);
            this.movingImage = CastImage(movingImage);
            this.registrationParameters = parameters;
            
            elastix = new sitk.ElastixImageFilter();
            if (parameterMap == null)
            {
                parameterMap = RegistrationUtils.GetDefaultParameterMap(parameters.RegistrationDefaultParams);
            }

            // set output dir
            outputDirectory = Path.Combine(registrationParameters.OutputDirectory, registrationParameters.Iteration.ToString());
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            elastix.SetOutputDirectory(outputDirectory);
            //elastix.SetLogFileName(Path.Combine(outputDirectory, registrationParameters.ElastixLogFileName));
            elastix.LogToFileOn();

            //base.SetGeneralParameters();
        }

        private sitk.Image CastImage(sitk.Image img)
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

        public RigidRegistration(RegistrationParameters parameters) : base(parameters)
        {
            elastix = new sitk.ElastixImageFilter();
            parameterMap = RegistrationUtils.GetDefaultParameterMap(parameters.RegistrationDefaultParams);
        }

        public override object Execute()
        {
            if(fixedImage != null && movingImage != null)
            {
                // set parameter vector here?
               elastix.SetParameterMap(parameterMap);

                // only use when kappa statistics is used as metric
                if (registrationParameters.RigidOptions != MaskedRigidRegistrationOptions.None)
                {
                    fixedImage = ImageUtils.Binarize(fixedImage);
                    movingImage = ImageUtils.Binarize(movingImage);
                }

                // set fixed and moving images
                elastix.AddFixedImage(fixedImage);
                if (fixedMask != null)
                {
                    elastix.SetFixedMask(fixedMask);
                    AddParameter(Constants.cFixedImagePyramid, ImagePyramid.FixedSmoothingImagePyramid);
                }

                elastix.AddMovingImage(movingImage);
                if (movingMask != null)
                {
                    elastix.SetMovingMask(movingMask);
                    AddParameter(Constants.cMovingImagePyramid, ImagePyramid.MovingSmoothingImagePyramid);
                }

                elastix.WriteParameterFile(parameterMap, Path.Combine(outputDirectory, "parameters.txt"));

                try
                {
                    transformedImage = elastix.Execute();
                } catch(Exception ex)
                {
                    Console.WriteLine("Exception occurred during elastix registration: ");
                    Console.WriteLine(ex);
                    return ex.Message;
                }
            }
            return null;
        }
    }
}
