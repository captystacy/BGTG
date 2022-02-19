using System.Threading.Tasks;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels;
using Calabonga.OperationResults;

namespace BGTG.Web.Infrastructure.Providers.POSProviders.Base
{
    public interface IProjectProvider : ISavable
    {
        Task<OperationResult<ProjectViewModel>> Write(ProjectViewModel viewModel);
    }
}
