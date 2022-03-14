using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.CalendarPlanTool;
using POS.Infrastructure.Tools.CalendarPlanTool.Base;

namespace POS.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void CalendarPlan(IServiceCollection services)
    {
        services.AddTransient<IConstructionMonthsCreator, ConstructionMonthsCreator>();
        services.AddTransient<ICalendarWorkCreator, CalendarWorkCreator>();
        services.AddTransient<ICalendarWorksProvider, CalendarWorksProvider>();
        services.AddTransient<ICalendarPlanCreator, CalendarPlanCreator>();
        services.AddTransient<ICalendarPlanWriter, CalendarPlanWriter>();

        services.AddTransient<ICalendarPlanService, CalendarPlanService>();
    }
}