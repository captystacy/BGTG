using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.CalendarPlanTool.Base;
using POS.Infrastructure.Tools.CalendarPlanTool.Models;
using System.Globalization;

namespace POS.Infrastructure.Tools.CalendarPlanTool;

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

        var preparatoryConstructionMonths = calendarPlan.PreparatoryCalendarWorks.First(x => x.WorkName == AppData.TotalWorkName).ConstructionMonths.ToArray();
        ModifyCalendarPlanTable(calendarPlan.PreparatoryCalendarWorks, preparatoryConstructionMonths);

        _documentService.Load(mainTemplatePath);
        _documentService.DocumentIndex = 1;

        var mainConstructionMonths = calendarPlan.MainCalendarWorks.First(x => x.WorkName == AppData.TotalWorkName).ConstructionMonths.ToArray();
        ModifyCalendarPlanTable(calendarPlan.MainCalendarWorks, mainConstructionMonths);

        if (calendarPlan.ConstructionDurationCeiling > 1)
        {
            var columnIndex = _documentService.GetColumnCount() - 1;
            var endRowIndex = _documentService.GetRowCount() - 2;
            _documentService.MergeCellsInColumn(columnIndex, AcceptanceTimeRowIndex, endRowIndex);
            _documentService.Append(AcceptanceTimeCellStr, AcceptanceTimeRowIndex, columnIndex, 0);
        }

        _documentService.InsertDocument(0, 1);

        var memoryStream = new MemoryStream();
        _documentService.SaveAs(memoryStream);

        _documentService.Dispose();
        return memoryStream;
    }

    private void ModifyCalendarPlanTable(IEnumerable<CalendarWork> calendarWorks, ConstructionMonth[] constructionMonths)
    {
        ReplaceDatePatternWithActualDate(constructionMonths);

        foreach (var calendarWork in calendarWorks)
        {
            AddRowToTable(calendarWork);
        }

        _documentService.RemoveRow(BottomPatternRowIndex);
        _documentService.RemoveRow(TopPatternRowIndex);

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
            _documentService.ReplaceText(rowIndex, $"%D{i}%", FormatMonth(monthName, constructionMonths[i].Date.Year));
        }

        if (constructionMonths.Length > 1)
        {
            var acceptanceDate = constructionMonths[^1].Date.AddMonths(1);
            var acceptanceMonthName = monthNames[acceptanceDate.Month - 1];
            _documentService.ReplaceText(rowIndex, DateAcceptancePattern, FormatMonth(acceptanceMonthName, acceptanceDate.Year));
        }
    }

    private string FormatMonth(string monthName, int year)
    {
        return char.ToUpper(monthName[0]) + monthName.Substring(1) + " " + year;
    }

    private void AddRowToTable(CalendarWork calendarWork)
    {
        var newTopRowIndex = _documentService.GetRowCount() - 1;
        _documentService.InsertTemplateRow(TopPatternRowIndex, newTopRowIndex);
        var newBottomRowIndex = _documentService.GetRowCount() - 1;
        _documentService.InsertTemplateRow(BottomPatternRowIndex, newBottomRowIndex);

        _documentService.ReplaceText(newTopRowIndex, WorkNamePattern, calendarWork.WorkName);
        _documentService.ReplaceText(newTopRowIndex, TotalCostPattern, calendarWork.TotalCost.ToString(AppData.DecimalThreePlacesFormat));
        _documentService.ReplaceText(newTopRowIndex, TotalCostIncludingCAIWPattern, calendarWork.TotalCostIncludingCAIW.ToString(AppData.DecimalThreePlacesFormat));

        var constructionMonths = calendarWork.ConstructionMonths.ToArray();
        foreach (var constructionMonth in constructionMonths)
        {
            var creationIndex = constructionMonth.CreationIndex;
            _documentService.ReplaceText(newTopRowIndex, $"%IV{creationIndex}%", constructionMonth.InvestmentVolume.ToString(AppData.DecimalThreePlacesFormat));
            _documentService.ReplaceText(newBottomRowIndex, $"%IW{creationIndex}%", constructionMonth.VolumeCAIW.ToString(AppData.DecimalThreePlacesFormat));
        }
    }

    private void ReplacePercentPartsWithActualPercentages(ConstructionMonth[] constructionMonths)
    {
        var lastRowIndex = _documentService.GetRowCount() - 1;
        if (_documentService.GetParagraphsCount(lastRowIndex) > 1)
        {
            for (int i = 0; i < constructionMonths.Length; i++)
            {
                _documentService.ReplaceText(lastRowIndex, $"%P{i}%", constructionMonths[i].PercentPart.ToString(AppData.PercentFormat));
            }
        }
    }

    private void MergeExtraConstructionMonthIntoDash()
    {
        for (int rowIndex = 2; rowIndex < _documentService.GetRowCount() - 1; rowIndex++)
        {
            for (int columnIndex = 3; columnIndex < _documentService.GetParagraphsCount(rowIndex); columnIndex++)
            {
                var paragraphText = _documentService.GetParagraphText(rowIndex, columnIndex);
                if (paragraphText.StartsWith("%IV"))
                {
                    _documentService.GetParagraphText(rowIndex + 1, columnIndex);
                    _documentService.RemoveText(rowIndex, columnIndex);
                    _documentService.RemoveText(rowIndex + 1, columnIndex);
                    _documentService.MergeCellsInColumn(columnIndex, rowIndex, rowIndex + 1);
                    _documentService.Append("-", rowIndex, columnIndex, 0);
                }
            }
        }
    }
}