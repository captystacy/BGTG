using System;
using System.Threading;
using System.Threading.Tasks;
using BGTG.Entities;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.POSControllers.CalendarPlans.Commands;

public record CalendarPlanDeleteRequest(Guid Id) : OperationResultRequestBase<Guid>;

public class CalendarPlanDeleteRequestHandler : OperationResultRequestHandlerBase<CalendarPlanDeleteRequest, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CalendarPlanDeleteRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<OperationResult<Guid>> Handle(CalendarPlanDeleteRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<Guid>();
        operation.Result = request.Id;

        var repository = _unitOfWork.GetRepository<CalendarPlanEntity>();
        var calendarPlanEntity = await repository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            include: x => x
                .Include(x => x.POS).ThenInclude(x => x.ConstructionObject)
                .Include(x => x.POS).ThenInclude(x => x.DurationByLC)
                .Include(x => x.POS).ThenInclude(x => x.InterpolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.ExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.StepwiseExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.EnergyAndWater)
            );

        if (calendarPlanEntity == null)
        {
            operation.AddError(new MicroserviceNotFoundException());
            return operation;
        }

        if (calendarPlanEntity.POS.DurationByLC is null
            && calendarPlanEntity.POS.InterpolationDurationByTCP is null
            && calendarPlanEntity.POS.ExtrapolationDurationByTCP is null
            && calendarPlanEntity.POS.StepwiseExtrapolationDurationByTCP is null
            && calendarPlanEntity.POS.EnergyAndWater is null)
        {
            _unitOfWork.GetRepository<ConstructionObjectEntity>().Delete(calendarPlanEntity.POS.ConstructionObject);
        }
        else
        {
            repository.Delete(calendarPlanEntity);
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