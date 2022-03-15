using POS.DomainModels;
using POS.DomainModels.DocumentDomainModels;
using POS.Infrastructure.Writers.Base;
using System.Globalization;
using POS.Infrastructure.Services;

namespace POS.Infrastructure.Writers;

public class DurationByLCWriter : IDurationByLCWriter
{
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

    public MemoryStream Write(DurationByLC durationByLC, string templatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        using var document = DocumentService.Load(templatePath);
        ReplacePatternsWithActualValues(document, durationByLC);
        var memoryStream = new MemoryStream();
        document.SaveAs(memoryStream);
        return memoryStream;
    }

    private void ReplacePatternsWithActualValues(MyDocument document, DurationByLC durationByLC)
    {
        document.ReplaceText(TechnologicalLaborCostsPattern,
            durationByLC.TechnologicalLaborCosts > 0
                ? TechnologicalLaborCostsText
                : string.Empty);
        document.ReplaceText(NumberOfWorkingDaysInMonthPattern,
            durationByLC.NumberOfWorkingDays.ToString(Constants.AppConstants.DecimalFormat));
        document.ReplaceText(NumberOfEmployeesPattern, durationByLC.NumberOfEmployees.ToString());
        document.ReplaceText(WorkingDayDurationPattern,
            durationByLC.WorkingDayDuration.ToString(Constants.AppConstants.DecimalFormat));
        document.ReplaceText(RoundedDurationPattern,
            durationByLC.RoundedDuration.ToString(Constants.AppConstants.DecimalFormat));
        document.ReplaceText(TotalDurationPattern,
            durationByLC.TotalDuration.ToString(Constants.AppConstants.DecimalFormat));
        document.ReplaceText(AcceptanceTimePattern,
            durationByLC.AcceptanceTime.ToString(Constants.AppConstants.DecimalFormat));
        document.ReplaceText(PreparatoryPeriodPattern,
            durationByLC.PreparatoryPeriod.ToString(Constants.AppConstants.DecimalFormat));
        document.ReplaceText(ShiftPattern, durationByLC.Shift.ToString(Constants.AppConstants.DecimalFormat));
        document.ReplaceText(DurationPattern, durationByLC.Duration.ToString(Constants.AppConstants.DecimalFormat));
        document.ReplaceText(TotalLaborCostsPattern,
            durationByLC.TotalLaborCosts.ToString(Constants.AppConstants.DecimalFormat));
    }
}