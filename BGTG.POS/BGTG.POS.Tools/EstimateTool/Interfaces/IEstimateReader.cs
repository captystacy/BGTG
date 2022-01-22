using System.IO;

namespace BGTG.POS.Tools.EstimateTool.Interfaces
{
    public interface IEstimateReader
    {
        Estimate Read(Stream stream, TotalWorkChapter totalWorkChapter);
    }
}
