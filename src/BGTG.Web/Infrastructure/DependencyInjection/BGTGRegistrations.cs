using BGTG.Web.Infrastructure.Services;
using BGTG.Web.Infrastructure.Services.Base;
using BGTG.Web.Infrastructure.TagHelpers.PagedListTagHelper;
using BGTG.Web.Infrastructure.TagHelpers.PagedListTagHelper.Base;
using Microsoft.Extensions.DependencyInjection;

namespace BGTG.Web.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void BGTG(IServiceCollection services)
    {
        services.AddScoped<IConstructionObjectService, ConstructionObjectService>();

        services.AddScoped<IPagerTagHelperService, PagerTagHelperService>();
    }
}