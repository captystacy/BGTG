using NUnit.Framework;
using POS.Infrastructure.Tools.ProjectTool;
using System.IO;
using Xceed.Words.NET;

namespace POS.Tests.Infrastructure.Tools.ProjectTool;

public class ProjectWriterTests
{
    private ECPProjectWriter _ecpProjectWriter = null!;

    private const string ECPProjectTemplatesDirectory = @"..\..\..\Infrastructure\Tools\ProjectTool\ProjectTemplates\ECP\Saiko\Kapitan\Employees4";
    private const string ECPProjectSourcesDirectory = @"..\..\..\Infrastructure\Tools\ProjectTool\Source";

    [SetUp]
    public void SetUp()
    {
        _ecpProjectWriter = new ECPProjectWriter();
    }

    [Test]
    public void Write_ConstructionObject548_DoesNotContainPatterns()
    {
        var objectCipher = "5.5-20.548";
        var durationByLCStream = File.OpenRead(Path.Combine(ECPProjectSourcesDirectory, "DurationByLC.docx"));
        var calendarPlanStream = File.OpenRead(Path.Combine(ECPProjectSourcesDirectory, "CalendarPlan.docx"));
        var energyAndWaterStream = File.OpenRead(Path.Combine(ECPProjectSourcesDirectory, "EnergyAndWater.docx"));
        var templatePath = Path.Combine(ECPProjectTemplatesDirectory, "HouseholdTown+.docx");
            
        var memoryStream = _ecpProjectWriter.Write(durationByLCStream, calendarPlanStream, energyAndWaterStream, objectCipher, templatePath);

        using var document = DocX.Load(memoryStream);
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