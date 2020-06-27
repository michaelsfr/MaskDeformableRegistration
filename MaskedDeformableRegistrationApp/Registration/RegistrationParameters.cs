using MaskedDeformableRegistrationApp.Utils;
using System;
using System.Collections.Generic;
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
            parameters.RegistrationType = RegistrationDefaultParameters.rigid;
            parameters.NumberOfResolutions = 10;
            return parameters;
        }

        public static RegistrationParameters GetNonRigidRegistrationParameters()
        {
            RegistrationParameters parameters = new RegistrationParameters();
            parameters.RegistrationType = RegistrationDefaultParameters.bspline;
            parameters.NumberOfResolutions = 5;
            parameters.Metric = SimilarityMetric.AdvancedMattesMutualInformation;
            parameters.RegistrationStrategy = RegistrationStrategy.MultiMetricMultiResolutionRegistration;
            parameters.Penaltyterm = PenaltyTerm.None;
            return parameters;
        }

        public void SetTransformPenaltyTerm(sitk.Image coefficientMap, int[] orthogonality, int[] linearity, int[] correctness)
        {
            this.Penaltyterm = PenaltyTerm.TransformRigidityPenalty;
            // see: https://elastix.lumc.nl/doxygen/classelastix_1_1TransformRigidityPenalty.html
            this.CoefficientMap = coefficientMap;
            this.Orthogonality = orthogonality;
            this.Linearity = linearity;
            this.Correctness = correctness;
        }

        public void SetTransformBendingEnergy()
        {
            this.Penaltyterm = PenaltyTerm.TransformBendingEnergyPenalty;
        }

        public void SetDistancePreservingRigidityPenalty(sitk.Image rigidSegments, int[] penaltyGridSpacingInVoxels)
        {
            this.Penaltyterm = PenaltyTerm.DistancePreservingRigidityPenalty;
            // see: https://elastix.lumc.nl/doxygen/classelastix_1_1DistancePreservingRigidityPenalty.html
            this.CoefficientMap = rigidSegments;
            this.PenaltyGridSpacingInVoxels = penaltyGridSpacingInVoxels;
        }

        // general parameters
        private string specifiedLogFilename = null;
        public string ElastixLogFileName {
            get
            {
                if (specifiedLogFilename != null) return specifiedLogFilename;
                else return string.Format("elastix_log_file_{0}.txt", RegistrationType.ToString());
            }
            set
            {
                specifiedLogFilename = value;
            }
        }
        public string SubDirectory { get; set; }

        // rigid and non rigid parameters
        public RegistrationDefaultParameters RegistrationType { get; set; }
        public sitk.ParameterMap ParamMapToUse { get; set; } = null;
        public uint NumberOfResolutions { get; set; } = 5;
        public SimilarityMetric Metric { get; set; } = SimilarityMetric.AdvancedMeanSquares;
        public Optimizer Optimizer { get; set; } = Optimizer.AdaptiveStochasticGradientDescent;
        public RegistrationStrategy RegistrationStrategy { get; set; } = RegistrationStrategy.MultiResolutionRegistration;

        // non rigid parameters
        public PenaltyTerm Penaltyterm { get; set; } = PenaltyTerm.None;
        public sitk.Image WholeParticleSegment { get; set; } = null;
        public sitk.Image InnerStructureSegement { get; set; } = null;
        public sitk.Image CoefficientMap { get; set; } = null;
        public int[] Orthogonality { get; set; }
        public int[] Linearity { get; set; }
        public int[] Correctness { get; set; }
        public int[] PenaltyGridSpacingInVoxels { get; set; }
    }
}
