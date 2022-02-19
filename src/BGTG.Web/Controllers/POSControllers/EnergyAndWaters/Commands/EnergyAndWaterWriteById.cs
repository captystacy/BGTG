using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.POS.EnergyAndWaterToolEntities;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;

namespace BGTG.Web.Controllers.POSControllers.EnergyAndWaters.Commands;

public record EnergyAndWaterWriteByIdRequest(Guid Id) : OperationResultRequestBase<Guid>;

public class EnergyAndWaterWriteByIdRequestHandler : OperationResultRequestHandlerBase<EnergyAndWaterWriteByIdRequest, Guid>
{
    private readonly IEnergyAndWaterService _energyAndWaterService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EnergyAndWaterWriteByIdRequestHandler(IEnergyAndWaterService energyAndWaterService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _energyAndWaterService = energyAndWaterService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<OperationResult<Guid>> Handle(EnergyAndWaterWriteByIdRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<Guid>();
        operation.Result = request.Id;

        var energyAndWaterEntity = await _unitOfWork.GetRepository<EnergyAndWaterEntity>().GetFirstOrDefaultAsync(predicate: x => x.Id == request.Id);

        if (energyAndWaterEntity == null)
        {
            operation.AddError(new MicroserviceNotFoundException());
            return operation;
        }

        var energyAndWater = _mapper.Map<EnergyAndWater>(energyAndWaterEntity);

        _energyAndWaterService.Write(energyAndWater);

        return operation;
    }
}