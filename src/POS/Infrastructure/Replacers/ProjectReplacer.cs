using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Replacers.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Infrastructure.Replacers
{
    public class ProjectReplacer : IProjectReplacer
    {
        private const string NamePattern = "%NAME%";
        private const string CipherPattern = "%CIPHER%";
        private const string DatePattern = "%DATE%";
        private const string ConstructionStartDatePattern = "%CONSTRUCTION_START_DATE%";
        private const string ConstructionYearPattern = "%CY%";
        private const string YearPattern = "%YEAR%";

        public Task ReplaceObjectCipher(IMyWordDocument document, string objectCipher)
        {
            document.Replace(CipherPattern, objectCipher);
            return Task.CompletedTask;
        }

        public Task ReplaceCurrentDate(IMyWordDocument document)
        {
            document.Replace(DatePattern, DateTime.Now.ToString(Constants.DateTimeMonthAndYearShortFormat));
            return Task.CompletedTask;
        }

        public Task ReplaceCurrentYear(IMyWordDocument document)
        {
            document.Replace(YearPattern, DateTime.Now.Year.ToString());
            return Task.CompletedTask;
        }

        public Task ReplaceConstructionStartDate(IMyWordDocument document, string constructionStartDateStr)
        {
            document.Replace(ConstructionStartDatePattern, constructionStartDateStr);
            return Task.CompletedTask;
        }

        public Task ReplaceConstructionYear(IMyWordDocument document, string constructionYearStr)
        {
            document.Replace(ConstructionYearPattern, constructionYearStr);
            return Task.CompletedTask;
        }

        public Task ReplaceObjectName(IMyWordDocument document, string objectName)
        {
            document.Replace(NamePattern, objectName);
            return Task.CompletedTask;
        }
    }
}
