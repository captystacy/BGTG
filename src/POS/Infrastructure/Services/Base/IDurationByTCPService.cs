using Calabonga.OperationResults;
using POS.ViewModels;

namespace POS.Infrastructure.Services.Base
{
    public interface IDurationByTCPService
    {
        Task<OperationResult<MemoryStream>> GetDurationByTCPStream(DurationByTCPViewModel viewModel);
    }
}