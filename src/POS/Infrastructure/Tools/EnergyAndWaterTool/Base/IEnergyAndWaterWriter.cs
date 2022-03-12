namespace POS.Infrastructure.Tools.EnergyAndWaterTool.Base;

public interface IEnergyAndWaterWriter
{
    MemoryStream Write(EnergyAndWater energyAndWater, string templatePath);
}