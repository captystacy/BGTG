using POS.Infrastructure.Creators;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;
using POS.Infrastructure.Writers.Base;

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