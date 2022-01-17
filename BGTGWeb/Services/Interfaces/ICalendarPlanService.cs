using System.Collections.Generic;
using BGTGWeb.Models;
using Microsoft.AspNetCore.Http;
using POS.EstimateLogic;

namespace BGTGWeb.Services.Interfaces
{
    public interface ICalendarPlanService
    {
        CalendarPlanVM GetCalendarPlanVM(IEnumerable<IFormFile> estimateFiles, TotalWorkChapter totalWorkChapter);
        void WriteCalendarPlan(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM, string userFullName);
        IEnumerable<decimal> GetTotalPercentages(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM);
        string GetCalendarPlansPath();
        string GetCalendarPlanFileName(string userFullName);
        string GetDownloadCalendarPlanFileName(string objectCipher);
    }
}