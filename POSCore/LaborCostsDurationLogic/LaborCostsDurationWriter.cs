using POSCore.LaborCostsDurationLogic.Interfaces;
using Xceed.Words.NET;

namespace POSCore.LaborCostsDurationLogic
{
    public class LaborCostsDurationWriter : ILaborCostsDurationWriter
    {
        private const string _durationPattern = "%D%";
        private const string _laborCostsPattern = "%LC%";
        private const string _workingDayDurationPattern = "%WDD%";
        private const string _shiftPattern = "%S%";
        private const string _numberOfWorkingDaysInMonthPattern = "%NOWDIM%";
        private const string _numberOfEmployeesPattern = "%NOE%";
        private const string _roundedDurationPattern = "%RD%";
        private const string _totalDurationPattern = "%TD%";
        private const string _preparatoryPeriodPattern = "%PP%";
        private const string _acceptanceTimePattern = "%AT%";

        public void Write(LaborCostsDuration laborCostsDuration, string templatePath, string savePath)
        {
            using (var document = DocX.Load(templatePath))
            {
                ReplacePatternsWithActualValues(document, laborCostsDuration);
                document.SaveAs(savePath);
            }
        }

        private void ReplacePatternsWithActualValues(DocX document, LaborCostsDuration laborCostsDuration)
        {
            document.ReplaceText(_numberOfWorkingDaysInMonthPattern, laborCostsDuration.NumberOfWorkingDays.ToString());
            document.ReplaceText(_numberOfEmployeesPattern, laborCostsDuration.NumberOfEmployees.ToString());
            document.ReplaceText(_workingDayDurationPattern, laborCostsDuration.WorkingDayDuration.ToString());
            document.ReplaceText(_roundedDurationPattern, laborCostsDuration.RoundedDuration.ToString());
            document.ReplaceText(_totalDurationPattern, laborCostsDuration.TotalDuration.ToString());
            document.ReplaceText(_acceptanceTimePattern, laborCostsDuration.AcceptanceTime.ToString());
            document.ReplaceText(_preparatoryPeriodPattern, laborCostsDuration.PreparatoryPeriod.ToString());
            document.ReplaceText(_shiftPattern, laborCostsDuration.Shift.ToString());
            document.ReplaceText(_durationPattern, laborCostsDuration.Duration.ToString());
            document.ReplaceText(_laborCostsPattern, laborCostsDuration.LaborCosts.ToString());
        }
    }
}
