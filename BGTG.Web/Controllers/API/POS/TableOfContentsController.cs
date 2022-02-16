using BGTG.Entities.Core;
using BGTG.Web.Infrastructure.Attributes;
using BGTG.Web.Infrastructure.Services.POS.Base;
using BGTG.Web.ViewModels.POS;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.API.POS;

[Route("api/[controller]")]
public class TableOfContentsController : OperationResultController
{
    private readonly ITableOfContentsService _tableOfContentsService;

    public TableOfContentsController(ITableOfContentsService tableOfContentsService)
    {
        _tableOfContentsService = tableOfContentsService;
    }

    [HttpPost("[action]")]
    [ProducesResponseType(200)]
    [ValidateModelState]
    public ActionResult<OperationResult<TableOfContentsViewModel>> Write([FromForm] TableOfContentsViewModel viewModel)
    {
        _tableOfContentsService.Write(viewModel);

        return OperationResultSuccess(viewModel);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(200)]
    public IActionResult Download()
    {
        var path = _tableOfContentsService.GetSavePath();

        return PhysicalFile(path, AppData.DocxMimeType, AppData.TableOfContentsDownloadFileName);
    }
}