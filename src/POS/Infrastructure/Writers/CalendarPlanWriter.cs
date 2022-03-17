using System.Globalization;
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

    private const int TopPatternRowIndex = 2;
    private const int BottomPatternRowIndex = 3;
    private const int AcceptanceTimeRowIndex = 2;

    private const string AcceptanceTimeCellStr = "Приемка объекта в эксплуатацию";

    public CalendarPlanWriter(IWordDocumentService wordDocumentService)
    {
        _wordDocumentService = wordDocumentService;
    }

    public MemoryStream Write(CalendarPlan calendarPlan, string preparatoryTemplatePath, string mainTemplatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        _wordDocumentService.Load(preparatoryTemplatePath);

        var preparatoryConstructionMonths = calendarPlan.PreparatoryCalendarWorks.First(x => x.WorkName == AppConstants.TotalWorkName).ConstructionMonths.ToArray();
        ModifyCalendarPlanTable(calendarPlan.PreparatoryCalendarWorks, preparatoryConstructionMonths);

        _wordDocumentService.Load(mainTemplatePath);
        _wordDocumentService.CurrentDocumentIndex = 1;

        var mainConstructionMonths = calendarPlan.MainCalendarWorks.First(x => x.WorkName == AppConstants.TotalWorkName).ConstructionMonths.ToArray();
        ModifyCalendarPlanTable(calendarPlan.MainCalendarWorks, mainConstructionMonths);

        if (calendarPlan.ConstructionDurationCeiling > 1)
        {
            var columnIndex = _wordDocumentService.ColumnCountInTable - 1;
            var endRowIndex = _wordDocumentService.RowCount - 2;
            _wordDocumentService.MergeCellsInColumn(columnIndex, AcceptanceTimeRowIndex, endRowIndex);
            _wordDocumentService.RowIndex = AcceptanceTimeRowIndex;
            _wordDocumentService.CellIndex = columnIndex;
            _wordDocumentService.AppendInRow(AcceptanceTimeCellStr);
        }

        _wordDocumentService.InsertDocument(0, 1);

        var memoryStream = new MemoryStream();
        _wordDocumentService.SaveAs(memoryStream);

        _wordDocumentService.DisposeAllDocuments();
        return memoryStream;
    }

    private void ModifyCalendarPlanTable(IEnumerable<CalendarWork> calendarWorks, ConstructionMonth[] constructionMonths)
    {
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
    }

    private void ReplaceDatePatternWithActualDate(ConstructionMonth[] constructionMonths)
    {
        var monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

        var rowIndex = 1;
        for (int i = 0; i < constructionMonths.Length; i++)
        {
            var monthName = monthNames[constructionMonths[i].Date.Month - 1];
            _wordDocumentService.RowIndex = rowIndex;
            _wordDocumentService.ReplaceTextInRow($"%D{i}%", FormatMonth(monthName, constructionMonths[i].Date.Year));
        }

        if (constructionMonths.Length > 1)
        {
            var acceptanceDate = constructionMonths[^1].Date.AddMonths(1);
            var acceptanceMonthName = monthNames[acceptanceDate.Month - 1];
            _wordDocumentService.RowIndex = rowIndex;
            _wordDocumentService.ReplaceTextInRow(DateAcceptancePattern, FormatMonth(acceptanceMonthName, acceptanceDate.Year));
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
        _wordDocumentService.ReplaceTextInRow(WorkNamePattern, calendarWork.WorkName);
        _wordDocumentService.ReplaceTextInRow(TotalCostPattern, calendarWork.TotalCost.ToString(AppConstants.DecimalThreePlacesFormat));
        _wordDocumentService.ReplaceTextInRow(TotalCostIncludingCAIWPattern, calendarWork.TotalCostIncludingCAIW.ToString(AppConstants.DecimalThreePlacesFormat));

        var constructionMonths = calendarWork.ConstructionMonths.ToArray();
        foreach (var constructionMonth in constructionMonths)
        {
            var creationIndex = constructionMonth.CreationIndex;
            _wordDocumentService.RowIndex = newTopRowIndex;
            _wordDocumentService.ReplaceTextInRow($"%IV{creationIndex}%", constructionMonth.InvestmentVolume.ToString(AppConstants.DecimalThreePlacesFormat));
            _wordDocumentService.RowIndex = newBottomRowIndex;
            _wordDocumentService.ReplaceTextInRow($"%IW{creationIndex}%", constructionMonth.VolumeCAIW.ToString(AppConstants.DecimalThreePlacesFormat));
        }
    }

    private void ReplacePercentPartsWithActualPercentages(ConstructionMonth[] constructionMonths)
    {
        _wordDocumentService.RowIndex = _wordDocumentService.RowCount - 1;
        if (_wordDocumentService.ParagraphsCountInRow > 1)
        {
            for (int i = 0; i < constructionMonths.Length; i++)
            {
                _wordDocumentService.ReplaceTextInRow($"%P{i}%", constructionMonths[i].PercentPart.ToString(AppConstants.PercentFormat));
            }
        }
    }

    private void MergeExtraConstructionMonthIntoDash()
    {
        for (int rowIndex = 2; rowIndex < _wordDocumentService.RowCount - 1; rowIndex++)
        {
            _wordDocumentService.RowIndex = rowIndex;
            var paragraphsCountInRow = _wordDocumentService.ParagraphsCountInRow;
            for (int paragraphIndex = 3; paragraphIndex < paragraphsCountInRow; paragraphIndex++)
            {
                _wordDocumentService.ParagraphIndex = paragraphIndex;
                _wordDocumentService.RowIndex = rowIndex;
                if (_wordDocumentService.ParagraphTextInRow.StartsWith("%IV"))
                {
                    _wordDocumentService.EmptyParagraphInRow();
                    _wordDocumentService.AppendInRow("-");

                    _wordDocumentService.RowIndex = rowIndex + 1;
                    _wordDocumentService.EmptyParagraphInRow();

                    _wordDocumentService.MergeCellsInColumn(paragraphIndex, rowIndex, rowIndex + 1);
                }
            }
        }
    }
}