using POSCore.CalendarPlanLogic.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlanWriter : ICalendarPlanWriter
    {
        private const string _workNamePattern = "%WN%";
        private const string _totalCostPattern = "%TC%";
        private const string _totalCostIncludingCaiwPattern = "%TIC%";
        private const string _dateAcceptancePattern = "%DA%";

        private const string _decimalFormat = "{0:f3}";
        private const string _percentFormat = "{0:P2}";

        private const int _topPatternRowIndex = 2;
        private const int _bottomPatternRowIndex = 3;

        public void Write(CalendarPlan calendarPlan, string preparatoryTemplatePath, string mainTemplatePath, string savePath)
        {
            using (var preparatoryDocument = DocX.Load(preparatoryTemplatePath))
            {
                var constructionMonths = calendarPlan.MainCalendarWorks.Single(x => x.WorkName == CalendarPlanInfo.TotalWorkName).ConstructionMonths.ToArray();

                var preparatoryTable = preparatoryDocument.Tables[0];
                ModifyCalendarPlanTable(preparatoryTable, calendarPlan.PreparatoryCalendarWorks, constructionMonths);

                using (var mainDocument = DocX.Load(mainTemplatePath))
                {
                    var mainTable = mainDocument.Tables[0];

                    ModifyCalendarPlanTable(mainTable, calendarPlan.MainCalendarWorks, constructionMonths);

                    if (calendarPlan.ConstructionDurationCeiling > 1)
                    {
                        mainTable.MergeCellsInColumn(mainTable.ColumnCount - 1, _topPatternRowIndex,
                            mainTable.RowCount - 2);
                        mainTable.Rows[_topPatternRowIndex].Cells[mainTable.ColumnCount - 1].Paragraphs[0]
                            .Append("Приемка объекта в эксплуатацию").FontSize(12);
                    }

                    preparatoryDocument.InsertDocument(mainDocument);
                    preparatoryDocument.SaveAs(savePath);
                }
            }
        }

        private void ModifyCalendarPlanTable(Table table, IEnumerable<CalendarWork> calendarWorks, ConstructionMonth[] constructionMonths)
        {
            ReplaceDatePatternWithActualDate(table, constructionMonths);

            var topPatternRow = table.Rows[_topPatternRowIndex];
            var bottomPatternRow = table.Rows[_bottomPatternRowIndex];
            foreach (var calendarWork in calendarWorks)
            {
                AddRowToTable(table, topPatternRow, bottomPatternRow, calendarWork);
            }
            topPatternRow.Remove();
            bottomPatternRow.Remove();

            ReplacePercentPartsWithActualPercentages(table, constructionMonths);

            MergeExtraConstructionMonthIntoDash(table);
        }

        private void ReplaceDatePatternWithActualDate(Table calendarPlanPatternTable, ConstructionMonth[] constructionMonths)
        {
            var monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

            for (int i = 0; i < constructionMonths.Length; i++)
            {
                var monthName = monthNames[constructionMonths[i].Date.Month - 1];
                calendarPlanPatternTable.Rows[1].ReplaceText($"%D{i}%", monthName + " " + constructionMonths[i].Date.Year);
            }

            if (constructionMonths.Length > 1)
            {
                var acceptanceDate = constructionMonths[^1].Date.AddMonths(1);
                var acceptanceMonthName = monthNames[acceptanceDate.Month - 1];
                calendarPlanPatternTable.Rows[1].ReplaceText(_dateAcceptancePattern, acceptanceMonthName + " " + acceptanceDate.Year);
            }
        }

        private void AddRowToTable(Table table, Row topPatternRow, Row bottomPatternRow, CalendarWork calendarWork)
        {
            var newTopRow = table.InsertRow(topPatternRow, table.RowCount - 2);
            var newBottomRow = table.InsertRow(bottomPatternRow, table.RowCount - 2);

            newTopRow.ReplaceText(_workNamePattern, calendarWork.WorkName);
            newTopRow.ReplaceText(_totalCostPattern, string.Format(_decimalFormat, calendarWork.TotalCost));
            newTopRow.ReplaceText(_totalCostIncludingCaiwPattern, string.Format(_decimalFormat, calendarWork.TotalCostIncludingCAIW));
            var constructionMonths = calendarWork.ConstructionMonths.ToArray();
            foreach (var constructionMonth in constructionMonths)
            {
                var creationIndex = constructionMonth.CreationIndex;
                newTopRow.ReplaceText($"%IV{creationIndex}%", string.Format(_decimalFormat, constructionMonth.InvestmentVolume));
                newBottomRow.ReplaceText($"%IW{creationIndex}%", string.Format(_decimalFormat, constructionMonth.CAIWVolume));
            }
        }

        private void ReplacePercentPartsWithActualPercentages(Table calendarPlanPatternTable, ConstructionMonth[] constructionMonths)
        {
            var lastRow = calendarPlanPatternTable.Rows[^1];
            if (lastRow.Paragraphs.Count > 1)
            {
                for (int i = 0; i < constructionMonths.Length; i++)
                {
                    lastRow.ReplaceText($"%P{i}%", string.Format(_percentFormat, constructionMonths[i].PercentPart));
                }
            }
        }

        private void MergeExtraConstructionMonthIntoDash(Table calendarPlanPatternTable)
        {
            for (int rowIndex = 2; rowIndex < calendarPlanPatternTable.RowCount - 1; rowIndex++)
            {
                for (int columnIndex = 3; columnIndex < calendarPlanPatternTable.Rows[rowIndex].Paragraphs.Count; columnIndex++)
                {
                    var paragraph = calendarPlanPatternTable.Rows[rowIndex].Paragraphs[columnIndex];
                    if (paragraph.Text.StartsWith("%IV"))
                    {
                        var nextRowParagraph = calendarPlanPatternTable.Rows[rowIndex + 1].Paragraphs[columnIndex];
                        paragraph.RemoveText(0, paragraph.Text.Length);
                        nextRowParagraph.RemoveText(0, nextRowParagraph.Text.Length);
                        calendarPlanPatternTable.MergeCellsInColumn(columnIndex, rowIndex, rowIndex + 1);
                        paragraph.Append("-").FontSize(12);
                    }
                }
            }
        }
    }
}
