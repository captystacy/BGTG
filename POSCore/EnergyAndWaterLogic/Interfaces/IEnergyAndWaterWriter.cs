namespace POS.EnergyAndWaterLogic.Interfaces
{
    public interface IEnergyAndWaterWriter
    {
        void Write(EnergyAndWater energyAndWater, string templatePath, string savePath);
    }
}
