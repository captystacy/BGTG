using Microsoft.Extensions.DependencyInjection;
using POSCore.CalendarPlanLogic;
using POSCore.CalendarPlanLogic.Interfaces;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using POSUI.Services;
using POSUI.Services.Interfaces;
using System.Windows;

namespace POSUI
{
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            RegisterServices(services);

            services.AddSingleton<EstimatePickWindow>();
            services.AddSingleton<CalendarPlanWindow>();
        }

        private void RegisterServices(ServiceCollection services)
        {
            services.AddSingleton<IEstimateReader>(new EstimateReader());
            services.AddSingleton<IEstimateConnector>(new EstimateConnector());
            services.AddSingleton<IConstructionPeriodCreator>(new ConstructionPeriodCreator());
            services.AddSingleton<ICalendarWorkCreator>(x => new CalendarWorkCreator(x.GetService<IConstructionPeriodCreator>()));
            services.AddSingleton<ICalendarPlanCreator>(x => new CalendarPlanCreator(x.GetService<ICalendarWorkCreator>()));

            services.AddSingleton<ICalendarPlanService>(x => new CalendarPlanService(
                    x.GetService<IEstimateReader>(),
                    x.GetService<ICalendarWorkCreator>(),
                    x.GetService<IEstimateConnector>(),
                    x.GetService<ICalendarPlanCreator>()
                ));
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var estimatePickWindow = serviceProvider.GetService<EstimatePickWindow>();
            estimatePickWindow.Show();
        }
    }
}
