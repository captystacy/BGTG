using POS.DomainModels;

namespace POS.Infrastructure.Creators.Base;

public interface IEmployeesNeedCreator
{
    EmployeesNeed Create(int numberOfEmployeesFromDuration, decimal shift);
}