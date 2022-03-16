using POS.Infrastructure.Creators;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Engineers;
using POS.Infrastructure.Helpers;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void DurationByTCP(IServiceCollection services)
    {
        services.AddTransient<ITCP212Helper, TCP212Helper>();
        services.AddTransient<IDurationByTCPEngineer, DurationByTCPEngineer>();
        services.AddTransient<IDurationByTCPWriter, DurationByTCPWriter>();
        services.AddTransient<IDurationByTCPCreator, DurationByTCPCreator>();

        services.AddTransient<IDurationByTCPService, DurationByTCPService>();
    }
}