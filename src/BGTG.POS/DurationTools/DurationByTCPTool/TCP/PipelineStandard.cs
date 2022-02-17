using System;

namespace BGTG.POS.DurationTools.DurationByTCPTool.TCP
{
    public class PipelineStandard : IEquatable<PipelineStandard>
    {
        public decimal PipelineLength { get; }
        public decimal Duration { get; }
        public decimal PreparatoryPeriod { get; }

        public PipelineStandard(decimal pipelineLength, decimal duration, decimal preparatoryPeriod)
        {
            PipelineLength = pipelineLength;
            Duration = duration;
            PreparatoryPeriod = preparatoryPeriod;
        }

        public bool Equals(PipelineStandard? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return PipelineLength == other.PipelineLength && Duration == other.Duration && PreparatoryPeriod == other.PreparatoryPeriod;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PipelineStandard) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PipelineLength, Duration, PreparatoryPeriod);
        }

        public static bool operator ==(PipelineStandard left, PipelineStandard right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PipelineStandard left, PipelineStandard right)
        {
            return !Equals(left, right);
        }
    }
}
