using NUnit.Framework;
using POS.DomainModels;
using POS.Infrastructure.Creators;

namespace POS.Tests.Infrastructure.Creators;

public class EmployeesNeedCreatorTests
{
    private EmployeesNeedCreator _employeesNeedCreator = null!;

    [SetUp]
    public void SetUp()
    {
        _employeesNeedCreator = new EmployeesNeedCreator();
    }

    [TestCase(4, 1.5, 6, 5, 1, 4, 4.2, 0.3, 1, 2.58, 1, 1, 12)]
    [TestCase(8, 1.5, 12, 10, 2, 8, 8.4, 0.6, 1, 5.16, 1, 1, 12)]
    public void Create_NumberOfEmployeesFromDurationIsFour(int numberOfEmployeesFromDuration, decimal shift, int totalNumberOfEmployees, int numberOfWorkingEmployees, int numberOfManagers, 
        decimal foremanRoom, decimal dressingRoom, decimal washingRoom, int washingCrane, decimal showerRoom, int showerMesh, int toilet, decimal foodPoint)
    {
        // arrange

        var expectedEmployeesNeed = new EmployeesNeed(totalNumberOfEmployees, numberOfWorkingEmployees, numberOfManagers, foremanRoom, dressingRoom, washingRoom, washingCrane, showerRoom, showerMesh, toilet, foodPoint);

        // act

        var actualEmployeesNeed = _employeesNeedCreator.Create(numberOfEmployeesFromDuration, shift);

        // assert

        Assert.AreEqual(expectedEmployeesNeed, actualEmployeesNeed);
    }
}