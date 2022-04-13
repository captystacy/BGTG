namespace POS.Models
{
    public class DurationByLC
    {
        public decimal Duration { get; set; }
        public decimal TotalLaborCosts { get; set; }
        public decimal EstimateLaborCosts { get; set; }
        public decimal TechnologicalLaborCosts { get; set; }
        public decimal WorkingDayDuration { get; set; }
        public decimal Shift { get; set; }
        public decimal NumberOfWorkingDays { get; set; }
        public int NumberOfEmployees { get; set; }
        public decimal RoundedDuration { get; set; }
        public decimal TotalDuration { get; set; }
        public decimal PreparatoryPeriod { get; set; }
        public decimal AcceptanceTime { get; set; }
        public bool AcceptanceTimeIncluded { get; set; }
        public bool RoundingIncluded { get; set; }

        protected bool Equals(DurationByLC other)
        {
            return Duration == other.Duration && TotalLaborCosts == other.TotalLaborCosts &&
                   EstimateLaborCosts == other.EstimateLaborCosts &&
                   TechnologicalLaborCosts == other.TechnologicalLaborCosts &&
                   WorkingDayDuration == other.WorkingDayDuration && Shift == other.Shift &&
                   NumberOfWorkingDays == other.NumberOfWorkingDays && NumberOfEmployees == other.NumberOfEmployees &&
                   RoundedDuration == other.RoundedDuration && TotalDuration == other.TotalDuration &&
                   PreparatoryPeriod == other.PreparatoryPeriod && AcceptanceTime == other.AcceptanceTime &&
                   AcceptanceTimeIncluded == other.AcceptanceTimeIncluded && RoundingIncluded == other.RoundingIncluded;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DurationByLC)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Duration);
            hashCode.Add(TotalLaborCosts);
            hashCode.Add(EstimateLaborCosts);
            hashCode.Add(TechnologicalLaborCosts);
            hashCode.Add(WorkingDayDuration);
            hashCode.Add(Shift);
            hashCode.Add(NumberOfWorkingDays);
            hashCode.Add(NumberOfEmployees);
            hashCode.Add(RoundedDuration);
            hashCode.Add(TotalDuration);
            hashCode.Add(PreparatoryPeriod);
            hashCode.Add(AcceptanceTime);
            hashCode.Add(AcceptanceTimeIncluded);
            hashCode.Add(RoundingIncluded);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(DurationByLC? left, DurationByLC? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DurationByLC? left, DurationByLC? right)
        {
            return !Equals(left, right);
        }
    }
}