using POS.Infrastructure.Appenders;
using POS.Infrastructure.Appenders.Base;
using POS.Infrastructure.Calculators;
using POS.Infrastructure.Calculators.Base;
using POS.Infrastructure.Engineers;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;

namespace POS.Infrastructure.DependencyInjection
{
    public partial class DependencyContainer
    {
        public static void DurationByTCP(IServiceCollection services)
        {
            services.AddTransient<IDurationByTCPEngineer, DurationByTCPEngineer>();

            services.AddTransient<IDurationByTCPAppender, DurationByTCPAppender>();

            services.AddTransient<IDurationByTCPCalculator, DurationByTCPCalculator>();

            services.AddTransient<IDurationByTCPService, DurationByTCPService>();
        }
    }
}