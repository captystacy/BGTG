using POS.Infrastructure.Tools.DurationTools.DurationByLCTool;
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

    public void Write(string objectCipher, string templatePath)
    {
        using var document = DocX.Load(templatePath);

        document.ReplaceText(CipherPattern, objectCipher);
        document.ReplaceText(DatePattern, DateTime.Now.ToString(AppData.DateTimeMonthAndYearShortFormat));
        //document.ReplaceText(ConstructionStartDatePattern, constructionStartDate.ToString(AppData.DateTimeMonthAndYearFormat).ToLower());
        //document.ReplaceText(ConstructionYearPattern, constructionStartDate.Year.ToString());

        //ReplacePatternsWithDurationByLC(durationByLCPath, document);
        //ReplacePatternsWithCalendarPlan(calendarPlanPath, document);
        //ReplacePatternsWithEnergyAndWater(energyAndWaterPath, document);

        //ReplacePatternsWithTechnicalAndEconomicIndicators(document, durationByLC);

        //document.SaveAs(savePath);
    }

    private void ReplacePatternsWithTechnicalAndEconomicIndicators(DocX document, DurationByLC durationByLC)
    {
        document.ReplaceText(TotalDurationPattern, durationByLC.TotalDuration.ToString(AppData.DecimalFormat));
        document.ReplaceText(PreparatoryPeriodPattern, durationByLC.PreparatoryPeriod.ToString(AppData.DecimalFormat));
        document.ReplaceText(AcceptanceTimePattern, durationByLC.AcceptanceTime.ToString(AppData.DecimalFormat));
        document.ReplaceText(TotalLaborCostsPattern, durationByLC.TotalLaborCosts.ToString(AppData.DecimalFormat));
    }

    private void ReplacePatternsWithEnergyAndWater(string energyAndWaterPath, DocX document)
    {
        using var energyAndWaterDocument = DocX.Load(energyAndWaterPath);
        var energyAndWaterTable = energyAndWaterDocument.Tables[0];

        document.ReplaceTextWithObject(EnergyAndWaterTablePattern, energyAndWaterTable);
    }

    private void ReplacePatternsWithCalendarPlan(string calendarPlanPath, DocX document)
    {
        using var calendarPlanDocument = DocX.Load(calendarPlanPath);
        var preparatoryTable = calendarPlanDocument.Tables[0];
        var mainTable = calendarPlanDocument.Tables[1];

        document.ReplaceTextWithObject(CalendarPlanPreparatoryTablePattern, preparatoryTable);
        document.ReplaceTextWithObject(CalendarPlanMainTablePattern, mainTable);
    }

    private void ReplacePatternsWithDurationByLC(string durationByLCPath, DocX document)
    {
        using var durationByLCDocument = DocX.Load(durationByLCPath);
        var durationByLCFirstParagraph = durationByLCDocument.Paragraphs[0].Text;
        var durationByLCTable = durationByLCDocument.Tables[0];
        var durationByLCDescriptionTable = durationByLCDocument.Tables[1];
        var durationByLCPenultimateParagraph = durationByLCDocument.Paragraphs.Count == 35 ? durationByLCDocument.Paragraphs[^2].Text : string.Empty;
        var durationByLCLastParagraph = durationByLCDocument.Paragraphs[^1].Text;

        document.ReplaceText(DurationByLCFirstParagraphPattern, durationByLCFirstParagraph);
        document.ReplaceTextWithObject(DurationByLCTablePattern, durationByLCTable);
        document.ReplaceTextWithObject(DurationByLCDescriptionTablePattern, durationByLCDescriptionTable);
        document.ReplaceText(DurationByLCPenultimateParagraphPattern, durationByLCPenultimateParagraph);
        document.ReplaceText(DurationByLCLastParagraphPattern, durationByLCLastParagraph);
    }
}