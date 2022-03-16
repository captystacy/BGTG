using Microsoft.AspNetCore.Mvc;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.ViewModels;

namespace POS.Controllers;

[Route("api/[controller]")]
public class DurationByTCPController : ControllerBase
{
    private readonly IDurationByTCPService _durationByTCPService;

    public DurationByTCPController(IDurationByTCPService durationByTCPService)
    {
        _durationByTCPService = durationByTCPService;
    }

    [HttpPost("[action]")]
    public IActionResult GetFile([FromBody] DurationByTCPViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var memoryStream = _durationByTCPService.Write(viewModel);

        if (memoryStream is null)
        {
            return BadRequest();
        }

        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(memoryStream, AppConstants.DocxMimeType);
    }
}