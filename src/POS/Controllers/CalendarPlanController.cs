using Calabonga.OperationResults;
using Microsoft.AspNetCore.Mvc;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Attributes;
using POS.Infrastructure.Services.Base;
using POS.ViewModels;

namespace POS.Controllers
{
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
        public async Task<OperationResult<CalendarPlanViewModel>> GetViewModelForCreation(CalendarPlanCreateViewModel viewModel) => 
            await _calendarPlanService.GetCalendarPlanViewModel(viewModel);

        [HttpPost("[action]")]
        [ValidateModelState]
        public async Task<OperationResult<IEnumerable<decimal>>> GetTotalPercentages(CalendarPlanViewModel viewModel) => 
            await _calendarPlanService.GetTotalPercentages(viewModel);

        [HttpPost("[action]")]
        public async Task<IActionResult> Download(CalendarPlanViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var getCalendarPlanStreamOperation = await _calendarPlanService.GetCalendarPlanStream(viewModel);

            if (!getCalendarPlanStreamOperation.Ok)
            {
                return BadRequest();
            }

            return File(getCalendarPlanStreamOperation.Result, Constants.DocxMimeType);
        }
    }
}