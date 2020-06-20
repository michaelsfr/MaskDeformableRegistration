using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp.Segmentation
{
    interface ISegmentation<T>
    {
        void Execute();
        T GetOutput();
        void Dispose();
    }
}
