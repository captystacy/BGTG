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

        services.AddSingleton<IECPProjectWriter, ECPProjectWriter>();

        services.AddScoped<IProjectService, ProjectService>();

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