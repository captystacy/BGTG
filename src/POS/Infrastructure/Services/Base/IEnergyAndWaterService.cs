using POS.Infrastructure.Tools.EnergyAndWaterTool;
using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface IEnergyAndWaterService
{
    EnergyAndWater Write(EnergyAndWaterCreateViewModel viewModel);
    EnergyAndWater Write(EnergyAndWater energyAndWater);
}