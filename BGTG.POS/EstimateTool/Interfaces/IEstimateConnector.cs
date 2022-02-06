using System.Collections.Generic;

namespace BGTG.POS.EstimateTool.Interfaces
{
    public interface IEstimateConnector
    {
        Estimate Connect(List<Estimate> estimates);
    }
}
