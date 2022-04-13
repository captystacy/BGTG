using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Replacers.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Infrastructure.Replacers
{
    public class DurationByLCReplacer : IDurationByLCReplacer
    {
        private const string DurationByLCFirstParagraphPattern = "%DURATION_BY_LC_FIRST_PARAGRAPH%";
        private const string DurationByLCFormulaTablePattern = "%DURATION_BY_LC_TABLE%";
        private const string DurationByLCDescriptionTablePattern = "%DURATION_BY_LC_DESCRIPTION_TABLE%";
        private const string DurationByLCPenultimateParagraphPattern = "%DURATION_BY_LC_PENULTIMATE_PARAGRAPH%";
        private const string DurationByLCLastParagraphPattern = "%DURATION_BY_LC_LAST_PARAGRAPH%";

        public Task Replace(IMyWordDocument baseDocument, IMyWordDocument durationByLCDocument)
        {
            var paragraphs = durationByLCDocument.Sections[0].Paragraphs;

            var durationByLCFirstParagraph = paragraphs[0].Text;

            var formulaTable = durationByLCDocument.Sections[0].Tables[0];

            var descriptionTable = durationByLCDocument.Sections[0].Tables[1];

            var durationByLCPenultimateParagraph = paragraphs.Count == Constants.DurationByLCParagraphCount
                ? paragraphs[^2].Text
                : string.Empty;

            var durationByLCLastParagraph = paragraphs[^1].Text;

            baseDocument.Replace(DurationByLCFirstParagraphPattern, durationByLCFirstParagraph);

            baseDocument.ReplaceTextWithTable(DurationByLCFormulaTablePattern, formulaTable);

            baseDocument.ReplaceTextWithTable(DurationByLCDescriptionTablePattern, descriptionTable);

            baseDocument.Replace(DurationByLCPenultimateParagraphPattern, durationByLCPenultimateParagraph);

            baseDocument.Replace(DurationByLCLastParagraphPattern, durationByLCLastParagraph);

            return Task.CompletedTask;
        }
    }
}
