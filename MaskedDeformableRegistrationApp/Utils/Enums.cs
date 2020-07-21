using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaskedDeformableRegistrationApp.Utils
{
    public enum RegistrationType
    {
        Rigid,
        NonRigid
    }

    public enum RegistrationStrategy
    {
        MultiMetricMultiResolutionRegistration,
        MultiResolutionRegistration
    }

    public enum RegistrationDefaultParameters
    {
        translation = 0,
        similarity = 1,
        rigid = 2,
        affine = 3,
        nonrigid = 4,
        bspline = 5,
        diffusion = 6,
        spline = 7,
        recursive = 8
    }

    public enum SimilarityMetric
    {
        AdvancedMeanSquares,
        AdvancedNormalizedCorrelation,
        AdvancedMattesMutualInformation,
        AdvancedKappaStatistic,
        CorrespondingPointsEuclideanDistanceMetric
    }

    public enum PenaltyTerm
    {
        None,
        DistancePreservingRigidityPenalty,
        TransformBendingEnergyPenalty,
        TransformRigidityPenalty,
        StatisticalShapePenalty,
        DisplacementMagnitudePenalty
    }

    public enum Optimizer
    {
        AdaptiveStochasticGradientDescent,
        AdaptiveStochasticLBFGS,
        AdaptiveStochasticVarianceReducedGradient,
        ConjugateGradient,
        FiniteDifferenceGradientDescent,
        RegularStepGradientDescent,
        PreconditionedGradientDescent,
        FullSearch
    }

    public enum Interpolator
    {
        NearestNighbour,
        LinearInterpolation,
        BSplineInterpolation
    }

    public enum ImageSampler
    {
        Full,
        Grid,
        RandomCoordinate,
        Random,
        RandomSparseMask
    }

    public enum ResampleInterpolator
    {
        FinalBSplineInterpolator,
        FinalLinearInterpolator,
        FinalNearestNeighborInterpolator
    }

    public enum ImagePyramid
    {
        FixedRecursiveImagePyramid,
        FixedShrinkingImagePyramid,
        FixedSmoothingImagePyramid,
        MovingRecursiveImagePyramid,
        MovingShrinkingImagePyramid,
        MovingSmoothingImagePyramid
    }

    public enum ColorSpace
    {
        HaematoxylinEosin = 0,
        HaematoxylinDAB = 1,
        FastRedFastBlueDAB = 2,
        MethylGreenDAB = 3,
        HaematoxylinEosinDAB = 4,
        HaematoxylinAEC = 5,
        AlcianBlueHaematoxylin = 6,
        HaematoxylinPAS = 7,
        RGB = 8,
        BGR = 9,
        HSV = 10,
        HLS = 11,
        LAB = 12,
        LUV = 13
    }

    public enum RegistrationOrder
    {
        FirstInStackIsReference,
        LastInStackIsReference,
        PreviousIsReference,
        MedianIsReference
    }

    public enum MaskedRigidRegistrationOptions
    {
        None,
        BinaryRegistrationWholeTissue,
        BinaryRegistrationInnerStructures,
        ComponentwiseRegistration
    }

    public enum MaskedNonRigidRegistrationOptions
    {
        None,
        BsplineWithPenaltyTerm,
        BsplineWithPenaltyTermAndCoefficientMap,
        ComposeIndependantRegistrations,
        DiffuseRegistration
    }
}
