using System;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.BGTG;
using BGTG.Entities.Core;
using BGTG.Entities.POS.DurationByTCPToolEntities;
using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.Web.Infrastructure.Attributes;
using BGTG.Web.Infrastructure.Services.BGTG.Base;
using BGTG.Web.Infrastructure.Services.POS.Base;
using BGTG.Web.ViewModels.POS.DurationByTCPViewModels;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Calabonga.UnitOfWork.Controllers.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.API.POS;

[Route("api/[controller]")]
public class DurationByTCPsController : UnitOfWorkController
{
    private readonly IMapper _mapper;
    private readonly IDurationByTCPService _durationByTCPService;
    private readonly IConstructionObjectService _constructionObjectService;
    private readonly IRepository<InterpolationDurationByTCPEntity> _interpolationDurationByTCPRepository;
    private readonly IRepository<ExtrapolationDurationByTCPEntity> _extrapolationDurationByTCPRepository;
    private readonly IRepository<StepwiseExtrapolationDurationByTCPEntity> _stepwiseExtrapolationDurationByTCPRepository;

    public DurationByTCPsController(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IDurationByTCPService durationByTCPService,
        IConstructionObjectService constructionObjectService)
        : base(unitOfWork)
    {
        _durationByTCPService = durationByTCPService;
        _constructionObjectService = constructionObjectService;
        _mapper = mapper;
        _interpolationDurationByTCPRepository = unitOfWork.GetRepository<InterpolationDurationByTCPEntity>();
        _extrapolationDurationByTCPRepository = unitOfWork.GetRepository<ExtrapolationDurationByTCPEntity>();
        _stepwiseExtrapolationDurationByTCPRepository = unitOfWork.GetRepository<StepwiseExtrapolationDurationByTCPEntity>();
    }

    [HttpPost("[action]")]
    [ProducesResponseType(200)]
    [ValidateModelState]
    public async Task<ActionResult<OperationResult<DurationByTCPCreateViewModel>>> Write([FromForm] DurationByTCPCreateViewModel viewModel)
    {
        var durationByTCP = _durationByTCPService.Write(viewModel);

        if (durationByTCP is null)
        {
            return OperationResultError(viewModel, AppData.DurationByTCPBadParametersValidationMessage);
        }

        var now = DateTime.UtcNow;
        switch (durationByTCP)
        {
            case InterpolationDurationByTCP interpolationDuration:
                var interpolationDurationByTCPEntity = _mapper.Map<InterpolationDurationByTCPEntity>(interpolationDuration);
                interpolationDurationByTCPEntity.CreatedBy = GetUserIdentityFromRequest();
                interpolationDurationByTCPEntity.CreatedAt = now;

                await _constructionObjectService.Update(viewModel.ObjectCipher, interpolationDurationByTCPEntity);
                break;
            case ExtrapolationDurationByTCP extrapolationDurationByTCP:
                if (extrapolationDurationByTCP is StepwiseExtrapolationDurationByTCP stepwiseExtrapolationDuration)
                {
                    var stepwiseExtrapolationDurationByTCPEntity = _mapper.Map<StepwiseExtrapolationDurationByTCPEntity>(stepwiseExtrapolationDuration);
                    stepwiseExtrapolationDurationByTCPEntity.CreatedBy = GetUserIdentityFromRequest();
                    stepwiseExtrapolationDurationByTCPEntity.CreatedAt = now;

                    await _constructionObjectService.Update(viewModel.ObjectCipher, stepwiseExtrapolationDurationByTCPEntity);
                    break;
                }
                else
                {
                    var extrapolationDurationByTCPEntity = _mapper.Map<ExtrapolationDurationByTCPEntity>(extrapolationDurationByTCP);
                    extrapolationDurationByTCPEntity.CreatedBy = GetUserIdentityFromRequest();
                    extrapolationDurationByTCPEntity.CreatedAt = now;

                    await _constructionObjectService.Update(viewModel.ObjectCipher, extrapolationDurationByTCPEntity);
                    break;
                }
            default:
                return OperationResultError(viewModel, AppData.DurationByTCPUnknown);
        }

        if (!UnitOfWork.LastSaveChangesResult.IsOk)
        {
            return OperationResultError(viewModel, new MicroserviceSaveChangesException());
        }

        return OperationResultSuccess(viewModel);
    }

    [HttpPost("[action]/{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<OperationResult<Guid>>> WriteById(Guid id)
    {
        var interpolationDurationByTCPEntity = await _interpolationDurationByTCPRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == id,
            include: x => x.Include(x => x.CalculationPipelineStandards));
        if (interpolationDurationByTCPEntity != null)
        {
            var interpolationDurationByTCP = _mapper.Map<InterpolationDurationByTCP>(interpolationDurationByTCPEntity);

            _durationByTCPService.Write(interpolationDurationByTCP);

            return OperationResultSuccess(id);
        }

        var extrapolationDurationByTCPEntity = await _extrapolationDurationByTCPRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == id,
            include: x => x.Include(x => x.CalculationPipelineStandards));
        if (extrapolationDurationByTCPEntity != null)
        {
            var extrapolationDurationByTCP = _mapper.Map<ExtrapolationDurationByTCP>(extrapolationDurationByTCPEntity);

            _durationByTCPService.Write(extrapolationDurationByTCP);

            return OperationResultSuccess(id);
        }

        var stepwiseExtrapolationDurationByTCPEntity = await _stepwiseExtrapolationDurationByTCPRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == id,
            include: x => x
                .Include(x => x.CalculationPipelineStandards)
                .Include(x => x.StepwisePipelineStandard));
        if (stepwiseExtrapolationDurationByTCPEntity != null)
        {
            var stepwiseExtrapolationDurationByTCP = _mapper.Map<StepwiseExtrapolationDurationByTCP>(stepwiseExtrapolationDurationByTCPEntity);

            _durationByTCPService.Write(stepwiseExtrapolationDurationByTCP);

            return OperationResultSuccess(id);
        }

        return OperationResultError(id, AppData.BadDurationByTCPId);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(200)]
    public IActionResult Download()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var path = _durationByTCPService.GetSavePath();

        return PhysicalFile(path, AppData.DocxMimeType, AppData.DurationByTCPDownloadFileName);
    }

    [HttpDelete("[action]/{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<OperationResult<Guid>>> DeleteItem(Guid id)
    {
        var interpolationDurationByTCPEntity = await _interpolationDurationByTCPRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == id,
            include: x => x
                .Include(x => x.POS).ThenInclude(x => x.ConstructionObject)
                .Include(x => x.POS).ThenInclude(x => x.CalendarPlan)
                .Include(x => x.POS).ThenInclude(x => x.DurationByLC)
                .Include(x => x.POS).ThenInclude(x => x.ExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.StepwiseExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.EnergyAndWater)
        );
        if (interpolationDurationByTCPEntity != null)
        {
            if (interpolationDurationByTCPEntity.POS.CalendarPlan is null
                && interpolationDurationByTCPEntity.POS.DurationByLC is null
                && interpolationDurationByTCPEntity.POS.ExtrapolationDurationByTCP is null
                && interpolationDurationByTCPEntity.POS.StepwiseExtrapolationDurationByTCP is null
                && interpolationDurationByTCPEntity.POS.EnergyAndWater is null)
            {
                UnitOfWork.GetRepository<ConstructionObjectEntity>().Delete(interpolationDurationByTCPEntity.POS.ConstructionObject);
            }
            else
            {
                _interpolationDurationByTCPRepository.Delete(interpolationDurationByTCPEntity);
            }

            await UnitOfWork.SaveChangesAsync();

            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                return OperationResultError(id, new MicroserviceDatabaseException());
            }

            return OperationResultSuccess(id);
        }

        var extrapolationDurationByTCPEntity = await _extrapolationDurationByTCPRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == id,
            include: x => x
                .Include(x => x.POS).ThenInclude(x => x.ConstructionObject)
                .Include(x => x.POS).ThenInclude(x => x.CalendarPlan)
                .Include(x => x.POS).ThenInclude(x => x.DurationByLC)
                .Include(x => x.POS).ThenInclude(x => x.InterpolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.StepwiseExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.EnergyAndWater)
        );
        if (extrapolationDurationByTCPEntity != null)
        {
            if (extrapolationDurationByTCPEntity.POS.CalendarPlan is null
                && extrapolationDurationByTCPEntity.POS.DurationByLC is null
                && extrapolationDurationByTCPEntity.POS.InterpolationDurationByTCP is null
                && extrapolationDurationByTCPEntity.POS.StepwiseExtrapolationDurationByTCP is null
                && extrapolationDurationByTCPEntity.POS.EnergyAndWater is null)
            {
                UnitOfWork.GetRepository<ConstructionObjectEntity>().Delete(extrapolationDurationByTCPEntity.POS.ConstructionObject);
            }
            else
            {
                _extrapolationDurationByTCPRepository.Delete(extrapolationDurationByTCPEntity);
            }

            await UnitOfWork.SaveChangesAsync();

            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                return OperationResultError(id, new MicroserviceDatabaseException());
            }

            return OperationResultSuccess(id);
        }

        var stepwiseExtrapolationDurationByTCPEntity = await _stepwiseExtrapolationDurationByTCPRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == id,
            include: x => x
                .Include(x => x.POS).ThenInclude(x => x.ConstructionObject)
                .Include(x => x.POS).ThenInclude(x => x.CalendarPlan)
                .Include(x => x.POS).ThenInclude(x => x.DurationByLC)
                .Include(x => x.POS).ThenInclude(x => x.InterpolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.ExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.EnergyAndWater)
        );
        if (stepwiseExtrapolationDurationByTCPEntity != null)
        {
            if (stepwiseExtrapolationDurationByTCPEntity.POS.CalendarPlan is null
                && stepwiseExtrapolationDurationByTCPEntity.POS.DurationByLC is null
                && stepwiseExtrapolationDurationByTCPEntity.POS.InterpolationDurationByTCP is null
                && stepwiseExtrapolationDurationByTCPEntity.POS.ExtrapolationDurationByTCP is null
                && stepwiseExtrapolationDurationByTCPEntity.POS.EnergyAndWater is null)
            {
                UnitOfWork.GetRepository<ConstructionObjectEntity>().Delete(stepwiseExtrapolationDurationByTCPEntity.POS.ConstructionObject);
            }
            else
            {
                _stepwiseExtrapolationDurationByTCPRepository.Delete(stepwiseExtrapolationDurationByTCPEntity);
            }

            await UnitOfWork.SaveChangesAsync();

            if (!UnitOfWork.LastSaveChangesResult.IsOk)
            {
                return OperationResultError(id, new MicroserviceDatabaseException());
            }

            return OperationResultSuccess(id);
        }

        return OperationResultError(id, new MicroserviceNotFoundException());
    }
}