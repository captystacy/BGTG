using POS.DomainModels;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;
using System.Globalization;

namespace POS.Infrastructure.Writers;

public class DurationByLCWriter : IDurationByLCWriter
{
    private readonly IWordDocumentService _wordDocumentService;

    private const string DurationPattern = "%D%";
    private const string TotalLaborCostsPattern = "%LC%";
    private const string WorkingDayDurationPattern = "%WDD%";
    private const string ShiftPattern = "%S%";
    private const string NumberOfWorkingDaysInMonthPattern = "%NOWDIM%";
    private const string NumberOfEmployeesPattern = "%NOE%";
    private const string RoundedDurationPattern = "%RD%";
    private const string TotalDurationPattern = "%TD%";
    private const string PreparatoryPeriodPattern = "%PP%";
    private const string AcceptanceTimePattern = "%AT%";

    private const string TechnologicalLaborCostsPattern = "%TLC%";
    private const string TechnologicalLaborCostsText =
        " (трудозатраты по сметам и трудозатраты по технологической карте)";

    public DurationByLCWriter(IWordDocumentService wordDocumentService)
    {
        _wordDocumentService = wordDocumentService;
    }

    public MemoryStream Write(DurationByLC durationByLC, string templatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        _wordDocumentService.Load(templatePath);
        ReplacePatternsWithActualValues(durationByLC);
        var memoryStream = new MemoryStream();
        _wordDocumentService.SaveAs(memoryStream, MyFileFormat.DocX);
        _wordDocumentService.DisposeLastDocument();
        return memoryStream;
    }

    private void ReplacePatternsWithActualValues(DurationByLC durationByLC)
    {
        _wordDocumentService.ReplaceTextInDocument(TechnologicalLaborCostsPattern,
            durationByLC.TechnologicalLaborCosts > 0
                ? TechnologicalLaborCostsText
                : string.Empty);
        _wordDocumentService.ReplaceTextInDocument(NumberOfWorkingDaysInMonthPattern, durationByLC.NumberOfWorkingDays.ToString());
        _wordDocumentService.ReplaceTextInDocument(NumberOfEmployeesPattern, durationByLC.NumberOfEmployees.ToString());
        _wordDocumentService.ReplaceTextInDocument(WorkingDayDurationPattern, durationByLC.WorkingDayDuration.ToString());
        _wordDocumentService.ReplaceTextInDocument(RoundedDurationPattern, durationByLC.RoundedDuration.ToString());
        _wordDocumentService.ReplaceTextInDocument(TotalDurationPattern, durationByLC.TotalDuration.ToString());
        _wordDocumentService.ReplaceTextInDocument(AcceptanceTimePattern, durationByLC.AcceptanceTime.ToString());
        _wordDocumentService.ReplaceTextInDocument(PreparatoryPeriodPattern, durationByLC.PreparatoryPeriod.ToString());
        _wordDocumentService.ReplaceTextInDocument(ShiftPattern, durationByLC.Shift.ToString());
        _wordDocumentService.ReplaceTextInDocument(DurationPattern, durationByLC.Duration.ToString());
        _wordDocumentService.ReplaceTextInDocument(TotalLaborCostsPattern, durationByLC.TotalLaborCosts.ToString());
    }
}