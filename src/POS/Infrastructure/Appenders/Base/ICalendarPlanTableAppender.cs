using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models.CalendarPlanModels;

namespace POS.Infrastructure.Appenders.Base
{
    public interface ICalendarPlanAppender
    {
        Task<IMyTable> AppendAsync(IMySection section, CalendarPlan calendarPlan, CalendarPlanType calendarPlanType);
    }
}