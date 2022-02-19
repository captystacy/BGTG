using BGTG.Data;
using Calabonga.UnitOfWork;
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
        services.AddRouting(x =>
        {
            x.LowercaseQueryStrings = true;
            x.LowercaseUrls = true;
        });
        services.AddHttpContextAccessor();

        services.AddWebOptimizer(x =>
        {
            x.AddCssBundle("/css/bundle.css", "css/**/*.css");
            x.AddJavaScriptBundle("/js/bundle.js", "js/**/*.js");
        });
    }
}