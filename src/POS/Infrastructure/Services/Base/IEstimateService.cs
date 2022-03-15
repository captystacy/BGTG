using POS.DomainModels.EstimateDomainModels;

namespace POS.Infrastructure.Services.Base;

public interface IEstimateService
{
    Estimate Estimate { get; }
    void Read(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter);
}