using Calabonga.OperationResults;
using POS.Infrastructure.Calculators.Base;
using POS.Models;

namespace POS.Infrastructure.Calculators
{
    public class EmployeesNeedCalculator : IEmployeesNeedCalculator
    {
        private decimal WorkingEmployeesMultiplier = 0.845M;

        private decimal ForemanRoomMultiplier = 4M;
        private decimal DressingRoomMultiplier = 0.7M;
        private decimal WashingRoomMultiplier = 0.05M;
        private int WashingCraneNumberOfEmployees = 20;
        private decimal ShowerRoomMultiplier = 0.43M;
        private int ShowerMeshNumberOfEmployees = 15;
        private int ToiletNumberOfEmployees = 18;
        private decimal MinimalFoodPoint = 12;

        public Task<OperationResult<EmployeesNeed>> Calculate(int numberOfEmployees, decimal shift)
        {
            var operation = OperationResult.CreateResult<EmployeesNeed>();

            if (numberOfEmployees <= 0)
            {
                operation.AddError("Number of employees could not be less or equal zero");
                return Task.FromResult(operation);
            }

            if (shift <= 0)
            {
                operation.AddError("Shift could not be less or equal zero");
                return Task.FromResult(operation);
            }

            var totalNumberOfEmployees = decimal.Ceiling(numberOfEmployees * shift);
            var numberOfWorkingEmployees = (int)(totalNumberOfEmployees * WorkingEmployeesMultiplier);
            var numberOfManagers = (int)(totalNumberOfEmployees - numberOfWorkingEmployees);

            var foremanRoom = numberOfManagers * ForemanRoomMultiplier;
            var dressingRoom = totalNumberOfEmployees * DressingRoomMultiplier;
            var washingRoom = totalNumberOfEmployees * WashingRoomMultiplier;
            var washingCrane = (int)decimal.Ceiling(totalNumberOfEmployees / WashingCraneNumberOfEmployees);
            var showerRoom = totalNumberOfEmployees * ShowerRoomMultiplier;
            var showerMesh = (int)decimal.Ceiling(totalNumberOfEmployees / ShowerMeshNumberOfEmployees);
            var toilet = (int)decimal.Ceiling(totalNumberOfEmployees / ToiletNumberOfEmployees);
            var foodPoint = totalNumberOfEmployees < MinimalFoodPoint
                ? MinimalFoodPoint
                : totalNumberOfEmployees;

            operation.Result = new EmployeesNeed
            {
                DressingRoom = dressingRoom,
                FoodPoint = foodPoint,
                ForemanRoom = foremanRoom,
                NumberOfManagers = numberOfManagers,
                NumberOfWorkingEmployees = numberOfWorkingEmployees,
                ShowerMesh = showerMesh,
                ShowerRoom = showerRoom,
                Toilet = toilet,
                TotalNumberOfEmployees = (int) totalNumberOfEmployees,
                WashingCrane = washingCrane,
                WashingRoom = washingRoom,
            };

            return Task.FromResult(operation);
        }
    }
}