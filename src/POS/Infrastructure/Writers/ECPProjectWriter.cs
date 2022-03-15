using POS.DomainModels.DocumentDomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services;
using POS.Infrastructure.Writers.Base;
using System.Text.RegularExpressions;

namespace POS.Infrastructure.Writers;

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

    public MemoryStream Write(Stream durationByLCStream, Stream calendarPlanStream, Stream energyAndWaterStream,
        string objectCipher, string templatePath)
    {
        using var ecpDocument = DocumentService.Load(templatePath);

        ecpDocument.ReplaceText(CipherPattern, objectCipher);
        ecpDocument.ReplaceText(DatePattern, DateTime.Now.ToString(AppConstants.DateTimeMonthAndYearShortFormat));

        ReplaceConstructionStartDateAndConstructionYear(calendarPlanStream, ecpDocument);

        ReplacePatternsWithDurationByLC(durationByLCStream, ecpDocument);
        ReplacePatternsWithCalendarPlan(calendarPlanStream, ecpDocument);
        ReplacePatternsWithEnergyAndWater(energyAndWaterStream, ecpDocument);

        var memoryStream = new MemoryStream();
        ecpDocument.SaveAs(memoryStream);

        durationByLCStream.Close();
        calendarPlanStream.Close();
        energyAndWaterStream.Close();
        return memoryStream;
    }

    public int GetNumberOfEmployees(Stream durationByLCStream)
    {
        using var durationByLCDocument = DocumentService.Load(durationByLCStream);
        return int.Parse(durationByLCDocument.Tables[1].Rows[4].Cells[1].Paragraphs[0].Text);
    }

    private void ReplaceConstructionStartDateAndConstructionYear(Stream calendarPlan, MyDocument ecpDocument)
    {
        using var calendarPlanDocument = DocumentService.Load(calendarPlan);

        var constructionStartDateStr = calendarPlanDocument.Tables[0].Rows[1].Cells[3].Paragraphs[0].Text.ToLower();

        var constructionYearStr = Regex.Match(constructionStartDateStr, @"\d+").Value;

        ecpDocument.ReplaceText(ConstructionStartDatePattern, constructionStartDateStr);

        ecpDocument.ReplaceText(ConstructionYearPattern, constructionYearStr);
    }

    private void ReplacePatternsWithEnergyAndWater(Stream energyAndWaterStream, MyDocument ecpDocument)
    {
        using var energyAndWaterDocument = DocumentService.Load(energyAndWaterStream);
        var energyAndWaterTable = energyAndWaterDocument.Tables[0];

        ecpDocument.ReplaceTextWithTable(EnergyAndWaterTablePattern, energyAndWaterTable);
    }

    private void ReplacePatternsWithCalendarPlan(Stream calendarPlanStream, MyDocument ecpDocument)
    {
        using var calendarPlanDocument = DocumentService.Load(calendarPlanStream);
        var preparatoryTable = calendarPlanDocument.Tables[0];
        var mainTable = calendarPlanDocument.Tables[1];

        ecpDocument.ReplaceTextWithTable(CalendarPlanPreparatoryTablePattern, preparatoryTable);
        ecpDocument.ReplaceTextWithTable(CalendarPlanMainTablePattern, mainTable);
    }

    private void ReplacePatternsWithDurationByLC(Stream durationByLCStream, MyDocument ecpDocument)
    {
        using var durationByLCDocument = DocumentService.Load(durationByLCStream);
        var durationByLCFirstParagraph = durationByLCDocument.Paragraphs[0].Text;
        var durationByLCTable = durationByLCDocument.Tables[0];
        var durationByLCDescriptionTable = durationByLCDocument.Tables[1];
        var durationByLCPenultimateParagraph = durationByLCDocument.Paragraphs.Count == 35
            ? durationByLCDocument.Paragraphs[^2].Text
            : string.Empty;
        var durationByLCLastParagraph = durationByLCDocument.Paragraphs[^1].Text;

        ecpDocument.ReplaceText(DurationByLCFirstParagraphPattern, durationByLCFirstParagraph);
        ecpDocument.ReplaceTextWithTable(DurationByLCTablePattern, durationByLCTable);
        ecpDocument.ReplaceTextWithTable(DurationByLCDescriptionTablePattern, durationByLCDescriptionTable);
        ecpDocument.ReplaceText(DurationByLCPenultimateParagraphPattern, durationByLCPenultimateParagraph);
        ecpDocument.ReplaceText(DurationByLCLastParagraphPattern, durationByLCLastParagraph);

        ReplacePatternsWithTechnicalAndEconomicIndicators(durationByLCDocument, ecpDocument);
    }

    private void ReplacePatternsWithTechnicalAndEconomicIndicators(MyDocument durationByLCDocument,
        MyDocument ecpDocument)
    {
        var lastParagraphParts = durationByLCDocument.Paragraphs[^1].Text.Split("мес,");

        var totalDurationStr = Regex.Match(lastParagraphParts[0], @"[\d,]+").Value;
        var preparatoryPeriodStr = Regex.Match(lastParagraphParts[1], @"[\d,]+").Value;
        var acceptanceTimeStr = string.Empty;
        if (lastParagraphParts.Length == 3)
        {
            acceptanceTimeStr = Regex.Match(lastParagraphParts[2], @"[\d,]+").Value;
        }

        var totalLaborCostsStr = durationByLCDocument.Tables[1].Rows[0].Cells[1].Paragraphs[0].Text;

        ecpDocument.ReplaceText(TotalDurationPattern, totalDurationStr);
        ecpDocument.ReplaceText(PreparatoryPeriodPattern, preparatoryPeriodStr);
        ecpDocument.ReplaceText(AcceptanceTimePattern, acceptanceTimeStr);
        ecpDocument.ReplaceText(TotalLaborCostsPattern, totalLaborCostsStr);
    }
}