using POS.DurationLogic.DurationByLaborCosts.Interfaces;
using Xceed.Words.NET;

namespace POS.DurationLogic.DurationByLaborCosts
{
    public class DurationByLaborCostsWriter : IDurationByLaborCostsWriter
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

        public void Write(DurationByLaborCosts durationByLaborCosts, string templatePath, string savePath)
        {
            using var document = DocX.Load(templatePath);
            ReplacePatternsWithActualValues(document, durationByLaborCosts);
            document.SaveAs(savePath);
        }

        private void ReplacePatternsWithActualValues(DocX document, DurationByLaborCosts durationByLaborCosts)
        {
            document.ReplaceText(TechnologicalLaborCostsPattern,
                durationByLaborCosts.TechnologicalLaborCosts > 0 
                ? TechnologicalLaborCostsText 
                : string.Empty);
            document.ReplaceText(NumberOfWorkingDaysInMonthPattern, durationByLaborCosts.NumberOfWorkingDays.ToString());
            document.ReplaceText(NumberOfEmployeesPattern, durationByLaborCosts.NumberOfEmployees.ToString());
            document.ReplaceText(WorkingDayDurationPattern, durationByLaborCosts.WorkingDayDuration.ToString());
            document.ReplaceText(RoundedDurationPattern, durationByLaborCosts.RoundedDuration.ToString());
            document.ReplaceText(TotalDurationPattern, durationByLaborCosts.TotalDuration.ToString());
            document.ReplaceText(AcceptanceTimePattern, durationByLaborCosts.AcceptanceTime.ToString());
            document.ReplaceText(PreparatoryPeriodPattern, durationByLaborCosts.PreparatoryPeriod.ToString());
            document.ReplaceText(ShiftPattern, durationByLaborCosts.Shift.ToString());
            document.ReplaceText(DurationPattern, durationByLaborCosts.Duration.ToString());
            document.ReplaceText(TotalLaborCostsPattern, durationByLaborCosts.TotalLaborCosts.ToString());
        }
    }
}
