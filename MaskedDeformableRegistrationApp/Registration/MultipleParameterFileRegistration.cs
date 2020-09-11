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
    class MultipleParameterFileRegistration : RegInitialization
    {
        public MultipleParameterFileRegistration(sitk.Image fixedImage, sitk.Image movingImage, RegistrationParameters parameters) : base(parameters)
        {
            this.fixedImage = ImageUtils.CastImage(fixedImage);
            this.movingImage = ImageUtils.CastImage(movingImage);

            elastix = new sitk.ElastixImageFilter();

            for (int i = 0; i < parameters.ParameterFiles.Count; i++)
            {
                if (i == 0) elastix.SetParameterMap(elastix.ReadParameterFile(parameters.ParameterFiles[i]));
                else elastix.AddParameterMap(elastix.ReadParameterFile(parameters.ParameterFiles[i]));
            }

            // set output dir
            outputDirectory = Path.Combine(registrationParameters.OutputDirectory, "multiple_" + DateTime.Now.ToShortDateString());
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            elastix.SetOutputDirectory(outputDirectory);
            //elastix.SetLogFileName(Path.Combine(outputDirectory, registrationParameters.ElastixLogFileName));
            elastix.LogToFileOn();
        }

        public override object Execute()
        {
            if (fixedImage != null && movingImage != null)
            {
                // set fixed and moving images
                elastix.AddFixedImage(fixedImage);
                if (fixedMask != null)
                {
                    elastix.SetFixedMask(fixedMask);
                }

                elastix.AddMovingImage(movingImage);
                if (movingMask != null)
                {
                    elastix.SetMovingMask(movingMask);
                }

                elastix.WriteParameterFile(parameterMap, Path.Combine(outputDirectory, "parameters.txt"));

                try
                {
                    transformedImage = elastix.Execute();
                    return transformedImage;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occurred during elastix registration: ");
                    Console.WriteLine(ex);
                    return ex.Message;
                }
            } else
            {
                return null;
            }
        }
    }
}
