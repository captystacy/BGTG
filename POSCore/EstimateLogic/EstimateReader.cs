using OfficeOpenXml;
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
        private const string _possibleFirstUnappropriateWorkSheet = "Лист5";

        private const int _constructionStartDateRow = 20;
        private const int _constructionStartDateColumn = 3;

        private const int _constructionDurationRow = 21;
        private const int _constructionDurationColumn = 3;

        private const int _patternsColumn = 1;
        private const int _workNamesColumn = 2;
        private const int _chaptersColumn = 2;
        private const int _equipmentCostColumn = 7;
        private const int _otherProductsCostColumn = 8;
        private const int _totalCostColumn = 9;

        #region Patterns for search
        private const string _objectEstimatePattern = "ОБЪЕКТНАЯ СМЕТА";
        private const string _niiBgtgPattern = "НИИ БЕЛГИПРОТОПГАЗ";
        private const string _niiBgtgQuotesPattern = "\"НИИ БЕЛГИПРОТОПГАЗ\"";
        private const string _nrr102Pattern = "НРР 8.01.102-2017";
        private const string _subUnit34dot1Pattern = "ПОДПУНКТ 34.1 ИНСТРУКЦИИ";

        private const string _chapterPattern = "ГЛАВА";
        #endregion

        public EstimateReader()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public Estimate Read(Stream stream)
        {
            var estimateWorks = new List<EstimateWork>();

            var constructionStartDate = default(DateTime);
            var constructionDuration = (decimal)0;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets[0].Name == _possibleFirstUnappropriateWorkSheet
                    ? package.Workbook.Worksheets[1]
                    : package.Workbook.Worksheets[0];

                var constructionStartDateCell = workSheet.Cells[_constructionStartDateRow, _constructionStartDateColumn].Value;
                var constructionDuraitonCell = workSheet.Cells[_constructionDurationRow, _constructionDurationColumn].Value;

                if (constructionStartDateCell == null || constructionDuraitonCell == null)
                {
                    return null;
                }

                constructionStartDate = ParseConstructionStartDate(constructionStartDateCell.ToString());
                constructionDuration = ParseConstructionDuration(constructionDuraitonCell.ToString());

                for (int row = 27; row < 100; row++)
                {
                    var estimateCalculationCell = workSheet.Cells[row, _patternsColumn].Value ?? "";
                    var estimateCalculationCellStr = estimateCalculationCell.ToString();

                    var previousCalculationCell = workSheet.Cells[row - 1, _patternsColumn].Value ?? "";
                    var previousCalculationCellStr = previousCalculationCell.ToString();

                    if (estimateCalculationCellStr.StartsWith(_objectEstimatePattern)
                        || estimateCalculationCellStr == _niiBgtgPattern
                        || estimateCalculationCellStr == _niiBgtgQuotesPattern
                        || estimateCalculationCellStr == _nrr102Pattern
                        || previousCalculationCellStr == _subUnit34dot1Pattern)
                    {
                        var estimateWork = ParseEstimateCellsToEstimateWork(workSheet, row);
                        estimateWorks.Add(estimateWork);
                    }
                }
            }
            var estimate = new Estimate(estimateWorks, constructionStartDate, constructionDuration);

            if (estimateWorks.Exists(x => x.TotalCost == 0 || x.Chapter == 0)
                || constructionStartDate == default(DateTime)
                || constructionDuration == 0
                || estimateWorks.Count == 0)
            {
                return null;
            }

            return estimate;
        }

        private decimal ParseConstructionDuration(string durationCellStr)
        {
            var durationStr = Regex.Match(durationCellStr, @"[\d,]+").Value;
            return decimal.Parse(durationStr);
        }

        private DateTime ParseConstructionStartDate(string dateCellStr)
        {
            var monthNameLower = Regex.Match(dateCellStr, @"[А-Я-а-я]+").Value;
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

        private EstimateWork ParseEstimateCellsToEstimateWork(ExcelWorksheet workSheet, int row)
        {
            var workNameCellStr = workSheet.Cells[row, _workNamesColumn].Value.ToString();
            var equipmentCostCellStr = workSheet.Cells[row, _equipmentCostColumn].Value.ToString();
            var otherProductsCostCellStr = workSheet.Cells[row, _otherProductsCostColumn].Value.ToString();
            var totalCostCellStr = workSheet.Cells[row, _totalCostColumn].Value.ToString();

            var chapter = ParseChapter(workSheet, row);
            var equipmentCost = ParseCost(equipmentCostCellStr);
            var otherProductsCost = ParseCost(otherProductsCostCellStr);
            var totalCost = ParseCost(totalCostCellStr);

            return new EstimateWork(workNameCellStr, equipmentCost, otherProductsCost, totalCost, chapter);
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
