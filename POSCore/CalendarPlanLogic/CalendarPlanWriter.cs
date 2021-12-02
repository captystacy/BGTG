using POSCore.CalendarPlanLogic.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace POSCore.CalendarPlanLogic
{
    public class CalendarPlanWriter : ICalendarPlanWriter
    {
        private const string _workNamePattern = "WN";
        private const string _totalCostPattern = "TC";
        private const string _totalCostIncludingContructionAndInstallationWorksPattern = "TIC";
        private const string _investmentVolumePattern = "IV";
        private const string _contructionAndInstallationWorksVolumePattern = "IWV";
        private const string _datePattern = "D";
        private const string _percentPattern = "P";


        private const string _decimalFormat = "{0:f3}";
        private const string _percentFormat = "{0:P2}";

        public void Write(CalendarPlan preparatoryCalendarPlan, CalendarPlan mainCalendarPlan, string templatePath, string savePath, string fileName)
        {
            using (var document = DocX.Load(templatePath))
            {
                var constructionMonths = mainCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;

                var preparatoryCalendarPlanPatternTable = document.Tables[0];
                ModifyCalendarPlanTable(preparatoryCalendarPlanPatternTable, preparatoryCalendarPlan, constructionMonths);

                var mainCalendarPlanPattrenTable = document.Tables[1];
                ModifyCalendarPlanTable(mainCalendarPlanPattrenTable, mainCalendarPlan, constructionMonths);

                var saveAsPath = Path.Combine(savePath, fileName);
                document.SaveAs(saveAsPath);
            }
        }

        private void ModifyCalendarPlanTable(Table calendarPlanPatternTable, CalendarPlan calendarPlan, List<ConstructionMonth> constructionMonths)
        {
            ReplaceDatePatternWithActualDate(calendarPlanPatternTable, constructionMonths);

            var i = 2;
            foreach (var calendarWork in calendarPlan.CalendarWorks)
            {
                ReplaceWorkPatternsWithActualValues(calendarPlanPatternTable.Rows[i], calendarPlanPatternTable.Rows[i + 1], calendarWork);
                i += 2;
            }

            ReplacePercentPartsWithActualPercentages(calendarPlanPatternTable, constructionMonths);

            RemoveExtraRows(calendarPlanPatternTable);

            MergeExtraConstructionMonthIntoDash(calendarPlanPatternTable);
        }

        private void MergeExtraConstructionMonthIntoDash(Table calendarPlanPatternTable)
        {
            for (int rowIndex = 2; rowIndex < calendarPlanPatternTable.RowCount - 1; rowIndex++)
            {
                for (int columnIndex = 3; columnIndex < calendarPlanPatternTable.Rows[rowIndex].Paragraphs.Count; columnIndex++)
                {
                    var paragraph = calendarPlanPatternTable.Rows[rowIndex].Paragraphs[columnIndex];
                    if (paragraph.Text.StartsWith(_investmentVolumePattern))
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

        private void ReplacePercentPartsWithActualPercentages(Table calendarPlanPatternTable, List<ConstructionMonth> constructionMonths)
        {
            var lastRow = calendarPlanPatternTable.Rows[^1];
            if (lastRow.Paragraphs.Count > 1)
            {
                for (int i = 0; i < constructionMonths.Count; i++)
                {
                    lastRow.ReplaceText(_percentPattern + i, string.Format(_percentFormat, constructionMonths[i].PercentePart));
                }
            }
        }

        private void ReplaceDatePatternWithActualDate(Table calendarPlanPatternTable, List<ConstructionMonth> constructionMonths)
        {
            for (int i = 0; i < constructionMonths.Count; i++)
            {
                var monthName = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.MonthNames[constructionMonths[i].Date.Month - 1];
                calendarPlanPatternTable.Rows[1].ReplaceText(_datePattern + i, monthName + " " + constructionMonths[i].Date.Year);
            }
        }

        private void RemoveExtraRows(Table calendarPlanPatternTable)
        {
            for (int rowIndex = 2; rowIndex < calendarPlanPatternTable.Rows.Count - 1; rowIndex++)
            {
                if (calendarPlanPatternTable.Rows[rowIndex].Paragraphs[0].Text == _workNamePattern)
                {
                    calendarPlanPatternTable.Rows[rowIndex + 1].Remove();
                    calendarPlanPatternTable.Rows[rowIndex].Remove();
                    rowIndex -= 2;
                }
            }
        }

        private void ReplaceWorkPatternsWithActualValues(Row topPatternRow, Row bottomPatternRow, CalendarWork calendarWork)
        {
            topPatternRow.ReplaceText(_workNamePattern, calendarWork.WorkName);
            topPatternRow.ReplaceText(_totalCostPattern, string.Format(_decimalFormat, calendarWork.TotalCost));
            topPatternRow.ReplaceText(_totalCostIncludingContructionAndInstallationWorksPattern, string.Format(_decimalFormat, calendarWork.TotalCostIncludingContructionAndInstallationWorks));
            for (int i = 0; i < calendarWork.ConstructionPeriod.ConstructionMonths.Count; i++)
            {
                topPatternRow.ReplaceText(_investmentVolumePattern + calendarWork.ConstructionPeriod.ConstructionMonths[i].CreationIndex, string.Format(_decimalFormat, calendarWork.ConstructionPeriod.ConstructionMonths[i].InvestmentVolume));
                bottomPatternRow.ReplaceText(_contructionAndInstallationWorksVolumePattern + calendarWork.ConstructionPeriod.ConstructionMonths[i].CreationIndex, string.Format(_decimalFormat, calendarWork.ConstructionPeriod.ConstructionMonths[i].ContructionAndInstallationWorksVolume));
            }
        }
    }
}
