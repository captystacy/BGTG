using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSWeb.Models;
using POSWeb.Presentations;
using POSWeb.Services;
using System.Collections.Generic;
using System.IO;

namespace POSWeb.Controllers
{
    public class CalendarPlanController : Controller
    {
        private readonly CalendarPlanPresentation _calendarPlanPresentation;

        public CalendarPlanController(CalendarPlanPresentation calendarPlanPresentation)
        {
            _calendarPlanPresentation = calendarPlanPresentation;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            _calendarPlanPresentation.WriteCalendarPlan(estimateFiles, calendarPlanVM, User.Identity.Name);
            return RedirectToAction("Download");
        }

        public IActionResult Download()
        {
            var calendarPlansPath = _calendarPlanPresentation.GetCalendarPlansPath();
            var userFileName = _calendarPlanPresentation.GetCalendarPlanFileName(User.Identity.Name);
            var filePath = Path.Combine(calendarPlansPath, userFileName);
            return PhysicalFile(filePath, CalendarPlanService.DocxMimeType, CalendarPlanService.DownloadCalendarPlanName);
        }

        public IActionResult GetCalendarPlanVMJson(IEnumerable<IFormFile> estimateFiles)
        {
            var calendarPlanVM = _calendarPlanPresentation.GetCalendarPlanVM(estimateFiles);
            return Json(calendarPlanVM);
        }

        public IActionResult GetMainTotalWorkJson(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            var mainTotalWork = _calendarPlanPresentation.GetMainTotalWork(estimateFiles, calendarPlanVM);
            return Json(mainTotalWork);
        }
    }
}
