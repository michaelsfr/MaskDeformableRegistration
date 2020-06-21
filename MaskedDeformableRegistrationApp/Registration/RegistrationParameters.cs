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
        public RegistrationParameters()
        {

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
