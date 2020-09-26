using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Registration
{
    public interface IRegistration<T>
    {
        object Execute();
        void SetFixedMask(T fixedMask);
        void SetMovingMask(T movingMask);
        void SetDefaultParameterMap(RegistrationDefaultParameters type, uint numberOfResolutions);
        void SetParameterMap(string file);
        T GetOutput();
        sitk.ParameterMap GetParameterMap();
        sitk.VectorOfParameterMap GetTransformationParameterMap();
        void Dispose();
    }
}
