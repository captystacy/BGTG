using POS.Models.DurationByTCPModels.TCPModels;

namespace POS.Models.DurationByTCPModels
{
    public class StepwiseExtrapolationDurationByTCP : ExtrapolationDurationByTCP
    {
        public decimal StepwiseDuration { get; set; }
        public PipelineStandard StepwisePipelineStandard { get; set; } = null!;

        protected bool Equals(StepwiseExtrapolationDurationByTCP other)
        {
            return base.Equals(other) && StepwiseDuration == other.StepwiseDuration && StepwisePipelineStandard.Equals(other.StepwisePipelineStandard);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StepwiseExtrapolationDurationByTCP)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), StepwiseDuration, StepwisePipelineStandard);
        }

        public static bool operator ==(StepwiseExtrapolationDurationByTCP? left, StepwiseExtrapolationDurationByTCP? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StepwiseExtrapolationDurationByTCP? left, StepwiseExtrapolationDurationByTCP? right)
        {
            return !Equals(left, right);
        }
    }
}