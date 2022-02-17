using System.Threading.Tasks;
using BGTG.Web.ViewModels.POS;
using Calabonga.OperationResults;

namespace BGTG.Web.Infrastructure.Services.POS.Base
{
    public interface IProjectService : ISavable
    {
        Task<OperationResult<ProjectViewModel>> Write(ProjectViewModel viewModel);
    }
}
