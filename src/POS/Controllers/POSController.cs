using Microsoft.AspNetCore.Mvc;
using POS.Infrastructure.Services.Base;
using POS.ViewModels;

namespace POS.Controllers;

[Route("api/[controller]")]
public class POSController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ITableOfContentsService _tableOfContentsService;
    private readonly ITitlePageService _titlePageService;

    public POSController(IProjectService projectService, ITableOfContentsService tableOfContentsService, ITitlePageService titlePageService)
    {
        _projectService = projectService;
        _tableOfContentsService = tableOfContentsService;
        _titlePageService = titlePageService;
    }

    [HttpPost("[action]")]
    public IActionResult DownloadProject(ProjectViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var memoryStream = _projectService.Write(viewModel);

        if (memoryStream is null)
        {
            return BadRequest();
        }

        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(memoryStream, AppData.DocxMimeType);
    }

    [HttpPost("[action]")]
    public IActionResult DownloadTableOfContents([FromBody] TableOfContentsViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var memoryStream = _tableOfContentsService.Write(viewModel);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(memoryStream, AppData.DocxMimeType);
    }

    [HttpPost("[action]")]
    public IActionResult DownloadTitlePage([FromBody] TitlePageViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var memoryStream = _titlePageService.Write(viewModel);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(memoryStream, AppData.DocxMimeType);
    }
}