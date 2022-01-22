using System.Text.Json;
using BGTG.POS.Tools.EstimateTool;
using BGTG.POS.Web.Infrastructure.Services.Interfaces;
using BGTG.POS.Web.ViewModels.CalendarPlanViewModels;
using BGTG.POS.Web.ViewModels.DurationByLCViewModels;
using BGTG.POS.Web.ViewModels.DurationByTCPViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.POS.Web.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ICalendarPlanService _calendarPlanService;
        private readonly IDurationByLCService _durationByLCService;
        private readonly IEnergyAndWaterService _energyAndWaterService;
        private readonly IDurationByTCPService _durationByTCPService;

        private const string DocxMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        public HomeController(ICalendarPlanService calendarPlanService, IDurationByLCService durationByLCService, IEnergyAndWaterService energyAndWaterService, IDurationByTCPService _durationByTCPService)
        {
            _calendarPlanService = calendarPlanService;
            _durationByLCService = durationByLCService;
            _energyAndWaterService = energyAndWaterService;
            this._durationByTCPService = _durationByTCPService;
        }

        [HttpPost("[action]")]
        public string Getcalendarplanviewmodel(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter)
        {
            var calendarPlanViewModel = _calendarPlanService.GetCalendarPlanViewModel(estimateFiles, totalWorkChapter);
            return JsonSerializer.Serialize(calendarPlanViewModel);
        }

        [HttpPost("[action]")]
        public string Gettotalpercentages(IFormFileCollection estimateFiles, CalendarPlanViewModel calendarPlanViewModel)
        {
            var totalPercentages = _calendarPlanService.GetTotalPercentages(estimateFiles, calendarPlanViewModel);
            return JsonSerializer.Serialize(totalPercentages);
        }

        [HttpPost("[action]")]
        public IActionResult Writecalendarplan(IFormFileCollection estimateFiles, CalendarPlanViewModel calendarPlanViewModel, string userFullName)
        {
            _calendarPlanService.Write(estimateFiles, calendarPlanViewModel, userFullName);
            return new OkResult();
        }

        [HttpPost("[action]")]
        public IActionResult Downloadcalendarplan(string userFullName)
        {
            var path = _calendarPlanService.GetSavePath(userFullName);

            return PhysicalFile(path, DocxMimeType);
        }

        [HttpPost("[action]")]
        public IActionResult Downloaddurationbylc(IFormFileCollection estimateFiles, DurationByLCViewModel durationByLaborCostsViewModel, string userFullName)
        {
            _durationByLCService.Write(estimateFiles, durationByLaborCostsViewModel, userFullName);

            var path = _durationByLCService.GetSavePath(userFullName);

            return PhysicalFile(path, DocxMimeType);
        }

        [HttpPost("[action]")]
        public IActionResult Downloadenergyandwater(IFormFileCollection estimateFiles, string userFullName)
        {
            _energyAndWaterService.Write(estimateFiles, userFullName);

            var path = _energyAndWaterService.GetSavePath(userFullName);

            return PhysicalFile(path, DocxMimeType);
        }

        [HttpPost("[action]")]
        public IActionResult Downloaddurationbytcp([FromBody] DurationByTCPViewModel durationByTCPViewModel)
        {
            _durationByTCPService.Write(durationByTCPViewModel);

            var path = _durationByTCPService.GetSavePath(durationByTCPViewModel.UserFullName);

            return PhysicalFile(path, DocxMimeType);
        }
    }
}

