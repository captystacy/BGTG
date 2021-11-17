using NUnit.Framework;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;

namespace POSCoreTests.CalendarWorkConverterTests
{
    public class ConvertShould
    {
        private ICalendarWorkConverter _calendarWorkConverter;

        [SetUp]
        public void SetUp()
        {
            _calendarWorkConverter = new CalendarWorkConverter();
        }

        [Test]
        public void SetWorkNameFromEstimateWorkName()
        {
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 0);

            var calendarWork = _calendarWorkConverter.Convert(estimateWork, new ConstructionPeriod());

            Assert.AreEqual(estimateWork.WorkName, calendarWork.WorkName);
        }

        [Test]
        public void SetTotalCostFromEstimateTotalСost()
        {
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 0);

            var calendarWork = _calendarWorkConverter.Convert(estimateWork, new ConstructionPeriod());

            Assert.AreEqual(estimateWork.TotalCost, calendarWork.TotalCost);
        }

        [Test]
        public void SetTotalCostIncludingContructionAndInstallationWorks_ToTotalCostMinusEquipmentCostAndOtherProductsCost()
        {
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 2.027, 1.541, 55.464);

            var calendarWork = _calendarWorkConverter.Convert(estimateWork, new ConstructionPeriod());

            Assert.AreEqual(estimateWork.TotalCost - estimateWork.EquipmentCost - estimateWork.OtherProductsCost, calendarWork.TotalCostIncludingContructionAndInstallationWorks);
        }

        [Test]
        public void SetConstructioPerod()
        {
            var estimateWork = new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 0);
            var constructionPeriod = new ConstructionPeriod();

            var calendarWork = _calendarWorkConverter.Convert(estimateWork, constructionPeriod);

            Assert.AreEqual(constructionPeriod, calendarWork.ConstructionPeriod);
        }
    }
}
