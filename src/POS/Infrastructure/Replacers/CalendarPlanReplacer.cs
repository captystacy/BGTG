using POS.Infrastructure.Replacers.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Infrastructure.Replacers
{
    public class CalendarPlanReplacer : ICalendarPlanReplacer
    {
        private const string CalendarPlanPreparatoryTablePattern = "%CALENDAR_PLAN_PREPARATORY_TABLE%";
        private const string CalendarPlanMainTablePattern = "%CALENDAR_PLAN_MAIN_TABLE%";

        public Task Replace(IMyWordDocument document, IMyTable preparatoryCalendarPlanTable, IMyTable mainCalendarPlanTable)
        {
            document.ReplaceTextWithTable(CalendarPlanPreparatoryTablePattern, preparatoryCalendarPlanTable);

            document.ReplaceTextWithTable(CalendarPlanMainTablePattern, mainCalendarPlanTable);

            return Task.CompletedTask;
        }
    }
}