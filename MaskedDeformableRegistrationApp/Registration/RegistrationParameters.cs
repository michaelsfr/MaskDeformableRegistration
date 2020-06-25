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
            parameters.PenaltyTerm = PenaltyTerm.TransformRigidityPenalty;
            return parameters;
        }

        public RegistrationDefaultParameters RegistrationType { get; set; }
        public sitk.ParameterMap ParamMapToUse { get; set; } = null;
        public uint NumberOfResolutions { get; set; } = 5;
        public SimilarityMetric Metric { get; set; } = SimilarityMetric.AdvancedMeanSquares;
        public Optimizer Optimizer { get; set; } = Optimizer.AdaptiveStochasticGradientDescent;
        public RegistrationStrategy RegistrationStrategy { get; set; } = RegistrationStrategy.MultiResolutionRegistration;
        public PenaltyTerm PenaltyTerm { get; set; } = PenaltyTerm.None;
    }
}
