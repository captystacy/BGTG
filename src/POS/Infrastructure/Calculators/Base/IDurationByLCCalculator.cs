using Calabonga.OperationResults;
using POS.Models;
using POS.ViewModels;

namespace POS.Infrastructure.Calculators.Base
{
    public interface IDurationByLCCalculator
    {
        Task<OperationResult<DurationByLC>> Calculate(DurationByLCViewModel viewModel);
    }
}