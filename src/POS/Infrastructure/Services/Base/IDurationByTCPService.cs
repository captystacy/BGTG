using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Models;
using POS.ViewModels;

namespace POS.Infrastructure.Services.Base;

public interface IDurationByTCPService
{
    DurationByTCP? Write(DurationByTCPCreateViewModel viewModel);
    DurationByTCP Write(DurationByTCP durationByTCP);
}