namespace POSCore.EnergyAndWaterLogic
{
    public class EnergyAndWater
    {
        public int ConstructionYear { get; }
        public decimal SmrVolume { get; }
        public decimal Energy { get; }
        public decimal Water { get; }
        public decimal CompressedAir { get; }
        public decimal Oxygen { get; }

        public EnergyAndWater(int constructionYear, decimal smrVolume, decimal energy, decimal water, decimal compressedAir, decimal oxygen)
        {
            ConstructionYear = constructionYear;
            SmrVolume = smrVolume;
            Energy = energy;
            Water = water;
            CompressedAir = compressedAir;
            Oxygen = oxygen;
        }
    }
}
