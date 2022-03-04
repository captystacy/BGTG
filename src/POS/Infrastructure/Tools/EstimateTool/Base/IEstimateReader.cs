using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.Infrastructure.Tools.EstimateTool.Base;

public interface IEstimateReader
{
    Estimate Read(Stream stream, TotalWorkChapter totalWorkChapter);
}