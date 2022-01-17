namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarPlanWriter
    {
        void Write(CalendarPlan calendarPlan, string preparatoryTemplatePath, string mainTemplatePath, string savePath);
    }
}