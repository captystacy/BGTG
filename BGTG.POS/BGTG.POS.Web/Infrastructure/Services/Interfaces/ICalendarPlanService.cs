using System.Collections.Generic;
using BGTG.POS.Tools.EstimateTool;
using BGTG.POS.Web.ViewModels.CalendarPlanViewModels;
using Microsoft.AspNetCore.Http;

namespace BGTG.POS.Web.Infrastructure.Services.Interfaces
{
    public interface ICalendarPlanService : ISavable
    {
        CalendarPlanViewModel GetCalendarPlanViewModel(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter);
        void Write(IFormFileCollection estimateFiles, CalendarPlanViewModel calendarPlanViewModel, string userFullName);
        IEnumerable<decimal> GetTotalPercentages(IFormFileCollection estimateFiles, CalendarPlanViewModel calendarPlanViewModel);
    }
}