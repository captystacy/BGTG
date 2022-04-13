using Calabonga.OperationResults;

namespace POS.Infrastructure.Readers.Base
{
    public interface ICalendarPlanReader
    {
        Task<OperationResult<DateTime>> GetConstructionStartDate(Stream stream);
    }
}