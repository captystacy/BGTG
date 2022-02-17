namespace BGTG.POS.EnergyAndWaterTool.Base
{
    public interface IEnergyAndWaterWriter
    {
        void Write(EnergyAndWater energyAndWater, string templatePath, string savePath);
    }
}
