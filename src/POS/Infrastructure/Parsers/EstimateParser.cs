using System.Text.RegularExpressions;
using Calabonga.OperationResults;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Extensions;
using POS.Infrastructure.Parsers.Base;
using POS.Infrastructure.Services.DocumentServices.ExcelService.Base;
using POS.Models.EstimateModels;

namespace POS.Infrastructure.Parsers
{
    public class EstimateParser : IEstimateParser
    {
        private const int WorkNamesColumn = 2;
        private const int EquipmentCostColumn = 7;
        private const int OtherProductsCostColumn = 8;
        private const int TotalCostColumn = 9;
        private const int LaborCostsColumn = 9;
        private const int ChaptersColumn = 2;
        private const string ChapterPattern = "глава";

        private const string TotalWork1To9SearchPattern = "итого по главам 1-9";
        private const string TotalWork1To11SearchPattern = "итого по главам 1-11";
        private const string TotalWork1To12SearchPattern1 = "всего по сводному сметному расчету";
        private const string TotalWork1To12SearchPattern2 = "итого на дату окончания строительства в пределах нормативной продолжительности строительства";

        private const string LaborCostsPattern = "итого по главе 1-8";

        public Task<OperationResult<int>> GetLaborCosts(IMyExcelRange cells, int nrr103Row)
        {
            var operation = OperationResult.CreateResult<int>();

            if (nrr103Row < Constants.EstimateStartRow || nrr103Row > Constants.EstimateEndRow)
            {
                operation.AddError($"NRR103 row should be in range of {Constants.EstimateStartRow} < row < {Constants.EstimateEndRow}. Value of row was {nrr103Row}");
                return Task.FromResult(operation);
            }

            var laborCostsRow = 0;
            for (int row = nrr103Row - 1; row >= Constants.EstimateStartRow; row--)
            {
                if (cells[row, WorkNamesColumn].Text?.ToLower() == LaborCostsPattern)
                {
                    laborCostsRow = row;
                    break;
                }
            }

            if (laborCostsRow <= 0)
            {
                operation.AddError("Labor costs row was not found");
                return Task.FromResult(operation);
            }

            var laborCostsCellStr = cells[laborCostsRow, LaborCostsColumn].Text;

            if (string.IsNullOrEmpty(laborCostsCellStr))
            {
                operation.AddError("Labor costs were not found");
                return Task.FromResult(operation);
            }

            var laborCostsStr = Regex.Match(laborCostsCellStr, @"\d+$").Value;

            if (!int.TryParse(laborCostsStr, out var laborCosts))
            {
                operation.AddError($"Labor costs value was {laborCostsStr}");
                return Task.FromResult(operation);
            }

            operation.Result = laborCosts;

            return Task.FromResult(operation);
        }

        public async Task<OperationResult<EstimateWork>> GetTotalEstimateWork(IMyExcelRange cells, int totalWorkPatternRow, TotalWorkChapter totalWorkChapter)
        {
            var operation = OperationResult.CreateResult<EstimateWork>();

            if (totalWorkPatternRow < Constants.EstimateStartRow || totalWorkPatternRow > Constants.EstimateEndRow)
            {
                operation.AddError($"Estimate work row should be in range of {Constants.EstimateStartRow} < row < {Constants.EstimateEndRow}. Value of row was {totalWorkPatternRow}");
                return operation;
            }

            if (totalWorkChapter is TotalWorkChapter.None)
            {
                operation.AddError("Total work chapter was not set");
                return operation;
            }

            var totalWorkRow = 0;
            for (int row = totalWorkPatternRow + 1; row < Constants.EstimateEndRow; row++)
            {
                if (IsTotalWorkChapterSearch(totalWorkChapter, cells[row, WorkNamesColumn].Text?.ToLower()))
                {
                    totalWorkRow = row;
                    break;
                }
            }

            if (totalWorkRow <= 0)
            {
                operation.AddError("Total estimate work was not found");
                return operation;
            }

            var getEstimateWorkOperation = await GetEstimateWork(cells, totalWorkRow, (int)totalWorkChapter);

            if (!getEstimateWorkOperation.Ok)
            {
                operation.AddError(getEstimateWorkOperation.GetMetadataMessages());
                return operation;
            }

            operation.Result = getEstimateWorkOperation.Result;

            return operation;
        }

        private bool IsTotalWorkChapterSearch(TotalWorkChapter totalWorkChapter, string? cellText)
        {
            if (string.IsNullOrWhiteSpace(cellText))
            {
                return false;
            }
            
            return totalWorkChapter switch
            {
                TotalWorkChapter.TotalWork1To9Chapter => TotalWork1To9SearchPattern == cellText,
                TotalWorkChapter.TotalWork1To11Chapter => TotalWork1To11SearchPattern == cellText,
                TotalWorkChapter.TotalWork1To12Chapter => cellText.StartsWith(TotalWork1To12SearchPattern1) || cellText.StartsWith(TotalWork1To12SearchPattern2),
                _ => throw new ArgumentOutOfRangeException(nameof(totalWorkChapter), totalWorkChapter, null)
            };
        }


        public async Task<OperationResult<EstimateWork>> GetEstimateWork(IMyExcelRange cells, int workRow, int chapter = 0)
        {
            var operation = OperationResult.CreateResult<EstimateWork>();

            if (workRow < Constants.EstimateStartRow || workRow > Constants.EstimateEndRow)
            {
                operation.AddError($"Estimate work row should be in range of {Constants.EstimateStartRow} < row < {Constants.EstimateEndRow}. Value of row was {workRow}");
                return operation;
            }

            var workNameCellStr = cells[workRow, WorkNamesColumn].Text;

            if (string.IsNullOrEmpty(workNameCellStr))
            {
                operation.AddError($"Work name was empty on row: {workRow}");
                return operation;
            }

            var equipmentCostCellStr = cells[workRow, EquipmentCostColumn].Text;

            if (string.IsNullOrEmpty(equipmentCostCellStr))
            {
                operation.AddError($"Equipment cost was empty on row: {workRow}");
                return operation;
            }

            var otherProductsCostCellStr = cells[workRow, OtherProductsCostColumn].Text;

            if (string.IsNullOrEmpty(otherProductsCostCellStr))
            {
                operation.AddError($"Other products cost was empty on row: {workRow}");
                return operation;
            }

            var totalCostCellStr = cells[workRow, TotalCostColumn].Text;

            if (string.IsNullOrEmpty(totalCostCellStr))
            {
                operation.AddError($"Total cost was empty on row: {workRow}");
                return operation;
            }

            var workNameCellLowerStr = workNameCellStr.ToLower();
            var workName = workNameCellLowerStr.MakeFirstLetterUppercase();

            if (chapter <= 0)
            {
                var parseChapterOperation = await GetChapter(cells, workRow);

                if (!parseChapterOperation.Ok)
                {
                    operation.AddError(parseChapterOperation.GetMetadataMessages());
                    return operation;
                }

                chapter = parseChapterOperation.Result;
            }

            var parseEquipmentCostOperation = await GetCost(equipmentCostCellStr);

            if (!parseEquipmentCostOperation.Ok)
            {
                operation.AddError(parseEquipmentCostOperation.GetMetadataMessages());
                return operation;
            }

            var parseOtherProductsCostOperation = await GetCost(otherProductsCostCellStr);

            if (!parseOtherProductsCostOperation.Ok)
            {
                operation.AddError(parseOtherProductsCostOperation.GetMetadataMessages());
                return operation;
            }

            var parseTotalCostOperation = await GetCost(totalCostCellStr);

            if (!parseTotalCostOperation.Ok)
            {
                operation.AddError(parseTotalCostOperation.GetMetadataMessages());
                return operation;
            }

            operation.Result = new EstimateWork
            {
                WorkName = workName,
                EquipmentCost = parseEquipmentCostOperation.Result,
                OtherProductsCost = parseOtherProductsCostOperation.Result,
                TotalCost = parseTotalCostOperation.Result,
                Chapter = chapter,
                Percentages = new List<decimal>(),
            };

            return operation;
        }

        private Task<OperationResult<decimal>> GetCost(string costCellStr)
        {
            var operation = OperationResult.CreateResult<decimal>();

            if (!Regex.Match(costCellStr, @"\d").Success)
            {
                operation.Result = 0;
                return Task.FromResult(operation);
            }

            var costStr = Regex.Match(costCellStr, @"^[0-9,]+").Value;

            if (!decimal.TryParse(costStr, out var cost))
            {
                operation.AddError($"Could not parse cost from {costStr}");
            }

            operation.Result = cost;

            return Task.FromResult(operation);
        }

        private Task<OperationResult<int>> GetChapter(IMyExcelRange cells, int workRow)
        {
            var operation = OperationResult.CreateResult<int>();

            if (workRow < Constants.EstimateStartRow || workRow > Constants.EstimateEndRow)
            {
                operation.AddError($"Estimate work row should be in range of {Constants.EstimateStartRow} < row < {Constants.EstimateEndRow}. Value of row was {workRow}");
                return Task.FromResult(operation);
            }

            for (int row = workRow - 1; row >= Constants.EstimateStartRow - 1; row--)
            {
                var chapterCellStr = cells[row, ChaptersColumn].Text?.ToLower();

                if (!string.IsNullOrEmpty(chapterCellStr) && chapterCellStr.StartsWith(ChapterPattern))
                {
                    var chapterStr = Regex.Match(chapterCellStr, @"\d+").Value;

                    if (!int.TryParse(chapterStr, out var chapter))
                    {
                        operation.AddError("Chapter row was without a number");
                        return Task.FromResult(operation);
                    }

                    operation.Result = chapter;

                    break;
                }
            }

            return Task.FromResult(operation);
        }
    }
}