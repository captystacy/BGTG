using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Tools.ProjectTool;
using POS.Infrastructure.Tools.TableOfContentsTool;
using POS.Infrastructure.Tools.TitlePageTool;

namespace POS.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void POS(IServiceCollection services)
    {
        #region Project tool

        services.AddTransient<IECPProjectWriter, ECPProjectWriter>();

        services.AddTransient<IProjectService, ProjectService>();

        #endregion

        #region Title page tool

        services.AddTransient<ITitlePageWriter, TitlePageWriter>();

        services.AddTransient<ITitlePageService, TitlePageService>();

        #endregion

        #region Table of contents tool

        services.AddTransient<ITableOfContentsWriter, TableOfContentsWriter>();

        services.AddTransient<ITableOfContentsService, TableOfContentsService>();

        #endregion
    }
}