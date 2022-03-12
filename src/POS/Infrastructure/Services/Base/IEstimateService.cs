using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.Infrastructure.Services.Base;

public interface IEstimateService
{
    Estimate Estimate { get; }
    void Read(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter);
}