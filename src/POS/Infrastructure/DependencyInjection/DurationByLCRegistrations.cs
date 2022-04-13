using POS.Infrastructure.Calculators;
using POS.Infrastructure.Calculators.Base;
using POS.Infrastructure.Rounders;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Appenders;
using POS.Infrastructure.Appenders.Base;
using POS.Infrastructure.Readers;
using POS.Infrastructure.Readers.Base;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Replacers.Base;

namespace POS.Infrastructure.DependencyInjection
{
    public partial class DependencyContainer
    {
        public static void DurationByLC(IServiceCollection services)
        {
            services.AddTransient<IDurationByLCCalculator, DurationByLCCalculator>();

            services.AddTransient<IDurationRounder, DurationRounder>();

            services.AddTransient<IDurationByLCAppender, DurationByLCAppender>();

            services.AddTransient<IDurationByLCReader, DurationByLCReader>();

            services.AddTransient<IDurationByLCReplacer, DurationByLCReplacer>();

            services.AddTransient<IDurationByLCService, DurationByLCService>();
        }
    }
}