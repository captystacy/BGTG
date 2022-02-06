using Microsoft.AspNetCore.Builder;

namespace BGTG.Web.AppStart.Configures
{
    public static class ConfigureEndpoints
    {
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
