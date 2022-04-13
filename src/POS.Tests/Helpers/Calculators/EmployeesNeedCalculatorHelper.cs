using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Calculators.Base;
using POS.Models;

namespace POS.Tests.Helpers.Calculators
{
    public static class EmployeesNeedCalculatorHelper
    {
        public static Mock<IEmployeesNeedCalculator> GetMock()
        {
            return new Mock<IEmployeesNeedCalculator>();
        }

        public static Mock<IEmployeesNeedCalculator> GetMock(int numberOfEmployees, decimal shift, EmployeesNeed employeesNeed)
        {
            var employeesNeedCalculator = GetMock();

            employeesNeedCalculator
                .Setup(x => x.Calculate(numberOfEmployees, shift))
                .ReturnsAsync(new OperationResult<EmployeesNeed> { Result = employeesNeed });

            return employeesNeedCalculator;
        }
    }
}
