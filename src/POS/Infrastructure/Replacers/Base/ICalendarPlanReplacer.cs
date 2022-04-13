using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Infrastructure.Replacers.Base
{
    public interface ICalendarPlanReplacer
    {
        Task Replace(IMyWordDocument document, IMyTable preparatoryCalendarPlanTable, IMyTable mainCalendarPlanTable);
    }
}