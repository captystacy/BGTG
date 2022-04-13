using System.Threading.Tasks;
using Moq;
using POS.Infrastructure.Writers;
using POS.Models;
using POS.Tests.Helpers;
using POS.Tests.Helpers.Factories;
using POS.Tests.Helpers.Replacers;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using POS.ViewModels;
using Xunit;

namespace POS.Tests.Infrastructure.Writers
{
    public class TitlePageWriterTests
    {
        [Fact]
        public async Task ItShould_replace_title_page_values()
        {
            // arrange

            var viewModel = new TitlePageViewModel
            {
                ObjectCipher = "5.5-21.548",
                ObjectName = "Object name",
                ChiefProjectEngineer = Engineer.Selivanova,
            };

            var document = MyWordDocumentHelper.GetMock();
            var documentFactory = MyWordDocumentFactoryHelper.GetMock(@"root\Infrastructure\Templates\TitlePageTemplates\TitlePageTemplate.doc", document);
            var engineerReplacer = EngineerReplacerHelper.GetMock();
            var projectReplacer = ProjectReplacerHelper.GetMock();
            var webHostEnvironment = WebHostEnvironmentHelper.GetMock("root");

            var sut = new TitlePageWriter(documentFactory.Object, engineerReplacer.Object, projectReplacer.Object, webHostEnvironment.Object);

            // act

            await sut.GetTitlePageStream(viewModel);

            // assert

            engineerReplacer.Verify(x => x.ReplaceSecondNameAndSignature(document.Object, viewModel.ChiefProjectEngineer, TypeOfEngineer.ChiefProjectEngineer), Times.Once);
            engineerReplacer.Verify(x => x.ReplaceSecondNameAndSignature(document.Object, Engineer.Cherota, TypeOfEngineer.ChiefOrganizationEngineer), Times.Once);

            projectReplacer.Verify(x => x.ReplaceObjectName(document.Object, viewModel.ObjectName), Times.Once);
            projectReplacer.Verify(x => x.ReplaceObjectCipher(document.Object, viewModel.ObjectCipher), Times.Once);
            projectReplacer.Verify(x => x.ReplaceCurrentYear(document.Object), Times.Once);
        }
    }
}
