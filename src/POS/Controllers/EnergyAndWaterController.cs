using Microsoft.AspNetCore.Mvc;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.ViewModels;

namespace POS.Controllers;

[Route("api/[controller]")]
public class EnergyAndWaterController : ControllerBase
{
    private readonly IEnergyAndWaterService _energyAndWaterService;

    public EnergyAndWaterController(IEnergyAndWaterService energyAndWaterService)
    {
        _energyAndWaterService = energyAndWaterService;
    }

    [HttpPost("[action]")]
    public IActionResult Download(EnergyAndWaterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var memoryStream = _energyAndWaterService.Write(viewModel);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(memoryStream, AppConstants.DocxMimeType);
    }
}