using System.IO;

namespace BGTG.POS.EstimateTool.Base
{
    public interface IEstimateReader
    {
        Estimate Read(Stream stream, TotalWorkChapter totalWorkChapter);
    }
}
