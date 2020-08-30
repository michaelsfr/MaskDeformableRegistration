using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
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
    public static class RegistrationUtils
    {
        /// <summary>
        /// Get a default parameter map for a specific registration type.
        /// </summary>
        /// <param name="type">registration type</param>
        /// <returns>sitk parameter map</returns>
        public static sitk.ParameterMap GetDefaultParameterMap(RegistrationDefaultParameters type)
        {
            if ((int)type < 4)
            {
                // rigid registration types
                if (type == RegistrationDefaultParameters.similarity)
                {
                    sitk.ParameterMap parameterMap = GetParameterMap(RegistrationDefaultParameters.translation);
                    parameterMap["Transform"][0] = "SimilarityTransform";
                    return parameterMap;
                } else
                {
                    return GetParameterMap(type);
                }
            } else
            {
                // non rigid registration types
                if(type == RegistrationDefaultParameters.spline 
                    || type == RegistrationDefaultParameters.bspline
                    || type == RegistrationDefaultParameters.nonrigid)
                    return GetDefaultNonRigidParameterMap();
                else
                {
                    sitk.ParameterMap parameterMap = GetDefaultNonRigidParameterMap();
                    if (type == RegistrationDefaultParameters.diffusion)
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
                        //parameterMap.Add("UseFixedSegmentation", GetVectorString("false"));
                        //parameterMap.Add("FixedSegmentationFileName", GetVectorString("filename"));
                        parameterMap.Add("UseMovingSegmentation", GetVectorString("false"));
                        parameterMap.Add("MovingSegmentationFileName", GetVectorString("filename"));
                        return parameterMap;
                    } else if(type == RegistrationDefaultParameters.recursive)
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

        /// <summary>
        /// Convert an arbitrary number of string values to a sitk.VectorString.
        /// </summary>
        /// <param name="values">string values</param>
        /// <returns>simple itk vector of strings</returns>
        public static sitk.VectorString GetVectorString(params string[] values)
        {
            sitk.VectorString vec = new sitk.VectorString();
            foreach(string value in values)
            {
                vec.Add(value);
            }
            return vec;
        }

        /// <summary>
        /// Get default parameter map from elastix image filter for registration type.
        /// </summary>
        /// <param name="registrationType">registration type</param>
        /// <returns>default params</returns>
        private static sitk.ParameterMap GetParameterMap(RegistrationDefaultParameters registrationType)
        {
            using (sitk.ElastixImageFilter elastix = new sitk.ElastixImageFilter())
            {
                return elastix.GetDefaultParameterMap(registrationType.ToString());
            }
        }

        /// <summary>
        /// Create default non rigid parameter map.
        /// </summary>
        /// <returns>default paramerets</returns>
        private static sitk.ParameterMap GetDefaultNonRigidParameterMap()
        {
            sitk.ParameterMap paramMap = new sitk.ParameterMap();
            paramMap.Add("FixedInternalImagePixelType", GetVectorString("float"));
            paramMap.Add("MovingInternalImagePixelType", GetVectorString("float"));
            paramMap.Add("FixedImageDimension", GetVectorString("2"));
            paramMap.Add("MovingImageDimension", GetVectorString("2"));
            paramMap.Add("UseDirectionCosines", GetVectorString("true"));
            paramMap.Add("Registration", GetVectorString("MultiResolutionRegistration"));
            paramMap.Add("Interpolator", GetVectorString("BSplineInterpolator"));
            paramMap.Add("ResampleInterpolator", GetVectorString("FinalBSplineInterpolator"));
            paramMap.Add("Resampler", GetVectorString("DefaultResampler"));
            paramMap.Add("FixedImagePyramid", GetVectorString("FixedRecursiveImagePyramid"));
            paramMap.Add("MovingImagePyramid", GetVectorString("MovingRecursiveImagePyramid"));
            paramMap.Add("Optimizer", GetVectorString("AdaptiveStochasticGradientDescent"));
            paramMap.Add("Transform", GetVectorString("BSplineTransform"));
            paramMap.Add("Metric", GetVectorString("AdvancedMeanSquares"));
            paramMap.Add("FinalGridSpacingInPhysicalUnits", GetVectorString("16"));
            paramMap.Add("GridSpacingSchedule", GetVectorString("5.0", "5.0", "4.0", "4.0", "3.0", "3.0", "2.0", "2.0", "1.0", "1.0"));
            paramMap.Add("HowToCombineTransforms", GetVectorString("Compose"));
            paramMap.Add("ErodeMask", GetVectorString("false"));
            paramMap.Add("NumberOfResolutions", GetVectorString("5"));
            paramMap.Add("MaximumNumberOfIterations", GetVectorString("1024"));
            paramMap.Add("NumberOfSpatialSamples", GetVectorString("2048"));
            paramMap.Add("NewSamplesEveryIteration", GetVectorString("true"));
            paramMap.Add("ImageSampler", GetVectorString("Random"));
            paramMap.Add("BSplineInterpolationOrder", GetVectorString("1"));
            paramMap.Add("FinalBSplineInterpolationOrder", GetVectorString("3"));
            paramMap.Add("DefaultPixelValue", GetVectorString("255.0"));
            paramMap.Add("WriteResultImage", GetVectorString("true"));
            paramMap.Add("ResultImagePixelType", GetVectorString("short"));
            paramMap.Add("ResultImageFormat", GetVectorString("mhd"));

            return paramMap;
        }

        /// <summary>
        /// Add transform bending energy penalty to parameter file.
        /// </summary>
        /// <param name="paramMap">parameter file</param>
        /// <param name="metricWeight0">weight of metric 0</param>
        /// <param name="metricWeight1">weight of metric 1</param>
        /// <returns>parameter file with penalty</returns>
        public static sitk.ParameterMap AddBendingEnergyPenaltyToParamMap(sitk.ParameterMap paramMap, int metricWeight0 = 1, int metricWeight1 = 1)
        {
            paramMap = RemovePenaltyTerm(paramMap);
            ChangeOrAddParamIfNotExist(ref paramMap, "Registration", GetVectorString("MultiMetricMultiResolutionRegistration"));
            ChangeOrAddParamIfNotExist(ref paramMap, "Metric", GetVectorString("AdvancedMeanSquares", "TransformBendingEnergyPenalty"));
            ChangeOrAddParamIfNotExist(ref paramMap, "Metric0Weight", GetVectorString(metricWeight0.ToString()));
            ChangeOrAddParamIfNotExist(ref paramMap, "Metric1Weight", GetVectorString(metricWeight1.ToString()));
            return paramMap;
        }

        /// <summary>
        /// Add transform rigidity penalty term to parameter file.
        /// If linearity, orthonormality and properness conditions are set to smaller than zero, they will be deactivated.
        /// </summary>
        /// <param name="paramMap">reference to parameter map</param>
        /// <param name="metricWeight0">weight of metric 0</param>
        /// <param name="metricWeight1">weight of metric 1</param>
        /// <param name="linearityConditionWeight">linearity condition weight (if set to smaller zero, it will not be calculated)</param>
        /// <param name="orthonormalityConditionWeight">orthonormality condition weight (if set to smaller zero, it will not be calculated)</param>
        /// <param name="propernessConditionWeight">properness condition weight (if set to smaller zero, it will not be calculated)</param>
        /// <returns></returns>
        public static sitk.ParameterMap AddTransformRigidityPenaltyToParamMap(sitk.ParameterMap paramMap,
            int metricWeight0 = 1, 
            int metricWeight1 = 1,
            int linearityConditionWeight = 1,
            int orthonormalityConditionWeight = 1,
            int propernessConditionWeight = 1)
        {
            paramMap = RemovePenaltyTerm(paramMap);
            ChangeOrAddParamIfNotExist(ref paramMap, "Registration", GetVectorString("MultiMetricMultiResolutionRegistration"));
            ChangeOrAddParamIfNotExist(ref paramMap, "Metric", GetVectorString("AdvancedMeanSquares", "TransformRigidityPenalty"));
            ChangeOrAddParamIfNotExist(ref paramMap, "Metric0Weight", GetVectorString(metricWeight0.ToString()));
            ChangeOrAddParamIfNotExist(ref paramMap, "Metric1Weight", GetVectorString(metricWeight1.ToString()));

            if (linearityConditionWeight > 0)
            {
                ChangeOrAddParamIfNotExist(ref paramMap, "LinearityConditionWeight", GetVectorString(linearityConditionWeight.ToString()));
            } else
            {
                ChangeOrAddParamIfNotExist(ref paramMap, "UseLinearityCondition", GetVectorString("false"));
            }

            if (orthonormalityConditionWeight > 0)
            {
                ChangeOrAddParamIfNotExist(ref paramMap, "OrthonormalityConditionWeight", GetVectorString(orthonormalityConditionWeight.ToString()));
            }
            else
            {
                ChangeOrAddParamIfNotExist(ref paramMap, "UseOrthonormalityCondition", GetVectorString("false"));
            }

            if (propernessConditionWeight > 0)
            {
                ChangeOrAddParamIfNotExist(ref paramMap, "PropernessConditionWeight", GetVectorString(propernessConditionWeight.ToString()));
            }
            else
            {
                ChangeOrAddParamIfNotExist(ref paramMap, "UsePropernessCondition", GetVectorString("false"));
            }

            return paramMap;
        }

        public static sitk.ParameterMap AddDistancePreservingRigidityPenaltyToParamMap(sitk.ParameterMap paramMap)
        {
            paramMap = RemovePenaltyTerm(paramMap);
            // todo add segmentation in registration class
            ChangeOrAddParamIfNotExist(ref paramMap, "Registration", GetVectorString("MultiMetricMultiResolutionRegistration"));
            ChangeOrAddParamIfNotExist(ref paramMap, "Metric", GetVectorString("AdvancedMeanSquares", "DistancePreservingRigidityPenalty"));
            return paramMap;
        }

        /// <summary>
        /// Remove Penalties fromparameter file if one exist.
        /// </summary>
        /// <param name="paramMap"></param>
        /// <returns></returns>
        public static sitk.ParameterMap RemovePenaltyTerm(sitk.ParameterMap paramMap)
        {
            ChangeOrAddParamIfNotExist(ref paramMap, "Registration", GetVectorString("MultiResolutionRegistration"));
            ChangeOrAddParamIfNotExist(ref paramMap, "Metric", GetVectorString("AdvancedMeanSquares"));
            RemoveParameter(ref paramMap, "Metric0Weight");
            RemoveParameter(ref paramMap, "Metric1Weight");
            RemoveParameter(ref paramMap, "LinearityConditionWeight");
            RemoveParameter(ref paramMap, "UseLinearityCondition");
            RemoveParameter(ref paramMap, "OrthonormalityConditionWeight");
            RemoveParameter(ref paramMap, "UseOrthonormalityCondition");
            RemoveParameter(ref paramMap, "PropernessConditionWeight");
            RemoveParameter(ref paramMap, "UsePropernessCondition");

            return paramMap;
        }

        private static void RemoveParameter(ref sitk.ParameterMap map, string key)
        {
            if (map.ContainsKey(key))
            {
                map.Remove(key);
            }
        }

        /// <summary>
        /// Change or add a parameter (if param does not exist) to parameter map.
        /// </summary>
        /// <param name="paramMap">reference to parameter map</param>
        /// <param name="key">parameter key</param>
        /// <param name="value">values as a vector of strings</param>
        public static void ChangeOrAddParamIfNotExist(ref sitk.ParameterMap paramMap, string key, sitk.VectorString value)
        {
            if (paramMap.ContainsKey(key))
            {
                paramMap[key] = value;
            }
            else
            {
                paramMap.Add(key, value);
            }
        }

        /// <summary>
        /// Calculate a composite transform for a list of vector parameter maps.
        /// </summary>
        /// <param name="movingImage">moving image</param>
        /// <param name="parameterMaps">list of vector of parameter maps</param>
        /// <param name="parameters">registration params</param>
        /// <param name="filename">filename of the result image</param>
        public static void WriteCompositeTransformForMovingImage(
            sitk.Image movingImage, 
            List<sitk.VectorOfParameterMap> parameterMaps,
            RegistrationParameters parameters,
            string filename)
        {
            sitk.VectorOfParameterMap initialTransform = parameterMaps.First();
            parameterMaps.Remove(initialTransform);

            TransformRGB transform = new TransformRGB(movingImage, parameterMaps, parameters);
            
            foreach(sitk.VectorOfParameterMap vectorOfMaps in parameterMaps)
            {
                transform.AddVectorOfParameterMap(vectorOfMaps);
            }
            transform.Execute();
            transform.WriteTransformedImage(filename);
        }

        /// <summary>
        /// Method performs multiple rigid registration for corresponding mask contours and returns
        /// a list of transform parameter maps
        /// </summary>
        /// <param name="fixedSegmentedMask">filename of the fixed segmented mask (inner sturctures!)</param>
        /// <param name="movingSegmentedMask">filename of the moving segmented mask (inner sturctures!)</param>
        /// <param name="numberOfComposedTransformations">number of components (segmented contours to register)</param>
        /// <param name="parameters">registration params</param>
        /// <returns>returns a list of transform parameter maps</returns>
        [Obsolete("Implemented in RegistrationController")]
        public static List<sitk.VectorOfParameterMap> PerformMultipleRigidRegistrationForComponents(
            string fixedSegmentedMask, 
            string movingSegmentedMask, 
            int numberOfComposedTransformations,
            RegistrationParameters parameters)
        {
            // created corresponding masks
            Image<Gray, byte> fixedSegments = new Image<Gray, byte>(fixedSegmentedMask);
            Image<Gray, byte> movingSegments = new Image<Gray, byte>(movingSegmentedMask);

            VectorOfVectorOfPoint contoursFixed = new VectorOfVectorOfPoint();
            VectorOfVectorOfPoint contoursMoving = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(fixedSegments, contoursFixed, null, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            CvInvoke.FindContours(movingSegments, contoursMoving, null, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            // retireve dict with contour index and area size ordered by size
            Dictionary<int, double> contoursFixedDict = GetContourAreaDict(ref contoursFixed);
            Dictionary<int, double> contoursMovingDict = GetContourAreaDict(ref contoursMoving);

            List<Tuple<string, string>> filenameOfMaskComponents = new List<Tuple<string, string>>();
            for(int i = 0; i <= numberOfComposedTransformations; i++)
            {
                var contourFixed = contoursFixed[contoursFixedDict.ElementAt(i).Key];
                var contourMoving = contoursMoving[contoursMovingDict.ElementAt(i).Key];

                Image<Gray, byte> maskFixed = new Image<Gray, byte>(fixedSegments.Width, fixedSegments.Height, new Gray(0.0));
                Image<Gray, byte> maskMoving = new Image<Gray, byte>(movingSegments.Width, movingSegments.Height, new Gray(0.0));
                CvInvoke.DrawContours(maskFixed, contourFixed, -1, new MCvScalar(255.0), thickness: -1);
                CvInvoke.DrawContours(maskMoving, contourMoving, -1, new MCvScalar(255.0), thickness: -1);

                string filenameFixed = Path.GetTempPath() + "\\fixed_0" + i + ".png";
                string filenameMoving = Path.GetTempPath() + "\\moving_0" + i + ".png";
                maskFixed.Save(filenameFixed);
                maskMoving.Save(filenameMoving);
                Tuple<string, string> temp = new Tuple<string, string>(filenameFixed, filenameMoving);
                filenameOfMaskComponents.Add(temp);
            }

            sitk.ParameterMap map = GetDefaultParameterMap(parameters.RegistrationDefaultParams);

            List<sitk.VectorOfParameterMap> list = new List<sitk.VectorOfParameterMap>();
            foreach(Tuple<string, string> tuple in filenameOfMaskComponents)
            {
                sitk.Image img01 = ReadWriteUtils.ReadITKImageFromFile(tuple.Item1);
                sitk.Image img02 = ReadWriteUtils.ReadITKImageFromFile(tuple.Item2);
                parameters.ParamMapToUse = map;
                RigidRegistration reg = new RigidRegistration(img01, img02, parameters);
                reg.Execute();
                sitk.VectorOfParameterMap toAdd = new sitk.VectorOfParameterMap(reg.GetTransformationParameterMap());
                list.Add(toAdd);
                reg.Dispose();
            }
            return list;
        }

        /// <summary>
        /// Returns a dictionary with contour index as key and the contour area as value.
        /// List is ordered by contour area.
        /// </summary>
        /// <param name="contours">contours as a vector of vector of points</param>
        /// <returns>dictionary</returns>
        public static Dictionary<int, double> GetContourAreaDict(ref VectorOfVectorOfPoint contours)
        {
            Dictionary<int, double> contoursDict = new Dictionary<int, double>();
            for (int i = 0; i < contours.Size; i++)
            {
                double area = CvInvoke.ContourArea(contours[i]);
                contoursDict.Add(i, area);
            }
            return contoursDict;
        }
    }
}
