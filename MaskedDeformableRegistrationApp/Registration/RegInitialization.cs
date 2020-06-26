using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaskedDeformableRegistrationApp.Utils;
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Registration
{
    public abstract class RegInitialization : IRegistration<sitk.Image, sitk.ParameterMap, sitk.VectorOfParameterMap>
    {
        protected sitk.ElastixImageFilter elastix = null;
        protected sitk.ParameterMap parameterMap = null;

        protected sitk.Image fixedImage = null;
        protected sitk.Image movingImage = null;
        protected sitk.Image fixedMask = null;
        protected sitk.Image movingMask = null;
        protected sitk.Image transformedImage = null;

        public RegInitialization(sitk.Image fixedImage, sitk.Image movingImage)
        {

        }

        public virtual void Dispose()
        {
            if (elastix != null) elastix.Dispose();
            if (fixedImage != null) fixedImage.Dispose();
            if (movingImage != null) movingImage.Dispose();
            if (fixedMask != null) fixedMask.Dispose();
            if (movingMask != null) movingMask.Dispose();
            if (transformedImage != null) transformedImage.Dispose();
        }

        public abstract void Execute();

        public virtual sitk.Image GetOutput()
        {
            if (transformedImage != null)
            {
                return transformedImage;
            }
            else
            {
                return null;
            }
        }

        public virtual sitk.ParameterMap GetParameterMap()
        {
            return parameterMap;
        }

        public virtual sitk.VectorOfParameterMap GetTransformationFile()
        {
            return elastix.GetTransformParameterMap();
        }

        public virtual void SetDefaultParameterMap(RegistrationDefaultParameters type, uint numberOfResolutions)
        {
            parameterMap = elastix.GetDefaultParameterMap(type.ToString(), numberOfResolutions);
        }

        public virtual void SetFixedMask(sitk.Image fixedMask)
        {
            sitk.CastImageFilter castImageFilter = new sitk.CastImageFilter();
            castImageFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkUInt8);
            this.fixedMask = castImageFilter.Execute(fixedMask);
        }

        public virtual void SetMovingMask(sitk.Image movingMask)
        {
            sitk.CastImageFilter castImageFilter = new sitk.CastImageFilter();
            castImageFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkUInt8);
            this.movingMask = castImageFilter.Execute(movingMask);
        }

        public virtual void SetParameterMap(string file)
        {
            elastix.ReadParameterFile(file);
        }

        public virtual void SetSimilarityMetric(SimilarityMetric metric)
        {
            foreach (var parameter in parameterMap.AsEnumerable())
            {
                if (parameter.Key == "Metric")
                {
                    parameter.Value[0] = metric.ToString();
                }
            }
        }
    }
}
