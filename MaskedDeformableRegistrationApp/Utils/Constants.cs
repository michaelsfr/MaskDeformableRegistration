using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp.Utils
{
    public static class Constants
    {
        public const string cMetric = "Metric";
        public const string cTransform = "Transform";
        public const string cRegistration = "Registration";
        public const string cOptimizer = "Optimizer";
        public const string cSegmentedImageName = "SegmentedImageName";
        public const string cMovingRigidityImageName = "MovingRigidityImageName";
        public const string cLinearityConditionWeight = "LinearityConditionWeight";
        public const string cOrthonormalityConditionWeight = "OrthonormalityConditionWeight";
        public const string cPropernessConditionWeight = "PropernessConditionWeight";
        public const string cNumberOfHistogramBins = "NumberOfHistogramBins";
        public const string cPenaltyGridSpacingInVoxels = "PenaltyGridSpacingInVoxels";
        public const string cFixedImagePyramid = "FixedImagePyramid";
        public const string cMovingImagePyramid = "MovingImagePyramid";
        public const string cMetric0Weight = "Metric0Weight";
        public const string cMetric1Weight = "Metric0Weight";
        public const string cCoefficientFilename = "\\coefficientMap.png";
    }
}
