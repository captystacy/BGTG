namespace POS.Models.DurationByTCPModels
{
    public class ExtrapolationDurationByTCP : DurationByTCP
    {
        public decimal VolumeChangePercent { get; set; }
        public decimal StandardChangePercent { get; set; }

        protected bool Equals(ExtrapolationDurationByTCP other)
        {
            return base.Equals(other) && VolumeChangePercent == other.VolumeChangePercent && StandardChangePercent == other.StandardChangePercent;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExtrapolationDurationByTCP)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), VolumeChangePercent, StandardChangePercent);
        }

        public static bool operator ==(ExtrapolationDurationByTCP? left, ExtrapolationDurationByTCP? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ExtrapolationDurationByTCP? left, ExtrapolationDurationByTCP? right)
        {
            return !Equals(left, right);
        }
    }
}