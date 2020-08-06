using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using MaskedDeformableRegistrationApp.Segmentation;
using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Registration
{
    public class RegistrationController
    {
        private RegistrationParameters _parameters;
        private BackgroundWorker _worker;

        public RegistrationController(RegistrationParameters parameters, BackgroundWorker worker)
        {
            _parameters = parameters;
            _worker = worker;

            // create output directory
            if (!Directory.Exists(_parameters.OutputDirectory))
            {
                Directory.CreateDirectory(_parameters.OutputDirectory);
            }
        }

        /// <summary>
        /// Entry point for registration. Checks for the registration order of the image stack.
        /// </summary>
        /// <param name="imageStack">image stack as list of filenames</param>
        public void RunRegistration(List<string> imageStack)
        {
            _worker.ReportProgress(1, string.Format("Start {0} registration process.", _parameters.Type.ToString()));
            switch(_parameters.Order)
            {
                case RegistrationOrder.FirstInStackIsReference: RunRegistrationFirstOrLastInStackOrder(imageStack, true); break;
                case RegistrationOrder.PreviousIsReference: RunRegistrationPreviousOrder(imageStack); break;
                case RegistrationOrder.LastInStackIsReference: RunRegistrationFirstOrLastInStackOrder(imageStack, false); break;
                case RegistrationOrder.MedianIsReference: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Run registration with the predecessor of the current image as fixed image.
        /// </summary>
        /// <param name="imageStack">image stack as list of filenames</param>
        private void RunRegistrationPreviousOrder(List<string> imageStack)
        {
            // TO CHECK
            for (int i = 1; i < imageStack.Count; i++)
            {
                _parameters.Iteration = i - 1;
                _parameters.FixedImageFilename = imageStack[_parameters.Iteration];
                _worker.ReportProgress(0, "Load reference image...");
                // load and resize fixed / reference image
                sitk.Image referenceImage = LoadAndResizeImage(_parameters.FixedImageFilename, _parameters.FixedImageFilename);

                // set mask if defined in parameters
                sitk.Image fixedMask = null;
                if (_parameters.UseFixedMask)
                {
                    fixedMask = GetMask(_parameters.FixedImageFilename);
                    ReadWriteUtils.WriteSitkImage(fixedMask, _parameters.OutputDirectory + "\\fixed_mask.png");

                }

                _worker.ReportProgress(0, "Load template image...");
                // load and resize moving / template image
                sitk.Image templateImage = LoadAndResizeImage(imageStack[i], imageStack[i]);

                _worker.ReportProgress(0, "Start registration.");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                // start actual registration
                List<sitk.VectorOfParameterMap> transformparams = PerformRegistration(referenceImage, templateImage, fixedMask, imageStack[i]);

                stopWatch.Stop();
                string elapsed = string.Format("[{0}m {1}s]", stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds);
                _worker.ReportProgress((99/imageStack.Count)/2, string.Format("Registration done (Elapsed time: {0}). Writing transform file...", elapsed));

                // transform moving image according to the transformation parameters
                WriteTransform(imageStack[i], transformparams);
                _worker.ReportProgress((99 / imageStack.Count) / 2, "Transformed image was written to output directory.");
            }
            _worker.ReportProgress(100, "Registration done.");
        }

        /// <summary>
        /// Run registration with fixed image as first or last image in stack.
        /// </summary>
        /// <param name="imageStack">image stack as list of filenames</param>
        /// <param name="firstAsReference">flag to specify if first or last image is taken as reference image</param>
        private void RunRegistrationFirstOrLastInStackOrder(List<string> imageStack, bool firstAsReference)
        {
            // set fixed image filename
            if(firstAsReference)
            {
                _parameters.FixedImageFilename = imageStack.First();
            } else
            {
                _parameters.FixedImageFilename = imageStack.Last();
            }

            _worker.ReportProgress(0, "Load reference image...");
            // load fixed / reference image once and resize
            sitk.Image referenceImage = LoadAndResizeImage(_parameters.FixedImageFilename);

            // set fixed mask if defined in parameters
            sitk.Image fixedMask = null;
            if (_parameters.UseFixedMask
                || _parameters.RigidOptions == MaskedRigidRegistrationOptions.BinaryRegistrationInnerStructures 
                || _parameters.RigidOptions == MaskedRigidRegistrationOptions.BinaryRegistrationWholeTissue
                || _parameters.RigidOptions == MaskedRigidRegistrationOptions.ComponentwiseRegistration)
            {
                fixedMask = GetMask(_parameters.FixedImageFilename);
                ReadWriteUtils.WriteSitkImage(fixedMask, _parameters.OutputDirectory + "\\fixed_mask.png");
            }

            int i = 0;
            foreach (string filename in imageStack)
            {
                _parameters.Iteration = i++;
                if (filename == _parameters.FixedImageFilename) continue;

                _worker.ReportProgress(0, "Load template image...");
                // load and resize moving / template image
                sitk.Image templateImage = LoadAndResizeImage(filename, filename);

                _worker.ReportProgress(0, "Start registration.");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                // start actual registration
                List<sitk.VectorOfParameterMap> transformparams = PerformRegistration(referenceImage, templateImage, fixedMask, filename);

                stopWatch.Stop();
                string elapsed = string.Format("[{0}m {1}s]", stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds);
                _worker.ReportProgress((99 / imageStack.Count) / 2, string.Format("Registration done (Elapsed time: {0}). Writing transform file...", elapsed));

                // transform moving image according to the transformation parameters
                WriteTransform(filename, transformparams);
                _worker.ReportProgress((99 / imageStack.Count) / 2, "Transformed image was written to output directory.");
            }
            _worker.ReportProgress(100, "Registration done.");
        }

        /// <summary>
        /// Load and resize image depending on the width and height of the largest image in stack.
        /// </summary>
        /// <param name="filename">filename of the image to load</param>
        /// <param name="writeFilename">output filename if image should be written back to disk</param>
        /// <returns>simpleitk image</returns>
        private sitk.Image LoadAndResizeImage(string filename, string writeFilename = null)
        {
            sitk.Image refImage = ReadWriteUtils.ReadITKImageFromFile(filename);
            sitk.Image refResized = ImageUtils.ResizeImage(refImage, _parameters.LargestImageWidth, _parameters.LargestImageHeight);

            // write image if output filename is defined
            if (writeFilename != null)
            {
                ReadWriteUtils.WriteSitkImage(refResized, Path.Combine(_parameters.OutputDirectory, Path.GetFileName(writeFilename)));
            }

            return refResized;
        }

        /// <summary>
        /// Add moving / template mask if necessary and start registration.
        /// </summary>
        /// <param name="refImage">reference image</param>
        /// <param name="movImage">template image</param>
        /// <param name="fixedMask">fixed mask (can be null)</param>
        /// <param name="movingImageFilename">filename of the template image</param>
        /// <returns></returns>
        private List<sitk.VectorOfParameterMap> PerformRegistration(sitk.Image refImage, sitk.Image movImage, sitk.Image fixedMask, string movingImageFilename)
        {
            sitk.Image movingMask = null;
            if (_parameters.UseMovingMask
                || _parameters.RigidOptions == MaskedRigidRegistrationOptions.BinaryRegistrationInnerStructures
                || _parameters.RigidOptions == MaskedRigidRegistrationOptions.BinaryRegistrationWholeTissue
                || _parameters.RigidOptions == MaskedRigidRegistrationOptions.ComponentwiseRegistration)
            {
                movingMask = GetMask(movingImageFilename);
                ReadWriteUtils.WriteSitkImage(movingMask, _parameters.OutputDirectory + "\\moving_mask.png");
            }

            if (_parameters.Type == RegistrationType.Rigid)
            {
                // do rigid registration
                return PerformRigidRegistration(refImage, movImage, fixedMask, movingMask, movingImageFilename);
            } else
            {
                // do non rigid registration
                return PerformNonRigidRegistration(refImage, movImage, fixedMask, movingMask, movingImageFilename);
            }
        }

        /// <summary>
        /// Retrieve mask for a given filename.
        /// </summary>
        /// <param name="filename">filename</param>
        /// <returns>mask of given image</returns>
        private sitk.Image GetMask(string filename)
        {
            if (_parameters.UseInnerStructuresSegmentation)
            {
                // segmentation of the inner structures of the particle
                return GetInnerStructureMask(filename);
            }
            else
            {
                // segmentation of the whole particle
                return GetWholeParticleMask(filename);
            }
        }

        /// <summary>
        /// Rigid registration specific options. Uses masking options.
        /// </summary>
        /// <param name="refImage">reference image</param>
        /// <param name="movImage">template image</param>
        /// <param name="fixedMask">fixed mask</param>
        /// <param name="movingMask">moving mask</param>
        /// <param name="imageFilename">moving image filename</param>
        /// <returns>transform parameters</returns>
        private List<sitk.VectorOfParameterMap> PerformRigidRegistration(sitk.Image refImage, sitk.Image movImage, 
            sitk.Image fixedMask, sitk.Image movingMask, string imageFilename)
        {
            // debug
            //CalculateMetrics(fixedMask, movingMask);
            //string fixedPTS = Path.GetDirectoryName(imageFilename) + "\\_landmarks\\195286_0-landmarks_global.pts";
            //string movPTS = Path.GetDirectoryName(imageFilename) + "\\_landmarks\\195287_0-landmarks_global.pts";
            //CalculateTRE(fixedPTS, movPTS);

            List<sitk.VectorOfParameterMap> resultMapList = new List<sitk.VectorOfParameterMap>();
            switch (_parameters.RigidOptions)
            {
                case MaskedRigidRegistrationOptions.None:
                    resultMapList.Add(DefaultRigidRegistration(refImage, movImage, fixedMask, movingMask, imageFilename));
                    break;
                case MaskedRigidRegistrationOptions.BinaryRegistrationWholeTissue:
                case MaskedRigidRegistrationOptions.BinaryRegistrationInnerStructures:
                    resultMapList.Add(DefaultRigidRegistration(fixedMask, movingMask, null, null, imageFilename));
                    break;
                case MaskedRigidRegistrationOptions.ComponentwiseRegistration:
                    resultMapList = ComponentWiseRigidRegistration(fixedMask, movingMask, 4, imageFilename);
                    break;
            }
            return resultMapList;
        }

        /// <summary>
        /// Only for debug / evaluation use.
        /// </summary>
        /// <param name="pointsetFixed"></param>
        /// <param name="pointsetMov"></param>
        private void CalculateTRE(string pointsetFixed, string pointsetMov)
        {
            var fixedPointsDict = ReadWriteUtils.ReadFixedPointSet(pointsetFixed).Values.ToList();
            var movPointsDict = ReadWriteUtils.ReadFixedPointSet(pointsetMov).Values.ToList();
            RegistrationError registrationError = VisualizationEvaluationUtils.GetRegistrationError(fixedPointsDict, movPointsDict);
            //Console.WriteLine(string.Format("Absolute TRE: {0}", registrationError.AbsoluteRegistrationError));
            //Console.WriteLine(string.Format("Max TRE: {0}", registrationError.MaximumRegistrationError));
            //Console.WriteLine(string.Format("Mean TRE: {0}", registrationError.MeanRegistrationError));
            //Console.WriteLine(string.Format("Std deviation RE: {0}", registrationError.StdDevRegistrationError));
        }

        /// <summary>
        /// Only for debug / evaluation use.
        /// </summary>
        /// <param name="fixedMask"></param>
        /// <param name="movingMask"></param>
        private void CalculateMetrics(sitk.Image fixedMask, sitk.Image movingMask)
        {
            if (fixedMask != null && movingMask != null)
            {
                sitk.LabelOverlapMeasuresImageFilter overlapFilter = VisualizationEvaluationUtils.GetOverlapImageFilter(fixedMask, movingMask);
                
                if (overlapFilter != null)
                {
                    double diceCoef = overlapFilter.GetDiceCoefficient();
                    double falseNegative = overlapFilter.GetFalseNegativeError();
                    double falsePositive = overlapFilter.GetFalsePositiveError();
                    double jaccard = overlapFilter.GetJaccardCoefficient();
                    double meanOverlap = overlapFilter.GetMeanOverlap();
                    double unionOverlap = overlapFilter.GetUnionOverlap();

                    string dice = diceCoef.ToString("0.##");
                    string jacc = jaccard.ToString("0.##");
                    string negPos = string.Format("{0} / {1}", falseNegative.ToString("0.##"), falsePositive.ToString("0.##"));

                    Console.WriteLine("### Metrics ###");
                    Console.WriteLine("Dice: " + dice);
                    Console.WriteLine("Jaccard: " + jacc);
                    Console.WriteLine("False negative / false positive: " + negPos);
                }
            }
        }

        /// <summary>
        /// Componentwise rigid registration.
        /// Each corresponding masked components will be registrated discretly.
        /// Transformation parameters will be composed befor image is transformed.
        /// </summary>
        /// <param name="fixedMask">reference mask</param>
        /// <param name="movingMask">template mask</param>
        /// <param name="v">number of corresponding components</param>
        /// <param name="filename">moving image filename</param>
        /// <returns></returns>
        private List<sitk.VectorOfParameterMap> ComponentWiseRigidRegistration(sitk.Image fixedMask, sitk.Image movingMask, int v, string filename)
        {
            // convert from sitk to opencv
            var fixedImage = ReadWriteUtils.ConvertSitkImageToOpenCv<Gray, byte>(fixedMask);
            var movingImage = ReadWriteUtils.ConvertSitkImageToOpenCv<Gray, byte>(movingMask);

            // find contours
            VectorOfVectorOfPoint contoursFixed = new VectorOfVectorOfPoint();
            VectorOfVectorOfPoint contoursMoving = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(fixedImage, contoursFixed, null, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            CvInvoke.FindContours(movingImage, contoursMoving, null, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            // retireve dict with contour index and area size ordered by size
            Dictionary<int, double> contoursFixedDict = RegistrationUtils.GetContourAreaDict(ref contoursFixed);
            Dictionary<int, double> contoursMovingDict = RegistrationUtils.GetContourAreaDict(ref contoursMoving);

            List<Tuple<string, string>> filenameOfMaskComponents = new List<Tuple<string, string>>();
            for (int i = 0; i <= v; i++)
            {
                var contourFixed = contoursFixed[contoursFixedDict.ElementAt(i).Key];
                var contourMoving = contoursMoving[contoursMovingDict.ElementAt(i).Key];

                Image<Gray, byte> maskFixed = new Image<Gray, byte>(fixedImage.Width, fixedImage.Height, new Gray(0.0));
                Image<Gray, byte> maskMoving = new Image<Gray, byte>(movingImage.Width, movingImage.Height, new Gray(0.0));
                CvInvoke.DrawContours(maskFixed, contourFixed, -1, new MCvScalar(255.0), thickness: -1);
                CvInvoke.DrawContours(maskMoving, contourMoving, -1, new MCvScalar(255.0), thickness: -1);

                string filenameFixed = Path.GetTempPath() + "\\fixed_0" + i + ".png";
                string filenameMoving = Path.GetTempPath() + "\\moving_0" + i + ".png";
                maskFixed.Save(filenameFixed);
                maskMoving.Save(filenameMoving);
                Tuple<string, string> temp = new Tuple<string, string>(filenameFixed, filenameMoving);
                filenameOfMaskComponents.Add(temp);
            }

            sitk.ParameterMap map = RegistrationUtils.GetDefaultParameterMap(_parameters.RegistrationDefaultParams);

            List<sitk.VectorOfParameterMap> list = new List<sitk.VectorOfParameterMap>();
            foreach (Tuple<string, string> tuple in filenameOfMaskComponents)
            {
                sitk.Image img01 = ReadWriteUtils.ReadITKImageFromFile(tuple.Item1);
                sitk.Image img02 = ReadWriteUtils.ReadITKImageFromFile(tuple.Item2);
                _parameters.ParamMapToUse = map;
                RigidRegistration reg = new RigidRegistration(img01, img02, _parameters);
                reg.Execute();
                sitk.VectorOfParameterMap toAdd = new sitk.VectorOfParameterMap(reg.GetTransformationParameterMap());
                list.Add(toAdd);
                reg.Dispose();
            }
            return list;
        }

        /// <summary>
        /// Runs the actual registration process with default settings.
        /// </summary>
        /// <param name="refImage">reference image</param>
        /// <param name="movImage">template image</param>
        /// <param name="fixedMask">fixed mask</param>
        /// <param name="movingMask">moving mask</param>
        /// <param name="imageFilename">filename of moving image</param>
        /// <returns></returns>
        private sitk.VectorOfParameterMap DefaultRigidRegistration(sitk.Image refImage, sitk.Image movImage, sitk.Image fixedMask, sitk.Image movingMask, string imageFilename)
        {
            RigidRegistration regRigid = new RigidRegistration(refImage, movImage, _parameters);

            // set fixed mask if defined
            if (fixedMask != null)
            {
                regRigid.SetFixedMask(fixedMask);
            }

            // set moving mask if defined
            if (movingMask != null)
            {
                regRigid.SetMovingMask(movingMask);
            }

            ExecuteRegistration(regRigid);
            sitk.VectorOfParameterMap transformparams = regRigid.GetTransformationParameterMap();
            regRigid.Dispose();

            return transformparams;
        }

        /// <summary>
        /// Non rigid registration specific options. Uses masking options.
        /// </summary>
        /// <param name="refImage">reference image</param>
        /// <param name="movImage">template image</param>
        /// <param name="fixedMask">fixed mask</param>
        /// <param name="movingMask">moving mask</param>
        /// <param name="imageFilename">filename of moving image</param>
        /// <returns></returns>
        private List<sitk.VectorOfParameterMap> PerformNonRigidRegistration(sitk.Image refImage, sitk.Image movImage, 
            sitk.Image fixedMask, sitk.Image movingMask, string imageFilename)
        {
            List<sitk.VectorOfParameterMap> resultMap = new List<sitk.VectorOfParameterMap>();
            switch(_parameters.NonRigidOptions)
            {
                case MaskedNonRigidRegistrationOptions.None:
                    resultMap.Add(DefaultNonRigidRegistration(refImage, movImage, fixedMask, movingMask, imageFilename));
                    break;
                case MaskedNonRigidRegistrationOptions.BsplineWithPenaltyTerm:
                    resultMap.Add(NonRigidRegistrationWithPenalty(refImage, movImage, null, null, imageFilename));
                    break;
                case MaskedNonRigidRegistrationOptions.BsplineWithPenaltyTermAndCoefficientMap:
                    resultMap.Add(NonRigidRegistrationWithPenalty(refImage, movImage, fixedMask, null, imageFilename));
                    break;
                case MaskedNonRigidRegistrationOptions.ComposeIndependantRegistrations:
                    throw new NotImplementedException();
                case MaskedNonRigidRegistrationOptions.DiffuseRegistration:
                    throw new NotImplementedException();
            }
            return resultMap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refImage"></param>
        /// <param name="movImage"></param>
        /// <param name="fixedImage"></param>
        /// <param name="movingImage"></param>
        /// <param name="imageFilename"></param>
        /// <returns></returns>
        private sitk.VectorOfParameterMap NonRigidRegistrationWithPenalty(sitk.Image refImage, sitk.Image movImage,
            sitk.Image fixedImage, sitk.Image movingImage, string imageFilename)
        {
            // todo 

            // coef map
            //string coefficientMapFilename = GetInnerStructureSegmentationsAsCoefficientMap(movingImageFilename, RegistrationParametersNonRigid);
            //RegistrationParametersNonRigid.CoefficientMapFilename = coefficientMapFilename;

            throw new NotImplementedException();
        }

        /// <summary>
        /// Run actual registration process for a default non rigid registration.
        /// </summary>
        /// <param name="refImage">reference image</param>
        /// <param name="movImage">moving image</param>
        /// <param name="fixedMask">fixed mask</param>
        /// <param name="movingMask">moving mask</param>
        /// <param name="imageFilename">filename of moving image</param>
        /// <returns></returns>
        private sitk.VectorOfParameterMap DefaultNonRigidRegistration(sitk.Image refImage, sitk.Image movImage, 
            sitk.Image fixedMask, sitk.Image movingMask, string imageFilename)
        {
            NonRigidRegistration nonRigidRegistration = new NonRigidRegistration(refImage, movImage, _parameters);

            if (fixedMask != null)
            {
                nonRigidRegistration.SetFixedMask(fixedMask);
            }

            if (movingMask != null)
            {
                nonRigidRegistration.SetMovingMask(movingMask);
            }

            ExecuteRegistration(nonRigidRegistration);
            sitk.VectorOfParameterMap transformparams = nonRigidRegistration.GetTransformationParameterMap();
            nonRigidRegistration.Dispose();

            return transformparams;
        }

        private void ExecuteRegistration<T>(T registration) where T : IRegistration<sitk.Image, sitk.ParameterMap, sitk.VectorOfParameterMap>
        {
            object result = registration.Execute();
            if (result != null)
            {
                _worker.ReportProgress(100, string.Format("Exception in registration occured. [ {0} ] ", result.ToString()));
                _worker.CancelAsync();
            }
        }

        private string GetWholeParticleMaskFilename(string filename)
        {
            string outputFilename = ReadWriteUtils.GetOutputDirectory(_parameters, _parameters.Iteration) + "\\mask_" + Path.GetFileName(filename);
            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile<Bgr, byte>(filename);

            WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, _parameters.WholeTissueSegParams);
            segImage.Execute();
            segImage.GetOutput().Save(outputFilename);
            segImage.Dispose();
            return outputFilename;
        }

        /// <summary>
        /// Retrieves the mask of the whole tissue.
        /// </summary>
        /// <param name="filename">filename of the image</param>
        /// <returns>masked image</returns>
        private sitk.Image GetWholeParticleMask(string filename)
        {
            string outputFilename = ReadWriteUtils.GetOutputDirectory(_parameters, _parameters.Iteration) + "\\mask_" + Path.GetFileName(filename);
            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile<Bgr, byte>(filename);

            WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, _parameters.WholeTissueSegParams);
            segImage.Execute();
            segImage.GetOutput().Save(outputFilename);
            segImage.Dispose();
            image.Dispose();
            sitk.Image mask = ReadWriteUtils.ReadITKImageFromFile(outputFilename);

            return mask;
        }

        /// <summary>
        /// Get the coefficient map of inner structures of tissue for the transform rigidity penalty term.
        /// </summary>
        /// <param name="filename">image filename</param>
        /// <returns>return coefficient map filename</returns>
        private string GetInnerStructureSegmentationsAsCoefficientMap(string filename)
        {
            InnerTissueSegmentation innerSegImage = GetInnerStructureSegmentation(filename);

            string filenameCoefficientMap = ReadWriteUtils.GetOutputDirectory(_parameters, _parameters.Iteration) + "\\coefficientMap.png";
            ReadWriteUtils.WriteUMatToFile(filenameCoefficientMap, innerSegImage.GetOutput().FirstOrDefault());
            innerSegImage.Dispose();

            // rescale image
            sitk.Image img = ReadWriteUtils.ReadITKImageFromFile(filenameCoefficientMap);
            sitk.CastImageFilter castFilter = new sitk.CastImageFilter();
            castFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkFloat32);
            img = castFilter.Execute(img);
            sitk.RescaleIntensityImageFilter filter = new sitk.RescaleIntensityImageFilter();
            filter.SetOutputMinimum(0.0);
            filter.SetOutputMaximum(1.0);
            sitk.Image coefficientMap = filter.Execute(img);

            // save as mhd
            filenameCoefficientMap = ReadWriteUtils.GetOutputDirectory(_parameters, _parameters.Iteration) + "\\coefficientMap.mhd";
            ReadWriteUtils.WriteSitkImage(coefficientMap, filenameCoefficientMap);
            coefficientMap.Dispose();
            return filenameCoefficientMap;
        }

        /// <summary>
        /// Get filename of the inner structures mask of an image.
        /// </summary>
        /// <param name="filename">filename of the image</param>
        /// <returns>filename of the masked image</returns>
        private string GetInnerStructureSegmentationFilename(string filename)
        {
            string outputFilename = ReadWriteUtils.GetOutputDirectory(_parameters, _parameters.Iteration) + "\\mask_" + Path.GetFileName(filename);
            InnerTissueSegmentation seg = GetInnerStructureSegmentation(filename);
            seg.GetOutput()[0].Save(filename);
            seg.Dispose();
            return filename;
        }

        /// <summary>
        /// Get image of the inner structures mask of an image.
        /// </summary>
        /// <param name="filename">filename of the image</param>
        /// <returns>masked image</returns>
        private sitk.Image GetInnerStructureMask(string filename)
        {
            string outputFilename = ReadWriteUtils.GetOutputDirectory(_parameters, _parameters.Iteration) + "\\mask_" + Path.GetFileName(filename);
            InnerTissueSegmentation seg = GetInnerStructureSegmentation(filename);
            seg.GetOutput()[0].Save(outputFilename);
            seg.Dispose();
            sitk.Image mask = ReadWriteUtils.ReadITKImageFromFile(outputFilename);

            return mask;
        }

        /// <summary>
        /// Do segmentation of inner structures of the tissue for a given image.
        /// </summary>
        /// <param name="filename">image filename</param>
        /// <param name="parameters">registration parameters containing segmentation params</param>
        /// <returns>returns the segmentation instance</returns>
        private InnerTissueSegmentation GetInnerStructureSegmentation(string filename)
        {
            Image<Bgr, byte> image = ReadWriteUtils.ReadOpenCVImageFromFile<Bgr, byte>(filename);

            WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, _parameters.WholeTissueSegParams);
            segImage.Execute();
            Image<Gray, byte> wholeMask = segImage.GetOutput().Clone();
            segImage.Dispose();

            InnerTissueSegmentation innerSegImage = new InnerTissueSegmentation(image, wholeMask, _parameters.InnerStructuresSegParams);
            innerSegImage.Execute();
            return innerSegImage;
        }

        /// <summary>
        /// Write transformed image and deformation field to disk
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="transformparams"></param>
        private void WriteTransform(string filename, List<sitk.VectorOfParameterMap> transformparams)
        {
            string resultFilename = ReadWriteUtils.GetOutputDirectory(_parameters) + "\\" + Path.GetFileNameWithoutExtension(filename) + ".png";

            // add transform parameter map to registration parameters
            if(_parameters.TransformationParameterMap.ContainsKey(resultFilename))
            {
                _parameters.TransformationParameterMap.Remove(resultFilename);
            }
            _parameters.TransformationParameterMap.Add(resultFilename, transformparams);

            // read moving image from file
            sitk.Image movingImageToTransform = ReadWriteUtils.ReadITKImageFromFile(filename, sitk.PixelIDValueEnum.sitkVectorUInt8);
            // initialize transform instance
            TransformRGB trans = new TransformRGB(movingImageToTransform, transformparams, _parameters);

            if (_parameters.IsBinaryTransform)
            {
                // for binary reg set interpolation order to zero
                trans.SetInterpolationOrder(0);
            }

            trans.Execute();
            trans.WriteTransformedImage(resultFilename);

            // write deformation field
            sitk.Image deformationField = trans.GetDeformationField();
            string filenameDeformationField = ReadWriteUtils.GetOutputDirectory(_parameters, _parameters.Iteration) + "\\deformationField.mhd";
            ReadWriteUtils.WriteSitkImage(deformationField, filenameDeformationField);
            trans.Dispose();
            movingImageToTransform.Dispose();
        }
    }
}
