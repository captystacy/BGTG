using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BGTG.Entities.POSEntities.CalendarPlanToolEntities;
using BGTG.POS.CalendarPlanTool;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Microservices.Core.Exceptions;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BGTG.Web.Controllers.POSControllers.CalendarPlans.Commands;

public record CalendarPlanWriteByIdRequest(Guid Id) : OperationResultRequestBase<Guid>;

public class CalendarPlanWriteByIdRequestHandler : OperationResultRequestHandlerBase<CalendarPlanWriteByIdRequest, Guid>
{
    private readonly ICalendarPlanService _calendarPlanService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CalendarPlanWriteByIdRequestHandler(ICalendarPlanService calendarPlanService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _calendarPlanService = calendarPlanService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<OperationResult<Guid>> Handle(CalendarPlanWriteByIdRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<Guid>();
        operation.Result = request.Id;

        var calendarPlanEntity = await _unitOfWork.GetRepository<CalendarPlanEntity>().GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            include: x => x
                .Include(x => x.MainCalendarWorks).ThenInclude(x => x.ConstructionMonths)
                .Include(x => x.PreparatoryCalendarWorks).ThenInclude(x => x.ConstructionMonths));

        if (calendarPlanEntity == null)
        {
            operation.AddError(new MicroserviceNotFoundException());
            return operation;
        }

        var calendarPlan = _mapper.Map<CalendarPlan>(calendarPlanEntity);

        _calendarPlanService.Write(calendarPlan);

        return operation;
    }
}