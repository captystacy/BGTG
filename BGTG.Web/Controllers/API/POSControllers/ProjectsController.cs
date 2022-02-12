using System.Threading.Tasks;
using BGTG.Core;
using BGTG.Web.Infrastructure.Services.POSServices.Interfaces;
using BGTG.Web.ViewModels.POSViewModels;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.API.POSControllers
{
    [Route("api/[controller]")]
    public class ProjectsController : UnitOfWorkController
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IUnitOfWork unitOfWork, IProjectService projectService) : base(unitOfWork)
        {
            _projectService = projectService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<OperationResult<string>>> Write([FromForm] ProjectViewModel viewModel)
        {
            var operation = OperationResult.CreateResult<string>();

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Root.Errors)
                {
                    operation.AddError(error.ErrorMessage);
                }
                return OperationResultBeforeReturn(operation);
            }

            operation = await _projectService.Write(viewModel, User.Identity.Name);

            return OperationResultBeforeReturn(operation);
        }

        [HttpGet("[action]")]
        public IActionResult Download()
        {
            var path = _projectService.GetSavePath(User.Identity.Name);

            return PhysicalFile(path, AppData.DocxMimeType, AppData.ECPProjectDownloadFileName);
        }
    }
}
