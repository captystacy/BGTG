using System;
using System.Threading.Tasks;
using Moq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Replacers;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Replacers
{
    public class ProjectReplacerTests
    {
        [Fact]
        public async Task ItShould_replace_cipher_pattern()
        {
            // arrange

            var baseDocument = MyWordDocumentHelper.GetMock();
            var objectCipher = "5.5-21.548";

            var sut = new ProjectReplacer();

            // act

            await sut.ReplaceObjectCipher(baseDocument.Object, objectCipher);

            // assert

            baseDocument.Verify(x => x.Replace("%CIPHER%", objectCipher), Times.Once);
        }

        [Fact]
        public async Task ItShould_replace_construction_start_date()
        {
            // arrange

            var baseDocument = MyWordDocumentHelper.GetMock();
            var constructionStartDateStr = "август 2022";

            var sut = new ProjectReplacer();

            // act

            await sut.ReplaceConstructionStartDate(baseDocument.Object, constructionStartDateStr);

            // assert

            baseDocument.Verify(x => x.Replace("%CONSTRUCTION_START_DATE%", constructionStartDateStr), Times.Once);
        }

        [Fact]
        public async Task ItShould_replace_current_date()
        {
            // arrange

            var baseDocument = MyWordDocumentHelper.GetMock();

            var sut = new ProjectReplacer();

            // act

            await sut.ReplaceCurrentDate(baseDocument.Object);

            // assert

            baseDocument.Verify(x => x.Replace("%DATE%", DateTime.Now.ToString(Constants.DateTimeMonthAndYearShortFormat)), Times.Once);
        }

        [Fact]
        public async Task ItShould_replace_construction_year()
        {
            // arrange

            var baseDocument = MyWordDocumentHelper.GetMock();
            var constructionYear = "2022";

            var sut = new ProjectReplacer();

            // act

            await sut.ReplaceConstructionYear(baseDocument.Object, constructionYear);

            // assert

            baseDocument.Verify(x => x.Replace("%CY%", constructionYear), Times.Once);
        }

        [Fact]
        public async Task ItShould_replace_current_year()
        {
            // arrange

            var document = MyWordDocumentHelper.GetMock();
            
            var sut = new ProjectReplacer();

            // act

            await sut.ReplaceCurrentYear(document.Object);

            // assert

            document.Verify(x => x.Replace("%YEAR%", DateTime.Now.Year.ToString()), Times.Once);
        }
    }
}
