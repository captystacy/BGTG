using POS.Infrastructure.Appenders;
using POS.Infrastructure.Appenders.Base;
using POS.Infrastructure.Calculators;
using POS.Infrastructure.Calculators.Base;
using POS.Infrastructure.Readers;
using POS.Infrastructure.Readers.Base;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Replacers.Base;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;

namespace POS.Infrastructure.DependencyInjection
{
    public partial class DependencyContainer
    {
        public static void CalendarPlan(IServiceCollection services)
        {
            services.AddTransient<ICalendarWorkCalculator, CalendarWorkCalculator>();
            services.AddTransient<IConstructionMonthsCalculator, ConstructionMonthsCalculator>();
            services.AddTransient<ICalendarPlanCalculator, CalendarPlanCalculator>();

            services.AddTransient<ICalendarPlanReader, CalendarPlanReader>();

            services.AddTransient<ICalendarPlanReplacer, CalendarPlanReplacer>();

            services.AddTransient<ICalendarPlanAppender, CalendarPlanAppender>();

            services.AddTransient<ICalendarPlanService, CalendarPlanService>();
        }
    }
}