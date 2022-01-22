using System.Collections.Generic;
using System.IO;

namespace BGTG.POS.Tools.EstimateTool.Interfaces
{
    public interface IEstimateManager
    {
        Estimate GetEstimate(IEnumerable<Stream> estimateStreams, TotalWorkChapter totalWorkChapter);
    }
}
