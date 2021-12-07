using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        private CalendarPlan CreateDefaultPreparatoryCalendarPlan()
        {
            return new CalendarPlan(new List<CalendarWork>
            {
                new CalendarWork("Подготовка территории строительства", (decimal)6.666, (decimal)3.333, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, (decimal)6.666, (decimal)3.333, 1, 0)}), 1),
                new CalendarWork("Временные здания и сооружения", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, (decimal)2.222, (decimal)1.111, 1, 0)}), 8),
                new CalendarWork("Итого:", (decimal)8.888, (decimal)4.444, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, (decimal)8.888, (decimal)4.444, 1, 0)}), 1),
            }, DateTime.Today, 1);
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

        private void AssertTableCalendarWork(Table calendarPlanTable, int rowIndex, CalendarPlan calendarPlan, string workName)
        {
            var tableWorkName = calendarPlanTable.Rows[rowIndex].Paragraphs[0].Text;
            var tableTotalCost = decimal.Parse(calendarPlanTable.Rows[rowIndex].Paragraphs[1].Text);
            var tableTotalCostIncludingContructionAndInstallationWorks = decimal.Parse(calendarPlanTable.Rows[rowIndex].Paragraphs[2].Text);

            var calendarWork = calendarPlan.CalendarWorks.Find(x => x.WorkName == workName);
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
            return new CalendarPlan(new List<CalendarWork>
            {
                new CalendarWork("Работы, выполняемые в подготовительный период", (decimal)8.888, (decimal)4.444, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, (decimal)8.888, (decimal)4.444, 1, 0)}), 2),
                new CalendarWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, 1, 0)}), 3),
                new CalendarWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, 1, 0)}), 4),
                new CalendarWork("ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, 1, 0)}), 5),
                new CalendarWork("ОБЪЕКТЫ ТРАНСПОРТА", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, 1, 0)}), 6),
                new CalendarWork("Прочие работы и затраты", (decimal)15.557, (decimal)23.334, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, (decimal)10.89, (decimal)16.334, 1, 0)}), 9),
                new CalendarWork("Итого:", (decimal)33.333, (decimal)32.222, new ConstructionPeriod(new List<ConstructionMonth> { new ConstructionMonth(DateTime.Today, 26, (decimal)23.889, 1, 0)}), 10),
            }, DateTime.Today, 1);
        }

        [Test]
        public void Write_CalendarPlan1Month_SaveCorrectCalendarPlan()
        {
            var calendarPlanWriter = CreateDefaultCalendarPlanWriter();
            var preparatoryCalendarPlan = CreateDefaultPreparatoryCalendarPlan();
            var mainCalendarPlan = CreateMainCalendarPlan1Month();
            var templatePath = @"..\..\..\CalendarPlanLogic\CalendarPlanTemplates\CalendarPlan1MonthsTemplate.docx";

            var calendarPlanFileName = "CalendarPlan.docx";
            calendarPlanWriter.Write(preparatoryCalendarPlan, mainCalendarPlan, templatePath, Directory.GetCurrentDirectory(), calendarPlanFileName);

            var calendarPlanPath = Path.Combine(Directory.GetCurrentDirectory(), calendarPlanFileName);
            using (var document = DocX.Load(calendarPlanPath))
            {
                var preparatoryCalendarPlanTable = document.Tables[0];
                var mainCalendarPlanTable = document.Tables[1];

                var mainTotalCalendarWorkConstructionMonths = mainCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;
                AssertTableDates(preparatoryCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);
                AssertTableDates(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);

                AssertTableCalendarWork(preparatoryCalendarPlanTable, 2, preparatoryCalendarPlan, "Подготовка территории строительства");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 4, preparatoryCalendarPlan, "Временные здания и сооружения");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 6, preparatoryCalendarPlan, "Итого:");

                AssertTableCalendarWork(mainCalendarPlanTable, 2, mainCalendarPlan, "Работы, выполняемые в подготовительный период");
                AssertTableCalendarWork(mainCalendarPlanTable, 4, mainCalendarPlan, "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ");
                AssertTableCalendarWork(mainCalendarPlanTable, 6, mainCalendarPlan, "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ");
                AssertTableCalendarWork(mainCalendarPlanTable, 8, mainCalendarPlan, "ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ");
                AssertTableCalendarWork(mainCalendarPlanTable, 10, mainCalendarPlan, "ОБЪЕКТЫ ТРАНСПОРТА");
                AssertTableCalendarWork(mainCalendarPlanTable, 12, mainCalendarPlan, "Прочие работы и затраты");
                AssertTableCalendarWork(mainCalendarPlanTable, 14, mainCalendarPlan, "Итого:");

                AssertPercentParts(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);
            }
        }

        private CalendarPlan CreateMainCalendarPlan2Months()
        {
            return new CalendarPlan(new List<CalendarWork>
            {
                new CalendarWork("Работы, выполняемые в подготовительный период", (decimal)8.888, (decimal)4.444, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)8.888, (decimal)4.444, 1, 0)}),
                    2),
                new CalendarWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                }), 3),
                new CalendarWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                }), 4),
                new CalendarWork("ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                }), 5),
                new CalendarWork("ОБЪЕКТЫ ТРАНСПОРТА", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                }), 6),
                new CalendarWork("Прочие работы и затраты", (decimal)15.557, (decimal)23.334, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)10.89, (decimal)16.334, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)4.667, 7, (decimal)0.3, 1),
                }), 9),
                new CalendarWork("Итого:", (decimal)33.333, (decimal)32.222, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 26, (decimal)23.889, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)7.334, (decimal)8.333, (decimal)0.3, 1),
                }), 10),
            }, DateTime.Today, 2);
        }

        private CalendarPlan CreateMainCalendarPlan2Months_SomeWorksHave1Month()
        {
            return new CalendarPlan(new List<CalendarWork>
            {
                new CalendarWork("Работы, выполняемые в подготовительный период", (decimal)8.888, (decimal)4.444, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)8.888, (decimal)4.444, 1, 0)}),
                    2),
                new CalendarWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                }), 3),
                new CalendarWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, (decimal)0.7, 0),
                }), 4),
                new CalendarWork("ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                }), 5),
                new CalendarWork("ОБЪЕКТЫ ТРАНСПОРТА", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, (decimal)0.7, 0),
                }), 6),
                new CalendarWork("Прочие работы и затраты", (decimal)15.557, (decimal)23.334, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)10.89, (decimal)16.334, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)4.667, 7, (decimal)0.3, 1),
                }), 9),
                new CalendarWork("Итого:", (decimal)33.333, (decimal)32.222, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, 26, (decimal)23.889, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)7.334, (decimal)8.333, (decimal)0.3, 1),
                }), 10),
            }, DateTime.Today, 2);
        }

        [Test]
        public void Write_CalendarPlan2Month_SaveCorrectCalendarPlan()
        {
            var calendarPlanWriter = CreateDefaultCalendarPlanWriter();
            var preparatoryCalendarPlan = CreateDefaultPreparatoryCalendarPlan();
            var mainCalendarPlan = CreateMainCalendarPlan2Months();
            var templatePath = @"..\..\..\CalendarPlanLogic\CalendarPlanTemplates\CalendarPlan2MonthsTemplate.docx"; ;

            var calendarPlanFileName = "CalendarPlan.docx";
            calendarPlanWriter.Write(preparatoryCalendarPlan, mainCalendarPlan, templatePath, Directory.GetCurrentDirectory(), calendarPlanFileName);

            var calendarPlanPath = Path.Combine(Directory.GetCurrentDirectory(), calendarPlanFileName);
            using (var document = DocX.Load(calendarPlanPath))
            {
                var preparatoryCalendarPlanTable = document.Tables[0];
                var mainCalendarPlanTable = document.Tables[1];

                var preparatoryTotalCalendarWorkConstructionMonths = preparatoryCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;
                var mainTotalCalendarWorkConstructionMonths = mainCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;

                AssertTableDates(preparatoryCalendarPlanTable, preparatoryTotalCalendarWorkConstructionMonths);
                AssertTableDates(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);

                AssertTableCalendarWork(preparatoryCalendarPlanTable, 2, preparatoryCalendarPlan, "Подготовка территории строительства");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 4, preparatoryCalendarPlan, "Временные здания и сооружения");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 6, preparatoryCalendarPlan, "Итого:");

                AssertTableCalendarWork(mainCalendarPlanTable, 2, mainCalendarPlan, "Работы, выполняемые в подготовительный период");
                AssertTableCalendarWork(mainCalendarPlanTable, 4, mainCalendarPlan, "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ");
                AssertTableCalendarWork(mainCalendarPlanTable, 6, mainCalendarPlan, "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ");
                AssertTableCalendarWork(mainCalendarPlanTable, 8, mainCalendarPlan, "ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ");
                AssertTableCalendarWork(mainCalendarPlanTable, 10, mainCalendarPlan, "ОБЪЕКТЫ ТРАНСПОРТА");
                AssertTableCalendarWork(mainCalendarPlanTable, 12, mainCalendarPlan, "Прочие работы и затраты");
                AssertTableCalendarWork(mainCalendarPlanTable, 14, mainCalendarPlan, "Итого:");

                AssertPercentParts(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);
            }
        }

        [Test]
        public void Write_SomeWorksHave1Month_SaveCorrectCalendarPlan()
        {
            var calendarPlanWriter = CreateDefaultCalendarPlanWriter();
            var preparatoryCalendarPlan = CreateDefaultPreparatoryCalendarPlan();
            var mainCalendarPlan = CreateMainCalendarPlan2Months_SomeWorksHave1Month();
            var templatePath = @"..\..\..\CalendarPlanLogic\CalendarPlanTemplates\CalendarPlan2MonthsTemplate.docx"; ;

            var calendarPlanFileName = "CalendarPlan.docx";
            calendarPlanWriter.Write(preparatoryCalendarPlan, mainCalendarPlan, templatePath, Directory.GetCurrentDirectory(), calendarPlanFileName);

            var calendarPlanPath = Path.Combine(Directory.GetCurrentDirectory(), calendarPlanFileName);
            using (var document = DocX.Load(calendarPlanPath))
            {
                var preparatoryCalendarPlanTable = document.Tables[0];
                var mainCalendarPlanTable = document.Tables[1];

                var preparatoryTotalCalendarWorkConstructionMonths = preparatoryCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;
                var mainTotalCalendarWorkConstructionMonths = mainCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;

                AssertTableDates(preparatoryCalendarPlanTable, preparatoryTotalCalendarWorkConstructionMonths);
                AssertTableDates(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);

                AssertTableCalendarWork(preparatoryCalendarPlanTable, 2, preparatoryCalendarPlan, "Подготовка территории строительства");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 4, preparatoryCalendarPlan, "Временные здания и сооружения");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 6, preparatoryCalendarPlan, "Итого:");

                AssertTableCalendarWork(mainCalendarPlanTable, 2, mainCalendarPlan, "Работы, выполняемые в подготовительный период");
                AssertTableCalendarWork(mainCalendarPlanTable, 4, mainCalendarPlan, "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ");
                AssertTableCalendarWork(mainCalendarPlanTable, 6, mainCalendarPlan, "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ");
                AssertTableCalendarWork(mainCalendarPlanTable, 8, mainCalendarPlan, "ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ");
                AssertTableCalendarWork(mainCalendarPlanTable, 10, mainCalendarPlan, "ОБЪЕКТЫ ТРАНСПОРТА");
                AssertTableCalendarWork(mainCalendarPlanTable, 12, mainCalendarPlan, "Прочие работы и затраты");
                AssertTableCalendarWork(mainCalendarPlanTable, 14, mainCalendarPlan, "Итого:");

                AssertPercentParts(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);
            }
        }

        private CalendarPlan CreateMainCalendarPlan3Months()
        {
            return new CalendarPlan(new List<CalendarWork>
            {
                new CalendarWork("Работы, выполняемые в подготовительный период", (decimal)8.888, (decimal)4.444, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)8.888, (decimal)4.444, 1, 0)}),
                    2),
                new CalendarWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)0.889, (decimal)0.444, (decimal)0.4, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), (decimal)0.667, (decimal)0.333, (decimal)0.3, 2),
                }), 3),
                new CalendarWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)0.889, (decimal)0.444, (decimal)0.4, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), (decimal)0.667, (decimal)0.333, (decimal)0.3, 2),
                }), 4),
                new CalendarWork("ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)0.889, (decimal)0.444, (decimal)0.4, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), (decimal)0.667, (decimal)0.333, (decimal)0.3, 2),
                }), 5),
                new CalendarWork("ОБЪЕКТЫ ТРАНСПОРТА", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)0.889, (decimal)0.444, (decimal)0.4, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), (decimal)0.667, (decimal)0.333, (decimal)0.3, 2),
                }), 6),
                new CalendarWork("Прочие работы и затраты", (decimal)15.557, (decimal)23.334, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)6.223, (decimal)9.334, (decimal)0.4, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)4.667, 7, (decimal)0.3, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), (decimal)4.667, 7, (decimal)0.3, 2),
                }), 9),
                new CalendarWork("Итого:", (decimal)33.333, (decimal)32.222, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)18.666, (decimal)15.555, (decimal)0.4, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)7.334, (decimal)8.333, (decimal)0.3, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), (decimal)7.334, (decimal)8.333, (decimal)0.3, 2),
                }), 10),
            }, DateTime.Today, 3);
        }

        [Test]
        public void Write_CalendarPlan3Month_SaveCorrectCalendarPlan()
        {
            var calendarPlanWriter = CreateDefaultCalendarPlanWriter();
            var preparatoryCalendarPlan = CreateDefaultPreparatoryCalendarPlan();
            var mainCalendarPlan = CreateMainCalendarPlan3Months();
            var templatePath = @"..\..\..\CalendarPlanLogic\CalendarPlanTemplates\CalendarPlan3MonthsTemplate.docx"; ;

            var calendarPlanFileName = "CalendarPlan.docx";
            calendarPlanWriter.Write(preparatoryCalendarPlan, mainCalendarPlan, templatePath, Directory.GetCurrentDirectory(), calendarPlanFileName);

            var calendarPlanPath = Path.Combine(Directory.GetCurrentDirectory(), calendarPlanFileName);
            using (var document = DocX.Load(calendarPlanPath))
            {
                var preparatoryCalendarPlanTable = document.Tables[0];
                var mainCalendarPlanTable = document.Tables[1];

                var preparatoryTotalCalendarWorkConstructionMonths = preparatoryCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;
                var mainTotalCalendarWorkConstructionMonths = mainCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;

                AssertTableDates(preparatoryCalendarPlanTable, preparatoryTotalCalendarWorkConstructionMonths);
                AssertTableDates(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);

                AssertTableCalendarWork(preparatoryCalendarPlanTable, 2, preparatoryCalendarPlan, "Подготовка территории строительства");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 4, preparatoryCalendarPlan, "Временные здания и сооружения");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 6, preparatoryCalendarPlan, "Итого:");

                AssertTableCalendarWork(mainCalendarPlanTable, 2, mainCalendarPlan, "Работы, выполняемые в подготовительный период");
                AssertTableCalendarWork(mainCalendarPlanTable, 4, mainCalendarPlan, "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ");
                AssertTableCalendarWork(mainCalendarPlanTable, 6, mainCalendarPlan, "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ");
                AssertTableCalendarWork(mainCalendarPlanTable, 8, mainCalendarPlan, "ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ");
                AssertTableCalendarWork(mainCalendarPlanTable, 10, mainCalendarPlan, "ОБЪЕКТЫ ТРАНСПОРТА");
                AssertTableCalendarWork(mainCalendarPlanTable, 12, mainCalendarPlan, "Прочие работы и затраты");
                AssertTableCalendarWork(mainCalendarPlanTable, 14, mainCalendarPlan, "Итого:");

                AssertPercentParts(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);
            }
        }

        private CalendarPlan CreateMainCalendarPlan3Months_SomeWorksHaveVariousNumberOfMonth()
        {
            return new CalendarPlan(new List<CalendarWork>
            {
                new CalendarWork("Работы, выполняемые в подготовительный период", (decimal)8.888, (decimal)4.444, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)8.888, (decimal)4.444, 1, 0)}),
                    2),
                new CalendarWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)0.889, (decimal)0.444, (decimal)0.4, 0),
                }), 3),
                new CalendarWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today.AddMonths(2), (decimal)0.667, (decimal)0.333, (decimal)0.3, 2),
                }), 4),
                new CalendarWork("ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                }), 5),
                new CalendarWork("ОБЪЕКТЫ ТРАНСПОРТА", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)0.889, (decimal)0.444, (decimal)0.4, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                }), 6),
                new CalendarWork("Прочие работы и затраты", (decimal)15.557, (decimal)23.334, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)6.223, (decimal)9.334, (decimal)0.4, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)4.667, 7, (decimal)0.3, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), (decimal)4.667, 7, (decimal)0.3, 2),
                }), 9),
                new CalendarWork("Итого:", (decimal)33.333, (decimal)32.222, new ConstructionPeriod(new List<ConstructionMonth> {
                    new ConstructionMonth(DateTime.Today, (decimal)18.666, (decimal)15.555, (decimal)0.4, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)7.334, (decimal)8.333, (decimal)0.3, 1),
                    new ConstructionMonth(DateTime.Today.AddMonths(2), (decimal)7.334, (decimal)8.333, (decimal)0.3, 2),
                }), 10),
            }, DateTime.Today, 3);
        }

        [Test]
        public void Write_SomeWorksHaveVariousNumberOfMonth_SaveCorrectCalendarPlan()
        {
            var calendarPlanWriter = CreateDefaultCalendarPlanWriter();
            var preparatoryCalendarPlan = CreateDefaultPreparatoryCalendarPlan();
            var mainCalendarPlan = CreateMainCalendarPlan3Months_SomeWorksHaveVariousNumberOfMonth();
            var templatePath = @"..\..\..\CalendarPlanLogic\CalendarPlanTemplates\CalendarPlan3MonthsTemplate.docx"; ;

            var calendarPlanFileName = "CalendarPlan.docx";
            calendarPlanWriter.Write(preparatoryCalendarPlan, mainCalendarPlan, templatePath, Directory.GetCurrentDirectory(), calendarPlanFileName);

            var calendarPlanPath = Path.Combine(Directory.GetCurrentDirectory(), calendarPlanFileName);
            using (var document = DocX.Load(calendarPlanPath))
            {
                var preparatoryCalendarPlanTable = document.Tables[0];
                var mainCalendarPlanTable = document.Tables[1];

                var preparatoryTotalCalendarWorkConstructionMonths = preparatoryCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;
                var mainTotalCalendarWorkConstructionMonths = mainCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Итого:").ConstructionPeriod.ConstructionMonths;

                AssertTableDates(preparatoryCalendarPlanTable, preparatoryTotalCalendarWorkConstructionMonths);
                AssertTableDates(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);

                AssertTableCalendarWork(preparatoryCalendarPlanTable, 2, preparatoryCalendarPlan, "Подготовка территории строительства");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 4, preparatoryCalendarPlan, "Временные здания и сооружения");
                AssertTableCalendarWork(preparatoryCalendarPlanTable, 6, preparatoryCalendarPlan, "Итого:");

                AssertTableCalendarWork(mainCalendarPlanTable, 2, mainCalendarPlan, "Работы, выполняемые в подготовительный период");
                AssertTableCalendarWork(mainCalendarPlanTable, 4, mainCalendarPlan, "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ");
                AssertTableCalendarWork(mainCalendarPlanTable, 6, mainCalendarPlan, "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ");
                AssertTableCalendarWork(mainCalendarPlanTable, 8, mainCalendarPlan, "ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ");
                AssertTableCalendarWork(mainCalendarPlanTable, 10, mainCalendarPlan, "ОБЪЕКТЫ ТРАНСПОРТА");
                AssertTableCalendarWork(mainCalendarPlanTable, 12, mainCalendarPlan, "Прочие работы и затраты");
                AssertTableCalendarWork(mainCalendarPlanTable, 14, mainCalendarPlan, "Итого:");

                AssertPercentParts(mainCalendarPlanTable, mainTotalCalendarWorkConstructionMonths);
            }
        }
    }
}
