using BGTG.POS.EnergyAndWaterTool;
using BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels;

namespace BGTG.Web.Infrastructure.Services.POSServices.Interfaces
{
    public interface IEnergyAndWaterService : ISavable
    {
        EnergyAndWater Write(EnergyAndWaterCreateViewModel viewModel, string identityName);
        EnergyAndWater Write(EnergyAndWater energyAndWater, string identityName);
    }
}
