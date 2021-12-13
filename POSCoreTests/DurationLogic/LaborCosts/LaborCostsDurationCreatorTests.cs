using NUnit.Framework;
using POSCore.DurationLogic.LaborCosts;

namespace POSCoreTests.DurationLogic.LaborCosts
{
    public class LaborCostsDurationCreatorTests
    {
        private LaborCostsDurationCreator CreateDefaultLaborCostsDurationCreator()
        {
            return new LaborCostsDurationCreator();
        }

        [Test]
        public void Create_ArgsWithoutRounding_CreateCorrectLaborCostsDuration()
        {
            var laborCostsDurationCreator = CreateDefaultLaborCostsDurationCreator();
            var laborCosts = 43M;
            var workingDayDuration = 8M;
            var shift = 1.5M;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = true;

            var laborCostsDuration = laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, true);

            Assert.AreEqual(laborCosts, laborCostsDuration.LaborCosts);
            Assert.AreEqual(workingDayDuration, laborCostsDuration.WorkingDayDuration);
            Assert.AreEqual(shift, laborCostsDuration.Shift);
            Assert.AreEqual(numberOfWorkingDaysInMonth, laborCostsDuration.NumberOfWorkingDays);
            Assert.AreEqual(numberOfEmployees, laborCostsDuration.NumberOfEmployees);

            Assert.AreEqual(0.04M, laborCostsDuration.Duration);
            Assert.AreEqual(0.1M, laborCostsDuration.RoundedDuration);
            Assert.AreEqual(0.6M, laborCostsDuration.TotalDuration);
            Assert.AreEqual(0.06M, laborCostsDuration.PreparatoryPeriod);
            Assert.AreEqual(acceptanceTimeIncluded, laborCostsDuration.AcceptanceTimeIncluded);
            Assert.AreEqual(0.5M, laborCostsDuration.AcceptanceTime);
            Assert.AreEqual(false, laborCostsDuration.RoundingIncluded);
        }

        [Test]
        public void Create_390LaborCosts4People_CreateCorrectLaborCostsDuration()
        {
            var laborCostsDurationCreator = CreateDefaultLaborCostsDurationCreator();
            var laborCosts = 390M;
            var workingDayDuration = 8M;
            var shift = 1.5M;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = true;

            var laborCostsDuration = laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded);

            Assert.AreEqual(laborCosts, laborCostsDuration.LaborCosts);
            Assert.AreEqual(workingDayDuration, laborCostsDuration.WorkingDayDuration);
            Assert.AreEqual(shift, laborCostsDuration.Shift);
            Assert.AreEqual(numberOfWorkingDaysInMonth, laborCostsDuration.NumberOfWorkingDays);
            Assert.AreEqual(numberOfEmployees, laborCostsDuration.NumberOfEmployees);

            Assert.AreEqual(0.38M, laborCostsDuration.Duration);
            Assert.AreEqual(0.5M, laborCostsDuration.RoundedDuration);
            Assert.AreEqual(1, laborCostsDuration.TotalDuration);
            Assert.AreEqual(0.1M, laborCostsDuration.PreparatoryPeriod);
            Assert.AreEqual(acceptanceTimeIncluded, laborCostsDuration.AcceptanceTimeIncluded);
            Assert.AreEqual(0.5M, laborCostsDuration.AcceptanceTime);
            Assert.AreEqual(true, laborCostsDuration.RoundingIncluded);
        }

        [Test]
        public void Create_1524LaborCosts8People_CreateCorrectLaborCostsDuration()
        {
            var laborCostsDurationCreator = CreateDefaultLaborCostsDurationCreator();
            var laborCosts = 1524M;
            var workingDayDuration = 8M;
            var shift = 1.5M;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 8;
            var acceptanceTimeIncluded = true;

            var laborCostsDuration = laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded);

            Assert.AreEqual(laborCosts, laborCostsDuration.LaborCosts);
            Assert.AreEqual(workingDayDuration, laborCostsDuration.WorkingDayDuration);
            Assert.AreEqual(shift, laborCostsDuration.Shift);
            Assert.AreEqual(numberOfWorkingDaysInMonth, laborCostsDuration.NumberOfWorkingDays);
            Assert.AreEqual(numberOfEmployees, laborCostsDuration.NumberOfEmployees);

            Assert.AreEqual(0.74M, laborCostsDuration.Duration);
            Assert.AreEqual(0.5M, laborCostsDuration.RoundedDuration);
            Assert.AreEqual(1, laborCostsDuration.TotalDuration);
            Assert.AreEqual(0.1M, laborCostsDuration.PreparatoryPeriod);
            Assert.AreEqual(acceptanceTimeIncluded, laborCostsDuration.AcceptanceTimeIncluded);
            Assert.AreEqual(0.5M, laborCostsDuration.AcceptanceTime);
            Assert.AreEqual(true, laborCostsDuration.RoundingIncluded);
        }

        [Test]
        public void Create_601LaborCosts4People_CreateCorrectLaborCostsDuration()
        {
            var laborCostsDurationCreator = CreateDefaultLaborCostsDurationCreator();
            var laborCosts = 601M;
            var workingDayDuration = 8M;
            var shift = 1.5M;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = true;

            var laborCostsDuration = laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded);

            Assert.AreEqual(laborCosts, laborCostsDuration.LaborCosts);
            Assert.AreEqual(workingDayDuration, laborCostsDuration.WorkingDayDuration);
            Assert.AreEqual(shift, laborCostsDuration.Shift);
            Assert.AreEqual(numberOfWorkingDaysInMonth, laborCostsDuration.NumberOfWorkingDays);
            Assert.AreEqual(numberOfEmployees, laborCostsDuration.NumberOfEmployees);

            Assert.AreEqual(0.58M, laborCostsDuration.Duration);
            Assert.AreEqual(0.5M, laborCostsDuration.RoundedDuration);
            Assert.AreEqual(1M, laborCostsDuration.TotalDuration);
            Assert.AreEqual(0.1M, laborCostsDuration.PreparatoryPeriod);
            Assert.AreEqual(acceptanceTimeIncluded, laborCostsDuration.AcceptanceTimeIncluded);
            Assert.AreEqual(0.5M, laborCostsDuration.AcceptanceTime);
            Assert.AreEqual(true, laborCostsDuration.RoundingIncluded);
        }

        [Test]
        public void Create_251LaborCosts4People_CreateCorrectLaborCostsDuration()
        {
            var laborCostsDurationCreator = CreateDefaultLaborCostsDurationCreator();
            var laborCosts = 251M;
            var workingDayDuration = 8M;
            var shift = 1.5M;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = true;

            var laborCostsDuration = laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded);

            Assert.AreEqual(laborCosts, laborCostsDuration.LaborCosts);
            Assert.AreEqual(workingDayDuration, laborCostsDuration.WorkingDayDuration);
            Assert.AreEqual(shift, laborCostsDuration.Shift);
            Assert.AreEqual(numberOfWorkingDaysInMonth, laborCostsDuration.NumberOfWorkingDays);
            Assert.AreEqual(numberOfEmployees, laborCostsDuration.NumberOfEmployees);

            Assert.AreEqual(0.24M, laborCostsDuration.Duration);
            Assert.AreEqual(0.3M, laborCostsDuration.RoundedDuration);
            Assert.AreEqual(0.8M, laborCostsDuration.TotalDuration);
            Assert.AreEqual(0.08M, laborCostsDuration.PreparatoryPeriod);
            Assert.AreEqual(acceptanceTimeIncluded, laborCostsDuration.AcceptanceTimeIncluded);
            Assert.AreEqual(0.5M, laborCostsDuration.AcceptanceTime);
            Assert.AreEqual(false, laborCostsDuration.RoundingIncluded);
        }

        [Test]
        public void Create_Borehole107_CreateCorrectLaborCostsDuration()
        {
            var laborCostsDurationCreator = CreateDefaultLaborCostsDurationCreator();
            var laborCosts = 2671.67M;
            var workingDayDuration = 11M;
            var shift = 1M;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = false;

            var laborCostsDuration = laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded);

            Assert.AreEqual(laborCosts, laborCostsDuration.LaborCosts);
            Assert.AreEqual(workingDayDuration, laborCostsDuration.WorkingDayDuration);
            Assert.AreEqual(shift, laborCostsDuration.Shift);
            Assert.AreEqual(numberOfWorkingDaysInMonth, laborCostsDuration.NumberOfWorkingDays);
            Assert.AreEqual(numberOfEmployees, laborCostsDuration.NumberOfEmployees);

            Assert.AreEqual(2.82M, laborCostsDuration.Duration);
            Assert.AreEqual(3M, laborCostsDuration.RoundedDuration);
            Assert.AreEqual(3M, laborCostsDuration.TotalDuration);
            Assert.AreEqual(0.3M, laborCostsDuration.PreparatoryPeriod);
            Assert.AreEqual(acceptanceTimeIncluded, laborCostsDuration.AcceptanceTimeIncluded);
            Assert.AreEqual(0M, laborCostsDuration.AcceptanceTime);
            Assert.AreEqual(true, laborCostsDuration.RoundingIncluded);
        }

        [Test]
        public void Create_Borehole108_CreateCorrectLaborCostsDuration()
        {
            var laborCostsDurationCreator = CreateDefaultLaborCostsDurationCreator();
            var laborCosts = 4328.65M;
            var workingDayDuration = 11;
            var shift = 1;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = false;

            var laborCostsDuration = laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded);

            Assert.AreEqual(laborCosts, laborCostsDuration.LaborCosts);
            Assert.AreEqual(workingDayDuration, laborCostsDuration.WorkingDayDuration);
            Assert.AreEqual(shift, laborCostsDuration.Shift);
            Assert.AreEqual(numberOfWorkingDaysInMonth, laborCostsDuration.NumberOfWorkingDays);
            Assert.AreEqual(numberOfEmployees, laborCostsDuration.NumberOfEmployees);

            Assert.AreEqual(4.58M, laborCostsDuration.Duration);
            Assert.AreEqual(4.5, laborCostsDuration.RoundedDuration);
            Assert.AreEqual(4.5, laborCostsDuration.TotalDuration);
            Assert.AreEqual(0.4, laborCostsDuration.PreparatoryPeriod);
            Assert.AreEqual(acceptanceTimeIncluded, laborCostsDuration.AcceptanceTimeIncluded);
            Assert.AreEqual(0, laborCostsDuration.AcceptanceTime);
            Assert.AreEqual(true, laborCostsDuration.RoundingIncluded);
        }

        [Test]
        public void Create_Borehole110_CreateCorrectLaborCostsDuration()
        {
            var laborCostsDurationCreator = CreateDefaultLaborCostsDurationCreator();
            var laborCosts = 2850.11M;
            var workingDayDuration = 11;
            var shift = 1;
            var numberOfWorkingDaysInMonth = 21.5M;
            var numberOfEmployees = 4;
            var acceptanceTimeIncluded = false;

            var laborCostsDuration = laborCostsDurationCreator.Create(laborCosts, workingDayDuration, shift, numberOfWorkingDaysInMonth, numberOfEmployees, acceptanceTimeIncluded);

            Assert.AreEqual(laborCosts, laborCostsDuration.LaborCosts);
            Assert.AreEqual(workingDayDuration, laborCostsDuration.WorkingDayDuration);
            Assert.AreEqual(shift, laborCostsDuration.Shift);
            Assert.AreEqual(numberOfWorkingDaysInMonth, laborCostsDuration.NumberOfWorkingDays);
            Assert.AreEqual(numberOfEmployees, laborCostsDuration.NumberOfEmployees);

            Assert.AreEqual(3.01M, laborCostsDuration.Duration);
            Assert.AreEqual(3, laborCostsDuration.RoundedDuration);
            Assert.AreEqual(3, laborCostsDuration.TotalDuration);
            Assert.AreEqual(0.3, laborCostsDuration.PreparatoryPeriod);
            Assert.AreEqual(acceptanceTimeIncluded, laborCostsDuration.AcceptanceTimeIncluded);
            Assert.AreEqual(0, laborCostsDuration.AcceptanceTime);
            Assert.AreEqual(true, laborCostsDuration.RoundingIncluded);
        }
    }
}
