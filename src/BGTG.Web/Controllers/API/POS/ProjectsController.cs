using System.Threading.Tasks;
using BGTG.Entities.Core;
using BGTG.Web.Infrastructure.Attributes;
using BGTG.Web.Infrastructure.Services.POS.Base;
using BGTG.Web.ViewModels.POS;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.API.POS;

[Route("api/[controller]")]
public class ProjectsController : UnitOfWorkController
{
    private readonly IProjectService _projectService;

    public ProjectsController(IUnitOfWork unitOfWork, IProjectService projectService) : base(unitOfWork)
    {
        _projectService = projectService;
    }

    [HttpPost("[action]")]
    [ProducesResponseType(200)]
    [ValidateModelState]
    public async Task<ActionResult<OperationResult<ProjectViewModel>>> Write([FromForm] ProjectViewModel viewModel)
    {
        var operation = await _projectService.Write(viewModel);

        return OperationResultResponse(OperationResultBeforeReturn(operation));
    }

    [HttpGet("[action]")]
    [ProducesResponseType(200)]
    public IActionResult Download()
    {
        var path = _projectService.GetSavePath();

        return PhysicalFile(path, AppData.DocxMimeType, AppData.ECPProjectDownloadFileName);
    }
}