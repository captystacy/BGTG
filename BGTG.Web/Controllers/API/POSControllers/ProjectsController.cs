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
            var operationResult = OperationResult.CreateResult<string>();

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Root.Errors)
                {
                    operationResult.AddError(error.ErrorMessage);
                }
                return OperationResultBeforeReturn(operationResult);
            }

            operationResult = await _projectService.Write(viewModel, User.Identity.Name);

            return OperationResultBeforeReturn(operationResult);
        }

        [HttpGet("[action]")]
        public IActionResult Download()
        {
            var path = _projectService.GetSavePath(User.Identity.Name);

            return PhysicalFile(path, AppData.DocxMimeType, AppData.ECPProjectDownloadFileName);
        }
    }
}
