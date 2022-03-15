using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface IProjectService
{
    MemoryStream? Write(ProjectViewModel dto);
}