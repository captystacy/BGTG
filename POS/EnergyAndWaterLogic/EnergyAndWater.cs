using System;

namespace POS.EnergyAndWaterLogic
{
    public class EnergyAndWater : IEquatable<EnergyAndWater>
    {
        public int ConstructionYear { get; }
        public decimal VolumeCAIW { get; }
        public decimal Energy { get; }
        public decimal Water { get; }
        public decimal CompressedAir { get; }
        public decimal Oxygen { get; }

        public EnergyAndWater(int constructionYear, decimal volumeCAIW, decimal energy, decimal water, decimal compressedAir, decimal oxygen)
        {
            ConstructionYear = constructionYear;
            VolumeCAIW = volumeCAIW;
            Energy = energy;
            Water = water;
            CompressedAir = compressedAir;
            Oxygen = oxygen;
        }

        public bool Equals(EnergyAndWater other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ConstructionYear == other.ConstructionYear && VolumeCAIW == other.VolumeCAIW && Energy == other.Energy && Water == other.Water && CompressedAir == other.CompressedAir && Oxygen == other.Oxygen;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EnergyAndWater) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConstructionYear, VolumeCAIW, Energy, Water, CompressedAir, Oxygen);
        }

        public static bool operator ==(EnergyAndWater left, EnergyAndWater right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EnergyAndWater left, EnergyAndWater right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(ConstructionYear)}: {ConstructionYear}, {nameof(VolumeCAIW)}: {VolumeCAIW}, {nameof(Energy)}: {Energy}, {nameof(Water)}: {Water}, {nameof(CompressedAir)}: {CompressedAir}, {nameof(Oxygen)}: {Oxygen}";
        }
    }
}
