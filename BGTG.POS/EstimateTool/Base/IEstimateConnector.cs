using System.Collections.Generic;

namespace BGTG.POS.EstimateTool.Base
{
    public interface IEstimateConnector
    {
        Estimate Connect(List<Estimate> estimates);
    }
}
