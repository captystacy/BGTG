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
            if (other == null)
            {
                return false;
            }

            return ConstructionYear == other.ConstructionYear
                && VolumeCAIW == other.VolumeCAIW
                && Energy == other.Energy
                && Water == other.Water
                && CompressedAir == other.CompressedAir
                && Oxygen == other.Oxygen;
        }

        public static bool operator ==(EnergyAndWater energyAndWater1, EnergyAndWater energyAndWater2)
        {
            if (energyAndWater1 is null)
            {
                return energyAndWater2 is null;
            }

            return energyAndWater1.Equals(energyAndWater2);
        }

        public static bool operator !=(EnergyAndWater energyAndWater1, EnergyAndWater energyAndWater2) => !(energyAndWater1 == energyAndWater2);

        public override bool Equals(object obj) => Equals(obj as EnergyAndWater);

        public override int GetHashCode() => HashCode.Combine(ConstructionYear, VolumeCAIW, Energy, Water, CompressedAir, Oxygen);

        public override string ToString()
        {
            return string.Join(", ", ConstructionYear, VolumeCAIW, Energy, Water, CompressedAir, Oxygen);
        }
    }
}
