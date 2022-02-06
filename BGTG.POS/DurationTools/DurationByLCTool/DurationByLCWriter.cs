using System.Globalization;
using BGTG.Core;
using BGTG.POS.DurationTools.DurationByLCTool.Interfaces;
using Xceed.Words.NET;

namespace BGTG.POS.DurationTools.DurationByLCTool
{
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

        public void Write(DurationByLC durationByLC, string templatePath, string savePath)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            using var document = DocX.Load(templatePath);
            ReplacePatternsWithActualValues(document, durationByLC);
            document.SaveAs(savePath);
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
}
