using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp.Registration
{
    public class TransformationController
    {
        private List<string> TransformParameters;
        private List<string> TransformImages;

        private bool ComposeTransforms = true;
        private Interpolator Interpolator = Interpolator.LinearInterpolation;
        private int Order = 3;
        private double DefaultPixelValue = 0;

        public TransformationController(List<string> filenamesTransformParameters, List<string> imagesToTransform)
        {
            TransformParameters = filenamesTransformParameters;
            TransformImages = imagesToTransform;
        }

        public void ComposeTransformsParameters(bool compose)
        {
            ComposeTransforms = compose;
        }

        public void SetInterpolationType(Interpolator type, int order = 3)
        {
            Interpolator = type;
            Order = order;
        }

        public void SetDefaultPixelValue(double pixelValue)
        {
            DefaultPixelValue = pixelValue;
        }

        public void StartTransformation()
        {
            // TODO
        }

        public void Dispose()
        {
            // TODO
        }
    }
}
