using Calabonga.OperationResults;
using POS.Models.DurationByTCPModels;
using POS.ViewModels;

namespace POS.Infrastructure.Calculators.Base
{
    public interface IDurationByTCPCalculator
    {
        Task<OperationResult<DurationByTCP>> Calculate(DurationByTCPViewModel viewModel);
    }
}