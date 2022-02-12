using BGTG.Core;
using BGTG.Web.Infrastructure.Services.POSServices.Interfaces;
using BGTG.Web.ViewModels.POSViewModels;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.API.POSControllers
{
    [Route("api/[controller]")]
    public class TitlePagesController : OperationResultController
    {
        private readonly ITitlePageService _titlePageService;

        public TitlePagesController(ITitlePageService titlePageService)
        {
            _titlePageService = titlePageService;
        }

        [HttpPost("[action]")]
        public ActionResult<OperationResult<string>> Write([FromForm] TitlePageViewModel viewModel)
        {
            var operation = OperationResult.CreateResult<string>();

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Root.Errors)
                {
                    operation.AddError(error.ErrorMessage);
                }
                return OperationResultBeforeReturn(operation);
            }

            _titlePageService.Write(viewModel, User.Identity.Name);

            operation.Result = string.Empty;

            return OperationResultBeforeReturn(operation);
        }

        [HttpGet("[action]")]
        public IActionResult Download()
        {
            var path = _titlePageService.GetSavePath(User.Identity.Name);

            return PhysicalFile(path, AppData.DocxMimeType, AppData.TitlePageDownloadFileName);
        }
    }
}
