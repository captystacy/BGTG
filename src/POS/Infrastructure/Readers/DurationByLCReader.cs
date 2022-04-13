using System.Globalization;
using System.Text.RegularExpressions;
using Calabonga.OperationResults;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Readers.Base;
using POS.Models;

namespace POS.Infrastructure.Readers
{
    public class DurationByLCReader : IDurationByLCReader
    {
        private readonly IMyWordDocumentFactory _documentFactory;

        public DurationByLCReader(IMyWordDocumentFactory documentFactory)
        {
            _documentFactory = documentFactory;
        }

        public async Task<OperationResult<DurationByLC>> GetDurationByLC(Stream stream)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            var operation = OperationResult.CreateResult<DurationByLC>();

            if (stream.Length == 0)
            {
                operation.AddError("Stream length was 0");
                return operation;
            }

            using var document = await _documentFactory.CreateAsync(stream);

            var durationCellStr = document.Sections[0].Tables[0].Rows[0].Cells[2].Paragraphs[0].Text;
            var durationMatch = Constants.DecimalRegex.Match(durationCellStr);
            if (!durationMatch.Success || !decimal.TryParse(durationMatch.Value, out var duration))
            {
                operation.AddError($"Could not parse duration. Was: {durationCellStr}");
                return operation;
            }

            var totalLaborCostsStr = document.Sections[0].Tables[1].Rows[0].Cells[1].Paragraphs[0].Text;
            if (!int.TryParse(totalLaborCostsStr, out var totalLaborCosts))
            {
                operation.AddError($"Could not parse total labor costs. Was: {totalLaborCostsStr}");
                return operation;
            }

            var workingDayDurationStr = document.Sections[0].Tables[1].Rows[1].Cells[1].Paragraphs[0].Text;
            if (!decimal.TryParse(workingDayDurationStr, out var workingDayDuration))
            {
                operation.AddError($"Could not parse working day duration. Was: {workingDayDurationStr}");
                return operation;
            }

            var shiftStr = document.Sections[0].Tables[1].Rows[2].Cells[1].Paragraphs[0].Text;
            if (!decimal.TryParse(shiftStr, out var shift))
            {
                operation.AddError($"Could not parse shift. Was: {shiftStr}");
                return operation;
            }

            var numberOfWorkingDaysStr = document.Sections[0].Tables[1].Rows[3].Cells[1].Paragraphs[0].Text;
            if (!decimal.TryParse(numberOfWorkingDaysStr, out var numberOfWorkingDays))
            {
                operation.AddError($"Could not parse number of working days. Was: {numberOfWorkingDaysStr}");
                return operation;
            }

            var numberOfEmployeesStr = document.Sections[0].Tables[1].Rows[4].Cells[1].Paragraphs[0].Text;
            if (!int.TryParse(numberOfEmployeesStr, out var numberOfEmployees))
            {
                operation.AddError($"Could not parse working day duration. Was: {numberOfEmployeesStr}");
                return operation;
            }

            // parse penultimate paragraph values

            var roundingIncluded = false;
            decimal roundedDuration = 0;
            if (document.Sections[0].Paragraphs.Count == Constants.DurationByLCParagraphCount)
            {
                var penultimateParagraph = document.Sections[0].Paragraphs[^2].Text;

                var penultimateParagraphParts = penultimateParagraph.Split(" мес");

                if (penultimateParagraph.StartsWith("С учетом округления"))
                {
                    roundingIncluded = true;

                    var roundedDurationMatch = Regex.Match(penultimateParagraphParts[0], @"[\d,]+$");
                    if (!roundedDurationMatch.Success || !decimal.TryParse(roundedDurationMatch.Value, out roundedDuration))
                    {
                        operation.AddError("Could not match or parse rounded duration.");
                        return operation;
                    }
                }
                else
                {
                    var durationSummingMatch = Regex.Match(penultimateParagraph, @"Tобщ = ([\d,]+) \+ [\d,]+ = [\d,]+ мес\.$");

                    if (!durationSummingMatch.Success || !decimal.TryParse(durationSummingMatch.Groups[1].Value, out roundedDuration))
                    {
                        operation.AddError("Could not match or parse rounded duration.");
                        return operation;
                    }
                }
            }

            // parse last paragraph values

            var lastParagraph = document.Sections[0].Paragraphs[^1].Text;

            var lastParagraphParts = lastParagraph.Split("мес,");

            var totalDurationMatch = Constants.DecimalRegex.Match(lastParagraphParts[0]);
            if (!totalDurationMatch.Success || !decimal.TryParse(totalDurationMatch.Value, out var totalDuration))
            {
                operation.AddError("Could not match or parse total duration.");
                return operation;
            }

            var preparatoryPeriodMatch = Constants.DecimalRegex.Match(lastParagraphParts[1]);
            if (!preparatoryPeriodMatch.Success || !decimal.TryParse(preparatoryPeriodMatch.Value, out var preparatoryPeriod))
            {
                operation.AddError("Could not match or parse preparatory period.");
                return operation;
            }

            decimal acceptanceTime = 0;
            if (lastParagraphParts.Length == 3)
            {
                var acceptanceTimeMatch = Constants.DecimalRegex.Match(lastParagraphParts[2]);

                if (!acceptanceTimeMatch.Success || !decimal.TryParse(acceptanceTimeMatch.Value, out acceptanceTime))
                {
                    operation.AddError("Could not match or parse acceptance time.");
                    return operation;
                }
            }

            operation.Result = new DurationByLC
            {
                TotalLaborCosts = totalLaborCosts,
                WorkingDayDuration = workingDayDuration,
                Shift = shift,
                NumberOfWorkingDays = numberOfWorkingDays,
                NumberOfEmployees = numberOfEmployees,
                TotalDuration = totalDuration,
                PreparatoryPeriod = preparatoryPeriod,
                AcceptanceTime = acceptanceTime,
                RoundingIncluded = roundingIncluded,
                RoundedDuration = roundedDuration,
                AcceptanceTimeIncluded = acceptanceTime > 0,
                Duration = duration,
            };

            return operation;
        }
    }
}
