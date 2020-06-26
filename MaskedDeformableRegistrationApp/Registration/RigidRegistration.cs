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
        private RegistrationParameters registrationParameters;

        public RigidRegistration(sitk.Image fixedImage, sitk.Image movingImage, RegistrationParameters parameters) : base(fixedImage, movingImage)
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
            parameterMap = elastix.GetDefaultParameterMap(RegistrationDefaultParameters.rigid.ToString(), 10);
        }

        public override void Execute()
        {
            if(fixedImage != null && movingImage != null)
            {
                // set output dir
                string outputDirectory = Path.Combine(ApplicationContext.OutputPath, registrationParameters.SubDirectory);
                if(!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                elastix.SetOutputDirectory(outputDirectory);
                elastix.SetLogFileName(Path.Combine(outputDirectory, registrationParameters.ElastixLogFileName));

                // set image masks
                if (fixedMask != null)
                {
                    elastix.SetFixedMask(fixedMask);
                    foreach (var parameter in parameterMap.AsEnumerable())
                    {
                        if(parameter.Key == "FixedImagePyramid")
                        {
                            parameter.Value[0] = "FixedSmoothingImagePyramid";
                        }
                    }
                }

                if (movingMask != null)
                {
                    elastix.SetMovingMask(movingMask);
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
                } finally
                {
                    elastix.Dispose();
                }
            }
        }
    }
}
