using NUnit.Framework;
using POS.LaborCostsDurationLogic;

namespace POSTests.LaborCostsDurationLogic
{
    public class LaborCostsDurationCreatorTests
    {
        private LaborCostsDurationCreator _laborCostsDurationCreator;

        [SetUp]
        public void SetUp()
        {
            _laborCostsDurationCreator = new LaborCostsDurationCreator();
        }

        [Test]
        public void Create_ArgsWithoutRounding_CreateCorrectLaborCostsDuration()
        {
            var laborCosts = 43M;
            var workingDayDuration = 8M;
            var shift = 1.5M;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = true;
            var duration = 0.04M;
            var totalDuration = 0.6M;
            var preparatoryPeriod = 0.06M;
            var roundedDuration = 0.1M;
            var acceptanceTime = 0.5M;
            var roundingIncluded = false;
            var technologicalLaborCosts = 0;

            var expectedLaborCostsDuration = new LaborCostsDuration(duration, laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth,
                numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime, acceptanceTimeIncluded, roundingIncluded, technologicalLaborCosts);

            var actualLaborCostsDuration = _laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, true, technologicalLaborCosts);

            Assert.AreEqual(expectedLaborCostsDuration, actualLaborCostsDuration);
        }

        [Test]
        public void Create_390LaborCosts4People_CreateCorrectLaborCostsDuration()
        {
            var laborCosts = 390M;
            var workingDayDuration = 8M;
            var shift = 1.5M;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = true;
            var duration = 0.38M;
            var totalDuration = 1M;
            var preparatoryPeriod = 0.1M;
            var roundedDuration = 0.5M;
            var acceptanceTime = 0.5M;
            var roundingIncluded = true;
            var technologicalLaborCosts = 0;

            var expectedLaborCostsDuration = new LaborCostsDuration(duration, laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth,
                numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime, acceptanceTimeIncluded, roundingIncluded, technologicalLaborCosts);

            var actualLaborCostsDuration = _laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded, technologicalLaborCosts);

            Assert.AreEqual(expectedLaborCostsDuration, actualLaborCostsDuration);
        }

        [Test]
        public void Create_1524LaborCosts8People_CreateCorrectLaborCostsDuration()
        {
            var laborCosts = 1524M;
            var workingDayDuration = 8M;
            var shift = 1.5M;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 8;
            var acceptanceTimeIncluded = true;
            var duration = 0.74M;
            var totalDuration = 1M;
            var preparatoryPeriod = 0.1M;
            var roundedDuration = 0.5M;
            var acceptanceTime = 0.5M;
            var roundingIncluded = true;
            var technologicalLaborCosts = 0;

            var expectedLaborCostsDuration = new LaborCostsDuration(duration, laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth,
                numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime, acceptanceTimeIncluded, roundingIncluded, technologicalLaborCosts);

            var actualLaborCostsDuration = _laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded, technologicalLaborCosts);

            Assert.AreEqual(expectedLaborCostsDuration, actualLaborCostsDuration);
        }

        [Test]
        public void Create_601LaborCosts4People_CreateCorrectLaborCostsDuration()
        {
            var laborCosts = 601M;
            var workingDayDuration = 8M;
            var shift = 1.5M;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = true;
            var duration = 0.58M;
            var totalDuration = 1M;
            var preparatoryPeriod = 0.1M;
            var roundedDuration = 0.5M;
            var acceptanceTime = 0.5M;
            var roundingIncluded = true;
            var technologicalLaborCosts = 0;

            var expectedLaborCostsDuration = new LaborCostsDuration(duration, laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth,
                numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime, acceptanceTimeIncluded, roundingIncluded, technologicalLaborCosts);

            var actualLaborCostsDuration = _laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded, technologicalLaborCosts);

            Assert.AreEqual(expectedLaborCostsDuration, actualLaborCostsDuration);
        }

        [Test]
        public void Create_251LaborCosts4People_CreateCorrectLaborCostsDuration()
        {
            var laborCosts = 251M;
            var workingDayDuration = 8M;
            var shift = 1.5M;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = true;
            var duration = 0.24M;
            var totalDuration = 0.8M;
            var preparatoryPeriod = 0.08M;
            var roundedDuration = 0.3M;
            var acceptanceTime = 0.5M;
            var roundingIncluded = false;
            var technologicalLaborCosts = 0;

            var expectedLaborCostsDuration = new LaborCostsDuration(duration, laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth,
                numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime, acceptanceTimeIncluded, roundingIncluded, technologicalLaborCosts);

            var actualLaborCostsDuration = _laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded, technologicalLaborCosts);

            Assert.AreEqual(expectedLaborCostsDuration, actualLaborCostsDuration);
        }

        [Test]
        public void Create_Borehole107_CreateCorrectLaborCostsDuration()
        {
            var laborCosts = 2671.67M;
            var workingDayDuration = 11M;
            var shift = 1M;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = false;
            var duration = 2.82M;
            var totalDuration = 3M;
            var preparatoryPeriod = 0.3M;
            var roundedDuration = 3M;
            var acceptanceTime = 0M;
            var roundingIncluded = true;
            var technologicalLaborCosts = 0;

            var expectedLaborCostsDuration = new LaborCostsDuration(duration, laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth,
                numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime, acceptanceTimeIncluded, roundingIncluded, technologicalLaborCosts);

            var actualLaborCostsDuration = _laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded, technologicalLaborCosts);

            Assert.AreEqual(expectedLaborCostsDuration, actualLaborCostsDuration);
        }

        [Test]
        public void Create_Borehole108_CreateCorrectLaborCostsDuration()
        {
            var laborCosts = 4328.65M;
            var workingDayDuration = 11;
            var shift = 1;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = false;
            var duration = 4.58M;
            var totalDuration = 4.5M;
            var preparatoryPeriod = 0.4M;
            var roundedDuration = 4.5M;
            var acceptanceTime = 0M;
            var roundingIncluded = true;
            var technologicalLaborCosts = 0;

            var expectedLaborCostsDuration = new LaborCostsDuration(duration, laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth,
                numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime, acceptanceTimeIncluded, roundingIncluded, technologicalLaborCosts);

            var actualLaborCostsDuration = _laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded, technologicalLaborCosts);

            Assert.AreEqual(expectedLaborCostsDuration, actualLaborCostsDuration);
        }

        [Test]
        public void Create_Borehole110_CreateCorrectLaborCostsDuration()
        {
            var laborCosts = 2850.11M;
            var workingDayDuration = 11;
            var shift = 1;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = false;
            var duration = 3.01M;
            var totalDuration = 3M;
            var preparatoryPeriod = 0.3M;
            var roundedDuration = 3M;
            var acceptanceTime = 0M;
            var roundingIncluded = true;
            var technologicalLaborCosts = 0;

            var expectedLaborCostsDuration = new LaborCostsDuration(duration, laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth,
                numberOfEmployees, totalDuration, preparatoryPeriod, roundedDuration, acceptanceTime, acceptanceTimeIncluded, roundingIncluded, technologicalLaborCosts);

            var actualLaborCostsDuration = _laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded, technologicalLaborCosts);
;
            Assert.AreEqual(expectedLaborCostsDuration, actualLaborCostsDuration);
        }
    }
}
