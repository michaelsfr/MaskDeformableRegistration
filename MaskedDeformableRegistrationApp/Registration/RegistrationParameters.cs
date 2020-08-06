using MaskedDeformableRegistrationApp.Segmentation;
using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sitk = itk.simple;

namespace MaskedDeformableRegistrationApp.Registration
{
    public class RegistrationParameters
    {
        private RegistrationParameters()
        {

        }

        public static RegistrationParameters GetRigidRegistrationParameters()
        {
            RegistrationParameters parameters = new RegistrationParameters();
            parameters.Type = RegistrationType.Rigid;
            parameters.RegistrationDefaultParams = RegistrationDefaultParameters.rigid;
            parameters.NumberOfResolutions = 10;

            return parameters;
        }

        public static RegistrationParameters GetNonRigidRegistrationParameters()
        {
            RegistrationParameters parameters = new RegistrationParameters();
            parameters.Type = RegistrationType.NonRigid;
            parameters.RegistrationDefaultParams = RegistrationDefaultParameters.bspline;
            parameters.NumberOfResolutions = 5;
            parameters.Metric = SimilarityMetric.AdvancedMattesMutualInformation;
            parameters.RegistrationStrategy = RegistrationStrategy.MultiMetricMultiResolutionRegistration;
            parameters.Penaltyterm = PenaltyTerm.None;
            return parameters;
        }

        public void SetTransformPenaltyTerm(string coefficientMapFilename = null, int orthonormality = 1, int linearity = 1, int properness = 1)
        {
            this.Penaltyterm = PenaltyTerm.TransformRigidityPenalty;
            // see: https://elastix.lumc.nl/doxygen/classelastix_1_1TransformRigidityPenalty.html
            this.CoefficientMapFilename = coefficientMapFilename;
            this.OrthonormalityConditionWeight = orthonormality;
            this.LinearityConditionWeight  = linearity;
            this.PropernessConditionWeight = properness;
        }

        public void SetTransformBendingEnergy()
        {
            this.Penaltyterm = PenaltyTerm.TransformBendingEnergyPenalty;
        }

        public void SetDistancePreservingRigidityPenalty(string segmentedImage = null, int[] penaltyGridSpacingInVoxels = null)
        {
            this.Penaltyterm = PenaltyTerm.DistancePreservingRigidityPenalty;
            // see: https://elastix.lumc.nl/doxygen/classelastix_1_1DistancePreservingRigidityPenalty.html
            this.SegmentedImageFilename = segmentedImage;
            this.PenaltyGridSpacingInVoxels = penaltyGridSpacingInVoxels;
        }

        // general parameters
        public RegistrationType Type;
        private string specifiedLogFilename = null;
        public string ElastixLogFileName {
            get
            {
                if (specifiedLogFilename != null) return specifiedLogFilename;
                else return string.Format("elastix_log_file_{0}.txt", RegistrationDefaultParams.ToString());
            }
            set
            {
                specifiedLogFilename = value;
            }
        }
        public string SubDirectory { get; set; } = "";
        public string OutputDirectory {
            get
            {
                return Path.Combine(ApplicationContext.OutputPath, SubDirectory);
            }
        }
        public int Iteration = -1;

        public sitk.ParameterMap ParamMapToUse { get; set; } = null;
        public RegistrationOrder Order { get; set; } = RegistrationOrder.FirstInStackIsReference;

        public uint LargestImageWidth { get; set; }
        public uint LargestImageHeight { get; set; }

        public bool UseFixedMask { get; set; } = false;
        public bool UseMovingMask { get; set; } = false;
        public bool UseInnerStructuresSegmentation { get; set; } = false;

        public bool IsBinaryTransform { get; set; } = false;
        public bool ComputeJaccobian { get; set; } = false;

        // rigid and non rigid parameters

        public MaskedRigidRegistrationOptions RigidOptions { get; set; } = MaskedRigidRegistrationOptions.None;
        // type
        public RegistrationDefaultParameters RegistrationDefaultParams { get; set; }

        // metric
        public SimilarityMetric Metric { get; set; } = SimilarityMetric.AdvancedMeanSquares;
        public int[] NumberOfHistogramBins { get; set; } = { 32 };

        // image pyramid
        public ImagePyramid FixedImagePyramid { get; set; } = ImagePyramid.FixedRecursiveImagePyramid;
        public ImagePyramid MovingImagePyramid { get; set; } = ImagePyramid.MovingRecursiveImagePyramid;

        // optimizer
        public Optimizer Optimizer { get; set; } = Optimizer.AdaptiveStochasticGradientDescent;

        // reg strategy
        public RegistrationStrategy RegistrationStrategy { get; set; } = RegistrationStrategy.MultiResolutionRegistration;
        public uint NumberOfResolutions { get; set; } = 5;

        // non rigid parameters

        public MaskedNonRigidRegistrationOptions NonRigidOptions { get; set; } = MaskedNonRigidRegistrationOptions.None;
        // penalties
        public PenaltyTerm Penaltyterm { get; set; } = PenaltyTerm.None;
        // transform rigidity
        public string CoefficientMapFilename { get; set; } = null;
        public int OrthonormalityConditionWeight { get; set; } = 1;
        public int LinearityConditionWeight { get; set; } = 1;
        public int PropernessConditionWeight { get; set; } = 1;
        // distance preserving
        public string SegmentedImageFilename { get; set; } = null;
        public int[] PenaltyGridSpacingInVoxels { get; set; } = null;

        // evaluation
        public string FixedImageFilename { get; set; }
        public Dictionary<string, List<sitk.VectorOfParameterMap>> TransformationParameterMap { get; set; } = new Dictionary<string, List<sitk.VectorOfParameterMap>>();
        public string MovingImagePointSetFilename { get; set; }
        public string FixedImagePointSetFilename { get; set; }
        //public sitk.Transform FixedPointSetTransform { get; set; } = null;
        //public sitk.Transform MovingPointSetTransform { get; set; } = null;

        // segmentaion
        public SegmentationParameters WholeTissueSegParams { get; set; } = new SegmentationParameters();
        public SegmentationParameters InnerStructuresSegParams { get; set; } = new SegmentationParameters();
    }
}
