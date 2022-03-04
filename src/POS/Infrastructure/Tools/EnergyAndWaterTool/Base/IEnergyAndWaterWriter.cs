namespace POS.Infrastructure.Tools.EnergyAndWaterTool.Base;

public interface IEnergyAndWaterWriter
{
    void Write(EnergyAndWater energyAndWater, string templatePath);
}