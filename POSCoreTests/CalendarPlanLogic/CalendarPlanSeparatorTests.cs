using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using System;
using System.Collections.Generic;

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
            return new CalendarPlan(new List<CalendarWork>
            {
                new CalendarWork("ТРАССИРОВКА КАНАЛОВ (8,04 КМ)", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth>
                {
                    new ConstructionMonth(DateTime.Today, (decimal)2.222, (decimal)1.111, 1, 0)
                }), 1),
                new CalendarWork("ВОССТАНОВЛЕНИЕ ТРАСС АВТОДОРОГИ (3,813 КМ)", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth>
                {
                    new ConstructionMonth(DateTime.Today, (decimal)2.222, (decimal)1.111, 1, 0)
                }), 1),
                new CalendarWork("СИСТЕМА ОСУШЕНИЯ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth>
                {
                    new ConstructionMonth(DateTime.Today, (decimal)2.222, (decimal)1.111, 1, 0)
                }), 1),
                new CalendarWork("(НЕ БУДЕТ ВКЛЮЧЕНА)", (decimal)2.222, 0, new ConstructionPeriod(new List<ConstructionMonth>
                {
                    new ConstructionMonth(DateTime.Today, (decimal)2.222, (decimal)1.111, 1, 0)
                }), 1),
                new CalendarWork("БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth>
                {
                    new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                }), 2),
                new CalendarWork("СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth>
                {
                    new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                }), 3),
                new CalendarWork("ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth>
                {
                    new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                }), 4),
                new CalendarWork("ОБЪЕКТЫ ТРАНСПОРТА", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth>
                {
                    new ConstructionMonth(DateTime.Today, (decimal)1.555, (decimal)0.778, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)0.667, (decimal)0.333, (decimal)0.3, 1),
                }), 5),
                new CalendarWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 9,1Х0,93 - 8,463%", (decimal)2.222, (decimal)1.111, new ConstructionPeriod(new List<ConstructionMonth>
                {
                    new ConstructionMonth(DateTime.Today, (decimal)2.222, (decimal)1.111, 1, 0),
                }), 8),
                new CalendarWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", (decimal)33.333, (decimal)32.222, new ConstructionPeriod(new List<ConstructionMonth>
                {
                    new ConstructionMonth(DateTime.Today, (decimal)25.9995, (decimal)23.8886, (decimal)0.7, 0),
                    new ConstructionMonth(DateTime.Today.AddMonths(1), (decimal)7.3335, (decimal)8.3334, (decimal)0.3, 1),
                }), 10),
            }, DateTime.Today, 2);
        }

        [Test]
        public void Separate_CalendarWorksWithEstimateChapter1_SetCorrectPreparatoryWork()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan, new List<decimal>());

            var preparatoryWork = calendarPlanSeparator.PreparatoryCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Подготовка территории строительства");

            Assert.AreEqual((decimal)6.666, preparatoryWork.TotalCost);
            Assert.AreEqual((decimal)3.333, preparatoryWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(1, preparatoryWork.EstimateChapter);

            Assert.AreEqual((decimal)6.666, preparatoryWork.ConstructionPeriod.ConstructionMonths[0].InvestmentVolume);
            Assert.AreEqual((decimal)3.333, preparatoryWork.ConstructionPeriod.ConstructionMonths[0].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(1, preparatoryWork.ConstructionPeriod.ConstructionMonths[0].PercentPart);
            Assert.AreEqual(DateTime.Today, preparatoryWork.ConstructionPeriod.ConstructionMonths[0].Date);
            Assert.AreEqual(0, preparatoryWork.ConstructionPeriod.ConstructionMonths[0].CreationIndex);
        }

        [Test]
        public void Separate_CalendarWorkWithEstimateChapter8_SetCorrectTemporaryBuildingsWork()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan, new List<decimal>());

            var temporaryBuildingsWork = calendarPlanSeparator.PreparatoryCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Временные здания и сооружения");

            Assert.AreEqual((decimal)2.222, temporaryBuildingsWork.TotalCost);
            Assert.AreEqual((decimal)1.111, temporaryBuildingsWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(8, temporaryBuildingsWork.EstimateChapter);

            Assert.AreEqual((decimal)2.222, temporaryBuildingsWork.ConstructionPeriod.ConstructionMonths[0].InvestmentVolume);
            Assert.AreEqual((decimal)1.111, temporaryBuildingsWork.ConstructionPeriod.ConstructionMonths[0].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(1, temporaryBuildingsWork.ConstructionPeriod.ConstructionMonths[0].PercentPart);
            Assert.AreEqual(DateTime.Today, temporaryBuildingsWork.ConstructionPeriod.ConstructionMonths[0].Date);
            Assert.AreEqual(0, temporaryBuildingsWork.ConstructionPeriod.ConstructionMonths[0].CreationIndex);
        }

        [Test]
        public void Separate_CalendarWorksWithEstimateChapter8And1_SetCorrectPreparatoryTotalWork()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan, new List<decimal>());

            var totalWork = calendarPlanSeparator.PreparatoryCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Итого:");

            Assert.AreEqual((decimal)8.888, totalWork.TotalCost);
            Assert.AreEqual((decimal)4.444, totalWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(1, totalWork.EstimateChapter);

            Assert.AreEqual((decimal)8.888, totalWork.ConstructionPeriod.ConstructionMonths[0].InvestmentVolume);
            Assert.AreEqual((decimal)4.444, totalWork.ConstructionPeriod.ConstructionMonths[0].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(1, totalWork.ConstructionPeriod.ConstructionMonths[0].PercentPart);
            Assert.AreEqual(DateTime.Today, totalWork.ConstructionPeriod.ConstructionMonths[0].Date);
            Assert.AreEqual(0, totalWork.ConstructionPeriod.ConstructionMonths[0].CreationIndex);
        }

        [Test]
        public void Separate_CalendarWorksWithEstimateChapter8And1_SetCorrectMainOverallPreparatoryWork()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan, new List<decimal>());

            var overallPreparatoryWork = calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Работы, выполняемые в подготовительный период");

            Assert.AreEqual((decimal)8.888, overallPreparatoryWork.TotalCost);
            Assert.AreEqual((decimal)4.444, overallPreparatoryWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(2, overallPreparatoryWork.EstimateChapter);

            Assert.AreEqual((decimal)8.888, overallPreparatoryWork.ConstructionPeriod.ConstructionMonths[0].InvestmentVolume);
            Assert.AreEqual((decimal)4.444, overallPreparatoryWork.ConstructionPeriod.ConstructionMonths[0].ContructionAndInstallationWorksVolume);
            Assert.AreEqual(1, overallPreparatoryWork.ConstructionPeriod.ConstructionMonths[0].PercentPart);
            Assert.AreEqual(DateTime.Today, overallPreparatoryWork.ConstructionPeriod.ConstructionMonths[0].Date);
            Assert.AreEqual(0, overallPreparatoryWork.ConstructionPeriod.ConstructionMonths[0].CreationIndex);
        }

        [Test]
        public void Separate_CalendarWorksWithEstimateChapterEverythingExcept1And8_SetCorrectMainCalendarPlan()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan, new List<decimal>());

            var channelTracingWorkExistance = calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Exists(x => x.WorkName == "ТРАССИРОВКА КАНАЛОВ (8,04 КМ)");
            var restorationOfhighwaysWorkExistance = calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Exists(x => x.WorkName == "ВОССТАНОВЛЕНИЕ ТРАСС АВТОДОРОГИ (3,813 КМ)");
            var dehumidificationSystemWorkExistance = calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Exists(x => x.WorkName == "СИСТЕМА ОСУШЕНИЯ");
            var swampPreparatoryWorkExistance = calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Exists(x => x.WorkName == "БОЛОТНО-ПОДГОТОВИТЕЛЬНЫЕ РАБОТЫ");
            var systemOfFirePreventionMeasuresWorkExistance = calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Exists(x => x.WorkName == "СИСТЕМА ПРОТИВОПОЖАРНЫХ МЕРОПРИЯТИЙ");
            var offSiteNetworksWorkExistance = calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Exists(x => x.WorkName == "ВНЕПЛОЩАДОЧНЫЕ СЕТИ 10 КВТ");
            var transportFacilitiesWorkExistance = calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Exists(x => x.WorkName == "ОБЪЕКТЫ ТРАНСПОРТА");
            var temporaryBuildingsWorkExistance = calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Exists(x => x.WorkName == "ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 9,1Х0,93 - 8,463%");
            var totalWorkExistance = calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Exists(x => x.WorkName == "Итого:");

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
            calendarPlanSeparator.Separate(calendarPlan, new List<decimal> { (decimal)0.7, (decimal)0.3 });

            var otherExpensesWork = calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Прочие работы и затраты");

            Assert.AreEqual((decimal)15.557, otherExpensesWork.TotalCost);
            Assert.AreEqual((decimal)23.334, otherExpensesWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(9, otherExpensesWork.EstimateChapter);

            Assert.AreEqual((decimal)10.89, otherExpensesWork.ConstructionPeriod.ConstructionMonths[0].InvestmentVolume);
            Assert.AreEqual((decimal)16.334, otherExpensesWork.ConstructionPeriod.ConstructionMonths[0].ContructionAndInstallationWorksVolume);
            Assert.AreEqual((decimal)0.7, otherExpensesWork.ConstructionPeriod.ConstructionMonths[0].PercentPart);
            Assert.AreEqual(DateTime.Today, otherExpensesWork.ConstructionPeriod.ConstructionMonths[0].Date);
            Assert.AreEqual(0, otherExpensesWork.ConstructionPeriod.ConstructionMonths[0].CreationIndex);

            Assert.AreEqual((decimal)4.667, otherExpensesWork.ConstructionPeriod.ConstructionMonths[1].InvestmentVolume);
            Assert.AreEqual((decimal)7, otherExpensesWork.ConstructionPeriod.ConstructionMonths[1].ContructionAndInstallationWorksVolume);
            Assert.AreEqual((decimal)0.3, otherExpensesWork.ConstructionPeriod.ConstructionMonths[1].PercentPart);
            Assert.AreEqual(DateTime.Today.AddMonths(1), otherExpensesWork.ConstructionPeriod.ConstructionMonths[1].Date);
            Assert.AreEqual(1, otherExpensesWork.ConstructionPeriod.ConstructionMonths[1].CreationIndex);
        }

        [Test]
        public void Separate_CalendarWorkWithEstimateChapter10_SetCorrectMainTotalWork()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan, new List<decimal> { (decimal)0.7, (decimal)0.3 });

            var totalWork = calendarPlanSeparator.MainCalendarPlan.CalendarWorks.Find(x => x.WorkName == "Итого:");

            Assert.AreEqual((decimal)33.333, totalWork.TotalCost);
            Assert.AreEqual((decimal)32.222, totalWork.TotalCostIncludingContructionAndInstallationWorks);
            Assert.AreEqual(10, totalWork.EstimateChapter);

            Assert.AreEqual((decimal)25.998, totalWork.ConstructionPeriod.ConstructionMonths[0].InvestmentVolume);
            Assert.AreEqual((decimal)23.89, totalWork.ConstructionPeriod.ConstructionMonths[0].ContructionAndInstallationWorksVolume);
            Assert.AreEqual((decimal)0.78, decimal.Round(totalWork.ConstructionPeriod.ConstructionMonths[0].PercentPart, 2));
            Assert.AreEqual(DateTime.Today, totalWork.ConstructionPeriod.ConstructionMonths[0].Date);
            Assert.AreEqual(0, totalWork.ConstructionPeriod.ConstructionMonths[0].CreationIndex);

            Assert.AreEqual((decimal)7.335, totalWork.ConstructionPeriod.ConstructionMonths[1].InvestmentVolume);
            Assert.AreEqual((decimal)8.332, totalWork.ConstructionPeriod.ConstructionMonths[1].ContructionAndInstallationWorksVolume);
            Assert.AreEqual((decimal)0.22, decimal.Round(totalWork.ConstructionPeriod.ConstructionMonths[1].PercentPart, 2));
            Assert.AreEqual(DateTime.Today.AddMonths(1), totalWork.ConstructionPeriod.ConstructionMonths[1].Date);
            Assert.AreEqual(1, totalWork.ConstructionPeriod.ConstructionMonths[1].CreationIndex);
        }

        public void Separate_CalendarPlan_SetConstractionStartDateAndDuration()
        {
            var calendarPlan = CreateDefaultCalendarPlan();
            var calendarPlanSeparator = CreateDefaultCalendarPlanSeparator();
            calendarPlanSeparator.Separate(calendarPlan, new List<decimal> { (decimal)0.7, (decimal)0.3 });

            Assert.AreEqual(calendarPlan.ConstructionStartDate, calendarPlanSeparator.PreparatoryCalendarPlan.ConstructionStartDate);
            Assert.AreEqual(calendarPlan.ConstructionDuration, calendarPlanSeparator.PreparatoryCalendarPlan.ConstructionDuration);

            Assert.AreEqual(calendarPlan.ConstructionStartDate, calendarPlanSeparator.MainCalendarPlan.ConstructionStartDate);
            Assert.AreEqual(calendarPlan.ConstructionDuration, calendarPlanSeparator.MainCalendarPlan.ConstructionDuration);
        }
    }
}
