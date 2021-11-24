using POSCore.CalendarPlanLogic.Interfaces;
using System.Globalization;
using System.IO;
using System.Linq;
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

        public void Write(CalendarPlan preparatoryCalendarPlan, CalendarPlan mainCalendarPlan, string templatePath)
        {
            using (var document = DocX.Load(templatePath))
            {
                var constructionMonths = mainCalendarPlan.CalendarWorks.Single(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths.ToArray();

                var preparatoryCalendarPlanPatternTable = document.Tables[0];
                ModifyCalendarPlanTable(preparatoryCalendarPlanPatternTable, preparatoryCalendarPlan, constructionMonths);

                var mainCalendarPlanPattrenTable = document.Tables[1];
                ModifyCalendarPlanTable(mainCalendarPlanPattrenTable, mainCalendarPlan, constructionMonths);

                document.SaveAs(Directory.GetCurrentDirectory() + @"\CalendarPlan" + constructionMonths.Length + "Month.docx");
            }
        }

        private void ModifyCalendarPlanTable(Table calendarPlanPatternTable, CalendarPlan calendarPlan, ConstructionMonth[] constructionMonths)
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

        private void ReplacePercentPartsWithActualPercentages(Table calendarPlanPatternTable, ConstructionMonth[] constructionMonths)
        {
            var lastRow = calendarPlanPatternTable.Rows[^1];
            if (lastRow.Paragraphs.Count > 1)
            {
                for (int i = 0; i < constructionMonths.Length; i++)
                {
                    lastRow.ReplaceText(_percentPattern + i, string.Format(_percentFormat, constructionMonths[i].PercentePart));
                }
            }
        }

        private void ReplaceDatePatternWithActualDate(Table calendarPlanPatternTable, ConstructionMonth[] constructionMonths)
        {
            for (int i = 0; i < constructionMonths.Length; i++)
            {
                var monthName = DateTimeFormatInfo.CurrentInfo.MonthNames[constructionMonths[i].Date.Month - 1];
                calendarPlanPatternTable.Rows[1].ReplaceText(_datePattern + i, char.ToUpper(monthName[0]) + monthName.Substring(1) + " " + constructionMonths[i].Date.Year);
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
            var constructionMonths = calendarWork.ConstructionPeriod.ConstructionMonths.ToArray();

            topPatternRow.ReplaceText(_workNamePattern, calendarWork.WorkName);
            topPatternRow.ReplaceText(_totalCostPattern, string.Format(_decimalFormat, calendarWork.TotalCost));
            topPatternRow.ReplaceText(_totalCostIncludingContructionAndInstallationWorksPattern, string.Format(_decimalFormat, calendarWork.TotalCostIncludingContructionAndInstallationWorks));
            for (int i = 0; i < constructionMonths.Length; i++)
            {
                topPatternRow.ReplaceText(_investmentVolumePattern + constructionMonths[i].Index, string.Format(_decimalFormat, constructionMonths[i].InvestmentVolume));
                bottomPatternRow.ReplaceText(_contructionAndInstallationWorksVolumePattern + constructionMonths[i].Index, string.Format(_decimalFormat, constructionMonths[i].ContructionAndInstallationWorksVolume));
            }
        }
    }
}
