namespace BGTG.POS.Tools.CalendarPlanTool.Interfaces
{
    public interface ICalendarPlanWriter
    {
        void Write(CalendarPlan calendarPlan, string preparatoryTemplatePath, string mainTemplatePath, string savePath);
    }
}