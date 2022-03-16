using Microsoft.AspNetCore.Mvc;
using POS.Infrastructure.Constants;
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
    public IActionResult DownloadProject(ProjectViewModel dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var memoryStream = _projectService.Write(dto);

        if (memoryStream is null)
        {
            return BadRequest();
        }

        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(memoryStream, AppConstants.DocxMimeType);
    }

    [HttpPost("[action]")]
    public IActionResult DownloadTableOfContents([FromBody] TableOfContentsViewModel dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var memoryStream = _tableOfContentsService.Write(dto);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(memoryStream, AppConstants.DocxMimeType);
    }

    [HttpPost("[action]")]
    public IActionResult DownloadTitlePage([FromBody] TitlePageViewModel dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var memoryStream = _titlePageService.Write(dto);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(memoryStream, AppConstants.DocxMimeType);
    }
}