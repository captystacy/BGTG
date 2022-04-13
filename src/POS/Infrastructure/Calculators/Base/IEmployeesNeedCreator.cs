using Calabonga.OperationResults;
using POS.Models;

namespace POS.Infrastructure.Calculators.Base
{
    public interface IEmployeesNeedCalculator
    {
        Task<OperationResult<EmployeesNeed>> Calculate(int numberOfEmployees, decimal shift);
    }
}