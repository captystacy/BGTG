using System.Globalization;
using POS.Infrastructure.Tools.DurationTools.DurationByLCTool.Base;
using Xceed.Words.NET;

namespace POS.Infrastructure.Tools.DurationTools.DurationByLCTool;

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

    public void Write(DurationByLC durationByLC, string templatePath)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        using var document = DocX.Load(templatePath);
        ReplacePatternsWithActualValues(document, durationByLC);
    }

    private void ReplacePatternsWithActualValues(DocX document, DurationByLC durationByLC)
    {
        document.ReplaceText(TechnologicalLaborCostsPattern,
            durationByLC.TechnologicalLaborCosts > 0 
                ? TechnologicalLaborCostsText 
                : string.Empty);
        document.ReplaceText(NumberOfWorkingDaysInMonthPattern, durationByLC.NumberOfWorkingDays.ToString(AppData.DecimalFormat));
        document.ReplaceText(NumberOfEmployeesPattern, durationByLC.NumberOfEmployees.ToString());
        document.ReplaceText(WorkingDayDurationPattern, durationByLC.WorkingDayDuration.ToString(AppData.DecimalFormat));
        document.ReplaceText(RoundedDurationPattern, durationByLC.RoundedDuration.ToString(AppData.DecimalFormat));
        document.ReplaceText(TotalDurationPattern, durationByLC.TotalDuration.ToString(AppData.DecimalFormat));
        document.ReplaceText(AcceptanceTimePattern, durationByLC.AcceptanceTime.ToString(AppData.DecimalFormat));
        document.ReplaceText(PreparatoryPeriodPattern, durationByLC.PreparatoryPeriod.ToString(AppData.DecimalFormat));
        document.ReplaceText(ShiftPattern, durationByLC.Shift.ToString(AppData.DecimalFormat));
        document.ReplaceText(DurationPattern, durationByLC.Duration.ToString(AppData.DecimalFormat));
        document.ReplaceText(TotalLaborCostsPattern, durationByLC.TotalLaborCosts.ToString(AppData.DecimalFormat));
    }
}