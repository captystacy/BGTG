using Calabonga.OperationResults;
using POS.ViewModels;

namespace POS.Infrastructure.Services.Base
{
    public interface IProjectService
    {
        Task<OperationResult<MemoryStream>> GetProjectStream(ProjectViewModel viewModel);
    }
}