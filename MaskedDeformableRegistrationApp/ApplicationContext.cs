using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp
{
    public static class ApplicationContext
    {
        public static string OutputPath { get; set; }
        public static List<string> WsiPaths { get; set; } = new List<string>();

    }
}
