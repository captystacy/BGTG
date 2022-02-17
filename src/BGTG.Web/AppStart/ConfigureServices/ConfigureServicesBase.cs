using BGTG.Data;
using BGTG.Web.Extensions;
using BGTG.Web.Infrastructure.Settings;
using Calabonga.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BGTG.Web.AppStart.ConfigureServices;

public static class ConfigureServicesBase
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<ApplicationDbContext>(config =>
        {
            config.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDbContext)));
        });

        services.AddAutoMapper(typeof(Startup));
        services.AddUnitOfWork<ApplicationDbContext>();
        services.AddMemoryCache();
        services.AddRouting(x => x.LowercaseUrls = true);
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        services.AddOptions();
        services.Configure<CurrentAppSettings>(configuration.GetSection(nameof(CurrentAppSettings)));
        services.Configure<MvcOptions>(options => options.UseRouteSlugify());
        services.AddLocalization();
        services.AddHttpContextAccessor();
        services.AddResponseCaching();
    }
}