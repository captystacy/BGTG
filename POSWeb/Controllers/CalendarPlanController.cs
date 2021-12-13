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

        public void WriteCalendarPlan(IEnumerable<IFormFile> estimateFiles, List<UserWork> userWorks)
        {
            _calendarPlanPresentation.WriteCalendarPlan(estimateFiles, userWorks, User.Identity.Name);
        }

        public IActionResult Download()
        {
            var calendarPlansPath = _calendarPlanPresentation.GetCalendarPlansPath();
            var calendarPlanFileName = _calendarPlanPresentation.GetCalendarPlanFileName(User.Identity.Name);
            var filePath = Path.Combine(calendarPlansPath, calendarPlanFileName);
            var downloadCalendarPlanName = _calendarPlanPresentation.GetDownloadCalendarPlanFileName();
            return PhysicalFile(filePath, CalendarPlanService.DocxMimeType, downloadCalendarPlanName);
        }

        public IActionResult GetCalendarPlanVM(IEnumerable<IFormFile> estimateFiles)
        {
            var calendarPlanVM = _calendarPlanPresentation.GetCalendarPlanVM(estimateFiles);
            return Json(calendarPlanVM);
        }

        public IActionResult GetTotalPercentages(IEnumerable<IFormFile> estimateFiles, List<UserWork> userWorks)
        {
            var totalPercentages = _calendarPlanPresentation.GetTotalPercentages(estimateFiles, userWorks);
            return Json(totalPercentages);
        }
    }
}
