namespace POS.Models.EstimateModels
{
    public class EstimateWork
    {
        public string WorkName { get; set; } = null!;
        public int Chapter { get; set; }
        public decimal EquipmentCost { get; set; }
        public decimal OtherProductsCost { get; set; }
        public decimal TotalCost { get; set; }
        public List<decimal> Percentages { get; set; } = null!;

        protected bool Equals(EstimateWork other)
        {
            return WorkName == other.WorkName && Chapter == other.Chapter && EquipmentCost == other.EquipmentCost &&
                   OtherProductsCost == other.OtherProductsCost && TotalCost == other.TotalCost &&
                   Percentages.SequenceEqual(other.Percentages);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EstimateWork)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(WorkName, Chapter, EquipmentCost, OtherProductsCost, TotalCost, Percentages);
        }

        public static bool operator ==(EstimateWork? left, EstimateWork? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EstimateWork? left, EstimateWork? right)
        {
            return !Equals(left, right);
        }
    }
}