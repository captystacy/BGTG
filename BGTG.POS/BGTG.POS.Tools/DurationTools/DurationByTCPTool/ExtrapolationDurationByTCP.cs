using System;
using System.Collections.Generic;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.TCP;

namespace BGTG.POS.Tools.DurationTools.DurationByTCPTool
{
    public class ExtrapolationDurationByTCP : DurationByTCP, IEquatable<ExtrapolationDurationByTCP>
    {
        public decimal VolumeChangePercent { get; }
        public decimal StandardChangePercent { get; }

        public ExtrapolationDurationByTCP(string pipelineMaterial, int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, IEnumerable<PipelineStandard> calculationPipelineStandards, DurationCalculationType durationCalculationType, decimal duration, decimal roundedDuration, decimal preparatoryPeriod, decimal volumeChangePercent, decimal standardChangePercent, Appendix appendix) 
            : base(pipelineMaterial, pipelineDiameter, pipelineDiameterPresentation, pipelineLength, calculationPipelineStandards, durationCalculationType, duration, roundedDuration, preparatoryPeriod, appendix)
        {
            VolumeChangePercent = volumeChangePercent;
            StandardChangePercent = standardChangePercent;
        }

        public bool Equals(ExtrapolationDurationByTCP other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return VolumeChangePercent == other.VolumeChangePercent && StandardChangePercent == other.StandardChangePercent && base.Equals(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExtrapolationDurationByTCP)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(VolumeChangePercent, StandardChangePercent);
        }

        public static bool operator ==(ExtrapolationDurationByTCP left, ExtrapolationDurationByTCP right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ExtrapolationDurationByTCP left, ExtrapolationDurationByTCP right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(VolumeChangePercent)}: {VolumeChangePercent}, {nameof(StandardChangePercent)}: {StandardChangePercent}";
        }
    }
}
