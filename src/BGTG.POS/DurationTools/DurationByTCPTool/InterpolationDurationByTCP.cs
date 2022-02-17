using System;
using System.Collections.Generic;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP;

namespace BGTG.POS.DurationTools.DurationByTCPTool
{
    public class InterpolationDurationByTCP : DurationByTCP, IEquatable<InterpolationDurationByTCP>
    {
        public decimal DurationChange { get; }
        public decimal VolumeChange { get; }

        public InterpolationDurationByTCP(string pipelineMaterial, int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, IEnumerable<PipelineStandard> calculationPipelineStandards, DurationCalculationType durationCalculationType, decimal duration, decimal roundedDuration, decimal preparatoryPeriod, decimal durationChange, decimal volumeChange, char appendixKey, int appendixPage)
            : base(pipelineMaterial, pipelineDiameter, pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType, duration, roundedDuration, preparatoryPeriod, appendixKey, appendixPage)
        {
            DurationChange = durationChange;
            VolumeChange = volumeChange;
        }

        public bool Equals(InterpolationDurationByTCP? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && DurationChange == other.DurationChange && VolumeChange == other.VolumeChange;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((InterpolationDurationByTCP) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), DurationChange, VolumeChange);
        }

        public static bool operator ==(InterpolationDurationByTCP left, InterpolationDurationByTCP right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InterpolationDurationByTCP left, InterpolationDurationByTCP right)
        {
            return !Equals(left, right);
        }
    }
}
