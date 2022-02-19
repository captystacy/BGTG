using System;
using System.Threading;
using System.Threading.Tasks;
using BGTG.Entities.BGTG;
using BGTG.Entities.POS.DurationByLCToolEntities;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.POSControllers.DurationByLCs.Commands;

public record DurationByLCDeleteRequest(Guid Id) : OperationResultRequestBase<Guid>;

public class DurationByLCDeleteRequestHandler : OperationResultRequestHandlerBase<DurationByLCDeleteRequest, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public DurationByLCDeleteRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<OperationResult<Guid>> Handle(DurationByLCDeleteRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<Guid>();
        operation.Result = request.Id;

        var repository = _unitOfWork.GetRepository<DurationByLCEntity>();
        var durationByLCEntity = await repository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            include: x => x
                .Include(x => x.POS).ThenInclude(x => x.ConstructionObject)
                .Include(x => x.POS).ThenInclude(x => x.CalendarPlan)
                .Include(x => x.POS).ThenInclude(x => x.InterpolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.ExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.StepwiseExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.EnergyAndWater)
        );

        if (durationByLCEntity == null)
        {
            operation.AddError(new MicroserviceNotFoundException());
            return operation;
        }

        if (durationByLCEntity.POS.CalendarPlan is null
            && durationByLCEntity.POS.InterpolationDurationByTCP is null
            && durationByLCEntity.POS.ExtrapolationDurationByTCP is null
            && durationByLCEntity.POS.StepwiseExtrapolationDurationByTCP is null
            && durationByLCEntity.POS.EnergyAndWater is null)
        {
            _unitOfWork.GetRepository<ConstructionObjectEntity>().Delete(durationByLCEntity.POS.ConstructionObject);
        }
        else
        {
            repository.Delete(durationByLCEntity);
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