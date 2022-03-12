using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.TCP.Models;

namespace POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Models;

public class StepwiseExtrapolationDurationByTCP : ExtrapolationDurationByTCP, IEquatable<StepwiseExtrapolationDurationByTCP>
{
    public decimal StepwiseDuration { get; }
    public PipelineStandard StepwisePipelineStandard { get; }

    public StepwiseExtrapolationDurationByTCP(string pipelineMaterial, int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, IEnumerable<PipelineStandard> calculationPipelineStandards, DurationCalculationType durationCalculationType, decimal duration, decimal roundedDuration, decimal preparatoryPeriod, decimal volumeChangePercent, decimal standardChangePercent, decimal stepwiseDuration, PipelineStandard stepwisePipelineStandard, char appendixKey, int appendixPage)
        : base(pipelineMaterial, pipelineDiameter, pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType, duration, roundedDuration, preparatoryPeriod, volumeChangePercent, standardChangePercent, appendixKey, appendixPage)
    {
        StepwiseDuration = stepwiseDuration;
        StepwisePipelineStandard = stepwisePipelineStandard;
    }

    public bool Equals(StepwiseExtrapolationDurationByTCP? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && StepwiseDuration == other.StepwiseDuration && base.Equals(other);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((StepwiseExtrapolationDurationByTCP)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), StepwiseDuration);
    }

    public static bool operator ==(StepwiseExtrapolationDurationByTCP left, StepwiseExtrapolationDurationByTCP right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(StepwiseExtrapolationDurationByTCP left, StepwiseExtrapolationDurationByTCP right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{nameof(StepwiseDuration)}: {StepwiseDuration}";
    }
}