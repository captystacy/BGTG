namespace POS.Models.DurationByTCPModels
{
    public class InterpolationDurationByTCP : DurationByTCP
    {
        public decimal DurationChange { get; set; }
        public decimal VolumeChange { get; set; }

        protected bool Equals(InterpolationDurationByTCP other)
        {
            return base.Equals(other) && DurationChange == other.DurationChange && VolumeChange == other.VolumeChange;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InterpolationDurationByTCP)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), DurationChange, VolumeChange);
        }

        public static bool operator ==(InterpolationDurationByTCP? left, InterpolationDurationByTCP? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InterpolationDurationByTCP? left, InterpolationDurationByTCP? right)
        {
            return !Equals(left, right);
        }
    }
}