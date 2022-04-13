namespace POS.Models.CalendarPlanModels
{
    public class ConstructionMonth
    {
        public DateTime Date { get; set; }
        public decimal InvestmentVolume { get; set; }
        public decimal VolumeCAIW { get; set; }
        public decimal PercentPart { get; set; }
        public int CreationIndex { get; set; }

        protected bool Equals(ConstructionMonth other)
        {
            return Date.Equals(other.Date) && InvestmentVolume == other.InvestmentVolume &&
                   VolumeCAIW == other.VolumeCAIW && PercentPart == other.PercentPart &&
                   CreationIndex == other.CreationIndex;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConstructionMonth)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, InvestmentVolume, VolumeCAIW, PercentPart, CreationIndex);
        }

        public static bool operator ==(ConstructionMonth? left, ConstructionMonth? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ConstructionMonth? left, ConstructionMonth? right)
        {
            return !Equals(left, right);
        }
    }
}