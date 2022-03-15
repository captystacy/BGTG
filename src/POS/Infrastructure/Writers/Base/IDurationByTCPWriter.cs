using POS.DomainModels.DurationByTCPDomainModels;

namespace POS.Infrastructure.Writers.Base;

public interface IDurationByTCPWriter
{
    MemoryStream Write(DurationByTCP durationByTCP, string templatePath);
}