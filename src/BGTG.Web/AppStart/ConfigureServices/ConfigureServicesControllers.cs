using Calabonga.AspNetCore.Controllers.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BGTG.Web.AppStart.ConfigureServices
{
    /// <summary>
    /// Configure controllers
    /// </summary>
    public static class ConfigureServicesControllers
    {
        /// <summary>
        /// Configure services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        public static void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment)
        {
            var builder = services.AddControllersWithViews();

            if (environment.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }
        }
    }
}
