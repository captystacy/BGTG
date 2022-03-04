namespace POS.Infrastructure.Tools.DurationTools.DurationByLCTool.Base;

public interface IDurationByLCWriter
{
    void Write(DurationByLC durationByLC, string templatePath);
}