using Calabonga.OperationResults;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Parsers.Base;

namespace POS.Infrastructure.Parsers
{
    public class ConstructionParser : IConstructionParser
    {
        public Task<OperationResult<decimal>> GetConstructionDuration(string? constructionDurationCellStr)
        {
            var operation = OperationResult.CreateResult<decimal>();

            if (string.IsNullOrEmpty(constructionDurationCellStr))
            {
                operation.AddError("Construction duration was empty");
                return Task.FromResult(operation);
            }


            var durationMatch = Constants.DecimalRegex.Match(constructionDurationCellStr);

            decimal duration = 0;
            if (!durationMatch.Success || !decimal.TryParse(durationMatch.Value, out duration))
            {
                operation.AddError("Could not match or parse construction duration");
            }

            operation.Result = duration;

            return Task.FromResult(operation);
        }

        public Task<OperationResult<DateTime>> GetConstructionStartDate(string? constructionStartDateCellStr)
        {
            var operation = OperationResult.CreateResult<DateTime>();

            if (!DateTime.TryParse(constructionStartDateCellStr, out var constructionStartDate))
            {
                operation.AddError("Construction start parse error");
                return Task.FromResult(operation);
            }

            operation.Result = constructionStartDate;

            return Task.FromResult(operation);
        }
    }
}
