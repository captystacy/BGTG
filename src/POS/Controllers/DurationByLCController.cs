using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetFile(DurationByLCViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var memoryStream = _durationByLCService.Write(viewModel);

            memoryStream.Seek(0, SeekOrigin.Begin);

            return File(memoryStream, AppData.DocxMimeType);
        }
    }
}
