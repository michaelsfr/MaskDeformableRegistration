using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp.Segmentation
{
    public class SegmentationParameters
    {
        public SegmentationParameters()
        {

        }

        public int ParticlePixelAmount { get; set; }
        public ColorSpace Colorspace { get; set; } = ColorSpace.HSV;
        public int Channel { get; set; } = 0;
        public bool UseOtsu { get; set; } = true;
        public int Threshold { get; set; } = 127;
        public bool UseKmeans { get; set; } = false;
        public bool ManualContourSizeRestriction { get; set; } = false;
        public int MinContourSize { get; set; }
        public int MaxContourSize { get; set; }

    }
}
