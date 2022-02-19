using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.OperationResults;

namespace BGTG.Web.Controllers.POSControllers.CalendarPlans.Queries;

public record GetTotalPercentagesRequest(CalendarPlanCreateViewModel ViewModel) : OperationResultRequestBase<IEnumerable<decimal>>;

public class GetTotalPercentagesRequestHandler : OperationResultRequestHandlerBase<GetTotalPercentagesRequest, IEnumerable<decimal>>
{
    private readonly ICalendarPlanService _calendarPlanService;

    public GetTotalPercentagesRequestHandler(ICalendarPlanService calendarPlanService)
    {
        _calendarPlanService = calendarPlanService;
    }

    public override Task<OperationResult<IEnumerable<decimal>>> Handle(GetTotalPercentagesRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<IEnumerable<decimal>>();

        operation.Result = _calendarPlanService.GetTotalPercentages(request.ViewModel);

        return Task.FromResult(operation);
    }
}