namespace POS.Models.EstimateModels
{
    public class Estimate
    {
        public DateTime ConstructionStartDate { get; set; }
        public decimal ConstructionDuration { get; set; }
        public int ConstructionDurationCeiling { get; set; }
        public IEnumerable<EstimateWork> PreparatoryEstimateWorks { get; set; } = null!;
        public IEnumerable<EstimateWork> MainEstimateWorks { get; set; } = null!;
        public TotalWorkChapter TotalWorkChapter { get; set; }

        protected bool Equals(Estimate other)
        {
            return ConstructionStartDate.Equals(other.ConstructionStartDate) &&
                   ConstructionDuration == other.ConstructionDuration &&
                   ConstructionDurationCeiling == other.ConstructionDurationCeiling &&
                   TotalWorkChapter == other.TotalWorkChapter;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Estimate)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConstructionStartDate, ConstructionDuration, ConstructionDurationCeiling, (int)TotalWorkChapter);
        }

        public static bool operator ==(Estimate? left, Estimate? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Estimate? left, Estimate? right)
        {
            return !Equals(left, right);
        }
    }
}