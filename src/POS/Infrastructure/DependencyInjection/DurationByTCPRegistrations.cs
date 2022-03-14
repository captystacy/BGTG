using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.Base;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.TCP;
using POS.Infrastructure.Tools.DurationTools.DurationByTCPTool.TCP.Interfaces;

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