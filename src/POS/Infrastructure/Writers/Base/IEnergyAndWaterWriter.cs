using POS.DomainModels;

namespace POS.Infrastructure.Writers.Base;

public interface IEnergyAndWaterWriter
{
    MemoryStream Write(EnergyAndWater energyAndWater, string templatePath);
}