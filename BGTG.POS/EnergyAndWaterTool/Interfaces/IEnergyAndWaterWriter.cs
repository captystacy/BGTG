namespace BGTG.POS.EnergyAndWaterTool.Interfaces
{
    public interface IEnergyAndWaterWriter
    {
        void Write(EnergyAndWater energyAndWater, string templatePath, string savePath);
    }
}
