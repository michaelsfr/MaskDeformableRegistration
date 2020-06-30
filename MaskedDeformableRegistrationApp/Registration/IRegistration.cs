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
        void Execute();
        void SetFixedMask(T fixedMask);
        void SetMovingMask(T movingMask);
        void SetDefaultParameterMap(RegistrationDefaultParameters type, uint numberOfResolutions);
        void SetParameterMap(string file);
        U GetParameterMap();
        N GetTransformationFile();
        T GetOutput();
        void Dispose();
    }
}
