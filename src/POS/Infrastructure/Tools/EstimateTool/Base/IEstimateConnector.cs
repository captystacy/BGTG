using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.Infrastructure.Tools.EstimateTool.Base;

public interface IEstimateConnector
{
    Estimate Connect(List<Estimate> estimates);
}