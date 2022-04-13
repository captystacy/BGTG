using Microsoft.AspNetCore.Mvc;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Services.Base;
using POS.ViewModels;

namespace POS.Controllers
{
    [Route("api/[controller]")]
    public class EnergyAndWaterController : ControllerBase
    {
        private readonly IEnergyAndWaterService _energyAndWaterService;

        public EnergyAndWaterController(IEnergyAndWaterService energyAndWaterService)
        {
            _energyAndWaterService = energyAndWaterService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Download(EnergyAndWaterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var getEnergyAndWaterStreamOperation = await _energyAndWaterService.GetEnergyAndWaterStream(viewModel);

            if (!getEnergyAndWaterStreamOperation.Ok)
            {
                return BadRequest();
            }

            return File(getEnergyAndWaterStreamOperation.Result, Constants.DocxMimeType);
        }
    }
}