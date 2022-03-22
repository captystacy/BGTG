using POS.ViewModels;

namespace POS.Infrastructure.Writers.Base;

public interface ILCProjectWriter
{
    MemoryStream Write(ProjectViewModel viewModel);
}