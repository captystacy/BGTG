using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Infrastructure.Replacers.Base
{
    public interface IDurationByLCReplacer
    {
        Task Replace(IMyWordDocument baseDocument, IMyWordDocument durationByLCDocument);
    }
}