using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.Infrastructure.Tools.EstimateTool.Base;

public interface IEstimateManager
{
    Estimate GetEstimate(IEnumerable<Stream> estimateStreams, TotalWorkChapter totalWorkChapter);
}