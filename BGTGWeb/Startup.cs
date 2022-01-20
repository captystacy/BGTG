using AutoMapper;
using BGTGWeb.MapProfiles;
using BGTGWeb.Services;
using BGTGWeb.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POS.CalendarPlanLogic;
using POS.CalendarPlanLogic.Interfaces;
using POS.DurationLogic;
using POS.DurationLogic.DurationByLaborCosts;
using POS.DurationLogic.DurationByLaborCosts.Interfaces;
using POS.DurationLogic.DurationByTCP;
using POS.DurationLogic.DurationByTCP.Interfaces;
using POS.DurationLogic.DurationByTCP.TCP;
using POS.DurationLogic.DurationByTCP.TCP.Interfaces;
using POS.DurationLogic.Interfaces;
using POS.EnergyAndWaterLogic;
using POS.EnergyAndWaterLogic.Interfaces;
using POS.EstimateLogic;
using POS.EstimateLogic.Interfaces;

namespace BGTGWeb
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
            services.AddSingleton<IEstimateReader, EstimateReader>();
            services.AddSingleton<IEstimateConnector, EstimateConnector>();
            services.AddSingleton<IEstimateManager, EstimateManager>();

            services.AddSingleton<IConstructionMonthsCreator, ConstructionMonthsCreator>();
            services.AddSingleton<ICalendarWorkCreator, CalendarWorkCreator>();
            services.AddSingleton<ICalendarWorksProvider, CalendarWorksProvider>();
            services.AddSingleton<ICalendarPlanCreator, CalendarPlanCreator>();
            services.AddSingleton<ICalendarPlanWriter, CalendarPlanWriter>();

            services.AddSingleton<IEnergyAndWaterCreator, EnergyAndWaterCreator>();
            services.AddSingleton<IEnergyAndWaterWriter, EnergyAndWaterWriter>();

            services.AddSingleton<IDurationByLaborCostsCreator, DurationByLaborCostsCreator>();
            services.AddSingleton<IDurationRounder, DurationRounder>();
            services.AddSingleton<IDurationByLaborCostsWriter, DurationByLaborCostsWriter>();

            services.AddSingleton<ITCP212Helper, TCP212Helper>();
            services.AddSingleton<IDurationByTCPEngineer, DurationByTCPEngineer>();
            services.AddSingleton<IDurationByTCPWriter, DurationByTCPWriter>();
            services.AddSingleton<IDurationByTCPCreator, DurationByTCPCreator>();

            RegisterServices(services);
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PosProfile>();
            }).CreateMapper());

            services.AddRouting(x => x.LowercaseUrls = true);
            services.AddControllersWithViews();
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IEstimateService, EstimateService>();
            services.AddScoped<ICalendarPlanService, CalendarPlanService>();
            services.AddScoped<IEnergyAndWaterService, EnergyAndWaterService>();
            services.AddScoped<IDurationByLaborCostsService, DurationByLaborCostsService>();
            services.AddScoped<IDurationByTCPService, DurationByTCPService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
