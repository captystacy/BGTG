using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Services.Base;
using POS.ViewModels;

namespace POS.Controllers
{
    [Route("api/[controller]")]
    public class DurationByLCController : ControllerBase
    {
        private readonly IDurationByLCService _durationByLCService;

        public DurationByLCController(IDurationByLCService durationByLCService)
        {
            _durationByLCService = durationByLCService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Download(DurationByLCViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var getDurationByLCStreamOperation = await _durationByLCService.GetDurationByLCStream(viewModel);

            if (!getDurationByLCStreamOperation.Ok)
            {
                return BadRequest();
            }

            return File(getDurationByLCStreamOperation.Result, Constants.DocxMimeType);
        }
    }
}