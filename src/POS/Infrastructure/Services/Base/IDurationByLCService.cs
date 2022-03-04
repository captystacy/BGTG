using POS.Infrastructure.Tools.DurationTools.DurationByLCTool;
using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface IDurationByLCService
{
    DurationByLC Write(DurationByLCCreateViewModel viewModel);
    DurationByLC Write(DurationByLC durationByLC);
}