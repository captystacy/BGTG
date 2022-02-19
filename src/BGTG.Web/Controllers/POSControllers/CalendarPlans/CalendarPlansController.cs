using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BGTG.Entities.Core;
using BGTG.Web.Controllers.POSControllers.CalendarPlans.Commands;
using BGTG.Web.Controllers.POSControllers.CalendarPlans.Queries;
using BGTG.Web.Infrastructure.Attributes;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using BGTG.Web.ViewModels.POSViewModels.CalendarPlanViewModels;
using Calabonga.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers.POSControllers.CalendarPlans;

public class CalendarPlansController : Controller
{
    private readonly ICalendarPlanService _calendarPlanService;
    private readonly IMediator _mediator;

    public CalendarPlansController(ICalendarPlanService calendarPlanService, IMediator mediator)
    {
        _calendarPlanService = calendarPlanService;
        _mediator = mediator;
    }

    [ValidateModelState]
    public async Task<OperationResult<CalendarPlanCreateViewModel>> GetCalendarPlanCreateViewmodel(CalendarPlanPreCreateViewModel viewModel)
        => await _mediator.Send(new GetCalendarPlanCreateViewModelRequest(viewModel), HttpContext.RequestAborted);

    [ValidateModelState]
    public async Task<OperationResult<IEnumerable<decimal>>> GetTotalPercentages(CalendarPlanCreateViewModel viewModel)
        => await _mediator.Send(new GetTotalPercentagesRequest(viewModel), HttpContext.RequestAborted);

    [ValidateModelState]
    public async Task<OperationResult<CalendarPlanCreateViewModel>> Write(CalendarPlanCreateViewModel viewModel)
        => await _mediator.Send(new CalendarPlanWriteRequest(viewModel), HttpContext.RequestAborted);

    public async Task<OperationResult<Guid>> WriteById(Guid id)
        => await _mediator.Send(new CalendarPlanWriteByIdRequest(id), HttpContext.RequestAborted);

    public async Task<OperationResult<Guid>> Delete(Guid id)
        => await _mediator.Send(new CalendarPlanDeleteRequest(id), HttpContext.RequestAborted);

    public IActionResult Download()
    {
        var path = _calendarPlanService.GetSavePath();
        return PhysicalFile(path, AppData.DocxMimeType, AppData.CalendarPlanDownloadFileName);
    }
}
