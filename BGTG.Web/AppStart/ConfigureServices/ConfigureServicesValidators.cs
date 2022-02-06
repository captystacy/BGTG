using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BGTG.Web.AppStart.ConfigureServices
{
    public static class ConfigureServicesValidators
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
        }
    }
}