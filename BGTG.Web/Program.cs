using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BGTG.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .ConfigureWebHost(x => x.ConfigureKestrel(x => x.ListenAnyIP(5000)))
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
