using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp.Registration
{
    public interface IRegistration<T, U, N>
    {
        object Execute();
        void SetFixedMask(T fixedMask);
        void SetMovingMask(T movingMask);
        void SetDefaultParameterMap(RegistrationDefaultParameters type, uint numberOfResolutions);
        void SetParameterMap(string file);
        T GetOutput();
        U GetParameterMap();
        N GetTransformationParameterMap();
        void Dispose();
    }
}
