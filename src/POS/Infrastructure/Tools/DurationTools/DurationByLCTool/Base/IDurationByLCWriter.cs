namespace POS.Infrastructure.Tools.DurationTools.DurationByLCTool.Base;

public interface IDurationByLCWriter
{
    MemoryStream Write(DurationByLC durationByLC, string templatePath);
}