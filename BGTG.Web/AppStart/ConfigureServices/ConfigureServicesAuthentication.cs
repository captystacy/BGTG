using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.DependencyInjection;

namespace BGTG.Web.AppStart.ConfigureServices
{
    public static class ConfigureServicesAuthentication
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate();
            services.AddAuthorization(x => x.FallbackPolicy = x.DefaultPolicy);
        }
    }
}