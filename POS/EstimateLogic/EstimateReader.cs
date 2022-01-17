using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using POS.EstimateLogic.Interfaces;

namespace POS.EstimateLogic
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
        private const int _endRow = 158;

        private const int _patternsColumn = 1;
        private const int _workNamesColumn = 2;
        private const int _chaptersColumn = 2;
        private const int _equipmentCostColumn = 7;
        private const int _otherProductsCostColumn = 8;
        private const int _totalCostColumn = 9;
        private const int _laborCostsColumn = 9;

        #region Estimate calculations patterns that will be skipped
        private const string _tablePattern = "ТАБЛИЦА";
        private const string _actPattern = "АКТ";
        private const string _referencePattern = "СПРАВКА";
        private const string _taxPattern = "НАЛОГ";
        private const string _subItemPattern = "ПОДПУНКТ";
        private const string _estimatePattern = "СМЕТА";
        private const string _reportPattern = "ОТЧЕТ";
        #endregion

        #region Estimate works that will not be included
        private const string _compensatoryLandingsWorkName = "КОМПЕНСАЦИОННЫЕ ПОСАДКИ";
        #endregion

        private const string _subUnit1To9Pattern = "ПОДПУНКТ 30.10 ИНСТРУКЦИИ";
        private const string _subUnit1To11Pattern = "ПОДПУНКТ 31.6 ИНСТРУКЦИИ";
        private const string _subUnit1To12Pattern = "ПОДПУНКТ 33.3.2  ИНСТРУКЦИИ";
        private const string _nrr103Pattern = "НРР 8.01.103-2017";
        private const string _laborCostsPattern = "ИТОГО ПО ГЛАВЕ 1-8";
        private const string _totalWork1To9SearchPattern = "ИТОГО ПО ГЛАВАМ 1-9";
        private const string _totalWork1To11SearchPattern = "ИТОГО ПО ГЛАВАМ 1-11";
        private const string _totalWork1To12SearchPattern = "ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ";

        private const string _chapterPattern = "ГЛАВА";

        public EstimateReader()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public Estimate Read(Stream stream, TotalWorkChapter totalWorkChapter)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

            var preparatoryEstimateWorks = new List<EstimateWork>();
            var mainEstimateWorks = new List<EstimateWork>();

            string objectCipher;
            DateTime constructionStartDate;
            int constructionDurationCeiling;

            var laborCosts = 0;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets[0].Name.StartsWith(_possibleFirstUnappropriateWorkSheet)
                    ? package.Workbook.Worksheets[1]
                    : package.Workbook.Worksheets[0];

                objectCipher = workSheet.Cells[_objectCipherRow, _objectCipherColumn].Text;
                constructionStartDate = ParseConstructionStartDate(workSheet.Cells[_constructionStartDateRow, _constructionStartDateColumn].Text);
                constructionDurationCeiling = ParseConstructionDuration(workSheet.Cells[_constructionDurationRow, _constructionDurationColumn].Text);

                for (int row = _startRow; row <= _endRow; row++)
                {
                    var estimateCalculationCellStr = workSheet.Cells[row, _patternsColumn].Text;

                    var totalWorkPattern = totalWorkChapter switch
                    {
                        TotalWorkChapter.TotalWork1To9Chapter => _subUnit1To9Pattern,
                        TotalWorkChapter.TotalWork1To11Chapter => _subUnit1To11Pattern,
                        TotalWorkChapter.TotalWork1To12Chapter => _subUnit1To12Pattern,
                        _ => throw new ArgumentOutOfRangeException(nameof(totalWorkChapter), totalWorkChapter, null)
                    };

                    if (estimateCalculationCellStr == totalWorkPattern
                        || !string.IsNullOrEmpty(estimateCalculationCellStr)
                        && !estimateCalculationCellStr.StartsWith(_tablePattern)
                        && !estimateCalculationCellStr.StartsWith(_actPattern)
                        && !estimateCalculationCellStr.StartsWith(_referencePattern)
                        && !estimateCalculationCellStr.StartsWith(_taxPattern)
                        && !estimateCalculationCellStr.StartsWith(_subItemPattern)
                        && !estimateCalculationCellStr.StartsWith(_estimatePattern)
                        && !estimateCalculationCellStr.StartsWith(_reportPattern))
                    {
                        if (estimateCalculationCellStr == _compensatoryLandingsWorkName)
                        {
                            continue;
                        }

                        if (estimateCalculationCellStr.StartsWith(_nrr103Pattern))
                        {
                            laborCosts = ParseLaborCosts(workSheet, row);
                            continue;
                        }

                        var estimateWork = estimateCalculationCellStr switch
                        {
                            _subUnit1To9Pattern => ParseTotalEstimateWorkRow(workSheet, row, totalWorkChapter),
                            _subUnit1To11Pattern => ParseTotalEstimateWorkRow(workSheet, row, totalWorkChapter),
                            _subUnit1To12Pattern => ParseTotalEstimateWorkRow(workSheet, row, totalWorkChapter),
                            _ => ParseEstimateWorkRow(workSheet, row),
                        };

                        if (estimateWork.Chapter == 1 || estimateWork.Chapter == 8)
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

            return new Estimate(preparatoryEstimateWorks, mainEstimateWorks, constructionStartDate, constructionDurationCeiling, objectCipher, laborCosts);
        }

        private int ParseLaborCosts(ExcelWorksheet workSheet, int row)
        {
            var laborCostsRow = Enumerable.Range(_startRow, row - _startRow).Reverse().First(i => workSheet.Cells[i, _workNamesColumn].Text == _laborCostsPattern);

            var laborCostsCellStr = workSheet.Cells[laborCostsRow, _laborCostsColumn].Text;

            var laborCostsStr = Regex.Match(laborCostsCellStr, @"\d+$").Value;

            return int.Parse(laborCostsStr);
        }

        private EstimateWork ParseTotalEstimateWorkRow(ExcelWorksheet workSheet, int row, TotalWorkChapter totalWorkChapter)
        {
            var totalWorkSearchPattern = totalWorkChapter switch
            {
                TotalWorkChapter.TotalWork1To9Chapter => _totalWork1To9SearchPattern,
                TotalWorkChapter.TotalWork1To11Chapter => _totalWork1To11SearchPattern,
                TotalWorkChapter.TotalWork1To12Chapter => _totalWork1To12SearchPattern,
                _ => throw new ArgumentOutOfRangeException(nameof(totalWorkChapter), totalWorkChapter, null)
            };

            var totalWorkRow = Enumerable
                .Range(row + 1, _endRow)
                .First(i => workSheet.Cells[i, _workNamesColumn].Text == totalWorkSearchPattern);

            return ParseEstimateWorkRow(workSheet, totalWorkRow, (int)totalWorkChapter);
        }

        private int ParseConstructionDuration(string constructionDurationCellStr)
        {
            var durationStr = Regex.Match(constructionDurationCellStr, @"^[\d,]+").Value;

            decimal.TryParse(durationStr, out var duration);

            return (int)decimal.Ceiling(duration);
        }

        private DateTime ParseConstructionStartDate(string constructionStartDateCellStr)
        {
            if (string.IsNullOrEmpty(constructionStartDateCellStr))
            {
                return default;
            }

            var monthNameLower = Regex.Match(constructionStartDateCellStr, @"[А-Я-а-я]+").Value.ToLower();
            var month = Array.IndexOf(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames, char.ToUpper(monthNameLower[0]) + monthNameLower.Substring(1)) + 1;

            if (month < 1 || month > 12)
            {
                return default;
            }

            var dateYearStr = Regex.Match(constructionStartDateCellStr, @"\d+").Value;

            if (!int.TryParse(dateYearStr, out var dateYear))
            {
                return default;
            }

            return new DateTime(dateYear, month, 1);
        }

        private int ParseChapter(ExcelWorksheet workSheet, int row)
        {
            var chapter = 0;

            for (int i = 1; i < row; i++)
            {
                var chapterCellStr = workSheet.Cells[row - i, _chaptersColumn].Text;
                if (chapterCellStr.StartsWith(_chapterPattern))
                {
                    var chapterStr = Regex.Match(chapterCellStr, @"\d+").Value;
                    int.TryParse(chapterStr, out chapter);
                    break;
                }
            }

            return chapter;
        }

        private EstimateWork ParseEstimateWorkRow(ExcelWorksheet workSheet, int row, int mainTotalEstimateWorkChapter = 0)
        {
            var workNameCellLowerStr = workSheet.Cells[row, _workNamesColumn].Text.ToLower();
            var equipmentCostCellStr = workSheet.Cells[row, _equipmentCostColumn].Text;
            var otherProductsCostCellStr = workSheet.Cells[row, _otherProductsCostColumn].Text;
            var totalCostCellStr = workSheet.Cells[row, _totalCostColumn].Text;

            var workName = char.ToUpper(workNameCellLowerStr[0]) + workNameCellLowerStr.Substring(1);
            var chapter = mainTotalEstimateWorkChapter == 0 ? ParseChapter(workSheet, row) : mainTotalEstimateWorkChapter;
            var equipmentCost = ParseCost(equipmentCostCellStr);
            var otherProductsCost = ParseCost(otherProductsCostCellStr);
            var totalCost = ParseCost(totalCostCellStr);

            var estimateWork = new EstimateWork(workName, equipmentCost, otherProductsCost, totalCost, chapter);
            if (chapter == 1 || chapter == 8)
            {
                estimateWork.Percentages.Add(1);
            }

            return estimateWork;
        }

        private decimal ParseCost(string costCellStr)
        {
            var costStr = Regex.Match(costCellStr, @"^[0-9,]+").Value;

            decimal.TryParse(costStr, out var cost);

            return cost;
        }
    }
}
