using POS.DomainModels.EstimateDomainModels;
using POS.Infrastructure.Managers;
using POS.Infrastructure.Services.Base;

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