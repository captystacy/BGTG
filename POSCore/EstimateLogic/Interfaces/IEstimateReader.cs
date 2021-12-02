using System.IO;

namespace POSCore.EstimateLogic.Interfaces
{
    public interface IEstimateReader
    {
        Estimate Read(Stream stream);
    }
}
