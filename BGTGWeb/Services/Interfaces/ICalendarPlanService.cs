using System.Collections.Generic;
using BGTGWeb.Models;
using Microsoft.AspNetCore.Http;
using POS.EstimateLogic;

namespace BGTGWeb.Services.Interfaces
{
    public interface ICalendarPlanService : ISavable
    {
        CalendarPlanVM GetCalendarPlanVM(IEnumerable<IFormFile> estimateFiles, TotalWorkChapter totalWorkChapter);
        void Write(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM, string userFullName);
        IEnumerable<decimal> GetTotalPercentages(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM);
    }
}