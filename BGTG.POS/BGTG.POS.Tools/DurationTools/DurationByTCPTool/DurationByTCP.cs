using System;
using System.Collections.Generic;
using System.Linq;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.TCP;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.POS.Tools.DurationTools.DurationByTCPTool
{
    public abstract class DurationByTCP : Identity, IEquatable<DurationByTCP>
    {
        public string PipelineMaterial { get; }
        public int PipelineDiameter { get; }
        public string PipelineDiameterPresentation { get; }
        public decimal PipelineLength { get; }
        public IEnumerable<PipelineStandard> CalculationPipelineStandards { get; }
        public DurationCalculationType DurationCalculationType { get; }
        public decimal Duration { get; }
        public decimal RoundedDuration { get; }
        public decimal PreparatoryPeriod { get; }
        public Appendix Appendix { get; }

        protected DurationByTCP(string pipelineMaterial, int pipelineDiameter, string pipelineDiameterPresentation, decimal pipelineLength, IEnumerable<PipelineStandard> calculationPipelineStandards, DurationCalculationType durationCalculationType, decimal duration, decimal roundedDuration, decimal preparatoryPeriod, Appendix appendix)
        {
            PipelineMaterial = pipelineMaterial;
            PipelineDiameterPresentation = pipelineDiameterPresentation;
            PipelineLength = pipelineLength;
            CalculationPipelineStandards = calculationPipelineStandards;
            DurationCalculationType = durationCalculationType;
            Duration = duration;
            RoundedDuration = roundedDuration;
            PreparatoryPeriod = preparatoryPeriod;
            Appendix = appendix;
            PipelineDiameter = pipelineDiameter;
        }

        public bool Equals(DurationByTCP other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return PipelineMaterial == other.PipelineMaterial && PipelineDiameter == other.PipelineDiameter &&
                   PipelineDiameterPresentation == other.PipelineDiameterPresentation &&
                   PipelineLength == other.PipelineLength &&
                   CalculationPipelineStandards.SequenceEqual(other.CalculationPipelineStandards) &&
                   DurationCalculationType == other.DurationCalculationType && Duration == other.Duration &&
                   RoundedDuration == other.RoundedDuration && PreparatoryPeriod == other.PreparatoryPeriod &&
                   Equals(Appendix, other.Appendix);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DurationByTCP) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(PipelineMaterial);
            hashCode.Add(PipelineDiameter);
            hashCode.Add(PipelineDiameterPresentation);
            hashCode.Add(PipelineLength);
            hashCode.Add(CalculationPipelineStandards);
            hashCode.Add((int) DurationCalculationType);
            hashCode.Add(Duration);
            hashCode.Add(RoundedDuration);
            hashCode.Add(PreparatoryPeriod);
            hashCode.Add(Appendix);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(DurationByTCP left, DurationByTCP right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DurationByTCP left, DurationByTCP right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(PipelineMaterial)}: {PipelineMaterial}, {nameof(PipelineDiameter)}: {PipelineDiameter}, {nameof(PipelineDiameterPresentation)}: {PipelineDiameterPresentation}, {nameof(PipelineLength)}: {PipelineLength}, {nameof(CalculationPipelineStandards)}: {CalculationPipelineStandards}, {nameof(DurationCalculationType)}: {DurationCalculationType}, {nameof(Duration)}: {Duration}, {nameof(RoundedDuration)}: {RoundedDuration}, {nameof(PreparatoryPeriod)}: {PreparatoryPeriod}, {nameof(Appendix)}: {Appendix}";
        }
    }
}
