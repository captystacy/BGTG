using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.POS.DurationByTCPToolEntities;
using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.POSControllers.DurationByTCPs.Commands;

public record DurationByTCPWriteByIdRequest(Guid Id) : OperationResultRequestBase<Guid>;

public class DurationByTCPWriteByIdRequestHandler : OperationResultRequestHandlerBase<DurationByTCPWriteByIdRequest, Guid>
{
    private readonly IDurationByTCPService _durationByTCPService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DurationByTCPWriteByIdRequestHandler(IDurationByTCPService durationByTCPService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _durationByTCPService = durationByTCPService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<OperationResult<Guid>> Handle(DurationByTCPWriteByIdRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<Guid>();
        operation.Result = request.Id;

        var interpolationDurationByTCPEntity = await _unitOfWork.GetRepository<InterpolationDurationByTCPEntity>().GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            include: x => x.Include(x => x.CalculationPipelineStandards));
        if (interpolationDurationByTCPEntity != null)
        {
            var interpolationDurationByTCP = _mapper.Map<InterpolationDurationByTCP>(interpolationDurationByTCPEntity);

            _durationByTCPService.Write(interpolationDurationByTCP);

            return operation;
        }

        var extrapolationDurationByTCPEntity = await _unitOfWork.GetRepository<ExtrapolationDurationByTCPEntity>().GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            include: x => x.Include(x => x.CalculationPipelineStandards));
        if (extrapolationDurationByTCPEntity != null)
        {
            var extrapolationDurationByTCP = _mapper.Map<ExtrapolationDurationByTCP>(extrapolationDurationByTCPEntity);

            _durationByTCPService.Write(extrapolationDurationByTCP);

            return operation;
        }

        var stepwiseExtrapolationDurationByTCPEntity = await _unitOfWork.GetRepository<StepwiseExtrapolationDurationByTCPEntity>().GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            include: x => x
                .Include(x => x.CalculationPipelineStandards)
                .Include(x => x.StepwisePipelineStandard));
        if (stepwiseExtrapolationDurationByTCPEntity != null)
        {
            var stepwiseExtrapolationDurationByTCP = _mapper.Map<StepwiseExtrapolationDurationByTCP>(stepwiseExtrapolationDurationByTCPEntity);

            _durationByTCPService.Write(stepwiseExtrapolationDurationByTCP);

            return operation;
        }

        operation.AddError(new MicroserviceNotFoundException());
        return operation;
    }
}