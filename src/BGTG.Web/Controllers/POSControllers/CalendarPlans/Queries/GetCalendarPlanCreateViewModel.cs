using System.Threading;
using System.Threading.Tasks;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.OperationResults;

namespace BGTG.Web.Controllers.POSControllers.CalendarPlans.Queries;

public record GetCalendarPlanCreateViewModelRequest(CalendarPlanPreCreateViewModel ViewModel) : OperationResultRequestBase<CalendarPlanCreateViewModel>;

public class GetCalendarPlanCreateViewModelRequestHandler : OperationResultRequestHandlerBase<
    GetCalendarPlanCreateViewModelRequest, CalendarPlanCreateViewModel>
{
    private readonly ICalendarPlanService _calendarPlanService;

    public GetCalendarPlanCreateViewModelRequestHandler(ICalendarPlanService calendarPlanService)
    {
        _calendarPlanService = calendarPlanService;
    }

    public override Task<OperationResult<CalendarPlanCreateViewModel>> Handle(GetCalendarPlanCreateViewModelRequest request, CancellationToken cancellationToken)
    {
        var operation = OperationResult.CreateResult<CalendarPlanCreateViewModel>();

        operation.Result = _calendarPlanService.GetCalendarPlanCreateViewModel(request.ViewModel);

        return Task.FromResult(operation);
    }
}