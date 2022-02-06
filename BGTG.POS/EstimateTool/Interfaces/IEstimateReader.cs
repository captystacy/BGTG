using System.IO;

namespace BGTG.POS.EstimateTool.Interfaces
{
    public interface IEstimateReader
    {
        Estimate Read(Stream stream, TotalWorkChapter totalWorkChapter);
    }
}
