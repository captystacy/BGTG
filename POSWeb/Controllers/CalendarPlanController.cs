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

        private const string _downloadActionName = "Download";

        public CalendarPlanController(CalendarPlanPresentation calendarPlanPresentation)
        {
            _calendarPlanPresentation = calendarPlanPresentation;
        }

        [HttpPost]
        public IActionResult WriteAndDownload(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            _calendarPlanPresentation.WriteCalendarPlan(estimateFiles, calendarPlanVM, User.Identity.Name);
            return RedirectToAction(_downloadActionName, new { objectCipher = calendarPlanVM.ObjectCipher });
        }

        public IActionResult WriteAndGetObjectCipher(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            _calendarPlanPresentation.WriteCalendarPlan(estimateFiles, calendarPlanVM, User.Identity.Name);
            return Json(calendarPlanVM.ObjectCipher);
        }

        public IActionResult Download(string objectCipher)
        {
            var calendarPlansPath = _calendarPlanPresentation.GetCalendarPlansPath();
            var calendarPlanFileName = _calendarPlanPresentation.GetCalendarPlanFileName(User.Identity.Name);
            var filePath = Path.Combine(calendarPlansPath, calendarPlanFileName);
            var downloadCalendarPlanName = _calendarPlanPresentation.GetDownloadCalendarPlanName(objectCipher);
            return PhysicalFile(filePath, CalendarPlanService.DocxMimeType, downloadCalendarPlanName);
        }

        public IActionResult GetCalendarPlanVM(IEnumerable<IFormFile> estimateFiles)
        {
            var calendarPlanVM = _calendarPlanPresentation.GetCalendarPlanVM(estimateFiles);
            return Json(calendarPlanVM);
        }

        public IActionResult GetMainTotalWork(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            var mainTotalWork = _calendarPlanPresentation.GetMainTotalWork(estimateFiles, calendarPlanVM);
            return Json(mainTotalWork);
        }
    }
}
