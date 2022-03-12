using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Models;

namespace POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Base;

public interface IDurationByTCPWriter
{
    MemoryStream Write(DurationByTCP durationByTCP, string templatePath);
}