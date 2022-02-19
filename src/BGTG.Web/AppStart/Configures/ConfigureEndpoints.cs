using Microsoft.AspNetCore.Builder;

namespace BGTG.Web.AppStart.Configures
{
    /// <summary>
    /// Configure pipeline
    /// </summary>
    public static class ConfigureEndpoints
    {
        /// <summary>
        /// Configure Routing
        /// </summary>
        /// <param name="app"></param>
        public static void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "index",
                    "{controller=ConstructionObjects}/{action=Index}/{objectCipher:regex([\\d\\.-])}/{pageIndex:int?}");

                endpoints.MapControllerRoute(
                    "index",
                    "{controller=ConstructionObjects}/{action=Index}/{pageIndex:int?}");

                endpoints.MapControllerRoute(
                    "default",
                    "{controller=ConstructionObjects}/{action=Index}/{id?}");
            });
        }
    }
}
