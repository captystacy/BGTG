using Microsoft.AspNetCore.Mvc;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using POS.ViewModels;

namespace POS.Controllers
{
    [Route("api/[controller]")]
    public class POSController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ITableOfContentsWriter _tableOfContentsWriter;
        private readonly ITitlePageWriter _titlePageWriter;

        public POSController(IProjectService projectService, ITableOfContentsWriter tableOfContentsWriter, ITitlePageWriter titlePageWriter)
        {
            _projectService = projectService;
            _tableOfContentsWriter = tableOfContentsWriter;
            _titlePageWriter = titlePageWriter;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DownloadProject(ProjectViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var getProjectStreamOperation = await _projectService.GetProjectStream(viewModel);

            if (!getProjectStreamOperation.Ok)
            {
                return BadRequest();
            }

            return File(getProjectStreamOperation.Result, Constants.DocxMimeType);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DownloadTableOfContents([FromBody] TableOfContentsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var memoryStream = await _tableOfContentsWriter.GetTableOfContentsStream(viewModel);

            return File(memoryStream, Constants.DocxMimeType);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DownloadTitlePage([FromBody] TitlePageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var memoryStream = await _titlePageWriter.GetTitlePageStream(viewModel);

            memoryStream.Seek(0, SeekOrigin.Begin);

            return File(memoryStream, Constants.DocxMimeType);
        }
    }
}