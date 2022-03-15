using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface IEnergyAndWaterService
{
    MemoryStream Write(EnergyAndWaterViewModel dto);
}