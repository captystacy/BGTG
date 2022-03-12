using POS.Infrastructure.Tools.EstimateTool.Base;
using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.Infrastructure.Tools.EstimateTool;

public class EstimateManager : IEstimateManager
{
    private readonly IEstimateReader _estimateReader;
    private readonly IEstimateConnector _estimateConnector;

    public EstimateManager(IEstimateReader estimateReader, IEstimateConnector estimateConnector)
    {
        _estimateReader = estimateReader;
        _estimateConnector = estimateConnector;
    }

    public Estimate GetEstimate(IEnumerable<Stream> estimateStreams, TotalWorkChapter totalWorkChapter)
    {
        var estimates = estimateStreams.Select(s => _estimateReader.Read(s, totalWorkChapter));
        return _estimateConnector.Connect(estimates.ToList());
    }
}