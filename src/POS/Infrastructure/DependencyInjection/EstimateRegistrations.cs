using POS.Infrastructure.Connectors;
using POS.Infrastructure.Parsers;
using POS.Infrastructure.Parsers.Base;
using POS.Infrastructure.Readers;
using POS.Infrastructure.Readers.Base;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;

namespace POS.Infrastructure.DependencyInjection
{
    public partial class DependencyContainer
    {
        public static void Estimate(IServiceCollection services)
        {
            services.AddTransient<IEstimateReader, EstimateReader>();

            services.AddTransient<IEstimateConnector, EstimateConnector>();

            services.AddTransient<IEstimateParser, EstimateParser>();

            services.AddTransient<IEstimateService, EstimateService>();
        }
    }
}