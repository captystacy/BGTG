using System.IO;

namespace POS.EstimateLogic.Interfaces
{
    public interface IEstimateReader
    {
        Estimate Read(Stream stream, TotalWorkChapter totalWorkChapter);
    }
}
