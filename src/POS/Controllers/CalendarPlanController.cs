using Calabonga.OperationResults;
using Microsoft.AspNetCore.Mvc;
using POS.Infrastructure.Attributes;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.ViewModels;

namespace POS.Controllers;

[Route("api/[controller]")]
public class CalendarPlanController : ControllerBase
{
    private readonly ICalendarPlanService _calendarPlanService;

    public CalendarPlanController(ICalendarPlanService calendarPlanService)
    {
        _calendarPlanService = calendarPlanService;
    }

    [HttpPost("[action]")]
    [ValidateModelState]
    public OperationResult<CalendarPlanViewModel> GetViewModelForCreation(CalendarPlanCreateViewModel dto)
    {
        var operation = OperationResult.CreateResult<CalendarPlanViewModel>();

        operation.Result = _calendarPlanService.GetCalendarPlanViewModel(dto);

        return operation;
    }

    [HttpPost("[action]")]
    [ValidateModelState]
    public OperationResult<IEnumerable<decimal>> GetTotalPercentages(CalendarPlanViewModel dto)
    {
        var operation = OperationResult.CreateResult<IEnumerable<decimal>>();

        operation.Result = _calendarPlanService.GetTotalPercentages(dto);

        return operation;
    }

    [HttpPost("[action]")]
    public IActionResult GetFile(CalendarPlanViewModel dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var memoryStream = _calendarPlanService.Write(dto);

        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(memoryStream, AppConstants.DocxMimeType);
    }
}