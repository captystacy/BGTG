using NUnit.Framework;
using POSCore.DurationLogic.LaborCosts;
using System.IO;
using Xceed.Words.NET;

namespace POSCoreTests.DurationLogic.LaborCosts
{
    public class LaborCostsDurationWriterTests
    {
        private LaborCostsDurationWriter CreateDefaultLaborCostsDurationWriter()
        {
            return new LaborCostsDurationWriter();
        }

        private LaborCostsDuration CreateDefaultLaborCostsDuration()
        {
            return new LaborCostsDuration(1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, true, true);
        }

        [Test]
        public void Write_WithRoundingAndAcceptanceTime_SaveCorrectLaborCostsDuration()
        {
            var laborCostsDurationWriter = CreateDefaultLaborCostsDurationWriter();
            var laborCostsDuration = CreateDefaultLaborCostsDuration();
            var templatePath = @"..\..\..\DurationLogic\LaborCosts\LaborCostsTemplates\LaborCostsDurationWithRoundingAndWithAcceptanceTimeTemplate.docx";

            var laborCostsDurationFileName = "LaborCostsDuration.docx";
            laborCostsDurationWriter.Write(laborCostsDuration, templatePath, Directory.GetCurrentDirectory(), laborCostsDurationFileName);

            var laborCostsDurationPath = Path.Combine(Directory.GetCurrentDirectory(), laborCostsDurationFileName);
            using (var document = DocX.Load(laborCostsDurationPath))
            {
                Assert.True(document.Text.Contains(laborCostsDuration.AcceptanceTime.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.Duration.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.LaborCosts.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.NumberOfEmployees.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.NumberOfWorkingDays.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.PreparatoryPeriod.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.RoundedDuration.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.Shift.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.TotalDuration.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.WorkingDayDuration.ToString()));
            }
        }

        [Test]
        public void Write_WithRoundingAndWithoutAcceptanceTime_SaveCorrectLaborCostsDuration()
        {
            var laborCostsDurationWriter = CreateDefaultLaborCostsDurationWriter();
            var laborCostsDuration = CreateDefaultLaborCostsDuration();
            var templatePath = @"..\..\..\DurationLogic\LaborCosts\LaborCostsTemplates\LaborCostsDurationWithRoundingAndWithoutAcceptanceTimeTemplate.docx";

            var laborCostsDurationFileName = "LaborCostsDuration.docx";
            laborCostsDurationWriter.Write(laborCostsDuration, templatePath, Directory.GetCurrentDirectory(), laborCostsDurationFileName);

            var laborCostsDurationPath = Path.Combine(Directory.GetCurrentDirectory(), laborCostsDurationFileName);
            using (var document = DocX.Load(laborCostsDurationPath))
            {
                Assert.True(document.Text.Contains(laborCostsDuration.Duration.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.LaborCosts.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.NumberOfEmployees.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.NumberOfWorkingDays.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.PreparatoryPeriod.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.RoundedDuration.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.Shift.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.TotalDuration.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.WorkingDayDuration.ToString()));
            }
        }

        [Test]
        public void Write_WithoutRoundingAndAcceptanceTime_SaveCorrectLaborCostsDuration()
        {
            var laborCostsDurationWriter = CreateDefaultLaborCostsDurationWriter();
            var laborCostsDuration = CreateDefaultLaborCostsDuration();
            var templatePath = @"..\..\..\DurationLogic\LaborCosts\LaborCostsTemplates\LaborCostsDurationWithoutRoundingAndWithoutAcceptanceTimeTemplate.docx";

            var laborCostsDurationFileName = "LaborCostsDuration.docx";
            laborCostsDurationWriter.Write(laborCostsDuration, templatePath, Directory.GetCurrentDirectory(), laborCostsDurationFileName);

            var laborCostsDurationPath = Path.Combine(Directory.GetCurrentDirectory(), laborCostsDurationFileName);
            using (var document = DocX.Load(laborCostsDurationPath))
            {
                Assert.True(document.Text.Contains(laborCostsDuration.Duration.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.LaborCosts.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.NumberOfEmployees.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.NumberOfWorkingDays.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.PreparatoryPeriod.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.RoundedDuration.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.Shift.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.WorkingDayDuration.ToString()));
            }
        }

        [Test]
        public void Write_WithoutRoundingAndWithAcceptanceTime_SaveCorrectLaborCostsDuration()
        {
            var laborCostsDurationWriter = CreateDefaultLaborCostsDurationWriter();
            var laborCostsDuration = CreateDefaultLaborCostsDuration();
            var templatePath = @"..\..\..\DurationLogic\LaborCosts\LaborCostsTemplates\LaborCostsDurationWithoutRoundingAndWithAcceptanceTimeTemplate.docx";

            var laborCostsDurationFileName = "LaborCostsDuration.docx";
            laborCostsDurationWriter.Write(laborCostsDuration, templatePath, Directory.GetCurrentDirectory(), laborCostsDurationFileName);

            var laborCostsDurationPath = Path.Combine(Directory.GetCurrentDirectory(), laborCostsDurationFileName);
            using (var document = DocX.Load(laborCostsDurationPath))
            {
                Assert.True(document.Text.Contains(laborCostsDuration.AcceptanceTime.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.Duration.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.LaborCosts.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.NumberOfEmployees.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.NumberOfWorkingDays.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.PreparatoryPeriod.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.RoundedDuration.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.Shift.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.TotalDuration.ToString()));
                Assert.True(document.Text.Contains(laborCostsDuration.WorkingDayDuration.ToString()));
            }
        }
    }
}
