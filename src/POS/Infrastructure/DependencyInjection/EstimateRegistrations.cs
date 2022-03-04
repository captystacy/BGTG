using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.EstimateTool;
using POS.Infrastructure.Tools.EstimateTool.Base;

namespace POS.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void Estimate(IServiceCollection services)
    {
        services.AddSingleton<IEstimateReader, EstimateReader>();
        services.AddSingleton<IEstimateConnector, EstimateConnector>();
        services.AddSingleton<IEstimateManager, EstimateManager>();

        services.AddScoped<IEstimateService, EstimateService>();
    }
}