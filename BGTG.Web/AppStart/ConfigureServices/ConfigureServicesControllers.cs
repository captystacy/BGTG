using BGTG.Web.Controllers.API;
using Microsoft.Extensions.DependencyInjection;

namespace BGTG.Web.AppStart.ConfigureServices
{
    public static class ConfigureServicesControllers
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<ConstructionObjectsController, ConstructionObjectsController>();
        }
    }
}
