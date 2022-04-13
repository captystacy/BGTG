using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Appenders.Base;
using POS.Infrastructure.Extensions;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using POS.Models.CalendarPlanModels;
using System.Globalization;

namespace POS.Infrastructure.Appenders
{
    public class CalendarPlanAppender : ICalendarPlanAppender
    {
        private const string FirstRowFirstCellText = "Наименование отдельных зданий, сооружений и видов работ";
        private const string FirstRowSecondCellText = "Сметная стоимость, тыс. руб.";

        private const string FirstRowFourthCellText =
            "Распределение кап. вложений и объемов СМР по месяцам строительства, тыс. руб.";

        private const string SecondRowSecondCellText = "всего";
        private const string SecondRowThirdCellText = "в т.ч. СМР";

        private const string LastRowFirstCellText =
            "Примечание: в числителе – объем капвложений, в знаменателе – объем СМР.";

        private const string LastRowSecondCellText = "Задел, %";

        private const string AcceptanceTimeCellText = "Приемка объекта в эксплуатацию";

        private const int DefaultRowsNumber = 3;
        private const int DefaultColumnsNumber = 3;
        private const int FirstConstructionMonthCellIndex = 3;
        private const int FirstCalendarWorkRowIndex = 2;
        private const int WorkNameColumnIndex = 0;
        private const int TotalCostColumnIndex = 1;
        private const int TotalCostIncludingCAIWColumnIndex = 2;

        private const string ConstructionMonthDateFormat = "MMMM yyyy";
        private const string PercentFormat = "P2";

        private const string TotalWorkName = "Итого:";

        public async Task<IMyTable> AppendAsync(IMySection section, CalendarPlan calendarPlan, CalendarPlanType calendarPlanType)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            var tasks = new List<Task>();

            int countOfMonthsWithAcceptanceMonth;
            int countOfMonthsWithoutAcceptanceMonth;
            switch (calendarPlanType)
            {
                case CalendarPlanType.Preparatory:
                    var maxMonthIndex = calendarPlan.CalendarWorks
                        .SelectMany(x => x.ConstructionMonths)
                        .Max(x => x.CreationIndex);
                    countOfMonthsWithAcceptanceMonth = maxMonthIndex + 1;
                    countOfMonthsWithoutAcceptanceMonth = countOfMonthsWithAcceptanceMonth;
                    break;
                case CalendarPlanType.Main:
                    countOfMonthsWithoutAcceptanceMonth = calendarPlan.ConstructionDurationCeiling;
                    countOfMonthsWithAcceptanceMonth = countOfMonthsWithoutAcceptanceMonth > 1 
                        ? calendarPlan.ConstructionDurationCeiling + 1
                        : countOfMonthsWithoutAcceptanceMonth;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(calendarPlanType), calendarPlanType, null);
            }

            var columnsNumber = DefaultColumnsNumber + countOfMonthsWithAcceptanceMonth;

            var rowsNumber = DefaultRowsNumber + calendarPlan.CalendarWorks.Count() * 2;

            var table = section.AddTable(columnsNumber, rowsNumber, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);
            var lastRowIndex = rowsNumber - 1;
            var lastColumnIndex = columnsNumber - 1;

            tasks.Add(SetHeader(table, calendarPlan, countOfMonthsWithAcceptanceMonth, lastColumnIndex));

            tasks.Add(SetCalendarWorks(table, calendarPlan, countOfMonthsWithoutAcceptanceMonth));

            tasks.Add(SetFooter(calendarPlan, calendarPlanType, table, lastRowIndex, lastColumnIndex));

            if (calendarPlan.ConstructionDurationCeiling > 1 && calendarPlanType is CalendarPlanType.Main)
            {
                tasks.Add(SetAcceptanceTime(table, lastRowIndex, lastColumnIndex));
            }

            await Task.WhenAll(tasks);

            return table;
        }

        private Task SetAcceptanceTime(IMyTable table, int lastRowIndex, int lastColumnIndex)
        {
            table.Rows[2].Cells[lastColumnIndex]
                .ApplyFormat(Constants.BottomToTopCentered)
                .AddParagraph(AcceptanceTimeCellText);
            table.ApplyVerticalMerge(lastColumnIndex, 2, lastRowIndex);
            return Task.CompletedTask;
        }

        private Task SetFooter(CalendarPlan calendarPlan, CalendarPlanType calendarPlanType, IMyTable table, int lastRowIndex, int lastColumnIndex)
        {
            switch (calendarPlanType)
            {
                case CalendarPlanType.Preparatory:
                    table.Rows[lastRowIndex].Cells[0]
                        .AddParagraph(LastRowFirstCellText);

                    table.ApplyHorizontalMerge(lastRowIndex, 0, lastColumnIndex);
                    break;
                case CalendarPlanType.Main:
                    var lastRow = table.Rows[lastRowIndex];

                    lastRow.Cells[0].AddParagraph(LastRowFirstCellText);
                    lastRow.Cells[1].AddParagraph(LastRowSecondCellText);

                    var constructionMonths = calendarPlan
                        .CalendarWorks
                        .First(x => x.WorkName == TotalWorkName)
                        .ConstructionMonths
                        .ToList();

                    foreach (var constructionMonth in constructionMonths)
                    {
                        lastRow.Cells[FirstConstructionMonthCellIndex + constructionMonth.CreationIndex]
                            .AddParagraph(constructionMonth.PercentPart.ToString(PercentFormat));
                    }

                    table.ApplyHorizontalMerge(lastRowIndex, 1, 2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Task.CompletedTask;
        }

        private Task SetCalendarWorks(IMyTable table, CalendarPlan calendarPlan, int countOfMonths)
        {
            var tasks = new List<Task>();

            var rowIndexStep = 0;
            foreach (var calendarWork in calendarPlan.CalendarWorks)
            {
                var firstRow = table.Rows[FirstCalendarWorkRowIndex + rowIndexStep];
                var secondRow = table.Rows[FirstCalendarWorkRowIndex + rowIndexStep + 1];

                tasks.Add(SetCalendarWork(table, countOfMonths, calendarWork, firstRow, secondRow));

                rowIndexStep += 2;
            }

            return Task.WhenAll(tasks);
        }

        private Task SetCalendarWork(IMyTable table, int countOfMonths, CalendarWork calendarWork, IMyRow firstRow, IMyRow secondRow)
        {
            firstRow.Cells[WorkNameColumnIndex].AddParagraph(calendarWork.WorkName);
            firstRow.Cells[TotalCostColumnIndex]
                .AddParagraph(calendarWork.TotalCost.ToString(Constants.DecimalThreePlacesFormat));
            firstRow.Cells[TotalCostIncludingCAIWColumnIndex]
                .AddParagraph(calendarWork.TotalCostIncludingCAIW.ToString(Constants.DecimalThreePlacesFormat));

            var firstRowIndex = firstRow.GetRowIndex();
            var secondRowIndex = secondRow.GetRowIndex();
            table.ApplyVerticalMerge(WorkNameColumnIndex, firstRowIndex, secondRowIndex);
            table.ApplyVerticalMerge(TotalCostColumnIndex, firstRowIndex, secondRowIndex);
            table.ApplyVerticalMerge(TotalCostIncludingCAIWColumnIndex, firstRowIndex, secondRowIndex);

            var constructionMonths = calendarWork.ConstructionMonths.ToList();

            var tasks = new List<Task>();

            for (int j = 0; j < countOfMonths; j++)
            {
                var cellIndex = FirstConstructionMonthCellIndex + j;

                var constructionMonth = constructionMonths.Find(x => x.CreationIndex == j);

                tasks.Add(SetConstructionMonth(table, constructionMonth, firstRow, secondRow, cellIndex));
            }

            return Task.WhenAll(tasks);
        }

        private Task SetConstructionMonth(IMyTable table, ConstructionMonth? constructionMonth, IMyRow firstRow, IMyRow secondRow,
            int cellIndex)
        {
            if (constructionMonth is null)
            {
                firstRow.Cells[cellIndex].AddParagraph(Constants.DashSymbolStr);
                table.ApplyVerticalMerge(cellIndex, firstRow.GetRowIndex(), secondRow.GetRowIndex());
                return Task.CompletedTask;
            }

            firstRow.Cells[cellIndex]
                .ApplyFormat(Constants.BottomBorderCleared)
                .AddParagraph(constructionMonth.InvestmentVolume.ToString(Constants.DecimalThreePlacesFormat), Constants.Underlined);
            secondRow.Cells[cellIndex]
                .ApplyFormat(Constants.TopBorderCleared)
                .AddParagraph(constructionMonth.VolumeCAIW.ToString(Constants.DecimalThreePlacesFormat));

            return Task.CompletedTask;
        }

        private async Task SetHeader(IMyTable table, CalendarPlan calendarPlan, int countOfMonths, int lastColumnIndex)
        {
            var firstRow = table.Rows[0];
            firstRow.Cells[0].AddParagraph(FirstRowFirstCellText);
            firstRow.Cells[1].AddParagraph(FirstRowSecondCellText);
            firstRow.Cells[3].AddParagraph(FirstRowFourthCellText);

            var secondRow = table.Rows[1];
            secondRow.Cells[1].AddParagraph(SecondRowSecondCellText);
            secondRow.Cells[2].AddParagraph(SecondRowThirdCellText);

            table.ApplyVerticalMerge(0, 0, 1);
            table.ApplyHorizontalMerge(0, 1, 2);
            table.ApplyHorizontalMerge(0, 3, lastColumnIndex);

            await SetConstructionMonthsDates(calendarPlan, secondRow, countOfMonths);
        }

        private Task SetConstructionMonthsDates(CalendarPlan calendarPlan, IMyRow row, int countOfMonths)
        {
            for (int i = 0; i < countOfMonths; i++)
            {
                row.Cells[FirstConstructionMonthCellIndex + i].AddParagraph(
                    calendarPlan.ConstructionStartDate.AddMonths(i).ToString(ConstructionMonthDateFormat)
                        .MakeFirstLetterUppercase());
            }

            return Task.CompletedTask;
        }
    }
}