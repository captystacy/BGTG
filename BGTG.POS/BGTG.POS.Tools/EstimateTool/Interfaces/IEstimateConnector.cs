using System.Collections.Generic;

namespace BGTG.POS.Tools.EstimateTool.Interfaces
{
    public interface IEstimateConnector
    {
        Estimate Connect(List<Estimate> estimates);
    }
}
