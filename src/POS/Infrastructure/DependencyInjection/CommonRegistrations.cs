using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;

namespace POS.Infrastructure.DependencyInjection;

public partial class DependencyContainer
{
    public static void Common(IServiceCollection services)
    {
        services.AddTransient<IDocumentService, DocumentService>();
    }
}