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
            if(parameterMap == null)
            {
                parameterMap = RegistrationUtils.GetDefaultParameterMap(parameters.RegistrationDefaultParams);
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
            SetParameters();
        }

        public NonRigidRegistration(RegistrationParameters parameters) : base(parameters)
        {
            elastix = new sitk.ElastixImageFilter();
            parameterMap = RegistrationUtils.GetDefaultParameterMap(parameters.RegistrationDefaultParams);
            //base.SetGeneralParameters();
            SetParameters();
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

        private void SetParameters()
        {
            if (registrationParameters.RegistrationDefaultParams == RegistrationDefaultParameters.bspline)
            {

            } else if (registrationParameters.RegistrationDefaultParams == RegistrationDefaultParameters.diffusion)
            {
                // todo
            } else if (registrationParameters.RegistrationDefaultParams == RegistrationDefaultParameters.spline)
            {
                // todo
            } else if (registrationParameters.RegistrationDefaultParams == RegistrationDefaultParameters.recursive)
            {
                // todo
            }

            SetPenaltyTerm();
        }

        private void SetPenaltyTerm()
        {
            if (registrationParameters.Penaltyterm != PenaltyTerm.None)
            {
                AddPenaltyTermToParameterMap(registrationParameters.Penaltyterm);

                if (registrationParameters.Penaltyterm == PenaltyTerm.TransformRigidityPenalty)
                {
                    SetTransformRigidityPenaltyParameters();
                }
                if (registrationParameters.Penaltyterm == PenaltyTerm.DistancePreservingRigidityPenalty)
                {
                    AddParameter(Constants.cSegmentedImageName, registrationParameters.SegmentedImageFilename);
                    AddParameter(Constants.cPenaltyGridSpacingInVoxels, registrationParameters.PenaltyGridSpacingInVoxels);
                }
            }
            
        }

        private void SetTransformRigidityPenaltyParameters()
        {
            if (!string.IsNullOrEmpty(registrationParameters.CoefficientMapFilename))
            {
                AddParameter(Constants.cMovingRigidityImageName, registrationParameters.CoefficientMapFilename);
            }
            if (registrationParameters.LinearityConditionWeight != 1)
            {
                AddParameter(Constants.cLinearityConditionWeight, registrationParameters.LinearityConditionWeight.ToString());
            }
            if(registrationParameters.OrthonormalityConditionWeight != 1)
            {
                AddParameter(Constants.cOrthonormalityConditionWeight, registrationParameters.OrthonormalityConditionWeight.ToString());
            }
            if (registrationParameters.PropernessConditionWeight != 1)
            {
                AddParameter(Constants.cPropernessConditionWeight, registrationParameters.PropernessConditionWeight.ToString());
            }
        }

        private void AddPenaltyTermToParameterMap(PenaltyTerm pt)
        {
            AddParameter(Constants.cRegistration, RegistrationStrategy.MultiMetricMultiResolutionRegistration.ToString());

            if(parameterMap.ContainsKey(Constants.cMetric))
            {
                sitk.VectorString metric = parameterMap[Constants.cMetric];
                metric.Add(pt.ToString());
            }

            // add weight for metric / penalty
            if(!parameterMap.ContainsKey(Constants.cMetric0Weight))
            {
                AddParameter(Constants.cMetric0Weight, "1.0");
            }
            if (!parameterMap.ContainsKey(Constants.cMetric1Weight))
            {
                AddParameter(Constants.cMetric1Weight, "1.0");
            }
        }
    }
}
