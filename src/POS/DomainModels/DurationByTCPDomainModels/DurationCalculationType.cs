namespace POS.DomainModels.DurationByTCPDomainModels;

public enum DurationCalculationType
{
    Interpolation = 0,
    ExtrapolationAscending = 1,
    ExtrapolationDescending = 2,
    StepwiseExtrapolationAscending = 3,
    StepwiseExtrapolationDescending = 4,
}