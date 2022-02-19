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

public record InterpolationDurationByTCPDeleteRequest(Guid Id) : OperationResultRequestBase<Guid>;

public class InterpolationDurationByTCPDeleteRequestHandler : OperationResultRequestHandlerBase<InterpolationDurationByTCPDeleteRequest, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public InterpolationDurationByTCPDeleteRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<OperationResult<Guid>> Handle(InterpolationDurationByTCPDeleteRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<Guid>();
        operation.Result = request.Id;

        var repository = _unitOfWork.GetRepository<InterpolationDurationByTCPEntity>();
        var interpolationDurationByTCPEntity = await repository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            include: x => x
                .Include(x => x.POS).ThenInclude(x => x.ConstructionObject)
                .Include(x => x.POS).ThenInclude(x => x.CalendarPlan)
                .Include(x => x.POS).ThenInclude(x => x.DurationByLC)
                .Include(x => x.POS).ThenInclude(x => x.ExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.StepwiseExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.EnergyAndWater)
        );

        if (interpolationDurationByTCPEntity == null)
        {
            operation.AddError(new MicroserviceNotFoundException());
            return operation;
        }

        if (interpolationDurationByTCPEntity.POS.CalendarPlan is null
            && interpolationDurationByTCPEntity.POS.DurationByLC is null
            && interpolationDurationByTCPEntity.POS.ExtrapolationDurationByTCP is null
            && interpolationDurationByTCPEntity.POS.StepwiseExtrapolationDurationByTCP is null
            && interpolationDurationByTCPEntity.POS.EnergyAndWater is null)
        {
            _unitOfWork.GetRepository<ConstructionObjectEntity>().Delete(interpolationDurationByTCPEntity.POS.ConstructionObject);
        }
        else
        {
            repository.Delete(interpolationDurationByTCPEntity);
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