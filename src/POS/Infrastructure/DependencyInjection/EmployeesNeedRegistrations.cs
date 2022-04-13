using POS.Infrastructure.Calculators;
using POS.Infrastructure.Calculators.Base;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Replacers.Base;

namespace POS.Infrastructure.DependencyInjection
{
    public partial class DependencyContainer
    {
        public static void EmployeesNeed(IServiceCollection services)
        {
            services.AddTransient<IEmployeesNeedCalculator, EmployeesNeedCalculator>();

            services.AddTransient<IEmployeesNeedReplacer, EmployeesNeedReplacer>();
        }
    }
}
