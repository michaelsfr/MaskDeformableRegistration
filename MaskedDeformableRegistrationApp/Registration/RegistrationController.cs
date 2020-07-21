using Emgu.CV;
using Emgu.CV.Structure;
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

            InitializeRegistration();
        }

        private void InitializeRegistration()
        {
            // output directory
            if (Directory.Exists(_parameters.OutputDirectory))
            {
                Directory.CreateDirectory(_parameters.OutputDirectory);
            }
        }

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

        private void RunRegistrationPreviousOrder(List<string> imageStack)
        {
            // TO CHECK
            for (int i = 1; i < imageStack.Count; i++)
            {
                _parameters.Iteration = i - 1;
                _parameters.FixedImageFilename = imageStack[_parameters.Iteration];
                _worker.ReportProgress(0, "Load reference image...");
                sitk.Image referenceImage = LoadAndResizeImage(_parameters.FixedImageFilename, _parameters.FixedImageFilename);

                sitk.Image fixedMask = null;
                if (_parameters.UseFixedMask)
                {
                    fixedMask = GetMask(_parameters.FixedImageFilename);
                }

                _worker.ReportProgress(0, "Load template image...");
                sitk.Image templateImage = LoadAndResizeImage(imageStack[i], imageStack[i]);

                _worker.ReportProgress(0, "Start registration.");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                sitk.VectorOfParameterMap transformparams = PerformRegistration(referenceImage, templateImage, fixedMask, imageStack[i]);

                stopWatch.Stop();
                string elapsed = string.Format("[{0}m {1}s]", stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds);

                _worker.ReportProgress((99/imageStack.Count)/2, string.Format("Registration done (Elapsed time: {0}). Writing transform file...", elapsed));

                WriteTransform(imageStack[i], transformparams);
                _worker.ReportProgress((99 / imageStack.Count) / 2, "Transformed image was written to output directory.");
            }
            _worker.ReportProgress(100, "Registration done.");
        }

        private void RunRegistrationFirstOrLastInStackOrder(List<string> imageStack, bool firstAsReference)
        {
            if(firstAsReference)
            {
                _parameters.FixedImageFilename = imageStack.First();
            } else
            {
                _parameters.FixedImageFilename = imageStack.Last();
            }

            _worker.ReportProgress(0, "Load reference image...");
            sitk.Image referenceImage = LoadAndResizeImage(_parameters.FixedImageFilename);

            sitk.Image fixedMask = null;
            if (_parameters.UseFixedMask)
            {
                fixedMask = GetMask(_parameters.FixedImageFilename);
            }

            int i = 0;
            foreach (string filename in imageStack)
            {
                _parameters.Iteration = i++;
                if (filename == _parameters.FixedImageFilename) continue;

                _worker.ReportProgress(0, "Load template image...");
                sitk.Image templateImage = LoadAndResizeImage(filename, filename);

                _worker.ReportProgress(0, "Start registration.");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                sitk.VectorOfParameterMap transformparams = PerformRegistration(referenceImage, templateImage, fixedMask, filename);

                stopWatch.Stop();
                string elapsed = string.Format("[{0}m {1}s]", stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds);

                _worker.ReportProgress((99 / imageStack.Count) / 2, string.Format("Registration done (Elapsed time: {0}). Writing transform file...", elapsed));


                WriteTransform(filename, transformparams);
                _worker.ReportProgress((99 / imageStack.Count) / 2, "Transformed image was written to output directory.");
            }
            _worker.ReportProgress(100, "Registration done.");
        }

        private sitk.Image LoadAndResizeImage(string filename, string writeFilename = null)
        {
            sitk.Image refImage = ReadWriteUtils.ReadITKImageFromFile(_parameters.FixedImageFilename);
            //_worker.ReportProgress(percentage += (int)(percentagePerIteration * 0.20), string.Format("Loaded fixed image {0}.\n", _parameters.FixedImageFilename));
            sitk.Image refResized = ImageUtils.ResizeImage(refImage, _parameters.LargestImageWidth, _parameters.LargestImageHeight);

            if (writeFilename != null)
            {
                ReadWriteUtils.WriteSitkImage(refResized, Path.Combine(_parameters.OutputDirectory, Path.GetFileName(_parameters.FixedImageFilename)));
            }

            return refResized;
        }

        private sitk.VectorOfParameterMap PerformRegistration(sitk.Image refImage, sitk.Image movImage, sitk.Image fixedMask, string movingImageFilename)
        {
            sitk.Image movingMask = null;
            if (_parameters.UseMovingMask)
            {
                movingMask = GetMask(movingImageFilename);
            }

            if (_parameters.Type == RegistrationType.Rigid)
            {
                return PerformNonRigidRegistration(refImage, movImage, fixedMask, movingMask, movingImageFilename);
            } else
            {
                return PerformRigidRegistration(refImage, movImage, fixedMask, movingMask, movingImageFilename);
            }
        }

        private sitk.Image GetMask(string filename)
        {
            if (_parameters.UseInnerStructuresSegmentation)
            {
                return GetWholeParticleMask(filename);
            }
            else
            {
                return GetInnerStructureMask(filename);
            }
        }

        private sitk.VectorOfParameterMap PerformRigidRegistration(sitk.Image refImage, sitk.Image movImage, 
            sitk.Image fixedMask, sitk.Image movingMask, string imageFilename)
        {
            sitk.VectorOfParameterMap resultMap = null;
            switch (_parameters.RigidOptions)
            {
                case MaskedRigidRegistrationOptions.None:
                    resultMap = DefaultRigidRegistration(refImage, movImage, fixedMask, movingMask, imageFilename);
                    break;
                case MaskedRigidRegistrationOptions.BinaryRegistrationWholeTissue:
                case MaskedRigidRegistrationOptions.BinaryRegistrationInnerStructures:
                    resultMap = DefaultRigidRegistration(fixedMask, movingMask, null, null, imageFilename);
                    break;
                case MaskedRigidRegistrationOptions.ComponentwiseRegistration:
                    resultMap = ComponentWiseRigidRegistration(fixedMask, movingMask, 4, imageFilename);
                    break;
            }
            return resultMap;
        }

        private sitk.VectorOfParameterMap ComponentWiseRigidRegistration(sitk.Image fixedMask, sitk.Image movingMask, int v, string filename)
        {
            // todo: see Registration utils
            throw new NotImplementedException();
        }

        private sitk.VectorOfParameterMap DefaultRigidRegistration(sitk.Image refImage, sitk.Image movImage, sitk.Image fixedMask, sitk.Image movingMask, string imageFilename)
        {
            RigidRegistration regRigid = new RigidRegistration(refImage, movImage, _parameters);
            regRigid.SetDefaultParameterMap(_parameters.RegistrationDefaultParams, _parameters.NumberOfResolutions);

            if (fixedMask != null)
            {
                regRigid.SetFixedMask(fixedMask);
            }

            if (movingMask != null)
            {
                regRigid.SetMovingMask(movingMask);
            }

            regRigid.Execute();
            sitk.VectorOfParameterMap transformparams = regRigid.GetTransformationParameterMap();
            regRigid.Dispose();

            return transformparams;
        }

        private sitk.VectorOfParameterMap PerformNonRigidRegistration(sitk.Image refImage, sitk.Image movImage, 
            sitk.Image fixedMask, sitk.Image movingMask, string imageFilename)
        {
            sitk.VectorOfParameterMap resultMap = null;
            switch(_parameters.NonRigidOptions)
            {
                case MaskedNonRigidRegistrationOptions.None:
                    resultMap = DefaultNonRigidRegistration(refImage, movImage, fixedMask, movingMask, imageFilename);
                    break;
                case MaskedNonRigidRegistrationOptions.BsplineWithPenaltyTerm:
                    resultMap = NonRigidRegistrationWithPenalty(refImage, movImage, null, null, imageFilename);
                    break;
                case MaskedNonRigidRegistrationOptions.BsplineWithPenaltyTermAndCoefficientMap:
                    resultMap = NonRigidRegistrationWithPenalty(refImage, movImage, fixedMask, null, imageFilename);
                    break;
                case MaskedNonRigidRegistrationOptions.ComposeIndependantRegistrations:
                    throw new NotImplementedException();
                case MaskedNonRigidRegistrationOptions.DiffuseRegistration:
                    throw new NotImplementedException();
            }
            return resultMap;
        }

        private sitk.VectorOfParameterMap NonRigidRegistrationWithPenalty(sitk.Image refImage, sitk.Image movImage,
            sitk.Image fixedImage, sitk.Image movingImage, string imageFilename)
        {
            // todo 

            // coef map
            //string coefficientMapFilename = GetInnerStructureSegmentationsAsCoefficientMap(movingImageFilename, RegistrationParametersNonRigid);
            //RegistrationParametersNonRigid.CoefficientMapFilename = coefficientMapFilename;

            throw new NotImplementedException();
        }

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

            nonRigidRegistration.Execute();
            sitk.VectorOfParameterMap transformparams = nonRigidRegistration.GetTransformationParameterMap();
            nonRigidRegistration.Dispose();

            return transformparams;
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
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="iteration"></param>
        /// <returns></returns>
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

        private string GetInnerStructureSegmentationFilename(string filename)
        {
            string outputFilename = ReadWriteUtils.GetOutputDirectory(_parameters, _parameters.Iteration) + "\\mask_" + Path.GetFileName(filename);
            InnerTissueSegmentation seg = GetInnerStructureSegmentation(filename);
            seg.GetOutput().First().Save(filename);
            seg.Dispose();
            return filename;
        }

        private sitk.Image GetInnerStructureMask(string filename)
        {
            string outputFilename = ReadWriteUtils.GetOutputDirectory(_parameters, _parameters.Iteration) + "\\mask_" + Path.GetFileName(filename);
            InnerTissueSegmentation seg = GetInnerStructureSegmentation(filename);
            seg.GetOutput().First().Save(outputFilename);
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

            WholeTissueSegmentation segImage = new WholeTissueSegmentation(image, _parameters.InnerStructuresSegParams);
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
        private void WriteTransform(string filename, sitk.VectorOfParameterMap transformparams)
        {
            string resultFilename = ReadWriteUtils.GetOutputDirectory(_parameters) + "\\" + Path.GetFileNameWithoutExtension(filename) + ".png";
            // add transform parameter map to registration parameters
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
