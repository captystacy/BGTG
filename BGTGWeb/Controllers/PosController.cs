using System.Collections.Generic;
using BGTGWeb.Models;
using BGTGWeb.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.EstimateLogic;

namespace BGTGWeb.Controllers
{
    public class PosController : Controller
    {
        private readonly ICalendarPlanService _calendarPlanService;
        private readonly IDurationByLaborCostsService _durationByLaborCostsService;
        private readonly IEnergyAndWaterService _energyAndWaterService;
        private readonly IDurationByTCPService _durationByTCPService;

        private const string DocxMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        public PosController(ICalendarPlanService calendarPlanService, IDurationByLaborCostsService durationByLaborCostsService,
            IEnergyAndWaterService energyAndWaterService, IDurationByTCPService durationByTCPService)
        {
            _calendarPlanService = calendarPlanService;
            _durationByLaborCostsService = durationByLaborCostsService;
            _energyAndWaterService = energyAndWaterService;
            _durationByTCPService = durationByTCPService;
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
                return BadRequest();
            }

            var totalPercentages = _calendarPlanService.GetTotalPercentages(estimateFiles, calendarPlanVM);
            return Json(totalPercentages);
        }

        public IActionResult WriteCalendarPlan(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _calendarPlanService.Write(estimateFiles, calendarPlanVM, User.Identity.Name);
            return new OkResult();
        }

        public IActionResult DownloadCalendarPlan(string objectCipher)
        {
            var path = _calendarPlanService.GetSavePath(User.Identity.Name);

            var fileName = _calendarPlanService.GetFileName(objectCipher);

            return PhysicalFile(path, DocxMimeType, fileName);
        }

        public IActionResult DownloadDurationByLaborCosts(IEnumerable<IFormFile> estimateFiles, DurationByLaborCostsVM durationByLaborCostsVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _durationByLaborCostsService.Write(estimateFiles, durationByLaborCostsVM, User.Identity.Name);

            var path = _durationByLaborCostsService.GetSavePath(User.Identity.Name);

            var fileName = _durationByLaborCostsService.GetFileName();

            return PhysicalFile(path, DocxMimeType, fileName);
        }

        public IActionResult DownloadEnergyAndWater(IEnumerable<IFormFile> estimateFiles)
        {
            _energyAndWaterService.Write(estimateFiles, User.Identity.Name);

            var path = _energyAndWaterService.GetSavePath(User.Identity.Name);

            var fileName = _energyAndWaterService.GetFileName();

            return PhysicalFile(path, DocxMimeType, fileName);
        }

        public IActionResult DownloadDurationByTCP(DurationByTCPVM durationByTCPVM)
        {
            if (!ModelState.IsValid || !_durationByTCPService.Write(durationByTCPVM, User.Identity.Name))
            {
                return BadRequest();
            }

            var path = _durationByTCPService.GetSavePath(User.Identity.Name);

            var fileName = _durationByTCPService.GetFileName();

            return PhysicalFile(path, DocxMimeType, fileName);
        }
    }
}
