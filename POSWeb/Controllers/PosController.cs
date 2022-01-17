using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSWeb.Models;
using POSWeb.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using POSCore.EstimateLogic;

namespace POSWeb.Controllers
{
    public class PosController : Controller
    {
        private readonly ICalendarPlanService _calendarPlanService;
        private readonly ILaborCostsDurationService _laborCostsDurationService;
        private readonly IEnergyAndWaterService _energyAndWaterService;

        private const string _docxMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        public PosController(ICalendarPlanService calendarPlanService, ILaborCostsDurationService laborCostsDurationService,
            IEnergyAndWaterService energyAndWaterService)
        {
            _calendarPlanService = calendarPlanService;
            _laborCostsDurationService = laborCostsDurationService;
            _energyAndWaterService = energyAndWaterService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetCalendarPlanVM(IEnumerable<IFormFile> estimateFiles, TotalWorkChapter totalWorkChapter)
        {
            var calendarPlanVM = _calendarPlanService.GetCalendarPlanVM(estimateFiles, totalWorkChapter);
            return Json(calendarPlanVM);
        }

        public IActionResult GetTotalPercentages(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var totalPercentages = _calendarPlanService.GetTotalPercentages(estimateFiles, calendarPlanVM);
            return Json(totalPercentages);
        }

        public IActionResult WriteCalendarPlan(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            _calendarPlanService.WriteCalendarPlan(estimateFiles, calendarPlanVM, User.Identity.Name);
            return new OkResult();
        }

        public IActionResult DownloadCalendarPlan(string objectCipher)
        {
            var calendarPlansPath = _calendarPlanService.GetCalendarPlansPath();
            var calendarPlanFileName = _calendarPlanService.GetCalendarPlanFileName(User.Identity.Name);
            var filePath = Path.Combine(calendarPlansPath, calendarPlanFileName);
            var downloadCalendarPlanName = _calendarPlanService.GetDownloadCalendarPlanFileName(objectCipher);
            return PhysicalFile(filePath, _docxMimeType, downloadCalendarPlanName);
        }

        public IActionResult DownloadLaborCostsDuration(IEnumerable<IFormFile> estimateFiles, LaborCostsDurationVM laborCostsDurationVM)
        {
            _laborCostsDurationService.WriteLaborCostsDuration(estimateFiles, laborCostsDurationVM, User.Identity.Name);
            var laborCostsDurationsPath = _laborCostsDurationService.GetLaborCostsDurationsPath();
            var laborCostsDurationFileName = _laborCostsDurationService.GetLaborCostsDurationFileName(User.Identity.Name);
            var filePath = Path.Combine(laborCostsDurationsPath, laborCostsDurationFileName);
            var downloadLaborCostsDurationName = _laborCostsDurationService.GetDownloadLaborCostsDurationFileName();
            return PhysicalFile(filePath, _docxMimeType, downloadLaborCostsDurationName);
        }

        public IActionResult DownloadEnergyAndWater(IEnumerable<IFormFile> estimateFiles)
        {
            _energyAndWaterService.WriteEnergyAndWater(estimateFiles, User.Identity.Name);
            var energyAndWatersPath = _energyAndWaterService.GetEnergyAndWatersPath();
            var energyAndWaterFileName = _energyAndWaterService.GetEnergyAndWaterFileName(User.Identity.Name);
            var filePath = Path.Combine(energyAndWatersPath, energyAndWaterFileName);
            var downloadEnergyAndWaterName = _energyAndWaterService.GetDownloadEnergyAndWaterFileName();
            return PhysicalFile(filePath, _docxMimeType, downloadEnergyAndWaterName);
        }
    }
}
