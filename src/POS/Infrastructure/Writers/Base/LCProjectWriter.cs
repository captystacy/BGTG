using POS.DomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Services.Base;
using POS.ViewModels;
using System.Globalization;
using System.Text.RegularExpressions;

namespace POS.Infrastructure.Writers.Base;

public class LCProjectWriter : ILCProjectWriter
{
    private readonly IWordDocumentService _wordDocumentService;
    private readonly IEmployeesNeedCreator _employeesNeedCreator;
    private readonly IEngineerReplacer _engineerReplacer;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private const string TemplatesPath = @"Infrastructure\Templates\ProjectTemplates";

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

    private const string TotalNumberOfEmployeesPattern = "%TNOE%";
    private const string NumberOfWorkingEmployeesPattern = "%NOWE%";
    private const string NumberOfManagersPattern = "%NOM%";
    private const string ForemanRoomPattern = "%FR%";
    private const string DressingRoomPattern = "%DR%";
    private const string WashingRoomPattern = "%WR%";
    private const string WashingCranePattern = "%WC%";
    private const string ShowerRoomPattern = "%SR%";
    private const string ShowerMeshPattern = "%SM%";
    private const string ToiletPattern = "%T%";
    private const string FoodPointPattern = "%FP%";

    private const int DurationDescriptionTableIndex = 1;
    private const int NumberOfEmployeesCellIndex = 1;
    private const int NumberOfEmployeesRowIndex = 4;
    private const int ShiftRowIndex = 2;

    public LCProjectWriter(IWordDocumentService wordDocumentService, IEmployeesNeedCreator employeesNeedCreator, IEngineerReplacer engineerReplacer, IWebHostEnvironment webHostEnvironment)
    {
        _wordDocumentService = wordDocumentService;
        _employeesNeedCreator = employeesNeedCreator;
        _engineerReplacer = engineerReplacer;
        _webHostEnvironment = webHostEnvironment;
        _wordDocumentService.ReplaceInBaseDocumentMode = true;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
    }

    public MemoryStream Write(ProjectViewModel viewModel)
    {
        var templatePath = Path.Combine(_webHostEnvironment.ContentRootPath, TemplatesPath, $"{viewModel.ProjectTemplate}ProjectTemplate.doc");
        _wordDocumentService.Load(templatePath);

        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(viewModel.ProjectEngineer, TypeOfEngineer.ProjectEngineer);
        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(viewModel.NormalInspectionEngineer, TypeOfEngineer.NormalInspectionProjectEngineer);
        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(viewModel.ChiefEngineer, TypeOfEngineer.ChiefEngineer);
        _engineerReplacer.ReplaceEngineerSecondNameAndSignature(viewModel.ChiefProjectEngineer, TypeOfEngineer.ChiefProjectEngineer);

        ReplaceCipherAndDate(viewModel.ObjectCipher);

        var durationByLCStream =
            viewModel.CalculationFiles.First(x => x.FileName.Contains(AppConstants.DurationByLCFileName)).OpenReadStream();
        ReplacePatternsWithDurationByLC(durationByLCStream);

        var calendarPlanStream =
            viewModel.CalculationFiles.First(x => x.FileName.Contains(AppConstants.CalendarPlanFileName)).OpenReadStream();
        ReplacePatternsWithCalendarPlan(calendarPlanStream);

        var energyAndWaterStream =
            viewModel.CalculationFiles.First(x => x.FileName.Contains(AppConstants.EnergyAndWaterFileName)).OpenReadStream();
        ReplacePatternsWithEnergyAndWater(energyAndWaterStream);

        var memoryStream = new MemoryStream();
        _wordDocumentService.SaveAs(memoryStream, MyFileFormat.Doc);
        _wordDocumentService.DisposeLastDocument();
        return memoryStream;
    }

    private void ReplaceCipherAndDate(string objectCipher)
    {
        _wordDocumentService.ReplaceTextInDocument(CipherPattern, objectCipher);
        _wordDocumentService.ReplaceTextInDocument(DatePattern, DateTime.Now.ToString(AppConstants.DateTimeMonthAndYearShortFormat));
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

        ReplacePatternsWithEmployeesNeed();

        _wordDocumentService.DisposeLastDocument();
    }

    private void ReplacePatternsWithEmployeesNeed()
    {
        _wordDocumentService.TableIndex = DurationDescriptionTableIndex;
        _wordDocumentService.RowIndex = NumberOfEmployeesRowIndex;
        _wordDocumentService.CellIndex = NumberOfEmployeesCellIndex;
        var numberOfEmployeesFromDuration = int.Parse(_wordDocumentService.ParagraphTextInCell);

        _wordDocumentService.RowIndex = ShiftRowIndex;
        var shift = decimal.Parse(_wordDocumentService.ParagraphTextInCell);

        var employeesNeed = _employeesNeedCreator.Create(numberOfEmployeesFromDuration, shift);

        _wordDocumentService.ReplaceTextInDocument(TotalNumberOfEmployeesPattern, employeesNeed.TotalNumberOfEmployees.ToString());
        _wordDocumentService.ReplaceTextInDocument(NumberOfWorkingEmployeesPattern, employeesNeed.NumberOfWorkingEmployees.ToString());
        _wordDocumentService.ReplaceTextInDocument(NumberOfManagersPattern, employeesNeed.NumberOfManagers.ToString());
        _wordDocumentService.ReplaceTextInDocument(ForemanRoomPattern, employeesNeed.ForemanRoom.ToString());
        _wordDocumentService.ReplaceTextInDocument(DressingRoomPattern, employeesNeed.DressingRoom.ToString());
        _wordDocumentService.ReplaceTextInDocument(WashingRoomPattern, employeesNeed.WashingRoom.ToString());
        _wordDocumentService.ReplaceTextInDocument(WashingCranePattern, employeesNeed.WashingCrane.ToString());
        _wordDocumentService.ReplaceTextInDocument(ShowerRoomPattern, employeesNeed.ShowerRoom.ToString());
        _wordDocumentService.ReplaceTextInDocument(ShowerMeshPattern, employeesNeed.ShowerMesh.ToString());
        _wordDocumentService.ReplaceTextInDocument(ToiletPattern, employeesNeed.Toilet.ToString());
        _wordDocumentService.ReplaceTextInDocument(FoodPointPattern, employeesNeed.FoodPoint.ToString());
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