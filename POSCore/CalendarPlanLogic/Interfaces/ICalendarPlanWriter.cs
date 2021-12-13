namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarPlanWriter
    {
        void Write(CalendarPlan calendarPlan, string templatePath, string savePath, string fileName);
    }
}