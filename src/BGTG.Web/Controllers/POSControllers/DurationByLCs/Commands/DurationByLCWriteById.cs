using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.POSEntities.DurationByLCToolEntities;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;

namespace BGTG.Web.Controllers.POSControllers.DurationByLCs.Commands;

public record DurationByLCWriteByIdRequest(Guid Id) : OperationResultRequestBase<Guid>;

public class DurationByLCWriteByIdRequestHandler : OperationResultRequestHandlerBase<DurationByLCWriteByIdRequest, Guid>
{
    private readonly IDurationByLCService _durationByLCService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DurationByLCWriteByIdRequestHandler(IDurationByLCService durationByLCService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _durationByLCService = durationByLCService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<OperationResult<Guid>> Handle(DurationByLCWriteByIdRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<Guid>();
        operation.Result = request.Id;

        var durationByLCEntity = await _unitOfWork.GetRepository<DurationByLCEntity>().FindAsync(request.Id);

        if (durationByLCEntity == null)
        {
            operation.AddError(new MicroserviceNotFoundException());
            return operation;
        }

        var durationByLC = _mapper.Map<DurationByLC>(durationByLCEntity);

        _durationByLCService.Write(durationByLC);

        return operation;
    }
}