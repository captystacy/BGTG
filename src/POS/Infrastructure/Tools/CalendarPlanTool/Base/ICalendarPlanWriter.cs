using POS.Infrastructure.Tools.CalendarPlanTool.Models;

namespace POS.Infrastructure.Tools.CalendarPlanTool.Base;

public interface ICalendarPlanWriter
{
    MemoryStream Write(CalendarPlan calendarPlan, string preparatoryTemplatePath, string mainTemplatePath);
}