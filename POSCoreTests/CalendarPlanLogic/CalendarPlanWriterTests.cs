using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace POSCoreTests.CalendarPlanLogic
{
    public class CalendarPlanWriterTests
    {
        private CalendarPlanWriter CreateDefaultCalendarPlanWriter()
        {
            return new CalendarPlanWriter();
        }

        private List<CalendarWork> CreateDefaultPreparatoryCalendarWorks()
        {
            return new List<CalendarWork>
            {
                new CalendarWork("Подготовка территории строительства", 6.666M, 3.333M, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, 6.666M, 3.333M, 1, 0)}), 1),
                new CalendarWork("Временные здания и сооружения", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, 2.222M, 1.111M, 1, 0)}), 8),
                new CalendarWork("Итого:", 8.888M, 4.444M, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, 8.888M, 4.444M, 1, 0)}), 1),
            };
        }

        private void AssertTableDates(Table calendarPlanTable, List<ConstructionMonth> totalCalendarWorkConstructionMonths)
        {
            var calendarPlanDateRow = calendarPlanTable.Rows[1];

            for (int i = 0; i < totalCalendarWorkConstructionMonths.Count; i++)
            {
                AssertDate(calendarPlanDateRow.Paragraphs[3 + i], totalCalendarWorkConstructionMonths[i].Date);
            }

            if (totalCalendarWorkConstructionMonths.Count > 1)
            {
                AssertDate(calendarPlanDateRow.Paragraphs[3 + totalCalendarWorkConstructionMonths.Count], totalCalendarWorkConstructionMonths[^1].Date.AddMonths(1));
            }
        }

        private void AssertDate(Paragraph paragraph, DateTime constructionMonthDate)
        {
            var dateStr = paragraph.Text;
            var monthName = Regex.Match(dateStr, @"[А-Я-а-я]+").Value;
            var dateYear = int.Parse(Regex.Match(dateStr, @"\d+").Value);

            var monthNames = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.MonthNames.ToList();
            var month = monthNames.IndexOf(monthName) + 1;

            Assert.AreEqual(constructionMonthDate.Month, month);
            Assert.AreEqual(constructionMonthDate.Year, dateYear);
        }

        private void AssertTableCalendarWork(Table calendarPlanTable, int rowIndex, List<CalendarWork> calendarWorks, string workName)
        {
            var tableWorkName = calendarPlanTable.Rows[rowIndex].Paragraphs[0].Text;
            var tableTotalCost = decimal.Parse(calendarPlanTable.Rows[rowIndex].Paragraphs[1].Text);
            var tableTotalCostIncludingContructionAndInstallationWorks = decimal.Parse(calendarPlanTable.Rows[rowIndex].Paragraphs[2].Text);

            var calendarWork = calendarWorks.Find(x => x.WorkName == workName);
            var constructionMonths = calendarWork.ConstructionPeriod.ConstructionMonths;

            Assert.AreEqual(calendarWork.WorkName, tableWorkName);
            Assert.AreEqual(calendarWork.TotalCost, tableTotalCost);
            Assert.AreEqual(calendarWork.TotalCostIncludingContructionAndInstallationWorks, tableTotalCostIncludingContructionAndInstallationWorks);
            AssertConstructionMonths(calendarPlanTable, rowIndex, constructionMonths);
        }

        private void AssertConstructionMonths(Table calendarPlanTable, int rowIndex, List<ConstructionMonth> constructionMonths)
        {
            var columnIndex = 3;
            for (int i = 0; i < constructionMonths.Count; i++)
            {
                var tableInvestmentVolumeStr = string.Empty;
                var tableContructionAndInstallationWorksVolumeStr = string.Empty;
                for (int j = columnIndex; j < calendarPlanTable.Rows[rowIndex].Paragraphs.Count; j++)
                {
                    tableInvestmentVolumeStr = calendarPlanTable.Rows[rowIndex].Paragraphs[j].Text;

                    if (string.IsNullOrEmpty(tableInvestmentVolumeStr) || tableInvestmentVolumeStr == "-")
                    {
                        continue;
                    }

                    tableContructionAndInstallationWorksVolumeStr = calendarPlanTable.Rows[rowIndex + 1].Paragraphs[j].Text;
                    columnIndex = j + 1;
                    break;
                }

                if (tableInvestmentVolumeStr == "-")
                {
                    return;
                }

                decimal.TryParse(tableInvestmentVolumeStr, out var tableIntvestmentVolume);
                decimal.TryParse(tableContructionAndInstallationWorksVolumeStr, out var tableContructionAndInstallationWorksVolume);

                Assert.AreEqual(constructionMonths[i].InvestmentVolume, tableIntvestmentVolume);
                Assert.AreEqual(constructionMonths[i].ContructionAndInstallationWorksVolume, tableContructionAndInstallationWorksVolume);
            }
        }

        private void AssertPercentParts(Table mainCalendarPlanTable, List<ConstructionMonth> mainTotalCalendarWorkConstructionMonths)
        {
            var lastRow = mainCalendarPlanTable.Rows[^1];

            for (int i = 0; i < mainTotalCalendarWorkConstructionMonths.Count; i++)
            {
                var percentPart = decimal.Parse(Regex.Match(lastRow.Paragraphs[i + 2].Text, @"[\d,]+").Value) / 100;

                Assert.AreEqual(mainTotalCalendarWorkConstructionMonths[i].PercentPart, percentPart);
            }
        }

        private CalendarPlan CreateMainCalendarPlan1Month()
        {
            return new CalendarPlan(CreateDefaultPreparatoryCalendarWorks(), new List<CalendarWork>
            {
                new CalendarWork("Работы, выполняемые в подготовительный период", 8.888M, 4.444M, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, 8.888M, 4.444M, 1, 0)}), 2),
                new CalendarWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, 1.555M, 0.778M, 1, 0)}), 3),
                new CalendarWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, 1.555M, 0.778M, 1, 0)}), 4),
                new CalendarWork("ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, 1.555M, 0.778M, 1, 0)}), 5),
                new CalendarWork("ОБЪЕКТЫ ТРАНСПОРТА", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, 1.555M, 0.778M, 1, 0)}), 6),
                new CalendarWork("Прочие работы и затраты", 15.557M, 23.334M, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, 10.89M, 16.334M, 1, 0)}), 9),
                new CalendarWork("Итого:", 33.333M, 32.222M, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, 26, 23.889M, 1, 0)}), 10),
            }, DateTime.Today, 1);
        }

        [Test]
        public void Write_CalendarPlan1Month_SaveCorrectCalendarPlan()
        {
            var calendarPlanWriter = CreateDefaultCalendarPlanWriter();
            var calendarPlan = CreateMainCalendarPlan1Month();
            var templatePath = @"..\..\..\CalendarPlanLogic\CalendarPlanTemplates\CalendarPlan1MonthsTemplate.docx";

            var calendarPlanFileName = "CalendarPlan.docx";
            calendarPlanWriter.Write(calendarPlan, templatePath, Directory.GetCurrentDirectory(), calendarPlanFileName);

            var calendarPlanPath = Path.Combine(Directory.GetCurrentDirectory(), calendarPlanFileName);
            using (var document = DocX.Load(calendarPlanPath))
            {
                var preparatoryCalendarPlanTable = document.Tables[0];
                var mainCalendarPlanTable = document.Tables[1];

                var mainTotalCalendarWorkConstructionMonths = calendarPlan.PreparatoryCalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;
                AssertTableDates(preparatoryCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);
                AssertTableDates(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);

                AssertTableCalendarWork(preparatoryCalendarPlanTable, 2, calendarPlan.PreparatoryCalendarWorks, "Подготовка территории строительства");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 4, calendarPlan.PreparatoryCalendarWorks, "Временные здания и сооружения");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 6, calendarPlan.PreparatoryCalendarWorks, "Итого:");

                AssertTableCalendarWork(mainCalendarPlanTable, 2, calendarPlan.MainCalendarWorks, "Работы, выполняемые в подготовительный период");
                AssertTableCalendarWork(mainCalendarPlanTable, 4, calendarPlan.MainCalendarWorks, "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ");
                AssertTableCalendarWork(mainCalendarPlanTable, 6, calendarPlan.MainCalendarWorks, "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ");
                AssertTableCalendarWork(mainCalendarPlanTable, 8, calendarPlan.MainCalendarWorks, "ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ");
                AssertTableCalendarWork(mainCalendarPlanTable, 10, calendarPlan.MainCalendarWorks, "ОБЪЕКТЫ ТРАНСПОРТА");
                AssertTableCalendarWork(mainCalendarPlanTable, 12, calendarPlan.MainCalendarWorks, "Прочие работы и затраты");
                AssertTableCalendarWork(mainCalendarPlanTable, 14, calendarPlan.MainCalendarWorks, "Итого:");

                AssertPercentParts(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);
            }
        }

        private CalendarPlan CreateMainCalendarPlan2Months()
        {
            return new CalendarPlan(CreateDefaultPreparatoryCalendarWorks(), new List<CalendarWork>
            {
                new CalendarWork("Работы, выполняемые в подготовительный период", 8.888M, 4.444M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 8.888M, 4.444M, 1, 0)}),
                    2),
                new CalendarWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 1.555M, 0.778M, 0.7M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 0.667M, 0.333M, 0.3M, 1),
                }), 3),
                new CalendarWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 1.555M, 0.778M, 0.7M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 0.667M, 0.333M, 0.3M, 1),
                }), 4),
                new CalendarWork("ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 1.555M, 0.778M, 0.7M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 0.667M, 0.333M, 0.3M, 1),
                }), 5),
                new CalendarWork("ОБЪЕКТЫ ТРАНСПОРТА", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 1.555M, 0.778M, 0.7M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 0.667M, 0.333M, 0.3M, 1),
                }), 6),
                new CalendarWork("Прочие работы и затраты", 15.557M, 23.334M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 10.89M, 16.334M, 0.7M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 4.667M, 7, 0.3M, 1),
                }), 9),
                new CalendarWork("Итого:", 33.333M, 32.222M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 26, 23.889M, 0.7M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 7.334M, 8.333M, 0.3M, 1),
                }), 10),
            }, DateTime.Today, 2);
        }

        private CalendarPlan CreateMainCalendarPlan2Months_SomeWorksHave1Month()
        {
            return new CalendarPlan(CreateDefaultPreparatoryCalendarWorks(), new List<CalendarWork>
            {
                new CalendarWork("Работы, выполняемые в подготовительный период", 8.888M, 4.444M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 8.888M, 4.444M, 1, 0)}),
                    2),
                new CalendarWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 0.667M, 0.333M, 0.3M, 1),
                }), 3),
                new CalendarWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 1.555M, 0.778M, 0.7M, 0),
                }), 4),
                new CalendarWork("ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 0.667M, 0.333M, 0.3M, 1),
                }), 5),
                new CalendarWork("ОБЪЕКТЫ ТРАНСПОРТА", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 1.555M, 0.778M, 0.7M, 0),
                }), 6),
                new CalendarWork("Прочие работы и затраты", 15.557M, 23.334M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 10.89M, 16.334M, 0.7M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 4.667M, 7, 0.3M, 1),
                }), 9),
                new CalendarWork("Итого:", 33.333M, 32.222M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 26, 23.889M, 0.7M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 7.334M, 8.333M, 0.3M, 1),
                }), 10),
            }, DateTime.Today, 2);
        }

        [Test]
        public void Write_CalendarPlan2Month_SaveCorrectCalendarPlan()
        {
            var calendarPlanWriter = CreateDefaultCalendarPlanWriter();
            var calendarPlan = CreateMainCalendarPlan2Months();
            var templatePath = @"..\..\..\CalendarPlanLogic\CalendarPlanTemplates\CalendarPlan2MonthsTemplate.docx"; ;

            var calendarPlanFileName = "CalendarPlan.docx";
            calendarPlanWriter.Write(calendarPlan, templatePath, Directory.GetCurrentDirectory(), calendarPlanFileName);

            var calendarPlanPath = Path.Combine(Directory.GetCurrentDirectory(), calendarPlanFileName);
            using (var document = DocX.Load(calendarPlanPath))
            {
                var preparatoryCalendarPlanTable = document.Tables[0];
                var mainCalendarPlanTable = document.Tables[1];

                var preparatoryTotalCalendarWorkConstructionMonths = calendarPlan.PreparatoryCalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;
                var mainTotalCalendarWorkConstructionMonths = calendarPlan.MainCalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;

                AssertTableDates(preparatoryCalendarPlanTable, preparatoryTotalCalendarWorkConstructionMonths);
                AssertTableDates(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);

                AssertTableCalendarWork(preparatoryCalendarPlanTable, 2, calendarPlan.PreparatoryCalendarWorks, "Подготовка территории строительства");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 4, calendarPlan.PreparatoryCalendarWorks, "Временные здания и сооружения");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 6, calendarPlan.PreparatoryCalendarWorks, "Итого:");

                AssertTableCalendarWork(mainCalendarPlanTable, 2, calendarPlan.MainCalendarWorks, "Работы, выполняемые в подготовительный период");
                AssertTableCalendarWork(mainCalendarPlanTable, 4, calendarPlan.MainCalendarWorks, "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ");
                AssertTableCalendarWork(mainCalendarPlanTable, 6, calendarPlan.MainCalendarWorks, "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ");
                AssertTableCalendarWork(mainCalendarPlanTable, 8, calendarPlan.MainCalendarWorks, "ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ");
                AssertTableCalendarWork(mainCalendarPlanTable, 10, calendarPlan.MainCalendarWorks, "ОБЪЕКТЫ ТРАНСПОРТА");
                AssertTableCalendarWork(mainCalendarPlanTable, 12, calendarPlan.MainCalendarWorks, "Прочие работы и затраты");
                AssertTableCalendarWork(mainCalendarPlanTable, 14, calendarPlan.MainCalendarWorks, "Итого:");

                AssertPercentParts(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);
            }
        }

        [Test]
        public void Write_SomeWorksHave1Month_SaveCorrectCalendarPlan()
        {
            var calendarPlanWriter = CreateDefaultCalendarPlanWriter();
            var calendarPlan = CreateMainCalendarPlan2Months_SomeWorksHave1Month();
            var templatePath = @"..\..\..\CalendarPlanLogic\CalendarPlanTemplates\CalendarPlan2MonthsTemplate.docx"; ;

            var calendarPlanFileName = "CalendarPlan.docx";
            calendarPlanWriter.Write(calendarPlan, templatePath, Directory.GetCurrentDirectory(), calendarPlanFileName);

            var calendarPlanPath = Path.Combine(Directory.GetCurrentDirectory(), calendarPlanFileName);
            using (var document = DocX.Load(calendarPlanPath))
            {
                var preparatoryCalendarPlanTable = document.Tables[0];
                var mainCalendarPlanTable = document.Tables[1];

                var preparatoryTotalCalendarWorkConstructionMonths = calendarPlan.PreparatoryCalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;
                var mainTotalCalendarWorkConstructionMonths = calendarPlan.MainCalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;

                AssertTableDates(preparatoryCalendarPlanTable, preparatoryTotalCalendarWorkConstructionMonths);
                AssertTableDates(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);

                AssertTableCalendarWork(preparatoryCalendarPlanTable, 2, calendarPlan.PreparatoryCalendarWorks, "Подготовка территории строительства");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 4, calendarPlan.PreparatoryCalendarWorks, "Временные здания и сооружения");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 6, calendarPlan.PreparatoryCalendarWorks, "Итого:");

                AssertTableCalendarWork(mainCalendarPlanTable, 2, calendarPlan.MainCalendarWorks, "Работы, выполняемые в подготовительный период");
                AssertTableCalendarWork(mainCalendarPlanTable, 4, calendarPlan.MainCalendarWorks, "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ");
                AssertTableCalendarWork(mainCalendarPlanTable, 6, calendarPlan.MainCalendarWorks, "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ");
                AssertTableCalendarWork(mainCalendarPlanTable, 8, calendarPlan.MainCalendarWorks, "ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ");
                AssertTableCalendarWork(mainCalendarPlanTable, 10, calendarPlan.MainCalendarWorks, "ОБЪЕКТЫ ТРАНСПОРТА");
                AssertTableCalendarWork(mainCalendarPlanTable, 12, calendarPlan.MainCalendarWorks, "Прочие работы и затраты");
                AssertTableCalendarWork(mainCalendarPlanTable, 14, calendarPlan.MainCalendarWorks, "Итого:");

                AssertPercentParts(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);
            }
        }

        private CalendarPlan CreateMainCalendarPlan3Months()
        {
            return new CalendarPlan(CreateDefaultPreparatoryCalendarWorks(), new List<CalendarWork>
            {
                new CalendarWork("Работы, выполняемые в подготовительный период", 8.888M, 4.444M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 8.888M, 4.444M, 1, 0)}),
                    2),
                new CalendarWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 0.889M, 0.444M, 0.4M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 0.667M, 0.333M, 0.3M, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), 0.667M, 0.333M, 0.3M, 2),
                }), 3),
                new CalendarWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 0.889M, 0.444M, 0.4M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 0.667M, 0.333M, 0.3M, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), 0.667M, 0.333M, 0.3M, 2),
                }), 4),
                new CalendarWork("ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 0.889M, 0.444M, 0.4M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 0.667M, 0.333M, 0.3M, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), 0.667M, 0.333M, 0.3M, 2),
                }), 5),
                new CalendarWork("ОБЪЕКТЫ ТРАНСПОРТА", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 0.889M, 0.444M, 0.4M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 0.667M, 0.333M, 0.3M, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), 0.667M, 0.333M, 0.3M, 2),
                }), 6),
                new CalendarWork("Прочие работы и затраты", 15.557M, 23.334M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 6.223M, 9.334M, 0.4M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 4.667M, 7, 0.3M, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), 4.667M, 7, 0.3M, 2),
                }), 9),
                new CalendarWork("Итого:", 33.333M, 32.222M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 18.666M, 15.555M, 0.4M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 7.334M, 8.333M, 0.3M, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), 7.334M, 8.333M, 0.3M, 2),
                }), 10),
            }, DateTime.Today, 3);
        }

        [Test]
        public void Write_CalendarPlan3Month_SaveCorrectCalendarPlan()
        {
            var calendarPlanWriter = CreateDefaultCalendarPlanWriter();
            var calendarPlan = CreateMainCalendarPlan3Months();
            var templatePath = @"..\..\..\CalendarPlanLogic\CalendarPlanTemplates\CalendarPlan3MonthsTemplate.docx"; ;

            var calendarPlanFileName = "CalendarPlan.docx";
            calendarPlanWriter.Write(calendarPlan, templatePath, Directory.GetCurrentDirectory(), calendarPlanFileName);

            var calendarPlanPath = Path.Combine(Directory.GetCurrentDirectory(), calendarPlanFileName);
            using (var document = DocX.Load(calendarPlanPath))
            {
                var preparatoryCalendarPlanTable = document.Tables[0];
                var mainCalendarPlanTable = document.Tables[1];

                var preparatoryTotalCalendarWorkConstructionMonths = calendarPlan.PreparatoryCalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;
                var mainTotalCalendarWorkConstructionMonths = calendarPlan.MainCalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;

                AssertTableDates(preparatoryCalendarPlanTable, preparatoryTotalCalendarWorkConstructionMonths);
                AssertTableDates(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);

                AssertTableCalendarWork(preparatoryCalendarPlanTable, 2, calendarPlan.PreparatoryCalendarWorks, "Подготовка территории строительства");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 4, calendarPlan.PreparatoryCalendarWorks, "Временные здания и сооружения");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 6, calendarPlan.PreparatoryCalendarWorks, "Итого:");

                AssertTableCalendarWork(mainCalendarPlanTable, 2, calendarPlan.MainCalendarWorks, "Работы, выполняемые в подготовительный период");
                AssertTableCalendarWork(mainCalendarPlanTable, 4, calendarPlan.MainCalendarWorks, "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ");
                AssertTableCalendarWork(mainCalendarPlanTable, 6, calendarPlan.MainCalendarWorks, "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ");
                AssertTableCalendarWork(mainCalendarPlanTable, 8, calendarPlan.MainCalendarWorks, "ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ");
                AssertTableCalendarWork(mainCalendarPlanTable, 10, calendarPlan.MainCalendarWorks, "ОБЪЕКТЫ ТРАНСПОРТА");
                AssertTableCalendarWork(mainCalendarPlanTable, 12, calendarPlan.MainCalendarWorks, "Прочие работы и затраты");
                AssertTableCalendarWork(mainCalendarPlanTable, 14, calendarPlan.MainCalendarWorks, "Итого:");

                AssertPercentParts(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);
            }
        }

        private CalendarPlan CreateMainCalendarPlan3Months_SomeWorksHaveVariousNumberOfMonth()
        {
            return new CalendarPlan(CreateDefaultPreparatoryCalendarWorks(), new List<CalendarWork>
            {
                new CalendarWork("Работы, выполняемые в подготовительный период", 8.888M, 4.444M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 8.888M, 4.444M, 1, 0)}),
                    2),
                new CalendarWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 0.889M, 0.444M, 0.4M, 0),
                }), 3),
                new CalendarWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today.AddMonths(2), 0.667M, 0.333M, 0.3M, 2),
                }), 4),
                new CalendarWork("ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 0.667M, 0.333M, 0.3M, 1),
                }), 5),
                new CalendarWork("ОБЪЕКТЫ ТРАНСПОРТА", 2.222M, 1.111M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 0.889M, 0.444M, 0.4M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 0.667M, 0.333M, 0.3M, 1),
                }), 6),
                new CalendarWork("Прочие работы и затраты", 15.557M, 23.334M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 6.223M, 9.334M, 0.4M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 4.667M, 7, 0.3M, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), 4.667M, 7, 0.3M, 2),
                }), 9),
                new CalendarWork("Итого:", 33.333M, 32.222M, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 18.666M, 15.555M, 0.4M, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), 7.334M, 8.333M, 0.3M, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), 7.334M, 8.333M, 0.3M, 2),
                }), 10),
            }, DateTime.Today, 3);
        }

        [Test]
        public void Write_SomeWorksHaveVariousNumberOfMonth_SaveCorrectCalendarPlan()
        {
            var calendarPlanWriter = CreateDefaultCalendarPlanWriter();
            var calendarPlan = CreateMainCalendarPlan3Months_SomeWorksHaveVariousNumberOfMonth();
            var templatePath = @"..\..\..\CalendarPlanLogic\CalendarPlanTemplates\CalendarPlan3MonthsTemplate.docx"; ;

            var calendarPlanFileName = "CalendarPlan.docx";
            calendarPlanWriter.Write(calendarPlan, templatePath, Directory.GetCurrentDirectory(), calendarPlanFileName);

            var calendarPlanPath = Path.Combine(Directory.GetCurrentDirectory(), calendarPlanFileName);
            using (var document = DocX.Load(calendarPlanPath))
            {
                var preparatoryCalendarPlanTable = document.Tables[0];
                var mainCalendarPlanTable = document.Tables[1];

                var preparatoryTotalCalendarWorkConstructionMonths = calendarPlan.PreparatoryCalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;
                var mainTotalCalendarWorkConstructionMonths = calendarPlan.MainCalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;

                AssertTableDates(preparatoryCalendarPlanTable, preparatoryTotalCalendarWorkConstructionMonths);
                AssertTableDates(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);

                AssertTableCalendarWork(preparatoryCalendarPlanTable, 2, calendarPlan.PreparatoryCalendarWorks, "Подготовка территории строительства");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 4, calendarPlan.PreparatoryCalendarWorks, "Временные здания и сооружения");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 6, calendarPlan.PreparatoryCalendarWorks, "Итого:");

                AssertTableCalendarWork(mainCalendarPlanTable, 2, calendarPlan.MainCalendarWorks, "Работы, выполняемые в подготовительный период");
                AssertTableCalendarWork(mainCalendarPlanTable, 4, calendarPlan.MainCalendarWorks, "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ");
                AssertTableCalendarWork(mainCalendarPlanTable, 6, calendarPlan.MainCalendarWorks, "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ");
                AssertTableCalendarWork(mainCalendarPlanTable, 8, calendarPlan.MainCalendarWorks, "ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ");
                AssertTableCalendarWork(mainCalendarPlanTable, 10, calendarPlan.MainCalendarWorks, "ОБЪЕКТЫ ТРАНСПОРТА");
                AssertTableCalendarWork(mainCalendarPlanTable, 12, calendarPlan.MainCalendarWorks, "Прочие работы и затраты");
                AssertTableCalendarWork(mainCalendarPlanTable, 14, calendarPlan.MainCalendarWorks, "Итого:");

                AssertPercentParts(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);
            }
        }
    }
}
