namespace POS.Models.CalendarPlanModels
{
    public class CalendarWork
    {
        public string WorkName { get; set; } = null!;
        public decimal TotalCost { get; set; }
        public decimal TotalCostIncludingCAIW { get; set; }
        public IEnumerable<ConstructionMonth> ConstructionMonths { get; set; } = null!;
        public int EstimateChapter { get; set; }

        protected bool Equals(CalendarWork other)
        {
            return WorkName == other.WorkName && TotalCost == other.TotalCost &&
                   TotalCostIncludingCAIW == other.TotalCostIncludingCAIW &&
                   ConstructionMonths.SequenceEqual(other.ConstructionMonths) && EstimateChapter == other.EstimateChapter;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CalendarWork)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(WorkName, TotalCost, TotalCostIncludingCAIW, ConstructionMonths, EstimateChapter);
        }

        public static bool operator ==(CalendarWork? left, CalendarWork? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CalendarWork? left, CalendarWork? right)
        {
            return !Equals(left, right);
        }
    }
}