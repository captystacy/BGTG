using System.Globalization;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using POS.DomainModels.EstimateDomainModels;

namespace POS.Infrastructure.Readers;

public class EstimateReader : IEstimateReader
{
    private const string PossibleFirstUnappropriateWorkSheet = "Лист";

    private const int ConstructionStartDateRow = 20;
    private const int ConstructionStartDateColumn = 3;

    private const int ConstructionDurationRow = 21;
    private const int ConstructionDurationColumn = 3;

    private const int StartRow = 27;
    private const int EndRow = 158;

    private const int PatternsColumn = 1;
    private const int WorkNamesColumn = 2;
    private const int ChaptersColumn = 2;
    private const int EquipmentCostColumn = 7;
    private const int OtherProductsCostColumn = 8;
    private const int TotalCostColumn = 9;
    private const int LaborCostsColumn = 9;

    #region Estimate calculations patterns that will be skipped
    private const string TablePattern = "ТАБЛИЦА";
    private const string ActPattern = "АКТ";
    private const string ReferencePattern = "СПРАВКА";
    private const string TaxPattern = "НАЛОГ";
    private const string SubItemPattern = "ПОДПУНКТ";
    private const string EstimatePattern = "СМЕТА";
    private const string EstimateLowerPattern = "смета";
    private const string ReportPattern = "ОТЧЕТ";
    private const string DecreePattern = "УКАЗ";

    #endregion

    #region Estimate works that will not be included
    private const string CompensatoryLandingsWorkName = "КОМПЕНСАЦИОННЫЕ ПОСАДКИ";
    #endregion

    private const string SubUnit1To9Pattern = "ПОДПУНКТ 30.10 ИНСТРУКЦИИ";
    private const string SubUnit1To11Pattern = "ПОДПУНКТ 31.6 ИНСТРУКЦИИ";
    private const string SubUnit1To12Pattern = "ПОДПУНКТ 33.3.2  ИНСТРУКЦИИ";
    private const string Nrr103Pattern = "НРР 8.01.103-2017";
    private const string LaborCostsPattern = "ИТОГО ПО ГЛАВЕ 1-8";
    private const string TotalWork1To9SearchPattern = "ИТОГО ПО ГЛАВАМ 1-9";
    private const string TotalWork1To11SearchPattern = "ИТОГО ПО ГЛАВАМ 1-11";
    private const string TotalWork1To12SearchPattern = "ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ";

    private const string ChapterPattern = "ГЛАВА";

    public EstimateReader()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public Estimate Read(Stream stream, TotalWorkChapter totalWorkChapter)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        var preparatoryEstimateWorks = new List<EstimateWork>();
        var mainEstimateWorks = new List<EstimateWork>();

        DateTime constructionStartDate;
        decimal constructionDuration;

        var laborCosts = 0;
        using (var package = new ExcelPackage(stream))
        {
            var workSheet = package.Workbook.Worksheets[0].Name.StartsWith(PossibleFirstUnappropriateWorkSheet)
                ? package.Workbook.Worksheets[1]
                : package.Workbook.Worksheets[0];

            constructionStartDate = ParseConstructionStartDate(workSheet.Cells[ConstructionStartDateRow, ConstructionStartDateColumn].Text);
            constructionDuration = ParseConstructionDuration(workSheet.Cells[ConstructionDurationRow, ConstructionDurationColumn].Text);

            for (int row = StartRow; row <= EndRow; row++)
            {
                var estimateCalculationCellStr = workSheet.Cells[row, PatternsColumn].Text;

                var totalWorkPattern = totalWorkChapter switch
                {
                    TotalWorkChapter.TotalWork1To9Chapter => SubUnit1To9Pattern,
                    TotalWorkChapter.TotalWork1To11Chapter => SubUnit1To11Pattern,
                    TotalWorkChapter.TotalWork1To12Chapter => SubUnit1To12Pattern,
                    _ => throw new ArgumentOutOfRangeException(nameof(totalWorkChapter), totalWorkChapter, null)
                };

                if (estimateCalculationCellStr == totalWorkPattern
                    || !string.IsNullOrEmpty(estimateCalculationCellStr)
                    && !estimateCalculationCellStr.StartsWith(TablePattern)
                    && !estimateCalculationCellStr.StartsWith(ActPattern)
                    && !estimateCalculationCellStr.StartsWith(ReferencePattern)
                    && !estimateCalculationCellStr.StartsWith(TaxPattern)
                    && !estimateCalculationCellStr.StartsWith(SubItemPattern)
                    && !estimateCalculationCellStr.StartsWith(EstimatePattern)
                    && !estimateCalculationCellStr.StartsWith(EstimateLowerPattern)
                    && !estimateCalculationCellStr.StartsWith(ReportPattern)
                    && !estimateCalculationCellStr.StartsWith(DecreePattern))
                {
                    if (estimateCalculationCellStr == CompensatoryLandingsWorkName)
                    {
                        continue;
                    }

                    if (estimateCalculationCellStr.StartsWith(Nrr103Pattern))
                    {
                        laborCosts = ParseLaborCosts(workSheet, row);
                        continue;
                    }

                    var estimateWork = estimateCalculationCellStr switch
                    {
                        SubUnit1To9Pattern => ParseTotalEstimateWorkRow(workSheet, row, totalWorkChapter),
                        SubUnit1To11Pattern => ParseTotalEstimateWorkRow(workSheet, row, totalWorkChapter),
                        SubUnit1To12Pattern => ParseTotalEstimateWorkRow(workSheet, row, totalWorkChapter),
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

        var constructionDurationCeiling = (int)decimal.Ceiling(constructionDuration);

        return new Estimate(preparatoryEstimateWorks, mainEstimateWorks, constructionStartDate, constructionDuration, constructionDurationCeiling, laborCosts);
    }

    private int ParseLaborCosts(ExcelWorksheet workSheet, int row)
    {
        var laborCostsRow = Enumerable.Range(StartRow, row - StartRow).Reverse().First(i => workSheet.Cells[i, WorkNamesColumn].Text == LaborCostsPattern);

        var laborCostsCellStr = workSheet.Cells[laborCostsRow, LaborCostsColumn].Text;

        var laborCostsStr = Regex.Match(laborCostsCellStr, @"\d+$").Value;

        return int.Parse(laborCostsStr);
    }

    private EstimateWork ParseTotalEstimateWorkRow(ExcelWorksheet workSheet, int row, TotalWorkChapter totalWorkChapter)
    {
        var totalWorkSearchPattern = totalWorkChapter switch
        {
            TotalWorkChapter.TotalWork1To9Chapter => TotalWork1To9SearchPattern,
            TotalWorkChapter.TotalWork1To11Chapter => TotalWork1To11SearchPattern,
            TotalWorkChapter.TotalWork1To12Chapter => TotalWork1To12SearchPattern,
            _ => throw new ArgumentOutOfRangeException(nameof(totalWorkChapter), totalWorkChapter, null)
        };

        var totalWorkRow = Enumerable
            .Range(row + 1, EndRow)
            .First(i => workSheet.Cells[i, WorkNamesColumn].Text == totalWorkSearchPattern);

        return ParseEstimateWorkRow(workSheet, totalWorkRow, (int)totalWorkChapter);
    }

    private decimal ParseConstructionDuration(string constructionDurationCellStr)
    {
        var durationStr = Regex.Match(constructionDurationCellStr, @"^[\d,]+").Value;

        decimal.TryParse(durationStr, out var duration);

        return duration;
    }

    private DateTime ParseConstructionStartDate(string constructionStartDateCellStr)
    {
        if (string.IsNullOrEmpty(constructionStartDateCellStr))
        {
            return default;
        }

        var monthNameLower = Regex.Match(constructionStartDateCellStr, @"[А-Я-а-я]+").Value.ToLower();
        var month = Array.IndexOf(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Select(x => x.ToLower()).ToArray(), monthNameLower) + 1;

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
            var chapterCellStr = workSheet.Cells[row - i, ChaptersColumn].Text;
            if (chapterCellStr.StartsWith(ChapterPattern))
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
        var workNameCellLowerStr = workSheet.Cells[row, WorkNamesColumn].Text.ToLower();
        var equipmentCostCellStr = workSheet.Cells[row, EquipmentCostColumn].Text;
        var otherProductsCostCellStr = workSheet.Cells[row, OtherProductsCostColumn].Text;
        var totalCostCellStr = workSheet.Cells[row, TotalCostColumn].Text;

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