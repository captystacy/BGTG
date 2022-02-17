using BGTG.Entities.Core;
using BGTG.Web.Infrastructure.Attributes;
using BGTG.Web.Infrastructure.Services.POS.Base;
using BGTG.Web.ViewModels.POS;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.API.POS;

[Route("api/[controller]")]
public class TitlePagesController : OperationResultController
{
    private readonly ITitlePageService _titlePageService;

    public TitlePagesController(ITitlePageService titlePageService)
    {
        _titlePageService = titlePageService;
    }

    [HttpPost("[action]")]
    [ProducesResponseType(200)]
    [ValidateModelState]
    public ActionResult<OperationResult<TitlePageViewModel>> Write([FromForm] TitlePageViewModel viewModel)
    {
        _titlePageService.Write(viewModel);

        return OperationResultSuccess(viewModel);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(200)]
    public IActionResult Download()
    {
        var path = _titlePageService.GetSavePath();

        return PhysicalFile(path, AppData.DocxMimeType, AppData.TitlePageDownloadFileName);
    }
}