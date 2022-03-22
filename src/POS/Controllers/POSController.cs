using Microsoft.AspNetCore.Mvc;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Controllers;

[Route("api/[controller]")]
public class POSController : ControllerBase
{
    private readonly ILCProjectWriter _lcProjectWriter;
    private readonly ITableOfContentsWriter _tableOfContentsWriter;
    private readonly ITitlePageWriter _titlePageWriter;

    public POSController(ILCProjectWriter lcProjectWriter, ITableOfContentsWriter tableOfContentsWriter, ITitlePageWriter titlePageWriter)
    {
        _lcProjectWriter = lcProjectWriter;
        _tableOfContentsWriter = tableOfContentsWriter;
        _titlePageWriter = titlePageWriter;
    }

    [HttpPost("[action]")]
    public IActionResult DownloadProject(ProjectViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var memoryStream = _lcProjectWriter.Write(viewModel);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(memoryStream, AppConstants.DocxMimeType);
    }

    [HttpPost("[action]")]
    public IActionResult DownloadTableOfContents([FromBody] TableOfContentsViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var memoryStream = _tableOfContentsWriter.Write(viewModel);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(memoryStream, AppConstants.DocxMimeType);
    }

    [HttpPost("[action]")]
    public IActionResult DownloadTitlePage([FromBody] TitlePageViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var memoryStream = _titlePageWriter.Write(viewModel);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(memoryStream, AppConstants.DocxMimeType);
    }
}