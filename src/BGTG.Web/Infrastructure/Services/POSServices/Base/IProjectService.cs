using System.Threading.Tasks;
using BGTG.Web.ViewModels.POSViewModels;
using Calabonga.OperationResults;

namespace BGTG.Web.Infrastructure.Services.POSServices.Base
{
    public interface IProjectService : ISavable
    {
        Task<OperationResult<ProjectViewModel>> Write(ProjectViewModel viewModel);
    }
}
