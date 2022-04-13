using POS.Infrastructure.Calculators;
using POS.Infrastructure.Calculators.Base;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Appenders;
using POS.Infrastructure.Appenders.Base;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Replacers.Base;

namespace POS.Infrastructure.DependencyInjection
{
    public partial class DependencyContainer
    {
        public static void EnergyAndWater(IServiceCollection services)
        {
            services.AddTransient<IEnergyAndWaterCalculator, EnergyAndWaterCalculator>();

            services.AddTransient<IEnergyAndWaterAppender, EnergyAndWaterAppender>();

            services.AddTransient<IEnergyAndWaterReplacer, EnergyAndWaterReplacer>();

            services.AddTransient<IEnergyAndWaterService, EnergyAndWaterService>();
        }
    }
}