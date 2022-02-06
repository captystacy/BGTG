using System.Globalization;
using BGTG.Web.AppStart.Configures;
using BGTG.Web.AppStart.ConfigureServices;
using BGTG.Web.Infrastructure.DependencyInjection;
using Calabonga.UnitOfWork.Controllers.DependencyContainer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BGTG.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesBase.ConfigureServices(services, Configuration);
            ConfigureServicesAuthentication.ConfigureServices(services);
            ConfigureServicesSwagger.ConfigureServices(services, Configuration);
            ConfigureServicesCors.ConfigureServices(services, Configuration);
            ConfigureServicesControllers.ConfigureServices(services);
            ConfigureServicesValidators.ConfigureServices(services);

            DependencyContainer.Common(services);
            NimbleDependencyContainer.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AutoMapper.IConfigurationProvider mapper)
        {
            var ruCulture = new CultureInfo("ru-RU") { NumberFormat = { NumberDecimalSeparator = "." } };
            CultureInfo.DefaultThreadCurrentCulture = ruCulture;
            CultureInfo.DefaultThreadCurrentUICulture = ruCulture;
            ConfigureCommon.Configure(app, env, mapper);
            ConfigureEndpoints.Configure(app);
        }
    }
}
