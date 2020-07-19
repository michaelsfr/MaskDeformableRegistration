using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
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
                    return GetParameterMap(type);
                else
                {
                    sitk.ParameterMap parameterMap = GetParameterMap(RegistrationDefaultParameters.nonrigid);
                    if(type == RegistrationDefaultParameters.diffusion)
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
        private static sitk.VectorString GetVectorString(params string[] values)
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

            TransformRGB transform = new TransformRGB(movingImage, initialTransform, parameters);
            
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
            Dictionary<int, double> contoursFixedDict = GetContourAreaDict(contoursFixed);
            Dictionary<int, double> contoursMovingDict = GetContourAreaDict(contoursMoving);

            List<Tuple<string, string>> filenameOfMaskComponents = new List<Tuple<string, string>>();
            for(int i = 0; i <= numberOfComposedTransformations; i++)
            {
                var contourFixed = contoursFixed[contoursFixedDict.ElementAt(i).Key];
                var contourMoving = contoursMoving[contoursMovingDict.ElementAt(i).Key];

                Image<Gray, byte> maskFixed = new Image<Gray, byte>(fixedSegments.Width, fixedSegments.Height, new Gray(0.0));
                Image<Gray, byte> maskMoving = new Image<Gray, byte>(movingSegments.Width, fixedSegments.Height, new Gray(0.0));
                CvInvoke.DrawContours(maskFixed, contourFixed, -1, new MCvScalar(255.0), thickness: -1);
                CvInvoke.DrawContours(maskMoving, contourMoving, -1, new MCvScalar(255.0), thickness: -1);

                string filenameFixed = "C:\\temp\\fixed_0" + i + ".png";
                string filenameMoving = "C:\\temp\\moving_0" + i + ".png";
                maskFixed.Save(filenameFixed);
                maskMoving.Save(filenameMoving);
                Tuple<string, string> temp = new Tuple<string, string>(filenameFixed, filenameMoving);
                filenameOfMaskComponents.Add(temp);
            }

            sitk.ParameterMap map = GetDefaultParameterMap(parameters.RegistrationType);

            List<sitk.VectorOfParameterMap> list = new List<sitk.VectorOfParameterMap>();
            foreach(Tuple<string, string> tuple in filenameOfMaskComponents)
            {
                sitk.Image img01 = ReadWriteUtils.ReadITKImageFromFile(tuple.Item1);
                sitk.Image img02 = ReadWriteUtils.ReadITKImageFromFile(tuple.Item2);
                RigidRegistration reg = new RigidRegistration(img01, img02, map, parameters);
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
        private static Dictionary<int, double> GetContourAreaDict(VectorOfVectorOfPoint contours)
        {
            Dictionary<int, double> contoursDict = new Dictionary<int, double>();
            for (int i = 0; i <= contours.Size; i++)
            {
                double area = CvInvoke.ContourArea(contours[i]);
                contoursDict.Add(i, area);
            }
            contoursDict.OrderBy(it => it.Value);
            return contoursDict;
        }
    }
}
