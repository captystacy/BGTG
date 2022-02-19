using System;
using System.Threading;
using System.Threading.Tasks;
using BGTG.Entities.BGTG;
using BGTG.Entities.POS.EnergyAndWaterToolEntities;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.POSControllers.EnergyAndWaters.Commands;

public record EnergyAndWaterDeleteRequest(Guid Id) : OperationResultRequestBase<Guid>;

public class EnergyAndWaterDeleteRequestHandler : OperationResultRequestHandlerBase<EnergyAndWaterDeleteRequest, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public EnergyAndWaterDeleteRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<OperationResult<Guid>> Handle(EnergyAndWaterDeleteRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<Guid>();
        operation.Result = request.Id;

        var repository = _unitOfWork.GetRepository<EnergyAndWaterEntity>();
        var energyAndWaterEntity = await repository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            include: x => x
                .Include(x => x.POS).ThenInclude(x => x.ConstructionObject)
                .Include(x => x.POS).ThenInclude(x => x.DurationByLC)
                .Include(x => x.POS).ThenInclude(x => x.CalendarPlan)
                .Include(x => x.POS).ThenInclude(x => x.InterpolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.ExtrapolationDurationByTCP)
                .Include(x => x.POS).ThenInclude(x => x.StepwiseExtrapolationDurationByTCP)
        );

        if (energyAndWaterEntity == null)
        {
            operation.AddError(new MicroserviceNotFoundException());
            return operation;
        }

        if (energyAndWaterEntity.POS.CalendarPlan is null
            && energyAndWaterEntity.POS.DurationByLC is null
            && energyAndWaterEntity.POS.ExtrapolationDurationByTCP is null
            && energyAndWaterEntity.POS.InterpolationDurationByTCP is null
            && energyAndWaterEntity.POS.StepwiseExtrapolationDurationByTCP is null)
        {
            _unitOfWork.GetRepository<ConstructionObjectEntity>().Delete(energyAndWaterEntity.POS.ConstructionObject);
        }
        else
        {
            repository.Delete(energyAndWaterEntity);
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