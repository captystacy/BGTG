using BGTG.Core;
using BGTG.Web.Infrastructure.Services.POSServices.Interfaces;
using BGTG.Web.ViewModels.POSViewModels;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.API.POSControllers
{
    [Route("api/[controller]")]
    public class TableOfContentsController : OperationResultController
    {
        private readonly ITableOfContentsService _tableOfContentsService;

        public TableOfContentsController(ITableOfContentsService tableOfContentsService)
        {
            _tableOfContentsService = tableOfContentsService;
        }

        [HttpPost("[action]")]
        public ActionResult<OperationResult<string>> Write([FromForm] TableOfContentsViewModel viewModel)
        {
            var operationResult = OperationResult.CreateResult<string>();

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Root.Errors)
                {
                    operationResult.AddError(error.ErrorMessage);
                }
                return OperationResultBeforeReturn(operationResult);
            }

            _tableOfContentsService.Write(viewModel, User.Identity.Name);

            operationResult.Result = string.Empty;

            return OperationResultBeforeReturn(operationResult);
        }

        [HttpGet("[action]")]
        public IActionResult Download()
        {
            var path = _tableOfContentsService.GetSavePath(User.Identity.Name);

            return PhysicalFile(path, AppData.DocxMimeType, AppData.TableOfContentsDownloadFileName);
        }
    }
}
