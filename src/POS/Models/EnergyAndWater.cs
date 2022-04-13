namespace POS.Models
{
    public class EnergyAndWater
    {
        public int ConstructionYear { get; set; }
        public decimal VolumeCAIW { get; set; }
        public decimal Energy { get; set; }
        public decimal Water { get; set; }
        public decimal CompressedAir { get; set; }
        public decimal Oxygen { get; set; }

        protected bool Equals(EnergyAndWater other)
        {
            return ConstructionYear == other.ConstructionYear && VolumeCAIW == other.VolumeCAIW &&
                   Energy == other.Energy && Water == other.Water && CompressedAir == other.CompressedAir &&
                   Oxygen == other.Oxygen;

        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EnergyAndWater)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConstructionYear, VolumeCAIW, Energy, Water, CompressedAir, Oxygen);
        }

        public static bool operator ==(EnergyAndWater? left, EnergyAndWater? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EnergyAndWater? left, EnergyAndWater? right)
        {
            return !Equals(left, right);
        }
    }
}