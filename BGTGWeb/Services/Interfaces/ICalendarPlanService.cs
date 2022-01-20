using System.Collections.Generic;
using BGTGWeb.ViewModels;
using Microsoft.AspNetCore.Http;
using POS.EstimateLogic;

namespace BGTGWeb.Services.Interfaces
{
    public interface ICalendarPlanService : ISavable
    {
        CalendarPlanViewModel GetCalendarPlanViewModel(IEnumerable<IFormFile> estimateFiles, TotalWorkChapter totalWorkChapter);
        void Write(IEnumerable<IFormFile> estimateFiles, CalendarPlanViewModel calendarPlanViewModel, string userFullName);
        IEnumerable<decimal> GetTotalPercentages(IEnumerable<IFormFile> estimateFiles, CalendarPlanViewModel calendarPlanViewModel);
    }
}