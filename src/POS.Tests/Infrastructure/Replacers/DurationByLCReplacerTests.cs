using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Replacers
{
    public class DurationByLCReplacerTests
    {
        [Fact]
        public async Task ItShould_replace_duration_by_lc_patterns_when_paragraphs_count_is_max()
        {
            // arrange

            var formulaTable = MyTableHelper.GetMock();
            var descriptionTable = MyTableHelper.GetMock();
            var section = MySectionHelper.GetMock(new List<Mock<IMyTable>> { formulaTable, descriptionTable });
            var durationDyLCDocument = MyWordDocumentHelper.GetMock(section);

            section.Setup(x => x.Paragraphs.Count).Returns(Constants.DurationByLCParagraphCount);

            var firstParagraph = "Нормативная продолжительность строительства объекта определена по (п.4.22) ТКП 45-1.03-122-2015 «Нормы продолжительности строительства предприятий, зданий и сооружений», а также по нормативной трудоемкости глав 1-8 ССР и ориентировочному количеству работающих:";
            section.Setup(x => x.Paragraphs[0]).Returns(MyParagraphHelper.GetMock(firstParagraph).Object);

            var penultimateParagraph = "Нормативная продолжительность строительства с учетом времени на приемку объекта в эксплуатацию и утверждения акта приемки объекта в эксплуатацию согласно ТКП 45-1.03-122-2015 п. 4.22 общая продолжительность строительства составит – Tобщ = 0,1 + 0,5 = 0,6 мес. ";
            section.Setup(x => x.Paragraphs[Constants.DurationByLCParagraphCount - 2]).Returns(MyParagraphHelper.GetMock(penultimateParagraph).Object);

            var lastParagraph = "Принимаем продолжительность строительства равную 0,6 мес, в том числе подготовительный период – 0,06 мес, приемка объекта в эксплуатацию – 0,5 мес.";
            section.Setup(x => x.Paragraphs[Constants.DurationByLCParagraphCount - 1]).Returns(MyParagraphHelper.GetMock(lastParagraph).Object);

            var baseDocument = MyWordDocumentHelper.GetMock();

            var sut = new DurationByLCReplacer();

            // act

            await sut.Replace(baseDocument.Object, durationDyLCDocument.Object);

            // assert

            baseDocument.Verify(x => x.Replace("%DURATION_BY_LC_FIRST_PARAGRAPH%", firstParagraph), Times.Once);

            baseDocument.Verify(x => x.ReplaceTextWithTable("%DURATION_BY_LC_TABLE%", formulaTable.Object), Times.Once);

            baseDocument.Verify(x => x.ReplaceTextWithTable("%DURATION_BY_LC_DESCRIPTION_TABLE%", descriptionTable.Object), Times.Once);

            baseDocument.Verify(x => x.Replace("%DURATION_BY_LC_PENULTIMATE_PARAGRAPH%", penultimateParagraph), Times.Once);

            baseDocument.Verify(x => x.Replace("%DURATION_BY_LC_LAST_PARAGRAPH%", lastParagraph), Times.Once);
        }

        [Fact]
        public async Task ItShould_replace_duration_by_lc_patterns_when_paragraphs_count_is_min()
        {
            // arrange

            var formulaTable = MyTableHelper.GetMock();
            var descriptionTable = MyTableHelper.GetMock();
            var section = MySectionHelper.GetMock(new List<Mock<IMyTable>> { formulaTable, descriptionTable });
            var durationDyLCDocument = MyWordDocumentHelper.GetMock(section);

            section.Setup(x => x.Paragraphs.Count).Returns(4);

            var firstParagraph = "Нормативная продолжительность строительства объекта определена по (п.4.22) ТКП 45-1.03-122-2015 «Нормы продолжительности строительства предприятий, зданий и сооружений», а также по нормативной трудоемкости глав 1-8 ССР и ориентировочному количеству работающих:";
            section.Setup(x => x.Paragraphs[0]).Returns(MyParagraphHelper.GetMock(firstParagraph).Object);

            var penultimateParagraph = "Нормативная продолжительность строительства с учетом времени на приемку объекта в эксплуатацию и утверждения акта приемки объекта в эксплуатацию согласно ТКП 45-1.03-122-2015 п. 4.22 общая продолжительность строительства составит – Tобщ = 0,1 + 0,5 = 0,6 мес. ";
            section.Setup(x => x.Paragraphs[2]).Returns(MyParagraphHelper.GetMock(penultimateParagraph).Object);

            var lastParagraph = "Принимаем продолжительность строительства равную 0,6 мес, в том числе подготовительный период – 0,06 мес, приемка объекта в эксплуатацию – 0,5 мес.";
            section.Setup(x => x.Paragraphs[3]).Returns(MyParagraphHelper.GetMock(lastParagraph).Object);

            var baseDocument = MyWordDocumentHelper.GetMock();

            var sut = new DurationByLCReplacer();

            // act

            await sut.Replace(baseDocument.Object, durationDyLCDocument.Object);

            // assert

            baseDocument.Verify(x => x.Replace("%DURATION_BY_LC_FIRST_PARAGRAPH%", firstParagraph), Times.Once);

            baseDocument.Verify(x => x.ReplaceTextWithTable("%DURATION_BY_LC_TABLE%", formulaTable.Object), Times.Once);

            baseDocument.Verify(x => x.ReplaceTextWithTable("%DURATION_BY_LC_DESCRIPTION_TABLE%", descriptionTable.Object), Times.Once);

            baseDocument.Verify(x => x.Replace("%DURATION_BY_LC_PENULTIMATE_PARAGRAPH%", penultimateParagraph), Times.Never);

            baseDocument.Verify(x => x.Replace("%DURATION_BY_LC_PENULTIMATE_PARAGRAPH%", string.Empty), Times.Once);

            baseDocument.Verify(x => x.Replace("%DURATION_BY_LC_LAST_PARAGRAPH%", lastParagraph), Times.Once);
        }
    }
}
