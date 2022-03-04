using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.EstimateTool.Base;
using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.Infrastructure.Services;

public class EstimateService : IEstimateService
{
    private readonly IEstimateManager _estimateManager;

    public Estimate Estimate { get; private set; } = null!;

    public EstimateService(IEstimateManager estimateManager)
    {
        _estimateManager = estimateManager;
    }

    public void Read(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter)
    {
        var estimateStreams = estimateFiles.Select(x => x.OpenReadStream());
        Estimate = _estimateManager.GetEstimate(estimateStreams, totalWorkChapter);
    }
}