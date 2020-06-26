using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Registration
{
    public static class RegistrationUtils
    {
        public static sitk.ParameterMap GetDefaultParameterMap(RegistrationDefaultParameters registrationType)
        {
            if ((int)registrationType < 4)
            {
                // rigid registration types
                if (registrationType == RegistrationDefaultParameters.similarity)
                {
                    sitk.ParameterMap parameterMap = GetParameterMap(RegistrationDefaultParameters.translation);
                    parameterMap["Transform"][0] = "SimilarityTransform";
                    return parameterMap;
                } else
                {
                    return GetParameterMap(registrationType);
                }
            } else
            {
                // non rigid registration types
                if(registrationType == RegistrationDefaultParameters.spline 
                    || registrationType == RegistrationDefaultParameters.bspline
                    || registrationType == RegistrationDefaultParameters.nonrigid)
                    return GetParameterMap(registrationType);
                else
                {
                    sitk.ParameterMap parameterMap = GetParameterMap(RegistrationDefaultParameters.nonrigid);
                    if(registrationType == RegistrationDefaultParameters.diffusion)
                    {
                        parameterMap["Transform"][0] = "BSplineTransformWithDiffusion";
                        // default parameters for bspline diffusion registration
                        parameterMap.Add("FinalGridSpacing", GetVectorString("8.0", "8.0", "8.0"));
                        parameterMap.Add("UpsampleGridOption", GetVectorString("true"));
                        parameterMap.Add("FilterPattern", GetVectorString("1"));
                        parameterMap.Add("DiffusionEachNIterations", GetVectorString("1"));
                        parameterMap.Add("AfterIterations", GetVectorString("50", "100"));
                        parameterMap.Add("HowManyIterations", GetVectorString("1", "5", "10"));
                        parameterMap.Add("NumberOfDiffusionIterations", GetVectorString("1"));
                        parameterMap.Add("Radius", GetVectorString("1"));
                        parameterMap.Add("ThresholdBool", GetVectorString("true"));
                        parameterMap.Add("ThresholdHU", GetVectorString("150"));
                        parameterMap.Add("GrayValueImageAlsoBasedOnFixedImage", GetVectorString("true"));
                        parameterMap.Add("UseFixedSegmentation", GetVectorString("false"));
                        parameterMap.Add("FixedSegmentationFileName", GetVectorString("filename"));
                        parameterMap.Add("UseMovingSegmentation", GetVectorString("false"));
                        parameterMap.Add("MovingSegmentationFileName", GetVectorString("filename"));
                        return parameterMap;
                    } else if(registrationType == RegistrationDefaultParameters.recursive)
                    {
                        parameterMap["Transform"][0] = "RecursiveBSplineTransform";
                        parameterMap.Add("PassiveEdgeWidth", GetVectorString("0"));
                        return parameterMap;
                    } else
                    {
                        return parameterMap;
                    }
                }
            }
        }

        private static sitk.VectorString GetVectorString(params string[] values)
        {
            sitk.VectorString vec = new sitk.VectorString();
            foreach(string value in values)
            {
                vec.Add(value);
            }
            return vec;
        }

        private static sitk.ParameterMap GetParameterMap(RegistrationDefaultParameters registrationType)
        {
            using (sitk.ElastixImageFilter elastix = new sitk.ElastixImageFilter())
            {
                return elastix.GetDefaultParameterMap(registrationType.ToString());
            }
        }
    }
}
