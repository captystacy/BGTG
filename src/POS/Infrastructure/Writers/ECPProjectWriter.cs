using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using System.Text.RegularExpressions;

namespace POS.Infrastructure.Writers;

public class ECPProjectWriter : IECPProjectWriter
{
    private readonly IDocumentService _documentService;
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

    public ECPProjectWriter(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public MemoryStream Write(Stream durationByLCStream, Stream calendarPlanStream, Stream energyAndWaterStream, string objectCipher, string templatePath)
    {
        _documentService.Load(templatePath);
        _documentService.ReplaceInBaseDocumentMode = true;

        _documentService.ReplaceTextInDocument(CipherPattern, objectCipher);
        _documentService.ReplaceTextInDocument(DatePattern, DateTime.Now.ToString(AppConstants.DateTimeMonthAndYearShortFormat));

        ReplacePatternsWithDurationByLC(durationByLCStream);
        ReplacePatternsWithCalendarPlan(calendarPlanStream);
        ReplacePatternsWithEnergyAndWater(energyAndWaterStream);

        var memoryStream = new MemoryStream();
        _documentService.SaveAs(memoryStream);
        _documentService.DisposeLastDocument();
        return memoryStream;
    }

    public int GetNumberOfEmployees(Stream durationByLCStream)
    {
        _documentService.Load(durationByLCStream);
        _documentService.TableIndex = 1;
        _documentService.RowIndex = 4;
        _documentService.ParagraphIndex = 1;
        var numberOfEmployees = int.Parse(_documentService.ParagraphTextInRow);
        _documentService.DisposeLastDocument();
        return numberOfEmployees;
    }

    private void ReplaceConstructionStartDateAndConstructionYear()
    {
        _documentService.RowIndex = 1;
        _documentService.ParagraphIndex = 3;
        var constructionStartDateStr = _documentService.ParagraphTextInRow.ToLower();

        var constructionYearStr = Regex.Match(constructionStartDateStr, @"\d+").Value;

        _documentService.ReplaceTextInDocument(ConstructionStartDatePattern, constructionStartDateStr);
        _documentService.ReplaceTextInDocument(ConstructionYearPattern, constructionYearStr);
    }

    private void ReplacePatternsWithEnergyAndWater(Stream energyAndWaterStream)
    {
        _documentService.Load(energyAndWaterStream);
        energyAndWaterStream.Close();

        _documentService.ReplaceTextWithTable(EnergyAndWaterTablePattern);

        _documentService.DisposeLastDocument();
    }

    private void ReplacePatternsWithCalendarPlan(Stream calendarPlanStream)
    {
        _documentService.Load(calendarPlanStream);
        calendarPlanStream.Close();

        _documentService.ReplaceTextWithTable(CalendarPlanPreparatoryTablePattern);
        _documentService.TableIndex = 1;
        _documentService.ReplaceTextWithTable(CalendarPlanMainTablePattern);

        ReplaceConstructionStartDateAndConstructionYear();
        _documentService.DisposeLastDocument();
    }

    private void ReplacePatternsWithDurationByLC(Stream durationByLCStream)
    {
        _documentService.Load(durationByLCStream);
        durationByLCStream.Close();

        var durationByLCFirstParagraph = _documentService.ParagraphTextInDocument;
        _documentService.ParagraphIndex = _documentService.ParagraphsCountInDocument - 2;
        var durationByLCPenultimateParagraph = _documentService.ParagraphsCountInDocument == 35 
            ? _documentService.ParagraphTextInDocument
            : string.Empty;
        _documentService.ParagraphIndex = _documentService.ParagraphsCountInDocument - 1;
        var durationByLCLastParagraph = _documentService.ParagraphTextInDocument;

        _documentService.ReplaceTextInDocument(DurationByLCFirstParagraphPattern, durationByLCFirstParagraph);
        _documentService.TableIndex = 0;
        _documentService.ReplaceTextWithTable(DurationByLCTablePattern);
        _documentService.TableIndex = 1;
        _documentService.ReplaceTextWithTable(DurationByLCDescriptionTablePattern);
        _documentService.ReplaceTextInDocument(DurationByLCPenultimateParagraphPattern, durationByLCPenultimateParagraph);
        _documentService.ReplaceTextInDocument(DurationByLCLastParagraphPattern, durationByLCLastParagraph);

        ReplacePatternsWithTechnicalAndEconomicIndicators();

        _documentService.DisposeLastDocument();
    }

    private void ReplacePatternsWithTechnicalAndEconomicIndicators()
    {
        _documentService.ParagraphIndex = _documentService.ParagraphsCountInDocument - 1;
        var lastParagraphParts = _documentService.ParagraphTextInDocument.Split("мес,");

        var totalDurationStr = Regex.Match(lastParagraphParts[0], @"[\d,]+").Value;
        var preparatoryPeriodStr = Regex.Match(lastParagraphParts[1], @"[\d,]+").Value;
        var acceptanceTimeStr = string.Empty;
        if (lastParagraphParts.Length == 3)
        {
            acceptanceTimeStr = Regex.Match(lastParagraphParts[2], @"[\d,]+").Value;
        }

        _documentService.TableIndex = 1;
        _documentService.ParagraphIndex = 1;
        var totalLaborCostsStr = _documentService.ParagraphTextInRow;

        _documentService.ReplaceTextInDocument(TotalDurationPattern, totalDurationStr);
        _documentService.ReplaceTextInDocument(PreparatoryPeriodPattern, preparatoryPeriodStr);
        _documentService.ReplaceTextInDocument(AcceptanceTimePattern, acceptanceTimeStr);
        _documentService.ReplaceTextInDocument(TotalLaborCostsPattern, totalLaborCostsStr);
    }
}