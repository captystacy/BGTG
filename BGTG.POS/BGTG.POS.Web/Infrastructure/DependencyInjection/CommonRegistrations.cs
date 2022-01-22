using BGTG.POS.Data;
using BGTG.POS.Tools.CalendarPlanTool;
using BGTG.POS.Tools.CalendarPlanTool.Interfaces;
using BGTG.POS.Tools.DurationTools;
using BGTG.POS.Tools.DurationTools.DurationByLCTool;
using BGTG.POS.Tools.DurationTools.DurationByLCTool.Interfaces;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.Interfaces;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.TCP;
using BGTG.POS.Tools.DurationTools.DurationByTCPTool.TCP.Interfaces;
using BGTG.POS.Tools.DurationTools.Interfaces;
using BGTG.POS.Tools.EnergyAndWaterTool;
using BGTG.POS.Tools.EnergyAndWaterTool.Interfaces;
using BGTG.POS.Tools.EstimateTool;
using BGTG.POS.Tools.EstimateTool.Interfaces;
using BGTG.POS.Web.Infrastructure.Services;
using BGTG.POS.Web.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BGTG.POS.Web.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Registrations for both points: API and Scheduler
    /// </summary>
    public partial class DependencyContainer
    {
        /// <summary>
        /// Register 
        /// </summary>
        /// <param name="services"></param>
        public static void Common(IServiceCollection services)
        {
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();

            #region POS.Tools
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

            services.AddSingleton<IDurationByLCCreator, DurationByLCCreator>();
            services.AddSingleton<IDurationRounder, DurationRounder>();
            services.AddSingleton<IDurationByLCWriter, DurationByLCWriter>();

            services.AddSingleton<ITCP212Helper, TCP212Helper>();
            services.AddSingleton<IDurationByTCPEngineer, DurationByTCPEngineer>();
            services.AddSingleton<IDurationByTCPWriter, DurationByTCPWriter>();
            services.AddSingleton<IDurationByTCPCreator, DurationByTCPCreator>();
            #endregion

            // services
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<ICacheService, CacheService>();

            services.AddScoped<IEstimateService, EstimateService>();
            services.AddScoped<ICalendarPlanService, CalendarPlanService>();
            services.AddScoped<IEnergyAndWaterService, EnergyAndWaterService>();
            services.AddScoped<IDurationByLCService, DurationByLCService>();
            services.AddScoped<IDurationByTCPService, DurationByTCPService>();
        }
    }


}
