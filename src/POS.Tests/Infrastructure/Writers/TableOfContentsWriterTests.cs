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
    public class TableOfContentsWriterTests
    {
        [Fact]
        public async Task ItShould_replace_table_of_contents_values()
        {
            // arrange

            var viewModel = new TableOfContentsViewModel
            {
                ObjectCipher = "5.5-20.548",
                ProjectTemplate = ProjectTemplate.CPS,
                NormalInspectionEngineer = Engineer.Kapitan,
                ChiefProjectEngineer = Engineer.Saiko,
            };

            var document = MyWordDocumentHelper.GetMock();
            var documentFactory = MyWordDocumentFactoryHelper.GetMock(@"root\Infrastructure\Templates\TableOfContentsTemplates\CPSTableOfContentsTemplate.doc", document);
            var webHostEnvironment = WebHostEnvironmentHelper.GetMock("root");
            var engineerReplacer = EngineerReplacerHelper.GetMock();
            var projectReplacer = ProjectReplacerHelper.GetMock();
            var sut = new TableOfContentsWriter(documentFactory.Object, webHostEnvironment.Object, engineerReplacer.Object, projectReplacer.Object);

            // act

            await sut.GetTableOfContentsStream(viewModel);

            // assert

            engineerReplacer.Verify(x =>
                x.ReplaceSecondNameAndSignature(document.Object, viewModel.ChiefProjectEngineer, TypeOfEngineer.ChiefProjectEngineer),
                Times.Once);

            engineerReplacer.Verify(x =>
                    x.ReplaceSecondNameAndSignature(document.Object, viewModel.NormalInspectionEngineer, TypeOfEngineer.NormalInspectionProjectEngineer),
                Times.Once);

            projectReplacer.Verify(x => x.ReplaceObjectCipher(document.Object, viewModel.ObjectCipher), Times.Once);

            projectReplacer.Verify(x => x.ReplaceCurrentDate(document.Object), Times.Once);
        }
    }
}
