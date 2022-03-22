using POS.DomainModels;
using POS.Infrastructure.Creators.Base;

namespace POS.Infrastructure.Creators;

public class EmployeesNeedCreator : IEmployeesNeedCreator
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

    public EmployeesNeed Create(int numberOfEmployeesFromDuration, decimal shift)
    {
        var totalNumberOfEmployees = decimal.Ceiling(numberOfEmployeesFromDuration * shift);
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

        return new EmployeesNeed((int)totalNumberOfEmployees, numberOfWorkingEmployees, numberOfManagers, foremanRoom, dressingRoom, washingRoom, washingCrane, showerRoom, showerMesh, toilet, foodPoint);
    }
}