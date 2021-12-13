using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.DurationLogic.LaborCosts;
using POSCore.DurationLogic.LaborCosts.Interfaces;
using POSCore.EnergyAndWaterLogic;
using POSCore.EnergyAndWaterLogic.Interfaces;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using POSWeb.Models;
using POSWeb.Presentations;
using POSWeb.Services;
using POSWeb.Services.Interfaces;
using System.Globalization;
using System.Linq;

namespace POSWeb
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
            RegisterServices(services);
            RegisterMapper(services);
            RegisterPresentations(services);

            services.AddControllersWithViews();
        }

        private void RegisterPresentations(IServiceCollection services)
        {
            services.AddSingleton(x => new CalendarPlanPresentation(
                x.GetService<ICalendarPlanService>(),
                x.GetService<IMapper>()));
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IEstimateReader>(new EstimateReader());
            services.AddSingleton<IEstimateConnector>(new EstimateConnector());
            services.AddSingleton<IEstimateManager>(x => new EstimateManager(
                x.GetService<IEstimateReader>(),
                x.GetService<IEstimateConnector>()
                ));
            services.AddSingleton<IConstructionPeriodCreator>(new ConstructionPeriodCreator());
            services.AddSingleton<ICalendarWorkCreator>(x => new CalendarWorkCreator(x.GetService<IConstructionPeriodCreator>()));
            services.AddSingleton<ICalendarPlanCreator>(x => new CalendarPlanCreator(x.GetService<ICalendarWorkCreator>()));
            services.AddSingleton<ICalendarPlanWriter>(new CalendarPlanWriter());

            services.AddSingleton<IEstimateService>(x => new EstimateService(x.GetService<IEstimateManager>()));

            services.AddSingleton<ICalendarPlanService>(x => new CalendarPlanService(
                x.GetService<IEstimateService>(),
                x.GetService<ICalendarPlanCreator>(),
                x.GetService<ICalendarPlanWriter>(),
                x.GetService<IWebHostEnvironment>()
                ));

            services.AddSingleton<IEnergyAndWaterCreator>(new EnergyAndWaterCreator());
            services.AddSingleton<IEnergyAndWaterWriter>(new EnergyAndWaterWriter());

            services.AddSingleton<IEnergyAndWaterService>(x => new EnergyAndWaterService(
                x.GetService<IEstimateService>(),
                x.GetService<IEnergyAndWaterCreator>(),
                x.GetService<IEnergyAndWaterWriter>(),
                x.GetService<IWebHostEnvironment>(),
                x.GetService<ICalendarWorkCreator>()
                ));

            services.AddSingleton<ILaborCostsDurationCreator>(new LaborCostsDurationCreator());
            services.AddSingleton<ILaborCostsDurationWriter>(new LaborCostsDurationWriter());

            services.AddSingleton<ILaborCostsDurationService>(x => new LaborCostsDurationService(
                x.GetService<IEstimateService>(),
                x.GetService<ILaborCostsDurationCreator>(),
                x.GetService<ILaborCostsDurationWriter>(),
                x.GetService<IWebHostEnvironment>()
                ));
        }

        private void RegisterMapper(IServiceCollection services)
        {
            var config = new MapperConfiguration(x =>
            {
                x.CreateMap<Estimate, CalendarPlanVM>()
                    .ForMember(d => d.UserWorks, o => o.MapFrom(s => s.MainEstimateWorks));
                x.CreateMap<EstimateWork, UserWork>();
                x.CreateMap<CalendarWork, UserWork>()
                    .ForMember(d => d.Percentages, o => o.MapFrom(s => s.ConstructionPeriod.ConstructionMonths.Select(x => x.PercentPart)));
            });
            services.AddSingleton(x => config.CreateMapper());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
