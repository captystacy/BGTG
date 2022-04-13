using Microsoft.AspNetCore.Mvc;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Services.Base;
using POS.ViewModels;

namespace POS.Controllers
{
    [Route("api/[controller]")]
    public class DurationByTCPController : ControllerBase
    {
        private readonly IDurationByTCPService _durationByTCPService;

        public DurationByTCPController(IDurationByTCPService durationByTCPService)
        {
            _durationByTCPService = durationByTCPService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Download([FromBody] DurationByTCPViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var getDurationByTCPStreamOperation = await _durationByTCPService.GetDurationByTCPStream(viewModel);

            if (!getDurationByTCPStreamOperation.Ok)
            {
                return BadRequest();
            }

            return File(getDurationByTCPStreamOperation.Result, Constants.DocxMimeType);
        }
    }
}