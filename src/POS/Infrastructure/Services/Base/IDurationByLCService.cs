using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface IDurationByLCService
{
    MemoryStream Write(DurationByLCViewModel viewModel);
}