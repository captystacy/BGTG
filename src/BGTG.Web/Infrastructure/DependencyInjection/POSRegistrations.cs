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
using BGTG.Web.Infrastructure.Providers.POSProviders;
using BGTG.Web.Infrastructure.Providers.POSProviders.Base;
using BGTG.Web.Infrastructure.Services.POSServices;
using BGTG.Web.Infrastructure.Services.POSServices.Base;
using Microsoft.Extensions.DependencyInjection;

namespace BGTG.Web.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void POS(IServiceCollection services)
    {
        #region Estimate tool

        services.AddSingleton<IEstimateReader, EstimateReader>();
        services.AddSingleton<IEstimateConnector, EstimateConnector>();
        services.AddSingleton<IEstimateManager, EstimateManager>();

        services.AddScoped<IEstimateService, EstimateService>();

        #endregion

        #region Calendar plan tool

        services.AddSingleton<IConstructionMonthsCreator, ConstructionMonthsCreator>();
        services.AddSingleton<ICalendarWorkCreator, CalendarWorkCreator>();
        services.AddSingleton<ICalendarWorksProvider, CalendarWorksProvider>();
        services.AddSingleton<ICalendarPlanCreator, CalendarPlanCreator>();
        services.AddSingleton<ICalendarPlanWriter, CalendarPlanWriter>();

        services.AddScoped<ICalendarPlanService, CalendarPlanService>();

        #endregion

        #region Energy and water tool

        services.AddSingleton<IEnergyAndWaterCreator, EnergyAndWaterCreator>();
        services.AddSingleton<IEnergyAndWaterWriter, EnergyAndWaterWriter>();

        services.AddScoped<IEnergyAndWaterService, EnergyAndWaterService>();

        #endregion

        #region Duration by LC

        services.AddSingleton<IDurationByLCCreator, DurationByLCCreator>();
        services.AddSingleton<IDurationRounder, DurationRounder>();
        services.AddSingleton<IDurationByLCWriter, DurationByLCWriter>();

        services.AddScoped<IDurationByLCService, DurationByLCService>();

        #endregion

        #region Duration by TCP

        services.AddSingleton<ITCP212Helper, TCP212Helper>();
        services.AddSingleton<IDurationByTCPEngineer, DurationByTCPEngineer>();
        services.AddSingleton<IDurationByTCPWriter, DurationByTCPWriter>();
        services.AddSingleton<IDurationByTCPCreator, DurationByTCPCreator>();

        services.AddScoped<IDurationByTCPService, DurationByTCPService>();

        #endregion

        #region Project tool

        services.AddSingleton<IECPProjectWriter, ECPProjectWriter>();

        services.AddScoped<IProjectProvider, ProjectProvider>();

        #endregion

        #region Title page tool

        services.AddSingleton<ITitlePageWriter, TitlePageWriter>();

        services.AddScoped<ITitlePageService, TitlePageService>();

        #endregion

        #region Table of contents tool

        services.AddSingleton<ITableOfContentsWriter, TableOfContentsWriter>();

        services.AddScoped<ITableOfContentsService, TableOfContentsService>();

        #endregion
    }
}