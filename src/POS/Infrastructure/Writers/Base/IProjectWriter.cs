using POS.ViewModels;

namespace POS.Infrastructure.Writers.Base;

public interface IProjectWriter
{
    MemoryStream Write(ProjectViewModel viewModel);
}