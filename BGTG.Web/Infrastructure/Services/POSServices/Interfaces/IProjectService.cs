using System.Threading.Tasks;
using BGTG.Web.ViewModels.POSViewModels;
using Calabonga.OperationResults;

namespace BGTG.Web.Infrastructure.Services.POSServices.Interfaces
{
    public interface IProjectService : ISavable
    {
        Task<OperationResult<string>> Write(ProjectViewModel viewModel, string identityName);
    }
}
