using System;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.Core;
using BGTG.Entities.POS.EnergyAndWaterToolEntities;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.Web.Infrastructure.Attributes;
using BGTG.Web.Infrastructure.Services.BGTG.Base;
using BGTG.Web.Infrastructure.Services.POS.Base;
using BGTG.Web.ViewModels.POS.EnergyAndWaterViewModels;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.API.POS;

[Route("api/[controller]")]
public class EnergyAndWatersController : UnitOfWorkController
{
    private readonly IMapper _mapper;
    private readonly IEnergyAndWaterService _energyAndWaterService;
    private readonly IConstructionObjectService _constructionObjectService;

    public EnergyAndWatersController(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IEnergyAndWaterService energyAndWaterService,
        IConstructionObjectService constructionObjectService)
        : base(unitOfWork)
    {
        _mapper = mapper;
        _energyAndWaterService = energyAndWaterService;
        _constructionObjectService = constructionObjectService;
    }

    [HttpPost("[action]")]
    [ProducesResponseType(200)]
    [ValidateModelState]
    public async Task<ActionResult<OperationResult<EnergyAndWaterCreateViewModel>>> Write([FromForm] EnergyAndWaterCreateViewModel viewModel)
    {
        var energyAndWater = _energyAndWaterService.Write(viewModel);
        var energyAndWaterEntity = _mapper.Map<EnergyAndWaterEntity>(energyAndWater);
        energyAndWaterEntity.CreatedBy = GetUserIdentityFromRequest();
        energyAndWaterEntity.CreatedAt = DateTime.UtcNow;

        await _constructionObjectService.Update(viewModel.ObjectCipher, energyAndWaterEntity);

        if (!UnitOfWork.LastSaveChangesResult.IsOk)
        {
            return OperationResultError(viewModel, new MicroserviceDatabaseException());
        }

        return OperationResultSuccess(viewModel);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(200)]
    public IActionResult Download()
    {
        var path = _energyAndWaterService.GetSavePath();

        return PhysicalFile(path, AppData.DocxMimeType, AppData.EnergyAndWaterDownloadFileName);
    }

    [HttpPost("[action]/{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<OperationResult<Guid>>> WriteById(Guid id)
    {
        var energyAndWaterEntity = await UnitOfWork.GetRepository<EnergyAndWaterEntity>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);

        if (energyAndWaterEntity == null)
        {
            return OperationResultError(id, AppData.BadEnergyAndWaterId);
        }

        var energyAndWater = _mapper.Map<EnergyAndWater>(energyAndWaterEntity);

        _energyAndWaterService.Write(energyAndWater);

        return OperationResultSuccess(id);
    }

    [HttpDelete("[action]/{id:guid}")]
    [ProducesResponseType(200)]
    public virtual async Task<ActionResult<OperationResult<Guid>>> DeleteItem(Guid id)
    {
        var repository = UnitOfWork.GetRepository<EnergyAndWaterEntity>();
        var energyAndWaterEntity = await repository.FindAsync(id);

        if (energyAndWaterEntity == null)
        {
            return OperationResultError(id, new MicroserviceNotFoundException());
        }

        repository.Delete(energyAndWaterEntity);

        await UnitOfWork.SaveChangesAsync();

        if (!UnitOfWork.LastSaveChangesResult.IsOk)
        {
            return OperationResultError(id, new MicroserviceDatabaseException());
        }

        return OperationResultSuccess(id);
    }
}