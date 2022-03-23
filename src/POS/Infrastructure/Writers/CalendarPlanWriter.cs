using System.Globalization;
using POS.DomainModels;
using POS.DomainModels.CalendarPlanDomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.Writers;

public class CalendarPlanWriter : ICalendarPlanWriter
{
    private readonly IWordDocumentService _wordDocumentService;

    private const string WorkNamePattern = "%WN%";
    private const string TotalCostPattern = "%TC%";
    private const string TotalCostIncludingCAIWPattern = "%TIC%";
    private const string DateAcceptancePattern = "%DA%";

    private const string PreparatoryTablePattern = "%CALENDAR_PLAN_PREPARATORY_TABLE%";
    private const string MainTablePattern = "%CALENDAR_PLAN_MAIN_TABLE%";

    private const int TopPatternRowIndex = 2;
    private const int BottomPatternRowIndex = 3;
    private const int AcceptanceTimeRowIndex = 2;

    private const int DateRowIndex = 1;
    private const int DateCellStartIndex = 3;

    private const int RowIndexWhichHasMaximumCellsCount = 1;

    private const int PercentageCellStartIndex = 2;

    private const int WorkNameCellIndex = 0;
    private const int TotalCostCellIndex = 1;
    private const int TotalCostIncludingCAIWCellIndex = 2;

    private const int InvestmentVolumeAndVolumeCAIWStartCellIndex = 3;

    private const string AcceptanceTimeCellStr = "Приемка объекта в эксплуатацию";

    private int _lastCellIndex;

    public CalendarPlanWriter(IWordDocumentService wordDocumentService)
    {
        _wordDocumentService = wordDocumentService;
    }

    public MemoryStream Write(CalendarPlan calendarPlan, string calendarPlanTemplate, string preparatoryTablePath, string mainTablePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        _wordDocumentService.Load(calendarPlanTemplate);

        var preparatoryConstructionMonths = calendarPlan.PreparatoryCalendarWorks.First(x => x.WorkName == AppConstants.TotalWorkName).ConstructionMonths.ToArray();
        ReplacePatternCalendarPlanTable(preparatoryTablePath, PreparatoryTablePattern, calendarPlan.PreparatoryCalendarWorks, preparatoryConstructionMonths);

        var mainConstructionMonths = calendarPlan.MainCalendarWorks.First(x => x.WorkName == AppConstants.TotalWorkName).ConstructionMonths.ToArray();
        ReplacePatternCalendarPlanTable(mainTablePath, MainTablePattern, calendarPlan.MainCalendarWorks, mainConstructionMonths);

        if (calendarPlan.ConstructionDurationCeiling > 1)
        {
            _wordDocumentService.TableIndex = 1;
            var endRowIndex = _wordDocumentService.RowCount - 2;
            _wordDocumentService.ApplyVerticalMerge(_lastCellIndex, AcceptanceTimeRowIndex, endRowIndex);
            _wordDocumentService.RowIndex = AcceptanceTimeRowIndex;
            _wordDocumentService.CellIndex = _lastCellIndex;
            _wordDocumentService.AddParagraph(AcceptanceTimeCellStr);
        }
        
        var memoryStream = new MemoryStream();
        _wordDocumentService.SaveAs(memoryStream, MyFileFormat.DocX);

        _wordDocumentService.DisposeLastDocument();
        return memoryStream;
    }

    private void ReplacePatternCalendarPlanTable(string tablePath, string pattern, IEnumerable<CalendarWork> calendarWorks, ConstructionMonth[] constructionMonths)
    {
        _wordDocumentService.Load(tablePath);

        _wordDocumentService.RowIndex = RowIndexWhichHasMaximumCellsCount;
        _lastCellIndex = _wordDocumentService.CellsCountInRow - 1;

        ReplaceDatePatternWithActualDate(constructionMonths);

        foreach (var calendarWork in calendarWorks)
        {
            AddRowToTable(calendarWork);
        }

        _wordDocumentService.RowIndex = BottomPatternRowIndex;
        _wordDocumentService.RemoveRow();
        _wordDocumentService.RowIndex = TopPatternRowIndex;
        _wordDocumentService.RemoveRow();

        ReplacePercentPartsWithActualPercentages(constructionMonths);

        MergeExtraConstructionMonthIntoDash();

        _wordDocumentService.ReplaceInBaseDocumentMode = true;
        _wordDocumentService.ReplaceTextWithTable(pattern);
        _wordDocumentService.ReplaceInBaseDocumentMode = false;

        _wordDocumentService.DisposeLastDocument();
    }

    private void ReplaceDatePatternWithActualDate(ConstructionMonth[] constructionMonths)
    {
        var monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

        for (int i = 0; i < constructionMonths.Length; i++)
        {
            var monthName = monthNames[constructionMonths[i].Date.Month - 1];
            _wordDocumentService.RowIndex = DateRowIndex;
            _wordDocumentService.CellIndex = DateCellStartIndex + i;
            _wordDocumentService.ReplaceTextInCell($"%D{i}%", FormatMonth(monthName, constructionMonths[i].Date.Year));
        }

        if (constructionMonths.Length > 1)
        {
            var acceptanceDate = constructionMonths[^1].Date.AddMonths(1);
            var acceptanceMonthName = monthNames[acceptanceDate.Month - 1];
            _wordDocumentService.RowIndex = DateRowIndex;
            _wordDocumentService.CellIndex = _lastCellIndex;
            _wordDocumentService.ReplaceTextInCell(DateAcceptancePattern, FormatMonth(acceptanceMonthName, acceptanceDate.Year));
        }
    }

    private string FormatMonth(string monthName, int year)
    {
        return char.ToUpper(monthName[0]) + monthName.Substring(1) + " " + year;
    }

    private void AddRowToTable(CalendarWork calendarWork)
    {
        var newTopRowIndex = _wordDocumentService.RowCount - 1;
        _wordDocumentService.InsertTemplateRow(TopPatternRowIndex, newTopRowIndex);
        var newBottomRowIndex = _wordDocumentService.RowCount - 1;
        _wordDocumentService.InsertTemplateRow(BottomPatternRowIndex, newBottomRowIndex);

        _wordDocumentService.RowIndex = newTopRowIndex;
        _wordDocumentService.CellIndex = WorkNameCellIndex;
        _wordDocumentService.ReplaceTextInCell(WorkNamePattern, calendarWork.WorkName);
        _wordDocumentService.CellIndex = TotalCostCellIndex;
        _wordDocumentService.ReplaceTextInCell(TotalCostPattern, calendarWork.TotalCost.ToString(AppConstants.DecimalThreePlacesFormat));
        _wordDocumentService.CellIndex = TotalCostIncludingCAIWCellIndex;
        _wordDocumentService.ReplaceTextInCell(TotalCostIncludingCAIWPattern, calendarWork.TotalCostIncludingCAIW.ToString(AppConstants.DecimalThreePlacesFormat));

        var constructionMonths = calendarWork.ConstructionMonths.ToArray();
        foreach (var constructionMonth in constructionMonths)
        {
            var creationIndex = constructionMonth.CreationIndex;
            _wordDocumentService.CellIndex = InvestmentVolumeAndVolumeCAIWStartCellIndex + creationIndex;
            _wordDocumentService.RowIndex = newTopRowIndex;
            _wordDocumentService.ReplaceTextInCell($"%IV{creationIndex}%",
                constructionMonth.InvestmentVolume.ToString(AppConstants.DecimalThreePlacesFormat));
            _wordDocumentService.RowIndex = newBottomRowIndex;
            _wordDocumentService.ReplaceTextInCell($"%IW{creationIndex}%",
                constructionMonth.VolumeCAIW.ToString(AppConstants.DecimalThreePlacesFormat));
        }
    }

    private void ReplacePercentPartsWithActualPercentages(ConstructionMonth[] constructionMonths)
    {
        _wordDocumentService.RowIndex = _wordDocumentService.RowCount - 1;
        if (_wordDocumentService.CellsCountInRow > 1)
        {
            for (int i = 0; i < constructionMonths.Length; i++)
            {
                _wordDocumentService.CellIndex = PercentageCellStartIndex + i;
                _wordDocumentService.ReplaceTextInCell($"%P{i}%", constructionMonths[i].PercentPart.ToString(AppConstants.PercentFormat));
            }
        }
    }

    private void MergeExtraConstructionMonthIntoDash()
    {
        for (int rowIndex = 2; rowIndex < _wordDocumentService.RowCount - 1; rowIndex++)
        {
            _wordDocumentService.RowIndex = rowIndex;
            for (int cellIndex = 3; cellIndex < _wordDocumentService.CellsCountInRow; cellIndex++)
            {
                _wordDocumentService.CellIndex = cellIndex;
                _wordDocumentService.RowIndex = rowIndex;
                if (_wordDocumentService.ParagraphsCountInCell != 0 && _wordDocumentService.ParagraphTextInCell.StartsWith("%IV"))
                {
                    _wordDocumentService.RemoveParagraphInCell();

                    _wordDocumentService.RowIndex = rowIndex + 1;
                    _wordDocumentService.ParagraphTextInCell = "-";

                    _wordDocumentService.ApplyVerticalMerge(cellIndex, rowIndex, rowIndex + 1);
                }
            }
        }
    }
}