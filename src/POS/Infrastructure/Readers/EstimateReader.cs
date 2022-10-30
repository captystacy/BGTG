﻿using Calabonga.OperationResults;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Factories.Base;
using POS.Infrastructure.Parsers.Base;
using POS.Infrastructure.Readers.Base;
using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;
using POS.Models.EstimateModels;
using System.Globalization;

namespace POS.Infrastructure.Readers
{
    public class EstimateReader : IEstimateReader
    {
        private readonly IMyExcelDocumentFactory _excelDocumentFactory;
        private readonly IEstimateParser _estimateParser;
        private readonly IConstructionParser _constructionParser;

        private const string PossibleFirstInappropriateWorkSheet = "лист";

        private const int ConstructionStartDateRow = 20;
        private const int ConstructionStartDateColumn = 3;

        private const int ConstructionDurationRow = 21;
        private const int ConstructionDurationColumn = 3;

        private const int PatternsColumn = 1;

        private static readonly IEnumerable<string> SkipPatterns = new List<string>
        {
            "таблица", "акт", "справка", "налог", "подпункт", "п.", "смета", "отчет", "указ", "предложение", "нрр"
        };

        private const string SubUnit1To9Pattern = "подпункт 30.10 инструкции";
        private const string SubUnit1To11Pattern = "подпункт 31.6 инструкции";
        private const string SubUnit1To12Pattern = "подпункт 33.3.2  инструкции";

        private const string Nrr102Pattern = "нрр 8.01.102-2017";
        private const string Nrr103Pattern = "нрр 8.01.103-2017";

        public EstimateReader(IMyExcelDocumentFactory excelDocumentFactory, IEstimateParser estimateParser, IConstructionParser constructionParser)
        {
            _excelDocumentFactory = excelDocumentFactory;
            _estimateParser = estimateParser;
            _constructionParser = constructionParser;
        }

        public async Task<OperationResult<EstimateWork>> GetTotalEstimateWork(Stream stream, TotalWorkChapter totalWorkChapter)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            var operation = OperationResult.CreateResult<EstimateWork>();

            if (stream.Length == 0)
            {
                operation.AddError("Stream has length 0");
                return operation;
            }

            if (totalWorkChapter is TotalWorkChapter.None)
            {
                operation.AddError("Total work chapter was not set");
                return operation;
            }

            using var document = await _excelDocumentFactory.CreateAsync(stream);

            stream.Close();

            var cells = document.WorkBook.WorkSheets[0].Name.Trim().ToLower().StartsWith(PossibleFirstInappropriateWorkSheet)
                ? document.WorkBook.WorkSheets[1].Cells
                : document.WorkBook.WorkSheets[0].Cells;

            var totalWorkChapterPattern = totalWorkChapter switch
            {
                TotalWorkChapter.TotalWork1To9Chapter => SubUnit1To9Pattern,
                TotalWorkChapter.TotalWork1To11Chapter => SubUnit1To11Pattern,
                TotalWorkChapter.TotalWork1To12Chapter => SubUnit1To12Pattern,
                _ => throw new ArgumentOutOfRangeException(nameof(totalWorkChapter), totalWorkChapter, null)
            };

            for (int row = Constants.EstimateStartRow; row <= Constants.EstimateEndRow; row++)
            {
                var estimateCalculationCellStr = cells[row, PatternsColumn].Text?.Trim().ToLower();

                if (estimateCalculationCellStr == totalWorkChapterPattern)
                {
                    var getTotalEstimateWorkOperation = await _estimateParser.GetTotalEstimateWork(cells, row, totalWorkChapter);

                    if (!getTotalEstimateWorkOperation.Ok)
                    {
                        operation.AddError(getTotalEstimateWorkOperation.GetMetadataMessages());
                        return operation;
                    }

                    operation.Result = getTotalEstimateWorkOperation.Result;
                    break;
                }
            }

            return operation;
        }

        public async Task<OperationResult<DateTime>> GetConstructionStartDate(Stream stream)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            var operation = OperationResult.CreateResult<DateTime>();

            if (stream.Length == 0)
            {
                operation.AddError("Stream has length 0");
                return operation;
            }

            using var document = await _excelDocumentFactory.CreateAsync(stream);

            stream.Close();

            var cells = document.WorkBook.WorkSheets[0].Name.Trim().ToLower().StartsWith(PossibleFirstInappropriateWorkSheet)
                ? document.WorkBook.WorkSheets[1].Cells
                : document.WorkBook.WorkSheets[0].Cells;

            operation.Result = await GetConstructionStartDate(cells, operation);

            return operation;
        }

        public async Task<OperationResult<int>> GetLaborCosts(Stream stream)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            var operation = OperationResult.CreateResult<int>();

            if (stream.Length == 0)
            {
                operation.AddError("Stream has length 0");
                return operation;
            }

            using var document = await _excelDocumentFactory.CreateAsync(stream);

            stream.Close();

            var cells = document.WorkBook.WorkSheets[0].Name.Trim().ToLower().StartsWith(PossibleFirstInappropriateWorkSheet)
                ? document.WorkBook.WorkSheets[1].Cells
                : document.WorkBook.WorkSheets[0].Cells;

            for (int row = Constants.EstimateStartRow; row <= Constants.EstimateEndRow; row++)
            {
                var estimateCalculationCellStr = cells[row, PatternsColumn].Text?.Trim().ToLower();

                if (!string.IsNullOrEmpty(estimateCalculationCellStr)
                    && estimateCalculationCellStr.StartsWith(Nrr103Pattern))
                {
                    var parseLaborCostsOperation = await _estimateParser.GetLaborCosts(cells, row);

                    if (!parseLaborCostsOperation.Ok)
                    {
                        operation.AddError(parseLaborCostsOperation.GetMetadataMessages());
                        return operation;
                    }

                    operation.Result = parseLaborCostsOperation.Result;

                    break;
                }
            }

            return operation;
        }

        public async Task<OperationResult<Estimate>> GetEstimate(Stream stream, TotalWorkChapter totalWorkChapter)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            var operation = OperationResult.CreateResult<Estimate>();

            if (stream.Length == 0)
            {
                operation.AddError("Stream has length 0");
                return operation;
            }

            if (totalWorkChapter is TotalWorkChapter.None)
            {
                operation.AddError("Total work chapter was not set");
                return operation;
            }

            using var document = await _excelDocumentFactory.CreateAsync(stream);

            stream.Close();

            var cells = document.WorkBook.WorkSheets[0].Name.ToLower().StartsWith(PossibleFirstInappropriateWorkSheet)
                ? document.WorkBook.WorkSheets[1].Cells
                : document.WorkBook.WorkSheets[0].Cells;

            var totalWorkChapterPattern = totalWorkChapter switch
            {
                TotalWorkChapter.TotalWork1To9Chapter => SubUnit1To9Pattern,
                TotalWorkChapter.TotalWork1To11Chapter => SubUnit1To11Pattern,
                TotalWorkChapter.TotalWork1To12Chapter => SubUnit1To12Pattern,
                _ => throw new ArgumentOutOfRangeException(nameof(totalWorkChapter), totalWorkChapter, null)
            };

            var totalEstimateWorkWasReached = false;
            var preparatoryEstimateWorks = new List<EstimateWork>();
            var mainEstimateWorks = new List<EstimateWork>();
            for (int row = Constants.EstimateStartRow; row <= Constants.EstimateEndRow; row++)
            {
                var estimateCalculationCellStr = cells[row, PatternsColumn].Text?.Trim().ToLower();

                if (string.IsNullOrEmpty(estimateCalculationCellStr))
                {
                    continue;
                }

                if (estimateCalculationCellStr == totalWorkChapterPattern
                    || estimateCalculationCellStr == Nrr102Pattern
                    || !SkipPatterns.Any(x => estimateCalculationCellStr.StartsWith(x)))
                {
                    OperationResult<EstimateWork> getTotalEstimateWorkOperation;
                    if (estimateCalculationCellStr == totalWorkChapterPattern)
                    {
                        getTotalEstimateWorkOperation = await _estimateParser.GetTotalEstimateWork(cells, row, totalWorkChapter);
                        totalEstimateWorkWasReached = true;
                    }
                    else
                    {
                        getTotalEstimateWorkOperation = await _estimateParser.GetEstimateWork(cells, row);
                    }

                    if (!getTotalEstimateWorkOperation.Ok)
                    {
                        operation.AddError(getTotalEstimateWorkOperation.GetMetadataMessages());
                        return operation;
                    }

                    var estimateWork = getTotalEstimateWorkOperation.Result;

                    if (estimateWork.Chapter is 1 or 8)
                    {
                        preparatoryEstimateWorks.Add(estimateWork);
                    }
                    else
                    {
                        mainEstimateWorks.Add(estimateWork);
                    }

                    if (totalEstimateWorkWasReached)
                    {
                        break;
                    }
                }
            }

            var constructionStartDate = await GetConstructionStartDate(cells, operation);

            var constructionDuration = await GetConstructionDuration(cells, operation);
            var constructionDurationCeiling = (int)decimal.Ceiling(constructionDuration);

            operation.Result = new Estimate
            {
                PreparatoryEstimateWorks = preparatoryEstimateWorks,
                MainEstimateWorks = mainEstimateWorks,
                ConstructionStartDate = constructionStartDate,
                ConstructionDuration = constructionDuration,
                ConstructionDurationCeiling = constructionDurationCeiling,
                TotalWorkChapter = totalWorkChapter,
            };

            return operation;
        }

        private async Task<decimal> GetConstructionDuration(IMyExcelRange cells, OperationResult operation)
        {
            var constructionDurationStr = cells[ConstructionDurationRow, ConstructionDurationColumn].Text;
            var parseConstructionDurationOperation =
                await _constructionParser.GetConstructionDuration(constructionDurationStr);

            decimal constructionDuration = 0;
            if (!parseConstructionDurationOperation.Ok)
            {
                operation.AddWarning(parseConstructionDurationOperation.GetMetadataMessages());
            }
            else
            {
                constructionDuration = parseConstructionDurationOperation.Result;
            }

            return constructionDuration;
        }

        private async Task<DateTime> GetConstructionStartDate(IMyExcelRange cells, OperationResult operation)
        {
            var constructionStartDateCellStr = cells[ConstructionStartDateRow, ConstructionStartDateColumn].Text;
            var parseConstructionStartDateOperation =
                await _constructionParser.GetConstructionStartDate(constructionStartDateCellStr);

            var constructionStartDate = new DateTime();
            if (!parseConstructionStartDateOperation.Ok)
            {
                operation.AddWarning(parseConstructionStartDateOperation.GetMetadataMessages());
            }
            else
            {
                constructionStartDate = parseConstructionStartDateOperation.Result;
            }

            return constructionStartDate;
        }
    }
}