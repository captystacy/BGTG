using POS.Infrastructure.Creators;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void CalendarPlan(IServiceCollection services)
    {
        services.AddTransient<IConstructionMonthsCreator, ConstructionMonthsCreator>();
        services.AddTransient<ICalendarWorkCreator, CalendarWorkCreator>();
        services.AddTransient<ICalendarWorksCreator, CalendarWorksCreator>();
        services.AddTransient<ICalendarPlanCreator, CalendarPlanCreator>();
        services.AddTransient<ICalendarPlanWriter, CalendarPlanWriter>();

        services.AddTransient<ICalendarPlanService, CalendarPlanService>();
    }
}