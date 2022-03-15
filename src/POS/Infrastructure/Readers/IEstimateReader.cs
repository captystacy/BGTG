using POS.DomainModels.EstimateDomainModels;

namespace POS.Infrastructure.Readers;

public interface IEstimateReader
{
    Estimate Read(Stream stream, TotalWorkChapter totalWorkChapter);
}