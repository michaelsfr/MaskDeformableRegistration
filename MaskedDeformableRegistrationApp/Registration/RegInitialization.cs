using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaskedDeformableRegistrationApp.Utils;
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Registration
{
    public abstract class RegInitialization : IRegistration<sitk.Image>, IDisposable
    {
        protected sitk.ElastixImageFilter elastix = null;
        protected sitk.ParameterMap parameterMap = null;
        protected RegistrationParameters registrationParameters;
        protected string outputDirectory;

        protected sitk.Image fixedImage = null;
        protected sitk.Image movingImage = null;
        protected sitk.Image fixedMask = null;
        protected sitk.Image movingMask = null;
        protected sitk.Image transformedImage = null;

        protected RegInitialization(RegistrationParameters parameters)
        {
            this.registrationParameters = parameters;
            this.parameterMap = parameters.ParamMapToUse;
        }

        /// <summary>
        /// Dispose all created instances.
        /// </summary>
        public virtual void Dispose()
        {
            if (elastix != null) elastix.Dispose();
            if (fixedImage != null) fixedImage.Dispose();
            if (movingImage != null) movingImage.Dispose();
            if (fixedMask != null) fixedMask.Dispose();
            if (movingMask != null) movingMask.Dispose();
            if (transformedImage != null) transformedImage.Dispose();
        }

        public abstract object Execute();

        /// <summary>
        /// Get registration output image.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get registration parameter map.
        /// </summary>
        /// <returns></returns>
        public virtual sitk.ParameterMap GetParameterMap()
        {
            return parameterMap;
        }

        /// <summary>
        /// Get transform parameter map.
        /// </summary>
        /// <returns></returns>
        public virtual sitk.VectorOfParameterMap GetTransformationParameterMap()
        {
            if(elastix != null)
            {
                return elastix.GetTransformParameterMap();
            }
            return null;
        }

        /// <summary>
        /// Set parameter map to default.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="numberOfResolutions"></param>
        public virtual void SetDefaultParameterMap(RegistrationDefaultParameters type, uint numberOfResolutions)
        {
            parameterMap = elastix.GetDefaultParameterMap(type.ToString(), numberOfResolutions);
        }

        /// <summary>
        /// Set fixed mask for registration process.
        /// </summary>
        /// <param name="fixedMask">fixed mask</param>
        public virtual void SetFixedMask(sitk.Image fixedMask)
        {
            try
            {
                sitk.CastImageFilter castImageFilter = new sitk.CastImageFilter();
                castImageFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkUInt8);
                this.fixedMask = castImageFilter.Execute(fixedMask);
            } catch (Exception)
            {
                // ignore when mask of pixeltype uint8
            }          
        }

        /// <summary>
        /// Set moving mask for registration process.
        /// </summary>
        /// <param name="movingMask">moving mask</param>
        public virtual void SetMovingMask(sitk.Image movingMask)
        {
            try
            {
                sitk.CastImageFilter castImageFilter = new sitk.CastImageFilter();
                castImageFilter.SetOutputPixelType(sitk.PixelIDValueEnum.sitkUInt8);
                this.movingMask = castImageFilter.Execute(movingMask);
            } catch (Exception)
            {
                // ignore when mask of pixeltype uint8
            }
        }

        /// <summary>
        /// Set parameter map by filename.
        /// </summary>
        /// <param name="file">filename of parameter map</param>
        public virtual void SetParameterMap(string file)
        {
            elastix.ReadParameterFile(file);
        }

        /// <summary>
        /// Add a parameter to the parameter map.
        /// </summary>
        /// <param name="parameterName">parameter key</param>
        /// <param name="value">parameter value</param>
        protected void AddParameter(string parameterName, sitk.VectorString value)
        {
            if (parameterMap.ContainsKey(parameterName))
            {
                parameterMap.Remove(parameterName);
            }

            parameterMap.Add(parameterName, value);
        }

        /// <summary>
        /// Add generic parameter value to map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterName">parameter key</param>
        /// <param name="values">parameter value</param>
        protected void AddParameter<T>(string parameterName, params T[] values)
        {
            if (parameterMap.ContainsKey(parameterName))
            {
                parameterMap.Remove(parameterName);
            }

            sitk.VectorString vec = new sitk.VectorString();
            foreach (T value in values)
            {
                vec.Add(value.ToString());
            }
            parameterMap.Add(parameterName, vec);
        }
    }
}
