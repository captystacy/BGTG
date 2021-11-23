namespace POSCore.CalendarPlanLogic.Interfaces
{
    public interface ICalendarPlanSeparator
    {
        CalendarPlan PreparatoryCalendarPlan { get; }
        CalendarPlan MainCalendarPlan { get; }

        void Separate(CalendarPlan calendarPlan);
    }
}
