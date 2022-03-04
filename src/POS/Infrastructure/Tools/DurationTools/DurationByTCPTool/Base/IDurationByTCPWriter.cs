using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Models;

namespace POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Base;

public interface IDurationByTCPWriter
{
    void Write(DurationByTCP durationByTCP, string templatePath);
}