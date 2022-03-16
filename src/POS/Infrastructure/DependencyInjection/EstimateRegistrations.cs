using POS.Infrastructure.Connectors;
using POS.Infrastructure.Managers;
using POS.Infrastructure.Readers;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;

namespace POS.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void Estimate(IServiceCollection services)
    {
        services.AddTransient<IEstimateReader, EstimateReader>();
        services.AddTransient<IEstimateConnector, EstimateConnector>();
        services.AddTransient<IEstimateManager, EstimateManager>();

        services.AddTransient<IEstimateService, EstimateService>();
    }
}