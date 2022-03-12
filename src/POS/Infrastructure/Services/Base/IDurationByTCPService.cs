using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface IDurationByTCPService
{
    MemoryStream? Write(DurationByTCPViewModel viewModel);
}