using BGTG.Data;
using BGTG.Data.CustomRepositories;
using BGTG.POS.CalendarPlanTool;
using BGTG.POS.CalendarPlanTool.Interfaces;
using BGTG.POS.DurationTools;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.POS.DurationTools.DurationByLCTool.Interfaces;
using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.POS.DurationTools.DurationByTCPTool.Interfaces;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP.Interfaces;
using BGTG.POS.DurationTools.Interfaces;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.POS.EnergyAndWaterTool.Interfaces;
using BGTG.POS.EstimateTool;
using BGTG.POS.EstimateTool.Interfaces;
using BGTG.POS.ProjectTool;
using BGTG.POS.ProjectTool.Interfaces;
using BGTG.POS.TableOfContentsTool;
using BGTG.POS.TableOfContentsTool.Interfaces;
using BGTG.POS.TitlePageTool;
using BGTG.POS.TitlePageTool.Interfaces;
using BGTG.Web.Infrastructure.Services;
using BGTG.Web.Infrastructure.Services.Interfaces;
using BGTG.Web.Infrastructure.Services.POSServices;
using BGTG.Web.Infrastructure.Services.POSServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BGTG.Web.Infrastructure.DependencyInjection
{
    public partial class DependencyContainer
    {
        public static void Common(IServiceCollection services)
        {
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();

            #region POS
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

            services.AddSingleton<IECPProjectWriter, ECPProjectWriter>();
            services.AddSingleton<ITitlePageWriter, TitlePageWriter>();
            services.AddSingleton<ITableOfContentsWriter, TableOfContentsWriter>();

            services.AddScoped<IEstimateService, EstimateService>();
            services.AddScoped<ICalendarPlanService, CalendarPlanService>();
            services.AddScoped<IEnergyAndWaterService, EnergyAndWaterService>();
            services.AddScoped<IDurationByLCService, DurationByLCService>();
            services.AddScoped<IDurationByTCPService, DurationByTCPService>();

            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITitlePageService, TitlePageService>();
            services.AddScoped<ITableOfContentsService, TableOfContentsService>();
            #endregion

            // services
            services.AddTransient<ICacheService, CacheService>();
        }
    }


}
