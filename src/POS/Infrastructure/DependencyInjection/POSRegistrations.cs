using POS.Infrastructure.Creators;
using POS.Infrastructure.Creators.Base;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Writers;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void POS(IServiceCollection services)
    {
        services.AddTransient<ILCProjectWriter, LCProjectWriter>();
        services.AddTransient<ITitlePageWriter, TitlePageWriter>();
        services.AddTransient<ITableOfContentsWriter, TableOfContentsWriter>();

        services.AddTransient<IEmployeesNeedCreator, EmployeesNeedCreator>();
        services.AddTransient<IEngineerReplacer, EngineerReplacer>();
    }
}