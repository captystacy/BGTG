using POS.DomainModels.CalendarPlanDomainModels;

namespace POS.Infrastructure.Writers.Base;

public interface ICalendarPlanWriter
{
    MemoryStream Write(CalendarPlan calendarPlan, string calendarPlanTemplate, string preparatoryTablePath, string mainTablePath);
}