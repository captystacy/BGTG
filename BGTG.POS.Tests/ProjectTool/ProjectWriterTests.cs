using System.IO;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.POS.ProjectTool;
using BGTG.POS.Tests.CalendarPlanTool;
using NUnit.Framework;
using Xceed.Words.NET;

namespace BGTG.POS.Tests.ProjectTool
{
    public class ProjectWriterTests
    {
        private ECPProjectWriter _ecpProjectWriter;

        private const string ECPProjectFileName = "Project.docx";
        private const string ECPProjectTemplatesDirectory = @"..\..\..\ProjectTool\ProjectTemplates\ECP\Saiko\Kapitan\Employees4";

        [SetUp]
        public void SetUp()
        {
            _ecpProjectWriter = new ECPProjectWriter();
        }

        [Test]
        public void Write_ConstructionObject548_DoesNotContainPatterns()
        {
            var objectCipher = "5.5-20.548";
            var constructionStartDate = CalendarPlanSource.CalendarPlan548.ConstructionStartDate;
            var durationByLC = new DurationByLC(0.02M, 16, 16, 0, 8, 1.5M, 21.5M, 4, 0.6M, 0.06M, 0.1M, 0.5M, true, false);
            var durationByLCPath = @"..\..\..\ProjectTool\Source\DurationByLC.docx";
            var calendarPlanPath = @"..\..\..\ProjectTool\Source\CalendarPlan.docx";
            var energyAndWaterPath = @"..\..\..\ProjectTool\Source\EnergyAndWater.docx";
            var templateFileName = "HouseholdTown+.docx";
            var templatePath = Path.Combine(ECPProjectTemplatesDirectory, templateFileName);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), ECPProjectFileName);

            _ecpProjectWriter.Write(objectCipher, durationByLC, constructionStartDate, durationByLCPath, calendarPlanPath, energyAndWaterPath, templatePath, savePath);

            using var document = DocX.Load(savePath);
            StringAssert.DoesNotContain(document.Text, "%DURATION_BY_LC_FIRST_PARAGRAPH%");
            StringAssert.DoesNotContain(document.Text, "%DURATION_BY_LC_TABLE%");
            StringAssert.DoesNotContain(document.Text, "%DURATION_BY_LC_DESCRIPTION_TABLE%");
            StringAssert.DoesNotContain(document.Text, "%DURATION_BY_LC_PENULTIMATE_PARAGRAPH%");
            StringAssert.DoesNotContain(document.Text, "%DURATION_BY_LC_LAST_PARAGRAPH%");
            StringAssert.DoesNotContain(document.Text, "%CALENDAR_PLAN_PREPARATORY_TABLE%");
            StringAssert.DoesNotContain(document.Text, "%CALENDAR_PLAN_MAIN_TABLE%");
            StringAssert.DoesNotContain(document.Text, "%ENERGY_AND_WATER_TABLE%");
            StringAssert.DoesNotContain(document.Text, "%CIPHER%");
            StringAssert.DoesNotContain(document.Text, "%DATE%");
            StringAssert.DoesNotContain(document.Text, "%CONSTRUCTION_START_DATE%");
            StringAssert.DoesNotContain(document.Text, "%CY%");
            StringAssert.DoesNotContain(document.Text, "%TD%");
            StringAssert.DoesNotContain(document.Text, "%PP%");
            StringAssert.DoesNotContain(document.Text, "%AT%");
            StringAssert.DoesNotContain(document.Text, "%TLC%");
        }
    }
}
