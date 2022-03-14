using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.EnergyAndWaterTool;
using POS.Infrastructure.Tools.EnergyAndWaterTool.Base;

namespace POS.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void EnergyAndWater(IServiceCollection services)
    {
        services.AddTransient<IEnergyAndWaterCreator, EnergyAndWaterCreator>();
        services.AddTransient<IEnergyAndWaterWriter, EnergyAndWaterWriter>();

        services.AddTransient<IEnergyAndWaterService, EnergyAndWaterService>();
    }
}