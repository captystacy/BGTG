using System.Collections.Generic;
using System.IO;

namespace POSCore.EstimateLogic.Interfaces
{
    public interface IEstimateManager
    {
        Estimate GetEstimate(IEnumerable<Stream> estimateStreams);
    }
}
