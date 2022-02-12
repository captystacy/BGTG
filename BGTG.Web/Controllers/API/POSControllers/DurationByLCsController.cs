using System;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Core;
using BGTG.Data.CustomRepositories.Interfaces;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.Web.Infrastructure.Services.POSServices.Interfaces;
using BGTG.Web.ViewModels.POSViewModels.DurationByLCViewModels;
using Calabonga.Microservices.Core.QueryParams;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Calabonga.UnitOfWork.Controllers.Factories;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.API.POSControllers
{
    [Route("api/[controller]")]
    public class DurationByLCsController : WritableController<DurationByLCViewModel, DurationByLCEntity,
        DurationByLCCreateViewModel, DurationByLCUpdateViewModel, PagedListQueryParams>
    {
        private readonly IDurationByLCService _durationByLCService;
        private readonly IConstructionObjectRepository _constructionObjectRepository;

        public DurationByLCsController(
            IMapper mapper,
            IEntityManagerFactory entityManagerFactory,
            IUnitOfWork unitOfWork,
            IDurationByLCService durationByLCService,
            IConstructionObjectRepository constructionObjectRepository)
            : base(entityManagerFactory, unitOfWork, mapper)
        {
            _durationByLCService = durationByLCService;
            _constructionObjectRepository = constructionObjectRepository;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<OperationResult<string>>> Write([FromForm] DurationByLCCreateViewModel viewModel)
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

            var durationByLC = _durationByLCService.Write(viewModel, User.Identity.Name);
            var durationByLCEntity = CurrentMapper.Map<DurationByLCEntity>(durationByLC);
            durationByLCEntity.CreatedBy = User.Identity.Name;
            durationByLCEntity.CreatedAt = new DateTime(DateTime.Now.Ticks);

            await _constructionObjectRepository.Update(viewModel.ObjectCipher, durationByLCEntity);

            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                operation.AddError(UnitOfWork.LastSaveChangesResult.Exception);
                return OperationResultBeforeReturn(operation);
            }

            operation.Result = string.Empty;
            return OperationResultBeforeReturn(operation);
        }

        [HttpGet("[action]")]
        public IActionResult Download()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var path = _durationByLCService.GetSavePath(User.Identity.Name);

            return PhysicalFile(path, AppData.DocxMimeType, AppData.DurationByLCDownloadFileName);
        }

        [HttpPost("[action]/{id:guid}")]
        public async Task<ActionResult<OperationResult<string>>> WriteById(Guid id)
        {
            var operation = OperationResult.CreateResult<string>();

            var durationByLCEntity = await Repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);

            if (durationByLCEntity == null)
            {
                operation.AddError(AppData.BadDurationByLCId);
                return OperationResultBeforeReturn(operation);
            }

            var durationByLC = CurrentMapper.Map<DurationByLC>(durationByLCEntity);

            _durationByLCService.Write(durationByLC, User.Identity.Name);

            operation.Result = string.Empty;
            return OperationResultBeforeReturn(operation);
        }
    }
}
