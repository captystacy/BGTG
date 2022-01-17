using System.Collections.Generic;
using System.IO;

namespace POS.EstimateLogic.Interfaces
{
    public interface IEstimateManager
    {
        Estimate GetEstimate(IEnumerable<Stream> estimateStreams, TotalWorkChapter totalWorkChapter);
    }
}
