using sitk = itk.simple;
using MaskedDeformableRegistrationApp.Registration;
using MaskedDeformableRegistrationApp.Segmentation;
using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Emgu.CV;
using MaskedDeformableRegistrationApp.Forms;
using Emgu.CV.Structure;

namespace MaskedDeformableRegistrationApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //ApplicationContext.OutputPath = @"C:\reg\testdata\appTestDir\Results\";

            Application.Run(new StartupForm());

            //string filenameImage1 = @"C:\reg\testdata\appTestDir\Results\rigid\old\195304_0.png";
            /*string filenameImage1 = @"C:\reg\testdata\bm_intra_1\195250_0.png";
            string filenameImage2 = @"C:\reg\testdata\appTestDir\Results\rigid\old\195305_0.png";
            var image1 = ReadWriteUtils.ReadOpenCVImageFromFile(filenameImage1);
            var image2 = ReadWriteUtils.ReadOpenCVImageFromFile(filenameImage2);

            // ###################################
            //    Whole particle seg
            // ################################### 

            WholeTissueSegmentation seg1 = new WholeTissueSegmentation(image1, ImageUtils.GetPercentualImagePixelCount(image1, 0.03f));
            seg1.Execute();
            Image<Gray, byte> wholeMask = seg1.GetOutput().Clone();
            seg1.Dispose();
            ReadWriteUtils.WriteUMatToFile(ApplicationContext.OutputPath + "whole_mask.png", wholeMask.ToUMat());
            //var imageConverted1 = ImageUtils.GetITKImageFromBitmap(seg1.GetOutput().Bitmap);
            //WholeTissueSegmentation seg2 = new WholeTissueSegmentation(image2, ImageUtils.GetPercentualImagePixelCount(image2, 0.3f));
            //seg2.Execute();
            //var imageConverted2 = ImageUtils.GetITKImageFromBitmap(seg1.GetOutput().Bitmap);

            // ###################################
            //    Inner structure seg
            // ################################### 

            InnerTissueSegmentation innerSeg = new InnerTissueSegmentation(image1, wholeMask, new SegmentationParameters());
            innerSeg.Execute();
            List<UMat> masks = innerSeg.GetOutput();

            int i = 0;
            foreach (UMat mask in masks)
            {
                string tempFilename = Path.Combine(@"C:\reg\testdata\appTestDir\Results\rigid\old\", "seg_mask_" + i + ".png");
                ReadWriteUtils.WriteUMatToFile(tempFilename, mask);
                i++;
            }

            UMat coefficentMatrix = innerSeg.GetCoefficientMatrix();
            string filenameCoefficientImage = Path.Combine(@"C:\reg\testdata\appTestDir\Results\rigid\old\", "umat_mask.png");
            ReadWriteUtils.WriteUMatToFile(filenameCoefficientImage, coefficentMatrix);
            image1.Dispose();
            image2.Dispose();

            /*var sitk_image1 = ReadWriteUtils.ReadITKImageFromFile(filenameImage1, sitk.PixelIDValueEnum.sitkVectorUInt8);
            var sitk_image2 = ReadWriteUtils.ReadITKImageFromFile(filenameImage2, sitk.PixelIDValueEnum.sitkVectorUInt8);

            BSplineRegistration reg = new BSplineRegistration(sitk_image1, sitk_image2, Path.Combine(ApplicationContext.OutputPath, "testBspline"));
            reg.SetRigidyPenaltyTerm(PenaltyTerm.TransformRigidityPenalty, movingRigidityMask: filenameCoefficientImage);
            reg.Execute();
            var transformparams = reg.GetTransformationFile();
            reg.Dispose();

            TransformRGB trans = new TransformRGB(sitk_image2, transformparams, Path.Combine(ApplicationContext.OutputPath, "testBspline"));
            trans.SetInterpolationOrder(1);
            trans.Execute();
            sitk.Image output = trans.GetOutput();
            trans.WriteTransformedImage("result_bspline.png");
            trans.Dispose();
            output.Dispose();
            

            /*int i = 0;
            List<string> filenames = new List<string>();
            foreach (var mask in masks)
            {
                string filename = Path.Combine(@"C:\reg\testdata\bm_he_1\", ("umat_mask_" + (i++).ToString() + ".png"));
                filenames.Add(filename);
                ReadWriteUtils.WriteUMatToFile(filename, mask);
            }*/

            //var sitk_image1 = ReadWriteUtils.ReadITKImageFromFile(@"C:\reg\testdata\bm_he_1\195330_0.png", sitk.PixelIDValueEnum.sitkVectorUInt8);
            //var sitk_image2 = ReadWriteUtils.ReadITKImageFromFile(@"C:\reg\testdata\bm_he_1\moving.png", sitk.PixelIDValueEnum.sitkVectorUInt8);

            // ###################################
            //    Bspline registration
            // ################################### 

            /*RigidRegistration regRigid = new RigidRegistration(sitk_image1, sitk_image2, @"C:\reg\testdata\bm_HE_1\results");
            regRigid.SetDefaultParameterMap(RegistrationDefaultParameters.translation, 10);
            regRigid.SetSimilarityMetric(SimilarityMetric.AdvancedMattesMutualInformation);
            regRigid.SetFixedMask(imageConverted1);
            regRigid.SetMovingMask(imageConverted2);
            regRigid.Execute();
            sitk.VectorOfParameterMap transformparams = regRigid.GetTransformationFile();
            regRigid.Dispose();

            TransformRGB trans = new TransformRGB(sitk_image2, transformparams, @"C:\reg\testdata\bm_HE_1\results");
            trans.Execute();
            sitk.Image output = trans.GetOutput();
            trans.WriteTransformedImage("result_translation.png");

            RigidRegistration regAffine = new RigidRegistration(sitk_image1, output, @"C:\reg\testdata\bm_HE_1\results");
            regAffine.SetDefaultParameterMap(RegistrationDefaultParameters.rigid, 10);
            regAffine.SetSimilarityMetric(SimilarityMetric.AdvancedMattesMutualInformation);
            regAffine.Execute();
            transformparams = regAffine.GetTransformationFile();
            regAffine.Dispose();

            TransformRGB trans2 = new TransformRGB(output, transformparams, @"C:\reg\testdata\bm_HE_1\results");
            trans2.Execute();
            sitk.Image output2 = trans2.GetOutput();
            trans2.WriteTransformedImage("result_rigid.png");

            sitk.Image checker = VisualizationUtils.GetCheckerBoard(sitk_image1, output2, size: 250);
            sitk.ImageFileWriter writer = new sitk.ImageFileWriter();
            writer.SetFileName(Path.Combine(@"C:\reg\testdata\bm_HE_1\results", "checker-result-image.png"));
            writer.Execute(checker);

            // ###################################
            //    Bspline registration
            // ################################### 

            /*BSplineRegistration reg = new BSplineRegistration(sitk_image1, sitk_image2, @"C:\reg\testdata\bm_HE_1\results");
            reg.SetRigidyPenaltyTerm(PenaltyTerm.TransformRigidityPenalty, movingRigidityMask: filenames[1]);
            reg.Execute();
            var transformparams = reg.GetTransformationFile();
            reg.Dispose();

            TransformRGBA trans = new TransformRGBA(sitk_image2, transformparams, @"C:\reg\testdata\bm_HE_1\results");
            trans.Execute();
            sitk.Image output = trans.GetOutput();
            trans.WriteTransformedImage();

            sitk.ImageFileWriter writer1 = new sitk.ImageFileWriter();
            writer1.SetFileName(Path.Combine(@"C:\reg\testdata\bm_HE_1\results", "deformation_field.mhd"));
            writer1.Execute(trans.GetTranform());

            sitk.Image checker = VisualizationUtils.GetCheckerBoard(sitk_image1, output, size: 50);

            sitk.ImageFileWriter writer = new sitk.ImageFileWriter();
            writer.SetFileName(Path.Combine(@"C:\reg\testdata\bm_HE_1\results", "checker-result-image.png"));
            writer.Execute(checker);*/


            /*var reader3 = new sitk.ImageFileReader();
            reader3.SetFileName(@"C:\reg\testdata\bm_HE_1\results\result-image.png");
            reader3.SetOutputPixelType(sitk.PixelIDValueEnum.sitkVectorUInt8);
            var transformed_image = reader3.Execute();
            RigidRegistration reg2 = new RigidRegistration(sitk_image1, transformed_image, @"C:\reg\testdata\bm_HE_1\results");
            reg2.SetDefaultParameterMap(RegistrationType.translation, 10);
            reg2.Execute();
            var transformparams2 = reg2.GetTransformationFile();
            reg2.Dispose();

            TransformRGBA trans2 = new TransformRGBA(sitk_image2, transformparams, @"C:\reg\testdata\bm_HE_1\results");
            trans2.Execute();
            trans2.WriteTransformedImage();*/

            // todo
        }
    }
}
