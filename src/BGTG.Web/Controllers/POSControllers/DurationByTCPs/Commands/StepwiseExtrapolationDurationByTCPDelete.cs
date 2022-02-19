using System;
using System.Threading;
using System.Threading.Tasks;
using BGTG.Entities;
using BGTG.Entities.POSEntities.DurationByTCPToolEntities;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.POSControllers.DurationByTCPs.Commands;

public record StepwiseExtrapolationDurationByTCPDeleteRequest(Guid Id) : OperationResultRequestBase<Guid>;

public class StepwiseExtrapolationDurationByTCPDeleteRequestHandler : OperationResultRequestHandlerBase<StepwiseExtrapolationDurationByTCPDeleteRequest, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public StepwiseExtrapolationDurationByTCPDeleteRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<OperationResult<Guid>> Handle(StepwiseExtrapolationDurationByTCPDeleteRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<Guid>();
        operation.Result = request.Id;

        var repository = _unitOfWork.GetRepository<StepwiseExtrapolationDurationByTCPEntity>();
        var stepwiseExtrapolationDurationByTCPEntity = await repository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            include: x => x
                .Include(x => x.POS).ThenInclude(x => x.ConstructionObject)
                .Include(x => x.POS).ThenInclude(x => x.CalendarPlan)
                .Include(x => x.POS).ThenInclude(x => x.DurationByLC)
                .Include(x => x.POS).ThenInclude(x => x.InterpolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.ExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.EnergyAndWater)
        );

        if (stepwiseExtrapolationDurationByTCPEntity == null)
        {
            operation.AddError(new MicroserviceNotFoundException());
            return operation;
        }

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
            repository.Delete(stepwiseExtrapolationDurationByTCPEntity);
        }

        await _unitOfWork.SaveChangesAsync();

        if (!_unitOfWork.LastSaveChangesResult.IsOk)
        {
            operation.AddError(new MicroserviceSaveChangesException());
            return operation;
        }

        return operation;
    }
}