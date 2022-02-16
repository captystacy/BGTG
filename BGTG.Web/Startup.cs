using System.Globalization;
using BGTG.Web.AppStart.Configures;
using BGTG.Web.AppStart.ConfigureServices;
using BGTG.Web.Infrastructure.DependencyInjection;
using Calabonga.UnitOfWork.Controllers.DependencyContainer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BGTG.Web;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        ConfigureServicesBase.ConfigureServices(services, Configuration);
        ConfigureServicesAuthentication.ConfigureServices(services, Configuration);
        ConfigureServicesSwagger.ConfigureServices(services, Configuration);
        ConfigureServicesCors.ConfigureServices(services, Configuration);
        ConfigureServicesControllers.ConfigureServices(services, Environment);

        DependencyContainer.Common(services);
        DependencyContainer.BGTG(services);
        DependencyContainer.POS(services);
        NimbleDependencyContainer.ConfigureServices(services);
    }

    public void Configure(IApplicationBuilder app, AutoMapper.IConfigurationProvider mapper)
    {
        var ruCulture = new CultureInfo("ru-RU") { NumberFormat = { NumberDecimalSeparator = "." } };
        CultureInfo.DefaultThreadCurrentCulture = ruCulture;
        CultureInfo.DefaultThreadCurrentUICulture = ruCulture;
        ConfigureCommon.Configure(app, Environment, mapper);
        ConfigureEndpoints.Configure(app);
    }
}