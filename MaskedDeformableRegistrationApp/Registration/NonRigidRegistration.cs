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
        private RegistrationParameters registrationParameters;

        public NonRigidRegistration(sitk.Image fixedImage, sitk.Image movingImage, RegistrationParameters parameters) : base(fixedImage, movingImage)
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

            elastix = new sitk.ElastixImageFilter();
            parameterMap = elastix.GetDefaultParameterMap(RegistrationDefaultParameters.bspline.ToString(), 5);
        }

        public override void Execute()
        {
            if (fixedImage != null && movingImage != null)
            {
                // set output dir
                string outputDirectory = Path.Combine(ApplicationContext.OutputPath, registrationParameters.SubDirectory);
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                elastix.SetOutputDirectory(outputDirectory);
                elastix.SetLogFileName(outputDirectory + registrationParameters.ElastixLogFileName);

                sitk.BSplineTransformInitializerFilter bSplineTransformInitializer = new sitk.BSplineTransformInitializerFilter();
                // todo: calculate mesh size here
                Console.WriteLine(bSplineTransformInitializer.GetTransformDomainMeshSize().ToString());
                bSplineTransformInitializer.Execute(fixedImage);

                if(this.fixedMask != null)
                {
                    elastix.SetFixedMask(this.fixedMask);
                }

                if(this.movingMask != null)
                {
                    elastix.SetMovingMask(this.movingMask);
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
                } finally
                {
                    //elastix.Dispose();
                }
            }
        }

        public void SetOptimizer(Optimizer optimizer, sitk.VectorString numberOfIterationsEachResolution)
        {
            // todo
        }

        public void SetRigidyPenaltyTerm(PenaltyTerm penalty, string movingPenalizedAreas = null, string movingRigidityMask = null)
        {
            foreach (var parameter in parameterMap.AsEnumerable())
            {
                if (parameter.Key == Constants.cRegistration)
                {
                    parameter.Value[0] = RegistrationStrategy.MultiMetricMultiResolutionRegistration.ToString();
                }
                if (parameter.Key == Constants.cMetric)
                {
                    parameter.Value[1] = penalty.ToString();
                }
            }

            if (penalty == PenaltyTerm.TransformRigidityPenalty
                  && movingRigidityMask != null)
            {
                sitk.Image img = ReadWriteUtils.ReadITKImageFromFile(movingRigidityMask);
                sitk.Image img02 = ReadWriteUtils.RescaleImageToFloat(img);
                ReadWriteUtils.WriteSitkImage(img02, movingRigidityMask);
                img.Dispose();
                img02.Dispose();


                sitk.VectorString vec = new sitk.VectorString();
                vec.Add(movingRigidityMask);
                parameterMap.Add(Constants.cMovingRigidityImageName, vec);

                // set orthogonality / linearity / correctness
            }

            // set parameters for specific penalty terms
        }
    }
}
