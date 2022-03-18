using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using System.Text.RegularExpressions;
using POS.DomainModels;

namespace POS.Infrastructure.Writers;

public class ECPProjectWriter : IECPProjectWriter
{
    private readonly IWordDocumentService _wordDocumentService;
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

    public ECPProjectWriter(IWordDocumentService wordDocumentService)
    {
        _wordDocumentService = wordDocumentService;
    }

    public MemoryStream Write(Stream durationByLCStream, Stream calendarPlanStream, Stream energyAndWaterStream, string objectCipher, string templatePath)
    {
        _wordDocumentService.Load(templatePath);
        _wordDocumentService.ReplaceInBaseDocumentMode = true;

        _wordDocumentService.ReplaceTextInDocument(CipherPattern, objectCipher);
        _wordDocumentService.ReplaceTextInDocument(DatePattern, DateTime.Now.ToString(AppConstants.DateTimeMonthAndYearShortFormat));

        ReplacePatternsWithDurationByLC(durationByLCStream);
        ReplacePatternsWithCalendarPlan(calendarPlanStream);
        ReplacePatternsWithEnergyAndWater(energyAndWaterStream);

        var memoryStream = new MemoryStream();
        _wordDocumentService.SaveAs(memoryStream, MyFileFormat.Doc);
        _wordDocumentService.DisposeLastDocument();
        return memoryStream;
    }

    public int GetNumberOfEmployees(Stream durationByLCStream)
    {
        _wordDocumentService.Load(durationByLCStream);
        _wordDocumentService.TableIndex = 1;
        _wordDocumentService.RowIndex = 4;
        _wordDocumentService.CellIndex = 1;
        var numberOfEmployees = int.Parse(_wordDocumentService.ParagraphTextInCell);
        _wordDocumentService.DisposeLastDocument();
        return numberOfEmployees;
    }

    private void ReplaceConstructionStartDateAndConstructionYear()
    {
        _wordDocumentService.RowIndex = 1;
        _wordDocumentService.CellIndex = 3;
        var constructionStartDateStr = _wordDocumentService.ParagraphTextInCell.ToLower();

        var constructionYearStr = Regex.Match(constructionStartDateStr, @"\d+").Value;

        _wordDocumentService.ReplaceTextInDocument(ConstructionStartDatePattern, constructionStartDateStr);
        _wordDocumentService.ReplaceTextInDocument(ConstructionYearPattern, constructionYearStr);
    }

    private void ReplacePatternsWithEnergyAndWater(Stream energyAndWaterStream)
    {
        _wordDocumentService.Load(energyAndWaterStream);
        energyAndWaterStream.Close();

        _wordDocumentService.ReplaceTextWithTable(EnergyAndWaterTablePattern);

        _wordDocumentService.DisposeLastDocument();
    }

    private void ReplacePatternsWithCalendarPlan(Stream calendarPlanStream)
    {
        _wordDocumentService.Load(calendarPlanStream);
        calendarPlanStream.Close();

        _wordDocumentService.ReplaceTextWithTable(CalendarPlanPreparatoryTablePattern);
        _wordDocumentService.SectionIndex = 1;
        _wordDocumentService.ReplaceTextWithTable(CalendarPlanMainTablePattern);

        ReplaceConstructionStartDateAndConstructionYear();
        _wordDocumentService.DisposeLastDocument();
    }

    private void ReplacePatternsWithDurationByLC(Stream durationByLCStream)
    {
        _wordDocumentService.Load(durationByLCStream);
        durationByLCStream.Close();

        var durationByLCFirstParagraph = _wordDocumentService.ParagraphTextInDocument;
        _wordDocumentService.ParagraphIndex = _wordDocumentService.ParagraphsCountInDocument - 2;
        var durationByLCPenultimateParagraph = _wordDocumentService.ParagraphsCountInDocument == 5 
            ? _wordDocumentService.ParagraphTextInDocument
            : string.Empty;
        _wordDocumentService.ParagraphIndex = _wordDocumentService.ParagraphsCountInDocument - 1;
        var durationByLCLastParagraph = _wordDocumentService.ParagraphTextInDocument;

        _wordDocumentService.ReplaceTextInDocument(DurationByLCFirstParagraphPattern, durationByLCFirstParagraph);
        _wordDocumentService.TableIndex = 0;
        _wordDocumentService.ReplaceTextWithTable(DurationByLCTablePattern);
        _wordDocumentService.TableIndex = 1;
        _wordDocumentService.ReplaceTextWithTable(DurationByLCDescriptionTablePattern);
        _wordDocumentService.ReplaceTextInDocument(DurationByLCPenultimateParagraphPattern, durationByLCPenultimateParagraph);
        _wordDocumentService.ReplaceTextInDocument(DurationByLCLastParagraphPattern, durationByLCLastParagraph);

        ReplacePatternsWithTechnicalAndEconomicIndicators();

        _wordDocumentService.DisposeLastDocument();
    }

    private void ReplacePatternsWithTechnicalAndEconomicIndicators()
    {
        _wordDocumentService.ParagraphIndex = _wordDocumentService.ParagraphsCountInDocument - 1;
        var lastParagraphParts = _wordDocumentService.ParagraphTextInDocument.Split("мес,");

        var totalDurationStr = Regex.Match(lastParagraphParts[0], @"[\d,]+").Value;
        var preparatoryPeriodStr = Regex.Match(lastParagraphParts[1], @"[\d,]+").Value;
        var acceptanceTimeStr = string.Empty;
        if (lastParagraphParts.Length == 3)
        {
            acceptanceTimeStr = Regex.Match(lastParagraphParts[2], @"[\d,]+").Value;
        }

        _wordDocumentService.TableIndex = 1;
        _wordDocumentService.CellIndex = 1;
        _wordDocumentService.ParagraphIndex = 0;
        var totalLaborCostsStr = _wordDocumentService.ParagraphTextInCell;

        _wordDocumentService.ReplaceTextInDocument(TotalDurationPattern, totalDurationStr);
        _wordDocumentService.ReplaceTextInDocument(PreparatoryPeriodPattern, preparatoryPeriodStr);
        _wordDocumentService.ReplaceTextInDocument(AcceptanceTimePattern, acceptanceTimeStr);
        _wordDocumentService.ReplaceTextInDocument(TotalLaborCostsPattern, totalLaborCostsStr);
    }
}