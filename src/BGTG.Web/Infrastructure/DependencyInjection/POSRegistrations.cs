using BGTG.POS.CalendarPlanTool;
using BGTG.POS.CalendarPlanTool.Base;
using BGTG.POS.DurationTools;
using BGTG.POS.DurationTools.Base;
using BGTG.POS.DurationTools.DurationByLCTool;
using BGTG.POS.DurationTools.DurationByLCTool.Base;
using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.POS.DurationTools.DurationByTCPTool.Base;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP.Interfaces;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.POS.EnergyAndWaterTool.Base;
using BGTG.POS.EstimateTool;
using BGTG.POS.EstimateTool.Base;
using BGTG.POS.ProjectTool;
using BGTG.POS.ProjectTool.Base;
using BGTG.POS.TableOfContentsTool;
using BGTG.POS.TableOfContentsTool.Base;
using BGTG.POS.TitlePageTool;
using BGTG.POS.TitlePageTool.Base;
using BGTG.Web.Infrastructure.Services.POS;
using BGTG.Web.Infrastructure.Services.POS.Base;
using Microsoft.Extensions.DependencyInjection;

namespace BGTG.Web.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void POS(IServiceCollection services)
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
    }
}