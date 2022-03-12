using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.DurationTools;
using POS.Infrastructure.Tools.DurationTools.Base;
using POS.Infrastructure.Tools.DurationTools.DurationByLCTool;
using POS.Infrastructure.Tools.DurationTools.DurationByLCTool.Base;

namespace POS.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void DurationByLC(IServiceCollection services)
    {
        services.AddSingleton<IDurationByLCCreator, DurationByLCCreator>();
        services.AddSingleton<IDurationRounder, DurationRounder>();
        services.AddSingleton<IDurationByLCWriter, DurationByLCWriter>();

        services.AddScoped<IDurationByLCService, DurationByLCService>();
    }
}