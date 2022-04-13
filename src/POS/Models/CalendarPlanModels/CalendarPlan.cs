namespace POS.Models.CalendarPlanModels
{
    public class CalendarPlan
    {
        public DateTime ConstructionStartDate { get; set; }
        public decimal ConstructionDuration { get; set; }
        public int ConstructionDurationCeiling { get; set; }
        public IEnumerable<CalendarWork> CalendarWorks { get; set; } = null!;

        protected bool Equals(CalendarPlan other)
        {
            return ConstructionStartDate.Equals(other.ConstructionStartDate) &&
                   ConstructionDuration == other.ConstructionDuration &&
                   ConstructionDurationCeiling == other.ConstructionDurationCeiling &&
                   CalendarWorks.SequenceEqual(other.CalendarWorks);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CalendarPlan)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConstructionStartDate, ConstructionDuration, ConstructionDurationCeiling, CalendarWorks);
        }

        public static bool operator ==(CalendarPlan? left, CalendarPlan? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CalendarPlan? left, CalendarPlan? right)
        {
            return !Equals(left, right);
        }
    }
}