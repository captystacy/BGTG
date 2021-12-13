namespace POSCore.DurationLogic.LaborCosts
{
    public class LaborCostsDuration
    {
        public decimal Duration { get; set; }
        public decimal LaborCosts { get; }
        public decimal WorkingDayDuration { get; }
        public decimal Shift { get; }
        public decimal NumberOfWorkingDays { get; }
        public int NumberOfEmployees { get; }
        public decimal RoundedDuration { get; }
        public decimal TotalDuration { get; }
        public decimal PreparatoryPeriod { get; }
        public decimal AcceptanceTime { get; }
        public bool AcceptanceTimeIncluded { get; }
        public bool RoundingIncluded { get; }

        public LaborCostsDuration(decimal duration, decimal laborCosts, decimal workingDayDuration, decimal shift, 
            decimal numberOfWorkingDaysInMonth, int numberOfEmployees, decimal totalDuration, decimal preparatoryPeriod,
            decimal roundedDuration, decimal acceptanceTime, bool acceptanceTimeIncluded, bool roundingIncluded)
        {
            Duration = duration;
            LaborCosts = laborCosts;
            WorkingDayDuration = workingDayDuration;
            Shift = shift;
            NumberOfWorkingDays = numberOfWorkingDaysInMonth;
            NumberOfEmployees = numberOfEmployees;
            RoundedDuration = roundedDuration;
            TotalDuration = totalDuration;
            PreparatoryPeriod = preparatoryPeriod;
            AcceptanceTime = acceptanceTime;
            AcceptanceTimeIncluded = acceptanceTimeIncluded;
            RoundingIncluded = roundingIncluded;
        }
    }
}
