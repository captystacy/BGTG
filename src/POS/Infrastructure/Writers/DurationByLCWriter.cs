using System.Globalization;
using POS.DomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.Writers;

public class DurationByLCWriter : IDurationByLCWriter
{
    private readonly IDocumentService _documentService;

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

    public DurationByLCWriter(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    public MemoryStream Write(DurationByLC durationByLC, string templatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        _documentService.Load(templatePath);
        ReplacePatternsWithActualValues(durationByLC);
        var memoryStream = new MemoryStream();
        _documentService.SaveAs(memoryStream);
        _documentService.DisposeLastDocument();
        return memoryStream;
    }

    private void ReplacePatternsWithActualValues(DurationByLC durationByLC)
    {
        _documentService.ReplaceTextInDocument(TechnologicalLaborCostsPattern,
            durationByLC.TechnologicalLaborCosts > 0
                ? TechnologicalLaborCostsText
                : string.Empty);
        _documentService.ReplaceTextInDocument(NumberOfWorkingDaysInMonthPattern, durationByLC.NumberOfWorkingDays.ToString(AppConstants.DecimalFormat));
        _documentService.ReplaceTextInDocument(NumberOfEmployeesPattern, durationByLC.NumberOfEmployees.ToString());
        _documentService.ReplaceTextInDocument(WorkingDayDurationPattern, durationByLC.WorkingDayDuration.ToString(AppConstants.DecimalFormat));
        _documentService.ReplaceTextInDocument(RoundedDurationPattern, durationByLC.RoundedDuration.ToString(AppConstants.DecimalFormat));
        _documentService.ReplaceTextInDocument(TotalDurationPattern, durationByLC.TotalDuration.ToString(AppConstants.DecimalFormat));
        _documentService.ReplaceTextInDocument(AcceptanceTimePattern, durationByLC.AcceptanceTime.ToString(AppConstants.DecimalFormat));
        _documentService.ReplaceTextInDocument(PreparatoryPeriodPattern, durationByLC.PreparatoryPeriod.ToString(AppConstants.DecimalFormat));
        _documentService.ReplaceTextInDocument(ShiftPattern, durationByLC.Shift.ToString(AppConstants.DecimalFormat));
        _documentService.ReplaceTextInDocument(DurationPattern, durationByLC.Duration.ToString(AppConstants.DecimalFormat));
        _documentService.ReplaceTextInDocument(TotalLaborCostsPattern, durationByLC.TotalLaborCosts.ToString(AppConstants.DecimalFormat));
    }
}