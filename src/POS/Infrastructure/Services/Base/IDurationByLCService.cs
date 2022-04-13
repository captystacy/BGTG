using Calabonga.OperationResults;
using POS.ViewModels;

namespace POS.Infrastructure.Services.Base
{
    public interface IDurationByLCService
    {
        Task<OperationResult<MemoryStream>> GetDurationByLCStream(DurationByLCViewModel viewModel);
    }
}