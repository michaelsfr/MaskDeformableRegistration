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
        public static sitk.ParameterMap GetDefaultParameterMap(RegistrationDefaultParameters registrationType)
        {
            using (sitk.ElastixImageFilter elastix = new sitk.ElastixImageFilter())
            {
                return elastix.GetDefaultParameterMap(registrationType.ToString());
            }            
        }
    }
}
