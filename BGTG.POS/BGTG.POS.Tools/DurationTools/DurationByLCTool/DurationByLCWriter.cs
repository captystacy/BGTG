using BGTG.POS.Tools.DurationTools.DurationByLCTool.Interfaces;
using Xceed.Words.NET;

namespace BGTG.POS.Tools.DurationTools.DurationByLCTool
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
            document.ReplaceText(NumberOfWorkingDaysInMonthPattern, durationByLC.NumberOfWorkingDays.ToString());
            document.ReplaceText(NumberOfEmployeesPattern, durationByLC.NumberOfEmployees.ToString());
            document.ReplaceText(WorkingDayDurationPattern, durationByLC.WorkingDayDuration.ToString());
            document.ReplaceText(RoundedDurationPattern, durationByLC.RoundedDuration.ToString());
            document.ReplaceText(TotalDurationPattern, durationByLC.TotalDuration.ToString());
            document.ReplaceText(AcceptanceTimePattern, durationByLC.AcceptanceTime.ToString());
            document.ReplaceText(PreparatoryPeriodPattern, durationByLC.PreparatoryPeriod.ToString());
            document.ReplaceText(ShiftPattern, durationByLC.Shift.ToString());
            document.ReplaceText(DurationPattern, durationByLC.Duration.ToString());
            document.ReplaceText(TotalLaborCostsPattern, durationByLC.TotalLaborCosts.ToString());
        }
    }
}
