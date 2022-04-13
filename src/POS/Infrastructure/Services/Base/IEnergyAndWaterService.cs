using Calabonga.OperationResults;
using POS.ViewModels;

namespace POS.Infrastructure.Services.Base
{
    public interface IEnergyAndWaterService
    {
        Task<OperationResult<MemoryStream>> GetEnergyAndWaterStream(EnergyAndWaterViewModel viewModel);
    }
}