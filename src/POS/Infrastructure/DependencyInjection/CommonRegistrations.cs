using POS.Infrastructure.Factories;
using POS.Infrastructure.Factories.Base;

namespace POS.Infrastructure.DependencyInjection
{
    public partial class DependencyContainer
    {
        public static void Common(IServiceCollection services)
        {
            services.AddScoped<IMyWordDocumentFactory, MyWordDocumentFactory>();
            services.AddTransient<IMyExcelDocumentFactory, MyExcelDocumentFactory>();
        }
    }
}