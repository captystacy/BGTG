namespace POS.DurationLogic.DurationByLaborCosts.Interfaces
{
    public interface IDurationByLaborCostsCreator
    {
        DurationByLaborCosts Create(decimal estimateLaborCosts, decimal technologicalLaborCosts, decimal workingDayDuration, decimal shift, decimal numberOfWorkingDaysInMonth, int numberOfEmployees, bool acceptanceTimeIncluded);
    }
}
