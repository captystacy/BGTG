using BGTG.POS.EnergyAndWaterTool;
using BGTG.Web.ViewModels.POS.EnergyAndWaterViewModels;

namespace BGTG.Web.Infrastructure.Services.POS.Base
{
    public interface IEnergyAndWaterService : ISavable
    {
        EnergyAndWater Write(EnergyAndWaterCreateViewModel viewModel);
        EnergyAndWater Write(EnergyAndWater energyAndWater);
    }
}
