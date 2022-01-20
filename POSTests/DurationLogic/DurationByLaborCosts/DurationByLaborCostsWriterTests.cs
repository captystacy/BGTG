using System.IO;
using NUnit.Framework;
using POS.DurationLogic.DurationByLaborCosts;
using Xceed.Words.NET;

namespace POSTests.DurationLogic.DurationByLaborCosts
{
    public class DurationByLaborCostsWriterTests
    {
        private DurationByLaborCostsWriter _durationByLaborCostsWriter;

        private const string DurationByLaborCostsFileName = "DurationByLaborCosts.docx";
        private const string DurationByLaborCostsTemplatesDirectory = @"..\..\..\DurationLogic\DurationByLaborCosts\DurationByLaborCostsTemplates";

        private POS.DurationLogic.DurationByLaborCosts.DurationByLaborCosts CreateDefaultDurationByLaborCosts()
        {
            return new POS.DurationLogic.DurationByLaborCosts.DurationByLaborCosts(1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 11111, 22222, 33333, true, true);
        }

        [SetUp]
        public void SetUp()
        {
            _durationByLaborCostsWriter = new DurationByLaborCostsWriter();
        }

        [Test]
        public void Write_RoundingPlusAcceptancePlus_SaveCorrectDurationByLaborCosts()
        {
            var durationByLaborCosts = CreateDefaultDurationByLaborCosts();

            var savePath = Path.Combine(Directory.GetCurrentDirectory(), DurationByLaborCostsFileName);
            var templatePath = Path.Combine(DurationByLaborCostsTemplatesDirectory, "Rounding+Acceptance+Template.docx");

            _durationByLaborCostsWriter.Write(durationByLaborCosts, templatePath, savePath);

            using (var document = DocX.Load(savePath))
            {
                StringAssert.Contains(durationByLaborCosts.AcceptanceTime.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.Duration.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.TotalLaborCosts.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.NumberOfEmployees.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.NumberOfWorkingDays.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.PreparatoryPeriod.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.RoundedDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.Shift.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.TotalDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.WorkingDayDuration.ToString(), document.Text);
            }
        }

        [Test]
        public void Write_RoundingPlusAcceptanceMinus_SaveCorrectDurationByLaborCosts()
        {
            var durationByLaborCosts = CreateDefaultDurationByLaborCosts();

            var savePath = Path.Combine(Directory.GetCurrentDirectory(), DurationByLaborCostsFileName);
            var templatePath = Path.Combine(DurationByLaborCostsTemplatesDirectory, "Rounding+Acceptance-Template.docx");

            _durationByLaborCostsWriter.Write(durationByLaborCosts, templatePath, savePath);

            using (var document = DocX.Load(savePath))
            {
                StringAssert.DoesNotContain(durationByLaborCosts.AcceptanceTime.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.TotalDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.Duration.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.TotalLaborCosts.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.NumberOfEmployees.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.NumberOfWorkingDays.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.PreparatoryPeriod.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.RoundedDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.Shift.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.WorkingDayDuration.ToString(), document.Text);
            }
        }

        [Test]
        public void Write_RoundingMinusAcceptanceMinus_SaveCorrectDurationByLaborCosts()
        {
            var durationByLaborCosts = CreateDefaultDurationByLaborCosts();

            var savePath = Path.Combine(Directory.GetCurrentDirectory(), DurationByLaborCostsFileName);
            var templatePath = Path.Combine(DurationByLaborCostsTemplatesDirectory, "Rounding-Acceptance-Template.docx");

            _durationByLaborCostsWriter.Write(durationByLaborCosts, templatePath, savePath);

            using (var document = DocX.Load(savePath))
            {
                StringAssert.DoesNotContain(durationByLaborCosts.AcceptanceTime.ToString(), document.Text);
                StringAssert.DoesNotContain(durationByLaborCosts.TotalDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.RoundedDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.Duration.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.TotalLaborCosts.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.NumberOfEmployees.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.NumberOfWorkingDays.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.PreparatoryPeriod.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.Shift.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.WorkingDayDuration.ToString(), document.Text);
            }
        }

        [Test]
        public void Write_RoundingMinusAcceptancePlus_SaveCorrectDurationByLaborCosts()
        {
            var durationByLaborCosts = CreateDefaultDurationByLaborCosts();

            var savePath = Path.Combine(Directory.GetCurrentDirectory(), DurationByLaborCostsFileName);
            var templatePath = Path.Combine(DurationByLaborCostsTemplatesDirectory, "Rounding-Acceptance+Template.docx");

            _durationByLaborCostsWriter.Write(durationByLaborCosts, templatePath, savePath);

            using (var document = DocX.Load(savePath))
            {
                StringAssert.Contains(durationByLaborCosts.TotalDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.AcceptanceTime.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.RoundedDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.Duration.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.TotalLaborCosts.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.NumberOfEmployees.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.NumberOfWorkingDays.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.PreparatoryPeriod.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.Shift.ToString(), document.Text);
                StringAssert.Contains(durationByLaborCosts.WorkingDayDuration.ToString(), document.Text);
            }
        }
    }
}
