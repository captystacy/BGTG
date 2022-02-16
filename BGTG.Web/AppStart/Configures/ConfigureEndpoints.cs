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
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
