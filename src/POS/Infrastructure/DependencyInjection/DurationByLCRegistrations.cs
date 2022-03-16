using POS.Infrastructure.Creators;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Rounders;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void DurationByLC(IServiceCollection services)
    {
        services.AddTransient<IDurationByLCCreator, DurationByLCCreator>();
        services.AddTransient<IDurationRounder, DurationRounder>();
        services.AddTransient<IDurationByLCWriter, DurationByLCWriter>();

        services.AddTransient<IDurationByLCService, DurationByLCService>();
    }
}