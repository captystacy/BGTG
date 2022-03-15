using POS.DomainModels.EstimateDomainModels;

namespace POS.Infrastructure.Connectors;

public interface IEstimateConnector
{
    Estimate Connect(List<Estimate> estimates);
}