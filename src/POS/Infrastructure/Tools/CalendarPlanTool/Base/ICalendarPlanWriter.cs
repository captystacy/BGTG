using POS.Infrastructure.Tools.CalendarPlanTool.Models;

namespace POS.Infrastructure.Tools.CalendarPlanTool.Base;

public interface ICalendarPlanWriter
{
    FileStream Write(CalendarPlan calendarPlan, string preparatoryTemplatePath, string mainTemplatePath);
}