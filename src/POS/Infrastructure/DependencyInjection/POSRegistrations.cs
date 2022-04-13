using POS.Infrastructure.Parsers;
using POS.Infrastructure.Parsers.Base;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Replacers.Base;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.DependencyInjection
{
    public partial class DependencyContainer
    {
        public static void POS(IServiceCollection services)
        {
            services.AddTransient<IProjectWriter, ProjectWriter>();
            services.AddTransient<ITitlePageWriter, TitlePageWriter>();
            services.AddTransient<ITableOfContentsWriter, TableOfContentsWriter>();

            services.AddTransient<IProjectService, ProjectService>();

            services.AddTransient<IConstructionParser, ConstructionParser>();

            services.AddTransient<IProjectReplacer, ProjectReplacer>();
            services.AddTransient<IEngineerReplacer, EngineerReplacer>();
            services.AddTransient<ITechnicalAndEconomicalIndicatorsReplacer, TechnicalAndEconomicalIndicatorsReplacer>();
        }
    }
}