using System.Globalization;
using POS.DomainModels.CalendarPlanDomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.Writers;

public class CalendarPlanWriter : ICalendarPlanWriter
{
    private readonly IDocumentService _documentService;

    private const string WorkNamePattern = "%WN%";
    private const string TotalCostPattern = "%TC%";
    private const string TotalCostIncludingCAIWPattern = "%TIC%";
    private const string DateAcceptancePattern = "%DA%";

    private const int TopPatternRowIndex = 2;
    private const int BottomPatternRowIndex = 3;
    private const int AcceptanceTimeRowIndex = 2;

    private const string AcceptanceTimeCellStr = "Приемка объекта в эксплуатацию";

    public CalendarPlanWriter(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public MemoryStream Write(CalendarPlan calendarPlan, string preparatoryTemplatePath, string mainTemplatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        _documentService.Load(preparatoryTemplatePath);

        var preparatoryConstructionMonths = calendarPlan.PreparatoryCalendarWorks.First(x => x.WorkName == AppConstants.TotalWorkName).ConstructionMonths.ToArray();
        ModifyCalendarPlanTable(calendarPlan.PreparatoryCalendarWorks, preparatoryConstructionMonths);

        _documentService.Load(mainTemplatePath);
        _documentService.CurrentDocumentIndex = 1;

        var mainConstructionMonths = calendarPlan.MainCalendarWorks.First(x => x.WorkName == AppConstants.TotalWorkName).ConstructionMonths.ToArray();
        ModifyCalendarPlanTable(calendarPlan.MainCalendarWorks, mainConstructionMonths);

        if (calendarPlan.ConstructionDurationCeiling > 1)
        {
            var columnIndex = _documentService.ColumnCountInTable - 1;
            var endRowIndex = _documentService.RowCount - 2;
            _documentService.MergeCellsInColumn(columnIndex, AcceptanceTimeRowIndex, endRowIndex);
            _documentService.RowIndex = AcceptanceTimeRowIndex;
            _documentService.CellIndex = columnIndex;
            _documentService.AppendInRow(AcceptanceTimeCellStr);
        }

        _documentService.InsertDocument(0, 1);

        var memoryStream = new MemoryStream();
        _documentService.SaveAs(memoryStream);

        _documentService.DisposeAllDocuments();
        return memoryStream;
    }

    private void ModifyCalendarPlanTable(IEnumerable<CalendarWork> calendarWorks, ConstructionMonth[] constructionMonths)
    {
        ReplaceDatePatternWithActualDate(constructionMonths);

        foreach (var calendarWork in calendarWorks)
        {
            AddRowToTable(calendarWork);
        }

        _documentService.RowIndex = BottomPatternRowIndex;
        _documentService.RemoveRow();
        _documentService.RowIndex = TopPatternRowIndex;
        _documentService.RemoveRow();

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
            _documentService.RowIndex = rowIndex;
            _documentService.ReplaceTextInRow($"%D{i}%", FormatMonth(monthName, constructionMonths[i].Date.Year));
        }

        if (constructionMonths.Length > 1)
        {
            var acceptanceDate = constructionMonths[^1].Date.AddMonths(1);
            var acceptanceMonthName = monthNames[acceptanceDate.Month - 1];
            _documentService.RowIndex = rowIndex;
            _documentService.ReplaceTextInRow(DateAcceptancePattern, FormatMonth(acceptanceMonthName, acceptanceDate.Year));
        }
    }

    private string FormatMonth(string monthName, int year)
    {
        return char.ToUpper(monthName[0]) + monthName.Substring(1) + " " + year;
    }

    private void AddRowToTable(CalendarWork calendarWork)
    {
        var newTopRowIndex = _documentService.RowCount - 1;
        _documentService.InsertTemplateRow(TopPatternRowIndex, newTopRowIndex);
        var newBottomRowIndex = _documentService.RowCount - 1;
        _documentService.InsertTemplateRow(BottomPatternRowIndex, newBottomRowIndex);

        _documentService.RowIndex = newTopRowIndex;
        _documentService.ReplaceTextInRow(WorkNamePattern, calendarWork.WorkName);
        _documentService.ReplaceTextInRow(TotalCostPattern, calendarWork.TotalCost.ToString(AppConstants.DecimalThreePlacesFormat));
        _documentService.ReplaceTextInRow(TotalCostIncludingCAIWPattern, calendarWork.TotalCostIncludingCAIW.ToString(AppConstants.DecimalThreePlacesFormat));

        var constructionMonths = calendarWork.ConstructionMonths.ToArray();
        foreach (var constructionMonth in constructionMonths)
        {
            var creationIndex = constructionMonth.CreationIndex;
            _documentService.RowIndex = newTopRowIndex;
            _documentService.ReplaceTextInRow($"%IV{creationIndex}%", constructionMonth.InvestmentVolume.ToString(AppConstants.DecimalThreePlacesFormat));
            _documentService.RowIndex = newBottomRowIndex;
            _documentService.ReplaceTextInRow($"%IW{creationIndex}%", constructionMonth.VolumeCAIW.ToString(AppConstants.DecimalThreePlacesFormat));
        }
    }

    private void ReplacePercentPartsWithActualPercentages(ConstructionMonth[] constructionMonths)
    {
        _documentService.RowIndex = _documentService.RowCount - 1;
        if (_documentService.ParagraphsCountInRow > 1)
        {
            for (int i = 0; i < constructionMonths.Length; i++)
            {
                _documentService.ReplaceTextInRow($"%P{i}%", constructionMonths[i].PercentPart.ToString(AppConstants.PercentFormat));
            }
        }
    }

    private void MergeExtraConstructionMonthIntoDash()
    {
        for (int rowIndex = 2; rowIndex < _documentService.RowCount - 1; rowIndex++)
        {
            _documentService.RowIndex = rowIndex;
            var paragraphsCountInRow = _documentService.ParagraphsCountInRow;
            for (int paragraphIndex = 3; paragraphIndex < paragraphsCountInRow; paragraphIndex++)
            {
                _documentService.ParagraphIndex = paragraphIndex;
                _documentService.RowIndex = rowIndex;
                if (_documentService.ParagraphTextInRow.StartsWith("%IV"))
                {
                    _documentService.EmptyParagraphInRow();
                    _documentService.AppendInRow("-");

                    _documentService.RowIndex = rowIndex + 1;
                    _documentService.EmptyParagraphInRow();

                    _documentService.MergeCellsInColumn(paragraphIndex, rowIndex, rowIndex + 1);
                }
            }
        }
    }
}