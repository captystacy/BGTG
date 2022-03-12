using System.Text.RegularExpressions;
using Xceed.Words.NET;

namespace POS.Infrastructure.Tools.ProjectTool;

public class ECPProjectWriter : IECPProjectWriter
{
    private const string DurationByLCFirstParagraphPattern = "%DURATION_BY_LC_FIRST_PARAGRAPH%";
    private const string DurationByLCTablePattern = "%DURATION_BY_LC_TABLE%";
    private const string DurationByLCDescriptionTablePattern = "%DURATION_BY_LC_DESCRIPTION_TABLE%";
    private const string DurationByLCPenultimateParagraphPattern = "%DURATION_BY_LC_PENULTIMATE_PARAGRAPH%";
    private const string DurationByLCLastParagraphPattern = "%DURATION_BY_LC_LAST_PARAGRAPH%";

    private const string CalendarPlanPreparatoryTablePattern = "%CALENDAR_PLAN_PREPARATORY_TABLE%";
    private const string CalendarPlanMainTablePattern = "%CALENDAR_PLAN_MAIN_TABLE%";

    private const string EnergyAndWaterTablePattern = "%ENERGY_AND_WATER_TABLE%";

    private const string CipherPattern = "%CIPHER%";
    private const string DatePattern = "%DATE%";

    private const string ConstructionStartDatePattern = "%CONSTRUCTION_START_DATE%";
    private const string ConstructionYearPattern = "%CY%";

    private const string TotalDurationPattern = "%TD%";
    private const string PreparatoryPeriodPattern = "%PP%";
    private const string AcceptanceTimePattern = "%AT%";
    private const string TotalLaborCostsPattern = "%TLC%";

    public MemoryStream Write(Stream durationByLCStream, Stream calendarPlanStream, Stream energyAndWaterStream, string objectCipher, string templatePath)
    {
        using var ecpDocX = DocX.Load(templatePath);

        ecpDocX.ReplaceText(CipherPattern, objectCipher);
        ecpDocX.ReplaceText(DatePattern, DateTime.Now.ToString(AppData.DateTimeMonthAndYearShortFormat));

        ReplaceConstructionStartDateAndConstructionYear(calendarPlanStream, ecpDocX);

        ReplacePatternsWithDurationByLC(durationByLCStream, ecpDocX);
        ReplacePatternsWithCalendarPlan(calendarPlanStream, ecpDocX);
        ReplacePatternsWithEnergyAndWater(energyAndWaterStream, ecpDocX);

        var memoryStream = new MemoryStream();
        ecpDocX.SaveAs(memoryStream);

        durationByLCStream.Close();
        calendarPlanStream.Close();
        energyAndWaterStream.Close();
        return memoryStream;
    }

    public int GetNumberOfEmployees(Stream durationByLCStream)
    {
        using var durationByLCDocX = DocX.Load(durationByLCStream);
        return int.Parse(durationByLCDocX.Tables[1].Rows[4].Cells[1].Paragraphs[0].Text);
    }

    private void ReplaceConstructionStartDateAndConstructionYear(Stream calendarPlan, DocX ecpDocX)
    {
        using var calendarPlanDocX = DocX.Load(calendarPlan);

        var constructionStartDateStr = calendarPlanDocX.Tables[0].Rows[1].Cells[3].Paragraphs[0].Text.ToLower();

        var constructionYearStr = Regex.Match(constructionStartDateStr, @"\d+").Value;

        ecpDocX.ReplaceText(ConstructionStartDatePattern, constructionStartDateStr);

        ecpDocX.ReplaceText(ConstructionYearPattern, constructionYearStr);
    }

    private void ReplacePatternsWithEnergyAndWater(Stream energyAndWaterStream, DocX ecpDocX)
    {
        using var energyAndWaterDocX = DocX.Load(energyAndWaterStream);
        var energyAndWaterTable = energyAndWaterDocX.Tables[0];

        ecpDocX.ReplaceTextWithObject(EnergyAndWaterTablePattern, energyAndWaterTable);
    }

    private void ReplacePatternsWithCalendarPlan(Stream calendarPlanStream, DocX ecpDocX)
    {
        using var calendarPlanDocX = DocX.Load(calendarPlanStream);
        var preparatoryTable = calendarPlanDocX.Tables[0];
        var mainTable = calendarPlanDocX.Tables[1];

        ecpDocX.ReplaceTextWithObject(CalendarPlanPreparatoryTablePattern, preparatoryTable);
        ecpDocX.ReplaceTextWithObject(CalendarPlanMainTablePattern, mainTable);
    }

    private void ReplacePatternsWithDurationByLC(Stream durationByLCStream, DocX ecpDocX)
    {
        using var durationByLCDocX = DocX.Load(durationByLCStream);
        var durationByLCFirstParagraph = durationByLCDocX.Paragraphs[0].Text;
        var durationByLCTable = durationByLCDocX.Tables[0];
        var durationByLCDescriptionTable = durationByLCDocX.Tables[1];
        var durationByLCPenultimateParagraph = durationByLCDocX.Paragraphs.Count == 35 ? durationByLCDocX.Paragraphs[^2].Text : string.Empty;
        var durationByLCLastParagraph = durationByLCDocX.Paragraphs[^1].Text;

        ecpDocX.ReplaceText(DurationByLCFirstParagraphPattern, durationByLCFirstParagraph);
        ecpDocX.ReplaceTextWithObject(DurationByLCTablePattern, durationByLCTable);
        ecpDocX.ReplaceTextWithObject(DurationByLCDescriptionTablePattern, durationByLCDescriptionTable);
        ecpDocX.ReplaceText(DurationByLCPenultimateParagraphPattern, durationByLCPenultimateParagraph);
        ecpDocX.ReplaceText(DurationByLCLastParagraphPattern, durationByLCLastParagraph);

        ReplacePatternsWithTechnicalAndEconomicIndicators(durationByLCDocX, ecpDocX);
    }

    private void ReplacePatternsWithTechnicalAndEconomicIndicators(DocX durationByLCDocX, DocX ecpDocX)
    {
        var lastParagraphParts = durationByLCDocX.Paragraphs[^1].Text.Split("мес,");

        var totalDurationStr = Regex.Match(lastParagraphParts[0], @"[\d,]+").Value;
        var preparatoryPeriodStr = Regex.Match(lastParagraphParts[1], @"[\d,]+").Value;
        var acceptanceTimeStr = string.Empty;
        if (lastParagraphParts.Length == 3)
        {
            acceptanceTimeStr = Regex.Match(lastParagraphParts[2], @"[\d,]+").Value;
        }
        var totalLaborCostsStr = durationByLCDocX.Tables[1].Rows[0].Cells[1].Paragraphs[0].Text;

        ecpDocX.ReplaceText(TotalDurationPattern, totalDurationStr);
        ecpDocX.ReplaceText(PreparatoryPeriodPattern, preparatoryPeriodStr);
        ecpDocX.ReplaceText(AcceptanceTimePattern, acceptanceTimeStr);
        ecpDocX.ReplaceText(TotalLaborCostsPattern, totalLaborCostsStr);
    }
}