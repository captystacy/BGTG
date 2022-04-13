using Calabonga.OperationResults;

namespace POS.Infrastructure.Parsers.Base
{
    public interface IConstructionParser
    {
        Task<OperationResult<decimal>> GetConstructionDuration(string? constructionDurationCellStr);
        Task<OperationResult<DateTime>> GetConstructionStartDate(string? constructionStartDateCellStr);
    }
}