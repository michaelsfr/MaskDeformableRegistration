using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp.WSIExtraction
{
    /// <summary>
    /// Class represents a list of stacks.
    /// </summary>
    class StackList
    {
        public List<Stack> Stacks { get; set; }
    }

    /// <summary>
    /// Class represents a stack of wsi images.
    /// </summary>
    class Stack
    {
        public string Stackname { get; set; }
        public int Threshold { get; set; }
        public List<int> Backgroundcolor { get; set; }
        public int Resolutionlevel { get; set; }
        public List<Slice> Section { get; set; }
    }
}
