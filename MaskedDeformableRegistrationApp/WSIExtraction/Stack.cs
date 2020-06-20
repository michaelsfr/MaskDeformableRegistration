using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp.WSIExtraction
{
    class StackList
    {
        public List<Stack> Stacks { get; set; }
    }

    class Stack
    {
        public string Stackname { get; set; }
        public int Threshold { get; set; }
        public List<int> Backgroundcolor { get; set; }
        public int Resolutionlevel { get; set; }
        public List<Slice> Section { get; set; }
    }
}
