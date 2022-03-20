using POS.DomainModels.EstimateDomainModels;
using POS.Infrastructure.Services.Base;
using System.Globalization;
using System.Text.RegularExpressions;

namespace POS.Infrastructure.Readers;

public class EstimateReader : IEstimateReader
{
    private readonly IExcelDocumentService _excelDocumentService;

    private const string PossibleFirstInappropriateWorkSheet = "лист";

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
    private const string TablePattern = "таблица";
    private const string ActPattern = "акт";
    private const string ReferencePattern = "справка";
    private const string TaxPattern = "налог";
    private const string SubItemPattern = "подпункт";
    private const string SubItemShortPattern = "п.";
    private const string EstimatePattern = "смета";
    private const string ReportPattern = "отчет";
    private const string DecreePattern = "указ";
    #endregion

    #region Estimate works that will not be included
    private const string CompensatoryLandingsWorkName = "компенсационные посадки";
    #endregion

    private const string SubUnit1To9Pattern = "подпункт 30.10 инструкции";
    private const string SubUnit1To11Pattern = "подпункт 31.6 инструкции";
    private const string SubUnit1To12Pattern = "подпункт 33.3.2  инструкции";
    private const string Nrr103Pattern = "нрр 8.01.103-2017";
    private const string LaborCostsPattern = "итого по главе 1-8";
    private const string TotalWork1To9SearchPattern = "итого по главам 1-9";
    private const string TotalWork1To11SearchPattern = "итого по главам 1-11";
    private const string TotalWork1To12SearchPattern = "всего по сводному сметному расчету";

    private const string ChapterPattern = "глава";

    private string _totalWorkChapterPattern = null!;
    private string _totalWorkChapterSearchPattern = null!;

    public EstimateReader(IExcelDocumentService excelDocumentService)
    {
        _excelDocumentService = excelDocumentService;
    }

    public Estimate Read(Stream stream, TotalWorkChapter totalWorkChapter)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        _excelDocumentService.Load(stream);
        stream.Close();

        var preparatoryEstimateWorks = new List<EstimateWork>();
        var mainEstimateWorks = new List<EstimateWork>();

        if (_excelDocumentService.WorkSheetName.ToLower().StartsWith(PossibleFirstInappropriateWorkSheet))
        {
            _excelDocumentService.WorkSheetIndex++;
        }
        var constructionStartDateCellStr = _excelDocumentService.GetCellText(ConstructionStartDateRow, ConstructionStartDateColumn);
        var constructionStartDate = ParseConstructionStartDate(constructionStartDateCellStr);
        var constructionDurationStr = _excelDocumentService.GetCellText(ConstructionDurationRow, ConstructionDurationColumn);
        var constructionDuration = ParseConstructionDuration(constructionDurationStr!);

        _totalWorkChapterPattern = totalWorkChapter switch
        {
            TotalWorkChapter.TotalWork1To9Chapter => SubUnit1To9Pattern,
            TotalWorkChapter.TotalWork1To11Chapter => SubUnit1To11Pattern,
            TotalWorkChapter.TotalWork1To12Chapter => SubUnit1To12Pattern,
            _ => throw new ArgumentOutOfRangeException(nameof(totalWorkChapter), totalWorkChapter, null)
        };

        _totalWorkChapterSearchPattern = totalWorkChapter switch
        {
            TotalWorkChapter.TotalWork1To9Chapter => TotalWork1To9SearchPattern,
            TotalWorkChapter.TotalWork1To11Chapter => TotalWork1To11SearchPattern,
            TotalWorkChapter.TotalWork1To12Chapter => TotalWork1To12SearchPattern,
            _ => throw new ArgumentOutOfRangeException(nameof(totalWorkChapter), totalWorkChapter, null)
        };

        var laborCosts = 0;
        for (int row = StartRow; row <= EndRow; row++)
        {
            var estimateCalculationCellStr = _excelDocumentService.GetCellText(row, PatternsColumn)?.ToLower();

            if (estimateCalculationCellStr == _totalWorkChapterPattern
                || !string.IsNullOrEmpty(estimateCalculationCellStr)
                && !estimateCalculationCellStr.StartsWith(TablePattern)
                && !estimateCalculationCellStr.StartsWith(ActPattern)
                && !estimateCalculationCellStr.StartsWith(ReferencePattern)
                && !estimateCalculationCellStr.StartsWith(TaxPattern)
                && !estimateCalculationCellStr.StartsWith(SubItemPattern)
                && !estimateCalculationCellStr.StartsWith(SubItemShortPattern)
                && !estimateCalculationCellStr.StartsWith(EstimatePattern)
                && !estimateCalculationCellStr.StartsWith(ReportPattern)
                && !estimateCalculationCellStr.StartsWith(DecreePattern))
            {
                if (estimateCalculationCellStr == CompensatoryLandingsWorkName)
                {
                    continue;
                }

                if (estimateCalculationCellStr.StartsWith(Nrr103Pattern))
                {
                    laborCosts = ParseLaborCosts(row);
                    continue;
                }

                var estimateWork = estimateCalculationCellStr switch
                {
                    SubUnit1To9Pattern => ParseTotalEstimateWorkRow(row, (int)totalWorkChapter),
                    SubUnit1To11Pattern => ParseTotalEstimateWorkRow(row, (int)totalWorkChapter),
                    SubUnit1To12Pattern => ParseTotalEstimateWorkRow(row, (int)totalWorkChapter),
                    _ => ParseEstimateWorkRow(row),
                };

                if (estimateWork.Chapter is 1 or 8)
                {
                    preparatoryEstimateWorks.Add(estimateWork);
                }
                else
                {
                    mainEstimateWorks.Add(estimateWork);
                }
            }
        }
        _excelDocumentService.Dispose();

        var constructionDurationCeiling = (int)decimal.Ceiling(constructionDuration);

        return new Estimate(preparatoryEstimateWorks, mainEstimateWorks, constructionStartDate, constructionDuration, constructionDurationCeiling, laborCosts);
    }

    private int ParseLaborCosts(int row)
    {
        var laborCostsRow = Enumerable.Range(StartRow, row - StartRow).Reverse().First(i => _excelDocumentService.GetCellText(i, WorkNamesColumn)?.ToLower() == LaborCostsPattern);

        var laborCostsCellStr = _excelDocumentService.GetCellText(laborCostsRow, LaborCostsColumn);

        var laborCostsStr = Regex.Match(laborCostsCellStr!, @"\d+$").Value;

        return int.Parse(laborCostsStr);
    }

    private EstimateWork ParseTotalEstimateWorkRow(int row, int totalWorkChapter)
    {
        var totalWorkRow = Enumerable
            .Range(row + 1, EndRow)
            .First(i => _excelDocumentService.GetCellText(i, WorkNamesColumn)?.ToLower() == _totalWorkChapterSearchPattern);

        return ParseEstimateWorkRow(totalWorkRow, totalWorkChapter);
    }

    private decimal ParseConstructionDuration(string constructionDurationCellStr)
    {
        var durationStr = Regex.Match(constructionDurationCellStr, @"^[\d,]+").Value;

        decimal.TryParse(durationStr, out var duration);

        return duration;
    }

    private DateTime ParseConstructionStartDate(string? constructionStartDateCellStr)
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

    private int ParseChapter(int row)
    {
        var chapter = 0;

        for (int i = 1; i < row; i++)
        {
            var chapterCellStr = _excelDocumentService.GetCellText(row - i, ChaptersColumn)?.ToLower();
            if (!string.IsNullOrEmpty(chapterCellStr) && chapterCellStr.StartsWith(ChapterPattern))
            {
                var chapterStr = Regex.Match(chapterCellStr, @"\d+").Value;
                int.TryParse(chapterStr, out chapter);
                break;
            }
        }

        return chapter;
    }

    private EstimateWork ParseEstimateWorkRow(int row, int mainTotalEstimateWorkChapter = 0)
    {
        var workNameCellLowerStr = _excelDocumentService.GetCellText(row, WorkNamesColumn)!.ToLower();
        var equipmentCostCellStr = _excelDocumentService.GetCellText(row, EquipmentCostColumn)!;
        var otherProductsCostCellStr = _excelDocumentService.GetCellText(row, OtherProductsCostColumn)!;
        var totalCostCellStr = _excelDocumentService.GetCellText(row, TotalCostColumn)!;

        var workName = char.ToUpper(workNameCellLowerStr[0]) + workNameCellLowerStr.Substring(1);
        var chapter = mainTotalEstimateWorkChapter == 0 ? ParseChapter(row) : mainTotalEstimateWorkChapter;
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