using POS.DomainModels.EstimateDomainModels;

namespace POS.Infrastructure.Managers;

public interface IEstimateManager
{
    Estimate GetEstimate(IEnumerable<Stream> estimateStreams, TotalWorkChapter totalWorkChapter);
}