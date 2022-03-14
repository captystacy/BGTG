using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.EstimateTool;
using POS.Infrastructure.Tools.EstimateTool.Base;

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