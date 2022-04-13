using POS.Models;
using POS.ViewModels;

namespace POS.Infrastructure.Writers.Base
{
    public interface IProjectWriter
    {
        Task<MemoryStream> GetProjectStream(ProjectViewModel viewModel, DateTime constructionStartDate, EmployeesNeed employeesNeed, DurationByLC durationByLC);
    }
}