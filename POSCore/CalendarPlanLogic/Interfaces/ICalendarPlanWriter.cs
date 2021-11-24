namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarPlanWriter
    {
        void Write(CalendarPlan preparatoryCalendarPlan, CalendarPlan mainCalendarPlan, string templatePath);
    }
}