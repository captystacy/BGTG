using System;
using System.Threading;
using System.Threading.Tasks;
using BGTG.Entities.BGTG;
using BGTG.Entities.POS.DurationByTCPToolEntities;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.POSControllers.DurationByTCPs.Commands;

public record DurationByTCPDelete(Guid Id) : OperationResultRequestBase<Guid>;

public class DurationByTCPDeleteHandler : OperationResultRequestHandlerBase<DurationByTCPDelete, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public DurationByTCPDeleteHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<OperationResult<Guid>> Handle(DurationByTCPDelete request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<Guid>();
        operation.Result = request.Id;

        var constructionObjectRepository = _unitOfWork.GetRepository<ConstructionObjectEntity>();
        var interpolationDurationByTCPRepository = _unitOfWork.GetRepository<InterpolationDurationByTCPEntity>();
        var interpolationDurationByTCPEntity = await interpolationDurationByTCPRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
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
                constructionObjectRepository.Delete(interpolationDurationByTCPEntity.POS.ConstructionObject);
            }
            else
            {
                interpolationDurationByTCPRepository.Delete(interpolationDurationByTCPEntity);
            }

            await _unitOfWork.SaveChangesAsync();

            if (!_unitOfWork.LastSaveChangesResult.IsOk)
            {
                operation.AddError(new MicroserviceDatabaseException());
                return operation;
            }

            return operation;
        }

        var extrapolationDurationByTCPRepository = _unitOfWork.GetRepository<ExtrapolationDurationByTCPEntity>();
        var extrapolationDurationByTCPEntity = await extrapolationDurationByTCPRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
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
                constructionObjectRepository.Delete(extrapolationDurationByTCPEntity.POS.ConstructionObject);
            }
            else
            {
                extrapolationDurationByTCPRepository.Delete(extrapolationDurationByTCPEntity);
            }

            await _unitOfWork.SaveChangesAsync();

            if (!_unitOfWork.LastSaveChangesResult.IsOk)
            {
                operation.AddError(new MicroserviceSaveChangesException());
                return operation;
            }

            return operation;
        }

        var stepwiseExtrapolationDurationByTCPRepository = _unitOfWork.GetRepository<StepwiseExtrapolationDurationByTCPEntity>();
        var stepwiseExtrapolationDurationByTCPEntity = await stepwiseExtrapolationDurationByTCPRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
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
                _unitOfWork.GetRepository<ConstructionObjectEntity>().Delete(stepwiseExtrapolationDurationByTCPEntity.POS.ConstructionObject);
            }
            else
            {
                stepwiseExtrapolationDurationByTCPRepository.Delete(stepwiseExtrapolationDurationByTCPEntity);
            }

            await _unitOfWork.SaveChangesAsync();

            if (!_unitOfWork.LastSaveChangesResult.IsOk)
            {
                operation.AddError(new MicroserviceSaveChangesException());
                return operation;
            }

            return operation;
        }

        operation.AddError(new MicroserviceNotFoundException());
        return operation;
    }
}