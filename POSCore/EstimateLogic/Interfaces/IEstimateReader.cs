using System.Collections.Generic;

namespace POSCore.EstimateLogic.Interfaces
{
    public interface IEstimateReader
    {
        IEnumerable<EstimateWork> Read(string filePath);
    }
}
