using POS.DomainModels.CalendarPlanDomainModels;

namespace POS.Infrastructure.Writers.Base;

public interface ICalendarPlanWriter
{
    MemoryStream Write(CalendarPlan calendarPlan, string preparatoryTemplatePath, string mainTemplatePath);
}