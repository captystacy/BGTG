using System.Threading.Tasks;
using POS.Infrastructure.Calculators;
using POS.Models;
using Xunit;

namespace POS.Tests.Infrastructure.Calculators
{
    public class EmployeesNeedCalculatorTests
    {
        [Theory]
        [InlineData(4, 1.5, 6, 5, 1, 4, 4.2, 0.3, 1, 2.58, 1, 1, 12)]
        [InlineData(8, 1.5, 12, 10, 2, 8, 8.4, 0.6, 1, 5.16, 1, 1, 12)]
        public async Task ItShould_calculate_correct_employees_need_creator(int numberOfEmployeesFromDuration, decimal shift, int totalNumberOfEmployees, int numberOfWorkingEmployees, int numberOfManagers, 
            decimal foremanRoom, decimal dressingRoom, decimal washingRoom, int washingCrane, decimal showerRoom, int showerMesh, int toilet, decimal foodPoint)
        {
            // arrange

            var expectedEmployeesNeed = new EmployeesNeed
            {
                WashingCrane = washingCrane,
                WashingRoom = washingRoom,
                DressingRoom = dressingRoom,
                FoodPoint = foodPoint,
                ForemanRoom = foremanRoom,
                NumberOfManagers = numberOfManagers,
                NumberOfWorkingEmployees = numberOfWorkingEmployees,
                ShowerMesh = showerMesh,
                ShowerRoom = showerRoom,
                Toilet = toilet,
                TotalNumberOfEmployees = totalNumberOfEmployees
            };

            var sut = new EmployeesNeedCalculator();

            // act

            var calculateOperation = await sut.Calculate(numberOfEmployeesFromDuration, shift);

            // assert

            Assert.True(calculateOperation.Ok);

            Assert.Equal(expectedEmployeesNeed, calculateOperation.Result);
        }
    }
}