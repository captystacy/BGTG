using BGTG.POS.EnergyAndWaterTool;
using BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels;

namespace BGTG.Web.Infrastructure.Services.POSServices.Base
{
    public interface IEnergyAndWaterService : ISavable
    {
        EnergyAndWater Write(EnergyAndWaterCreateViewModel viewModel);
        EnergyAndWater Write(EnergyAndWater energyAndWater);
    }
}
