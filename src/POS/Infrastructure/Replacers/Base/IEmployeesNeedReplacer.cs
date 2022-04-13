using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models;

namespace POS.Infrastructure.Replacers.Base
{
    public interface IEmployeesNeedReplacer
    {
        Task Replace(IMyWordDocument document, EmployeesNeed employeesNeed);
    }
}