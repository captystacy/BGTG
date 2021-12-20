using OfficeOpenXml;
using POSCore.CalendarPlanLogic;
using POSCore.EstimateLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace POSCore.EstimateLogic
{
    public class EstimateReader : IEstimateReader
    {
        private const string _possibleFirstUnappropriateWorkSheet = "Лист";

        private const int _objectCipherRow = 17;
        private const int _objectCipherColumn = 3;

        private const int _constructionStartDateRow = 20;
        private const int _constructionStartDateColumn = 3;

        private const int _constructionDurationRow = 21;
        private const int _constructionDurationColumn = 3;

        private const int _startRow = 27;
        private const int _endRow = 100;

        private const int _patternsColumn = 1;
        private const int _workNamesColumn = 2;
        private const int _chaptersColumn = 2;
        private const int _equipmentCostColumn = 7;
        private const int _otherProductsCostColumn = 8;
        private const int _totalCostColumn = 9;
        private const int _laborCostsColumn = 9;

        #region Patterns for search
        private const string _objectEstimatePattern = "ОБЪЕКТНАЯ СМЕТА";
        private const string _niiBgtgPattern = "НИИ БЕЛГИПРОТОПГАЗ";
        private const string _niiBgtgQuotesPattern = "\"НИИ БЕЛГИПРОТОПГАЗ\"";
        private const string _nrr102Pattern = "НРР 8.01.102-2017";
        private const string _subUnit34dot3dot2Pattern = "ПОДПУНКТ 33.3.2  ИНСТРУКЦИИ";
        private const string _nrr103Pattern = "НРР 8.01.103-2017";

        private const string _laborCostsPattern = "ИТОГО ПО ГЛАВЕ 1-8";
        private const string _totalEstimateWorkSearchPattern = "ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ";

        private const string _compensatoryLandingsWorkName = "КОМПЕНСАЦИОННЫЕ ПОСАДКИ";

        private const string _chapterPattern = "ГЛАВА";
        #endregion

        public EstimateReader()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public Estimate Read(Stream stream)
        {
            var preparatoryEstimateWorks = new List<EstimateWork>();
            var mainEstimateWorks = new List<EstimateWork>();

            var constructionStartDate = default(DateTime);
            decimal constructionDuration = 0;
            var objectCipher = string.Empty;
            var laborCosts = 0;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets[0].Name.StartsWith(_possibleFirstUnappropriateWorkSheet)
                    ? package.Workbook.Worksheets[1]
                    : package.Workbook.Worksheets[0];

                var constructionStartDateCell = workSheet.Cells[_constructionStartDateRow, _constructionStartDateColumn].Value;
                var constructionDuraitonCell = workSheet.Cells[_constructionDurationRow, _constructionDurationColumn].Value;
                var objectCipherCell = workSheet.Cells[_objectCipherRow, _objectCipherColumn].Value;

                if (constructionStartDateCell == null || constructionDuraitonCell == null || objectCipherCell == null)
                {
                    return null;
                }

                constructionStartDate = ParseConstructionStartDate(constructionStartDateCell.ToString());
                constructionDuration = ParseConstructionDuration(constructionDuraitonCell.ToString());
                objectCipher = objectCipherCell.ToString();

                for (int row = _startRow; row < _endRow; row++)
                {
                    var estimateCalculationCell = workSheet.Cells[row, _patternsColumn].Value ?? "";
                    var estimateCalculationCellStr = estimateCalculationCell.ToString();

                    if (estimateCalculationCellStr == _nrr103Pattern)
                    {
                        laborCosts = ParseLaborCosts(workSheet, row);
                    }

                    if (estimateCalculationCellStr.StartsWith(_objectEstimatePattern)
                        || estimateCalculationCellStr == _niiBgtgPattern
                        || estimateCalculationCellStr == _niiBgtgQuotesPattern
                        || estimateCalculationCellStr == _nrr102Pattern
                        || estimateCalculationCellStr == _subUnit34dot3dot2Pattern)
                    {
                        if (workSheet.Cells[row, _workNamesColumn].Value.ToString() == _compensatoryLandingsWorkName)
                        {
                            continue;
                        }

                        var estimateWork = estimateCalculationCellStr switch
                        {
                            _subUnit34dot3dot2Pattern => ParseTotalEstimateWorkRow(workSheet, row),
                            _ => ParseEstimateWorkRow(workSheet, row),
                        };

                        if (estimateWork.Chapter == 1 || estimateWork.WorkName.StartsWith(CalendarWorkCreator.TemporaryBuildingsWorkName))
                        {
                            preparatoryEstimateWorks.Add(estimateWork);
                        }
                        else
                        {
                            mainEstimateWorks.Add(estimateWork);
                        }
                    }
                }
                stream.Close();
            }
            var estimate = new Estimate(preparatoryEstimateWorks, mainEstimateWorks, constructionStartDate, constructionDuration, objectCipher, laborCosts);

            if (preparatoryEstimateWorks.Exists(x => x.TotalCost == 0 || x.Chapter == 0)
                || preparatoryEstimateWorks.Count == 0
                || mainEstimateWorks.Exists(x => x.TotalCost == 0 || x.Chapter == 0)
                || mainEstimateWorks.Count == 0
                || constructionStartDate == default(DateTime)
                || constructionDuration == 0
                || string.IsNullOrEmpty(objectCipher)
                || laborCosts == 0)
            {
                return null;
            }

            return estimate;
        }

        private int ParseLaborCosts(ExcelWorksheet workSheet, int row)
        {
            var laborCostsRow = Enumerable.Range(_startRow, row - _startRow).Reverse().First(i => workSheet.Cells[i, _workNamesColumn].Value.ToString() == _laborCostsPattern);

            var laborCostsCellStr = workSheet.Cells[laborCostsRow, _laborCostsColumn].Value.ToString();

            var laborCostsStr = Regex.Match(laborCostsCellStr, @"\d+$").Value;

            return int.Parse(laborCostsStr);
        }

        private EstimateWork ParseTotalEstimateWorkRow(ExcelWorksheet workSheet, int row)
        {
            var totalWorkRow = Enumerable
                .Range(row + 1, _endRow)
                .First(i => workSheet.Cells[i, _workNamesColumn].Value.ToString() == _totalEstimateWorkSearchPattern);

            return ParseEstimateWorkRow(workSheet, totalWorkRow);
        }

        private decimal ParseConstructionDuration(string durationCellStr)
        {
            var durationStr = Regex.Match(durationCellStr, @"[\d,]+").Value;
            return decimal.Parse(durationStr);
        }

        private DateTime ParseConstructionStartDate(string dateCellStr)
        {
            var monthNameLower = Regex.Match(dateCellStr, @"[А-Я-а-я]+").Value.ToLower();
            var monthNames = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.MonthNames.ToList();
            var month = monthNames.IndexOf(char.ToUpper(monthNameLower[0]) + monthNameLower.Substring(1)) + 1;

            var dateYear = int.Parse(Regex.Match(dateCellStr, @"\d+").Value);
            return new DateTime(dateYear, month, 1);
        }

        private int ParseChapter(ExcelWorksheet workSheet, int row)
        {
            var chapter = 0;

            for (int i = 1; i < row; i++)
            {
                var chapterCellStr = workSheet.Cells[row - i, _chaptersColumn].Value.ToString();
                if (chapterCellStr.StartsWith(_chapterPattern))
                {
                    var chapterStr = Regex.Match(chapterCellStr, @"\d+").Value;
                    int.TryParse(chapterStr, out chapter);
                    break;
                }
            }

            return chapter;
        }

        private EstimateWork ParseEstimateWorkRow(ExcelWorksheet workSheet, int row)
        {
            var workNameCellLowerStr = workSheet.Cells[row, _workNamesColumn].Value.ToString().ToLower();
            var equipmentCostCellStr = workSheet.Cells[row, _equipmentCostColumn].Value.ToString();
            var otherProductsCostCellStr = workSheet.Cells[row, _otherProductsCostColumn].Value.ToString();
            var totalCostCellStr = workSheet.Cells[row, _totalCostColumn].Value.ToString();

            var workName = char.ToUpper(workNameCellLowerStr[0]) + workNameCellLowerStr.Substring(1);
            var chapter = ParseChapter(workSheet, row);
            var equipmentCost = ParseCost(equipmentCostCellStr);
            var otherProductsCost = ParseCost(otherProductsCostCellStr);
            var totalCost = ParseCost(totalCostCellStr);

            List<decimal> percentages = null;
            if (chapter == 1 || workName.StartsWith(CalendarWorkCreator.TemporaryBuildingsWorkName))
            {
                percentages = new List<decimal> { 1 };
            }

            return new EstimateWork(workName, equipmentCost, otherProductsCost, totalCost, chapter, percentages);
        }

        private decimal ParseCost(string costCellStr)
        {
            var costStr = Regex.Match(costCellStr, @"[0-9,]+").Value;

            decimal.TryParse(costStr, NumberStyles.Any, CultureInfo.GetCultureInfo("ru-RU"), out var cost);

            return cost % 1 != 0
                ? cost
                : 0;
        }
    }
}
