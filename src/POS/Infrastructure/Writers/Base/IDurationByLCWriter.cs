using POS.DomainModels;

namespace POS.Infrastructure.Writers.Base;

public interface IDurationByLCWriter
{
    MemoryStream Write(DurationByLC durationByLC, string templatePath);
}