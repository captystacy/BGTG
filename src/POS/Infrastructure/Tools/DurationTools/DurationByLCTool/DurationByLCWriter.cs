using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.DurationTools.DurationByLCTool.Base;
using System.Globalization;

namespace POS.Infrastructure.Tools.DurationTools.DurationByLCTool;

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
        _documentService.Dispose();
        return memoryStream;
    }

    private void ReplacePatternsWithActualValues(DurationByLC durationByLC)
    {
        _documentService.ReplaceText(TechnologicalLaborCostsPattern,
            durationByLC.TechnologicalLaborCosts > 0
                ? TechnologicalLaborCostsText
                : string.Empty);
        _documentService.ReplaceText(NumberOfWorkingDaysInMonthPattern, durationByLC.NumberOfWorkingDays.ToString(AppData.DecimalFormat));
        _documentService.ReplaceText(NumberOfEmployeesPattern, durationByLC.NumberOfEmployees.ToString());
        _documentService.ReplaceText(WorkingDayDurationPattern, durationByLC.WorkingDayDuration.ToString(AppData.DecimalFormat));
        _documentService.ReplaceText(RoundedDurationPattern, durationByLC.RoundedDuration.ToString(AppData.DecimalFormat));
        _documentService.ReplaceText(TotalDurationPattern, durationByLC.TotalDuration.ToString(AppData.DecimalFormat));
        _documentService.ReplaceText(AcceptanceTimePattern, durationByLC.AcceptanceTime.ToString(AppData.DecimalFormat));
        _documentService.ReplaceText(PreparatoryPeriodPattern, durationByLC.PreparatoryPeriod.ToString(AppData.DecimalFormat));
        _documentService.ReplaceText(ShiftPattern, durationByLC.Shift.ToString(AppData.DecimalFormat));
        _documentService.ReplaceText(DurationPattern, durationByLC.Duration.ToString(AppData.DecimalFormat));
        _documentService.ReplaceText(TotalLaborCostsPattern, durationByLC.TotalLaborCosts.ToString(AppData.DecimalFormat));
    }
}