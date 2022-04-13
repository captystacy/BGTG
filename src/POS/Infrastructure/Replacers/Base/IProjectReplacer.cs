using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Infrastructure.Replacers.Base
{
    public interface IProjectReplacer
    {
        Task ReplaceObjectCipher(IMyWordDocument document, string objectCipher);
        Task ReplaceCurrentDate(IMyWordDocument document);
        Task ReplaceCurrentYear(IMyWordDocument document);
        Task ReplaceConstructionStartDate(IMyWordDocument document, string constructionStartDateStr);
        Task ReplaceConstructionYear(IMyWordDocument document, string constructionYearStr);
        Task ReplaceObjectName(IMyWordDocument document, string objectName);
    }
}