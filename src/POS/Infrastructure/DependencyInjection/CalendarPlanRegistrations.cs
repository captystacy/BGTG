using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.CalendarPlanTool;
using POS.Infrastructure.Tools.CalendarPlanTool.Base;

namespace POS.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void CalendarPlan(IServiceCollection services)
    {
        services.AddSingleton<IConstructionMonthsCreator, ConstructionMonthsCreator>();
        services.AddSingleton<ICalendarWorkCreator, CalendarWorkCreator>();
        services.AddSingleton<ICalendarWorksProvider, CalendarWorksProvider>();
        services.AddSingleton<ICalendarPlanCreator, CalendarPlanCreator>();
        services.AddSingleton<ICalendarPlanWriter, CalendarPlanWriter>();

        services.AddScoped<ICalendarPlanService, CalendarPlanService>();
    }
}