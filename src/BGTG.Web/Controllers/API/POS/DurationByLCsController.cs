using System;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.Core;
using BGTG.Entities.POS.DurationByLCToolEntities;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.Web.Infrastructure.Attributes;
using BGTG.Web.Infrastructure.Services.BGTG.Base;
using BGTG.Web.Infrastructure.Services.POS.Base;
using BGTG.Web.ViewModels.POS.DurationByLCViewModels;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.API.POS;

[Route("api/[controller]")]
public class DurationByLCsController : UnitOfWorkController
{
    private readonly IMapper _mapper;
    private readonly IDurationByLCService _durationByLCService;
    private readonly IConstructionObjectService _constructionObjectService;

    public DurationByLCsController(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IDurationByLCService durationByLCService,
        IConstructionObjectService constructionObjectService)
        : base(unitOfWork)
    {
        _mapper = mapper;
        _durationByLCService = durationByLCService;
        _constructionObjectService = constructionObjectService;
    }

    [HttpPost("[action]")]
    [ProducesResponseType(200)]
    [ValidateModelState]
    public async Task<ActionResult<OperationResult<DurationByLCCreateViewModel>>> Write([FromForm] DurationByLCCreateViewModel viewModel)
    {
        var durationByLC = _durationByLCService.Write(viewModel);
        var durationByLCEntity = _mapper.Map<DurationByLCEntity>(durationByLC);
        durationByLCEntity.CreatedBy = GetUserIdentityFromRequest();
        durationByLCEntity.CreatedAt = DateTime.UtcNow;

        await _constructionObjectService.Update(viewModel.ObjectCipher, durationByLCEntity);

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
        var path = _durationByLCService.GetSavePath();

        return PhysicalFile(path, AppData.DocxMimeType, AppData.DurationByLCDownloadFileName);
    }

    [HttpPost("[action]/{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<OperationResult<Guid>>> WriteById(Guid id)
    {
        var durationByLCEntity = await UnitOfWork.GetRepository<DurationByLCEntity>().FindAsync(id);

        if (durationByLCEntity == null)
        {
            return OperationResultError(id, AppData.BadDurationByLCId);
        }

        var durationByLC = _mapper.Map<DurationByLC>(durationByLCEntity);

        _durationByLCService.Write(durationByLC);

        return OperationResultSuccess(id);
    }

    [HttpDelete("[action]/{id:guid}")]
    [ProducesResponseType(200)]
    public virtual async Task<ActionResult<OperationResult<Guid>>> DeleteItem(Guid id)
    {
        var repository = UnitOfWork.GetRepository<DurationByLCEntity>();
        var durationByLCEntity = await repository.FindAsync(id);

        if (durationByLCEntity == null)
        {
            return OperationResultError(id, new MicroserviceNotFoundException());
        }

        repository.Delete(durationByLCEntity);

        await UnitOfWork.SaveChangesAsync();

        if (!UnitOfWork.LastSaveChangesResult.IsOk)
        {
            return OperationResultError(id, new MicroserviceDatabaseException());
        }

        return OperationResultSuccess(id);
    }
}