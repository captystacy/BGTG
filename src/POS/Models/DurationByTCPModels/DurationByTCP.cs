using POS.Models.DurationByTCPModels.TCPModels;

namespace POS.Models.DurationByTCPModels
{
    public abstract class DurationByTCP
    {
        public string PipelineMaterial { get; set; } = null!;
        public int PipelineDiameter { get; set; }
        public string PipelineDiameterPresentation { get; set; } = null!;
        public decimal PipelineLength { get; set; }
        public IEnumerable<PipelineStandard> CalculationPipelineStandards { get; set; } = null!;
        public DurationCalculationType DurationCalculationType { get; set; }
        public decimal Duration { get; set; }
        public decimal RoundedDuration { get; set; }
        public decimal PreparatoryPeriod { get; set; }
        public char AppendixKey { get; set; }
        public int AppendixPage { get; set; }

        protected bool Equals(DurationByTCP other)
        {
            return PipelineMaterial == other.PipelineMaterial && PipelineDiameter == other.PipelineDiameter &&
                   PipelineDiameterPresentation == other.PipelineDiameterPresentation &&
                   PipelineLength == other.PipelineLength &&
                   CalculationPipelineStandards.SequenceEqual(other.CalculationPipelineStandards) &&
                   DurationCalculationType == other.DurationCalculationType && Duration == other.Duration &&
                   RoundedDuration == other.RoundedDuration && PreparatoryPeriod == other.PreparatoryPeriod &&
                   AppendixKey == other.AppendixKey && AppendixPage == other.AppendixPage;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DurationByTCP)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(PipelineMaterial);
            hashCode.Add(PipelineDiameter);
            hashCode.Add(PipelineDiameterPresentation);
            hashCode.Add(PipelineLength);
            hashCode.Add(CalculationPipelineStandards);
            hashCode.Add((int)DurationCalculationType);
            hashCode.Add(Duration);
            hashCode.Add(RoundedDuration);
            hashCode.Add(PreparatoryPeriod);
            hashCode.Add(AppendixKey);
            hashCode.Add(AppendixPage);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(DurationByTCP? left, DurationByTCP? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DurationByTCP? left, DurationByTCP? right)
        {
            return !Equals(left, right);
        }
    }
}