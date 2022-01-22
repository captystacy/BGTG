using System.IO;
using BGTG.POS.Tools.DurationTools.DurationByLCTool;
using NUnit.Framework;
using Xceed.Words.NET;

namespace BGTG.POS.Tools.Tests.DurationTools.DurationByLCTool
{
    public class DurationByLCWriterTests
    {
        private DurationByLCWriter _durationByLCWriter;

        private const string DurationByLCFileName = "durationByLC.docx";
        private const string DurationByLCTemplatesDirectory = @"..\..\..\DurationTools\DurationByLCTool\DurationByLCTemplates";

        private DurationByLC CreateDefaultDurationByLC()
        {
            return new DurationByLC(1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 11111, 22222, 33333, true, true);
        }

        [SetUp]
        public void SetUp()
        {
            _durationByLCWriter = new DurationByLCWriter();
        }

        [Test]
        public void Write_RoundingPlusAcceptancePlus_SaveCorrectDurationByLC()
        {
            var durationByLC = CreateDefaultDurationByLC();

            var savePath = Path.Combine(Directory.GetCurrentDirectory(), DurationByLCFileName);
            var templatePath = Path.Combine(DurationByLCTemplatesDirectory, "Rounding+Acceptance+Template.docx");

            _durationByLCWriter.Write(durationByLC, templatePath, savePath);

            using (var document = DocX.Load(savePath))
            {
                StringAssert.Contains(durationByLC.AcceptanceTime.ToString(), document.Text);
                StringAssert.Contains(durationByLC.Duration.ToString(), document.Text);
                StringAssert.Contains(durationByLC.TotalLaborCosts.ToString(), document.Text);
                StringAssert.Contains(durationByLC.NumberOfEmployees.ToString(), document.Text);
                StringAssert.Contains(durationByLC.NumberOfWorkingDays.ToString(), document.Text);
                StringAssert.Contains(durationByLC.PreparatoryPeriod.ToString(), document.Text);
                StringAssert.Contains(durationByLC.RoundedDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLC.Shift.ToString(), document.Text);
                StringAssert.Contains(durationByLC.TotalDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLC.WorkingDayDuration.ToString(), document.Text);
            }
        }

        [Test]
        public void Write_RoundingPlusAcceptanceMinus_SaveCorrectDurationByLC()
        {
            var durationByLC = CreateDefaultDurationByLC();

            var savePath = Path.Combine(Directory.GetCurrentDirectory(), DurationByLCFileName);
            var templatePath = Path.Combine(DurationByLCTemplatesDirectory, "Rounding+Acceptance-Template.docx");

            _durationByLCWriter.Write(durationByLC, templatePath, savePath);

            using (var document = DocX.Load(savePath))
            {
                StringAssert.DoesNotContain(durationByLC.AcceptanceTime.ToString(), document.Text);
                StringAssert.Contains(durationByLC.TotalDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLC.Duration.ToString(), document.Text);
                StringAssert.Contains(durationByLC.TotalLaborCosts.ToString(), document.Text);
                StringAssert.Contains(durationByLC.NumberOfEmployees.ToString(), document.Text);
                StringAssert.Contains(durationByLC.NumberOfWorkingDays.ToString(), document.Text);
                StringAssert.Contains(durationByLC.PreparatoryPeriod.ToString(), document.Text);
                StringAssert.Contains(durationByLC.RoundedDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLC.Shift.ToString(), document.Text);
                StringAssert.Contains(durationByLC.WorkingDayDuration.ToString(), document.Text);
            }
        }

        [Test]
        public void Write_RoundingMinusAcceptanceMinus_SaveCorrectDurationByLC()
        {
            var durationByLC = CreateDefaultDurationByLC();

            var savePath = Path.Combine(Directory.GetCurrentDirectory(), DurationByLCFileName);
            var templatePath = Path.Combine(DurationByLCTemplatesDirectory, "Rounding-Acceptance-Template.docx");

            _durationByLCWriter.Write(durationByLC, templatePath, savePath);

            using (var document = DocX.Load(savePath))
            {
                StringAssert.DoesNotContain(durationByLC.AcceptanceTime.ToString(), document.Text);
                StringAssert.DoesNotContain(durationByLC.TotalDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLC.RoundedDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLC.Duration.ToString(), document.Text);
                StringAssert.Contains(durationByLC.TotalLaborCosts.ToString(), document.Text);
                StringAssert.Contains(durationByLC.NumberOfEmployees.ToString(), document.Text);
                StringAssert.Contains(durationByLC.NumberOfWorkingDays.ToString(), document.Text);
                StringAssert.Contains(durationByLC.PreparatoryPeriod.ToString(), document.Text);
                StringAssert.Contains(durationByLC.Shift.ToString(), document.Text);
                StringAssert.Contains(durationByLC.WorkingDayDuration.ToString(), document.Text);
            }
        }

        [Test]
        public void Write_RoundingMinusAcceptancePlus_SaveCorrectDurationByLC()
        {
            var durationByLC = CreateDefaultDurationByLC();

            var savePath = Path.Combine(Directory.GetCurrentDirectory(), DurationByLCFileName);
            var templatePath = Path.Combine(DurationByLCTemplatesDirectory, "Rounding-Acceptance+Template.docx");

            _durationByLCWriter.Write(durationByLC, templatePath, savePath);

            using (var document = DocX.Load(savePath))
            {
                StringAssert.Contains(durationByLC.TotalDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLC.AcceptanceTime.ToString(), document.Text);
                StringAssert.Contains(durationByLC.RoundedDuration.ToString(), document.Text);
                StringAssert.Contains(durationByLC.Duration.ToString(), document.Text);
                StringAssert.Contains(durationByLC.TotalLaborCosts.ToString(), document.Text);
                StringAssert.Contains(durationByLC.NumberOfEmployees.ToString(), document.Text);
                StringAssert.Contains(durationByLC.NumberOfWorkingDays.ToString(), document.Text);
                StringAssert.Contains(durationByLC.PreparatoryPeriod.ToString(), document.Text);
                StringAssert.Contains(durationByLC.Shift.ToString(), document.Text);
                StringAssert.Contains(durationByLC.WorkingDayDuration.ToString(), document.Text);
            }
        }
    }
}
