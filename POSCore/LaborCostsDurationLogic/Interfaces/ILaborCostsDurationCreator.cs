namespace POSCore.LaborCostsDurationLogic.Interfaces
{
    public interface ILaborCostsDurationCreator
    {
        LaborCostsDuration Create(decimal laborCosts, decimal workingDayDuration, decimal shift, decimal numberOfWorkingDaysInMonth, int numberOfEmployees, bool acceptanceTimeIncluded);
        decimal GetDuration(decimal laborCosts, decimal workingDayDuration, decimal shift, decimal numberOfWorkingDaysInMonth, int numberOfEmployees);
    }
}
