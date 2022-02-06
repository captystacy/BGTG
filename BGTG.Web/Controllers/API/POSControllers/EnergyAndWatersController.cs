using System;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Core;
using BGTG.Data.CustomRepositories.Interfaces;
using BGTG.Entities.POSEntities.EnergyAndWaterToolEntities;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.Web.Infrastructure.Services.Interfaces;
using BGTG.Web.ViewModels.POSViewModels.EnergyAndWaterViewModels;
using Calabonga.Microservices.Core.QueryParams;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Calabonga.UnitOfWork.Controllers.Factories;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.API.POSControllers
{
    [Route("api/[controller]")]
    public class EnergyAndWatersController : WritableController<EnergyAndWaterViewModel, EnergyAndWaterEntity, EnergyAndWaterCreateViewModel, EnergyAndWaterUpdateViewModel,
        PagedListQueryParams>
    {
        private readonly IEnergyAndWaterService _energyAndWaterService;
        private readonly IConstructionObjectRepository _constructionObjectRepository;

        public EnergyAndWatersController(
            IMapper mapper,
            IEntityManagerFactory entityManagerFactory,
            IUnitOfWork unitOfWork,
            IEnergyAndWaterService energyAndWaterService,
            IConstructionObjectRepository constructionObjectRepository)
            : base(entityManagerFactory, unitOfWork, mapper)
        {
            _energyAndWaterService = energyAndWaterService;
            _constructionObjectRepository = constructionObjectRepository;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<OperationResult<string>>> Write([FromForm] EnergyAndWaterCreateViewModel viewModel)
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

            var energyAndWater = _energyAndWaterService.Write(viewModel, User.Identity.Name);
            var energyAndWaterEntity = CurrentMapper.Map<EnergyAndWaterEntity>(energyAndWater);
            energyAndWaterEntity.CreatedBy = User.Identity.Name;
            energyAndWaterEntity.CreatedAt = new DateTime(DateTime.Now.Ticks);

            await _constructionObjectRepository.Update(viewModel.ObjectCipher, energyAndWaterEntity);

            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                operationResult.AddError(UnitOfWork.LastSaveChangesResult.Exception);
                return OperationResultBeforeReturn(operationResult);
            }

            operationResult.Result = string.Empty;
            return OperationResultBeforeReturn(operationResult);
        }

        [HttpGet("[action]")]
        public IActionResult Download()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var path = _energyAndWaterService.GetSavePath(User.Identity.Name);

            return PhysicalFile(path, AppData.DocxMimeType, AppData.EnergyAndWaterDownloadFileName);
        }

        [HttpPost("[action]/{id:guid}")]
        public async Task<ActionResult<OperationResult<string>>> WriteById(Guid id)
        {
            var operationResult = OperationResult.CreateResult<string>();

            var energyAndWaterEntity = await Repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);

            if (energyAndWaterEntity == null)
            {
                operationResult.AddError(AppData.BadEnergyAndWaterId);
                return OperationResultBeforeReturn(operationResult);
            }

            var energyAndWater = CurrentMapper.Map<EnergyAndWater>(energyAndWaterEntity);

            _energyAndWaterService.Write(energyAndWater, User.Identity.Name);

            operationResult.Result = string.Empty;
            return OperationResultBeforeReturn(operationResult);
        }
    }
}
