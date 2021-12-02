using Microsoft.AspNetCore.Http;
using POSCore.CalendarPlanLogic;
using POSCore.EstimateLogic;
using POSWeb.Models;
using System.Collections.Generic;

namespace POSWeb.Services.Interfaces
{
    public interface ICalendarPlanService
    {
        void WriteCalendarPlan(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM, string fileName);
        CalendarWork GetMainTotalWork(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM);
        Estimate GetEstimate(IEnumerable<IFormFile> estimateFiles);
        string GetCalendarPlansPath();
    }
}