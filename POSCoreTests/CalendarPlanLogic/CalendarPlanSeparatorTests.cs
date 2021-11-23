using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using System;
using System.Linq;

namespace POSCoreTests.CalendarPlanLogic
{
    public class CalendarPlanSeparatorTests
    {
        private ICalendarPlanSeparator CreateDefaultCalendarPlanSeparator()
        {
            return new CalendarPlanSeparator(new ConstructionPeriodCreator());
        }

        private CalendarPlan CreateDefaultCalendarPlan()
        {
            return new CalendarPlan(new CalendarWork[]
            {
                new CalendarWork("ТРАССИРОВКА КАНАЛОВ (8,04 КМ)", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new ConstructionMonth[]
                {
                    new ConstructionMonth(DateTime.Today, (decimal)2.222, (decimal)1.111, 100)
                }), 1),
                new CalendarWork("ВОССТАНОВЛЕНИЕ ТРАСС АВТОДОРОГИ (3,813 КМ)", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new ConstructionMonth[]
                {
                    new ConstructionMonth(DateTime.Today, (decimal)2.222, (decimal)1.111, 100)
                }), 1),
                new CalendarWork("СИСТЕМА ОСУШЕНИЯ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new ConstructionMonth[]
                {
                    new ConstructionMonth(DateTime.Today, (decimal)2.222, (decimal)1.111, 100)
                }), 1),
                new CalendarWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new ConstructionMonth[]
                {
                    new ConstructionMonth(DateTime.Today, (decimal)1.5554, (decimal)0.7777, 70),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.6666, (decimal)0.3333, 30),
                }), 2),
                new CalendarWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new ConstructionMonth[]
                {
                    new ConstructionMonth(DateTime.Today, (decimal)1.5554, (decimal)0.7777, 70),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.6666, (decimal)0.3333, 30),
                }), 3),
                new CalendarWork("ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new ConstructionMonth[]
                {
                    new ConstructionMonth(DateTime.Today, (decimal)1.5554, (decimal)0.7777, 70),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.6666, (decimal)0.3333, 30),
                }), 4),
                new CalendarWork("ОБЪЕКТЫ ТРАНСПОРТА", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new ConstructionMonth[]
                {
                    new ConstructionMonth(DateTime.Today, (decimal)1.5554, (decimal)0.7777, 70),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.6666, (decimal)0.3333, 30),
                }), 5),
                new CalendarWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 9,1Х0,93 - 8,463%", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new ConstructionMonth[]
                {
                    new ConstructionMonth(DateTime.Today, (decimal)2.222, (decimal)1.111, 100),
                }), 8),
                new CalendarWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", (decimal)33.333, (decimal)32.222, new ConstructionPeriod(new ConstructionMonth[]
                {
                    new ConstructionMonth(DateTime.Today, (decimal)25.9995, (decimal)23.8886, 70),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)7.3335, (decimal)8.3334, 30),
                }), 10),
            });
        }

        [Test]
        public void Separate_CalendarWorksWithEstimateChapter1_SetCorrectPreparatoryWork()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan);

            var preparatoryWork = calendarPlanSeparator.PreparatoryCalendarPlan.CalendarPlanWorks.First(x => x.WorkName == "Подготовка территории строительства");
            var constructionMonth = preparatoryWork.ConstructionPeriod.ConstructionMonths.First();

            Assert.AreEqual((decimal)6.666, preparatoryWork.TotalCost);
            Assert.AreEqual((decimal)3.333, preparatoryWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(1, preparatoryWork.EstimateChapter);

            Assert.AreEqual((decimal)6.666, constructionMonth.InvestmentVolume);
            Assert.AreEqual((decimal)3.333, constructionMonth.ContructionAndInstallationWorksVolume);
            Assert.AreEqual(100, constructionMonth.PercentePart);
            Assert.AreEqual(DateTime.Today, constructionMonth.Date);
        }

        [Test]
        public void Separate_CalendarWorkWithEstimateChapter8_SetCorrectTemporaryBuildingsWork()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan);

            var temporaryBuildingsWork = calendarPlanSeparator.PreparatoryCalendarPlan.CalendarPlanWorks.First(x => x.WorkName == "Временные здания и сооружения");
            var constructionMonth = temporaryBuildingsWork.ConstructionPeriod.ConstructionMonths.First();

            Assert.AreEqual((decimal)2.222, temporaryBuildingsWork.TotalCost);
            Assert.AreEqual((decimal)1.111, temporaryBuildingsWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(8, temporaryBuildingsWork.EstimateChapter);

            Assert.AreEqual((decimal)2.222, constructionMonth.InvestmentVolume);
            Assert.AreEqual((decimal)1.111, constructionMonth.ContructionAndInstallationWorksVolume);
            Assert.AreEqual(100, constructionMonth.PercentePart);
            Assert.AreEqual(DateTime.Today, constructionMonth.Date);
        }

        [Test]
        public void Separate_CalendarWorksWithEstimateChapter8And1_SetCorrectPreparatoryTotalWork()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan);

            var totalWork = calendarPlanSeparator.PreparatoryCalendarPlan.CalendarPlanWorks.First(x => x.WorkName == "Итого:");
            var constructionMonth = totalWork.ConstructionPeriod.ConstructionMonths.First();

            Assert.AreEqual((decimal)8.888, totalWork.TotalCost);
            Assert.AreEqual((decimal)4.444, totalWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(1, totalWork.EstimateChapter);

            Assert.AreEqual((decimal)8.888, constructionMonth.InvestmentVolume);
            Assert.AreEqual((decimal)4.444, constructionMonth.ContructionAndInstallationWorksVolume);
            Assert.AreEqual(100, constructionMonth.PercentePart);
            Assert.AreEqual(DateTime.Today, constructionMonth.Date);
        }

        [Test]
        public void Separate_CalendarWorksWithEstimateChapter8And1_SetCorrectMainOverallPreparatoryWork()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan);

            var overallPreparatoryWork = calendarPlanSeparator.MainCalendarPlan.CalendarPlanWorks.First(x => x.WorkName == "Работы, выполняемые в подготовительный период");
            var constructionMonth = overallPreparatoryWork.ConstructionPeriod.ConstructionMonths.First();

            Assert.AreEqual((decimal)8.888, overallPreparatoryWork.TotalCost);
            Assert.AreEqual((decimal)4.444, overallPreparatoryWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(2, overallPreparatoryWork.EstimateChapter);

            Assert.AreEqual((decimal)8.888, constructionMonth.InvestmentVolume);
            Assert.AreEqual((decimal)4.444, constructionMonth.ContructionAndInstallationWorksVolume);
            Assert.AreEqual(100, constructionMonth.PercentePart);
            Assert.AreEqual(DateTime.Today, constructionMonth.Date);
        }

        [Test]
        public void Separate_CalendarWorksWithEstimateChapterEverythingExcept1And8_SetCorrectMainCalendarPlan()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan);
            var mainCalendarPlanWork = calendarPlanSeparator.MainCalendarPlan.CalendarPlanWorks.ToList();

            var channelTracingWorkExistance = mainCalendarPlanWork.Exists(x => x.WorkName == "ТРАССИРОВКА КАНАЛОВ (8,04 КМ)");
            var restorationOfhighwaysWorkExistance = mainCalendarPlanWork.Exists(x => x.WorkName == "ВОССТАНОВЛЕНИЕ ТРАСС АВТОДОРОГИ (3,813 КМ)");
            var dehumidificationSystemWorkExistance = mainCalendarPlanWork.Exists(x => x.WorkName == "СИСТЕМА ОСУШЕНИЯ");
            var swampPreparatoryWorkExistance = mainCalendarPlanWork.Exists(x => x.WorkName == "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ");
            var systemOfFirePreventionMeasuresWorkExistance = mainCalendarPlanWork.Exists(x => x.WorkName == "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ");
            var offSiteNetworksWorkExistance = mainCalendarPlanWork.Exists(x => x.WorkName == "ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ");
            var transportFacilitiesWorkExistance = mainCalendarPlanWork.Exists(x => x.WorkName == "ОБЪЕКТЫ ТРАНСПОРТА");
            var temporaryBuildingsWorkExistance = mainCalendarPlanWork.Exists(x => x.WorkName == "ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 9,1Х0,93 - 8,463%");
            var totalWorkExistance = mainCalendarPlanWork.Exists(x => x.WorkName == "Итого:");

            Assert.False(channelTracingWorkExistance);
            Assert.False(restorationOfhighwaysWorkExistance);
            Assert.False(dehumidificationSystemWorkExistance);
            Assert.True(swampPreparatoryWorkExistance);
            Assert.True(systemOfFirePreventionMeasuresWorkExistance);
            Assert.True(offSiteNetworksWorkExistance);
            Assert.True(transportFacilitiesWorkExistance);
            Assert.False(temporaryBuildingsWorkExistance);
            Assert.True(totalWorkExistance);
        }

        [Test]
        public void Separate_AllCalendarWorks_SetCorrectOtherExpensesWork()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan);

            var otherExpensesWork = calendarPlanSeparator.MainCalendarPlan.CalendarPlanWorks.First(x => x.WorkName == "Прочие работы и затраты");
            var constructionMonths = otherExpensesWork.ConstructionPeriod.ConstructionMonths.ToArray();

            Assert.AreEqual((decimal)15.557, otherExpensesWork.TotalCost);
            Assert.AreEqual((decimal)23.334, otherExpensesWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(9, otherExpensesWork.EstimateChapter);

            Assert.AreEqual((decimal)10.8899, constructionMonths[0].InvestmentVolume);
            Assert.AreEqual((decimal)16.3338, constructionMonths[0].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(70, constructionMonths[0].PercentePart);
            Assert.AreEqual(DateTime.Today, constructionMonths[0].Date);

            Assert.AreEqual((decimal)4.6671, constructionMonths[1].InvestmentVolume);
            Assert.AreEqual((decimal)7.0002, constructionMonths[1].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(30, constructionMonths[1].PercentePart);
            Assert.AreEqual(DateTime.Today.AddMonths(1), constructionMonths[1].Date);
        }

        [Test]
        public void Separate_CalendarWorkWithEstimateChapter10_SetCorrectMainTotalWork()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan);

            var totalWork = calendarPlanSeparator.MainCalendarPlan.CalendarPlanWorks.First(x => x.WorkName == "Итого:");
            var constructionMonths = totalWork.ConstructionPeriod.ConstructionMonths.ToArray();

            Assert.AreEqual((decimal)33.333, totalWork.TotalCost);
            Assert.AreEqual((decimal)32.222, totalWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(10, totalWork.EstimateChapter);

            Assert.AreEqual((decimal)25.9995, constructionMonths[0].InvestmentVolume);
            Assert.AreEqual((decimal)23.8886, constructionMonths[0].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(78, constructionMonths[0].PercentePart);
            Assert.AreEqual(DateTime.Today, constructionMonths[0].Date);

            Assert.AreEqual((decimal)7.3335, constructionMonths[1].InvestmentVolume);
            Assert.AreEqual((decimal)8.3334, constructionMonths[1].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(22, constructionMonths[1].PercentePart);
            Assert.AreEqual(DateTime.Today.AddMonths(1), constructionMonths[1].Date);
        }
    }
}
