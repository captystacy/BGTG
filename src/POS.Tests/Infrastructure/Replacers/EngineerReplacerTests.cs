using System.Threading.Tasks;
using Moq;
using POS.Infrastructure.Replacers;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models;
using POS.Tests.Helpers;
using Xunit;

namespace POS.Tests.Infrastructure.Replacers
{
    public class EngineerReplacerTests
    {
        [Fact]
        public async Task ReplaceEngineerSecondNameAndSignature_ChiefProjectEngineer()
        {
            // arrange

            var engineer = Engineer.Saiko;
            var typeOfEngineer = TypeOfEngineer.ChiefProjectEngineer;
            var document = new Mock<IMyWordDocument>();

            var webHostEnvironment = WebHostEnvironmentHelper.GetMock("root");
            var sut = new EngineerReplacer(webHostEnvironment.Object);

            // act

            await sut.ReplaceSecondNameAndSignature(document.Object, engineer, typeOfEngineer);

            // assert

            document.Verify(x => x.Replace("%CPEFN%", "А.М. Сайко"), Times.Once);
            document.Verify(x => x.Replace("%CPESN%", "Сайко"), Times.Once);
            document.Verify(x => x.ReplaceTextWithImage("%CPES%", @"root\Infrastructure\Templates\EngineerSignatures\Saiko.png"), Times.Once);
        }

        [Fact]
        public async Task ReplaceEngineerSecondNameAndSignature_ProjectEngineer()
        {
            // arrange

            var engineer = Engineer.Kapitan;
            var typeOfEngineer = TypeOfEngineer.ProjectEngineer;
            var signaturePath = @"root\Infrastructure\Templates\EngineerSignatures\Kapitan.png";
            var document = new Mock<IMyWordDocument>();

            var webHostEnvironment = WebHostEnvironmentHelper.GetMock("root");
            var sut = new EngineerReplacer(webHostEnvironment.Object);

            // act

            await sut.ReplaceSecondNameAndSignature(document.Object, engineer, typeOfEngineer);

            // assert

            document.Verify(x => x.Replace("%PESN%", "Капитан"), Times.Once);
            document.Verify(x => x.ReplaceTextWithImage("%PES%", signaturePath), Times.Once);
        }

        [Fact]
        public async Task ReplaceEngineerSecondNameAndSignature_NormalInspectionEngineer()
        {
            // arrange

            var engineer = Engineer.Prishep;
            var typeOfEngineer = TypeOfEngineer.NormalInspectionProjectEngineer;
            var signaturePath = @"root\Infrastructure\Templates\EngineerSignatures\Prishep.png";
            var document = new Mock<IMyWordDocument>();

            var webHostEnvironment = WebHostEnvironmentHelper.GetMock("root");
            var sut = new EngineerReplacer(webHostEnvironment.Object);

            // act

            await sut.ReplaceSecondNameAndSignature(document.Object, engineer, typeOfEngineer);

            // assert

            document.Verify(x => x.Replace("%NIESN%", "Прищеп"), Times.Once);
            document.Verify(x => x.ReplaceTextWithImage("%NIES%", signaturePath), Times.Once);
        }

        [Fact]
        public async Task ReplaceEngineerSecondNameAndSignature_ChiefEngineer()
        {
            // arrange

            var engineer = Engineer.Selivanova;
            var typeOfEngineer = TypeOfEngineer.ChiefEngineer;
            var signaturePath = @"root\Infrastructure\Templates\EngineerSignatures\Selivanova.png";
            var document = new Mock<IMyWordDocument>();

            var webHostEnvironment = WebHostEnvironmentHelper.GetMock("root");
            var sut = new EngineerReplacer(webHostEnvironment.Object);

            // act

            await sut.ReplaceSecondNameAndSignature(document.Object, engineer, typeOfEngineer);

            // assert

            document.Verify(x => x.Replace("%CESN%", "Селиванова"), Times.Once);
            document.Verify(x => x.ReplaceTextWithImage("%CES%", signaturePath), Times.Once);
        }


        [Fact]
        public async Task ReplaceEngineerSecondNameAndSignature_UnknownEngineer()
        {
            // arrange

            var engineer = Engineer.Unknown;
            var typeOfEngineer = TypeOfEngineer.ProjectEngineer;
            var signaturePath = @"root\Infrastructure\Templates\EngineerSignatures\Kapitan.png";
            var document = new Mock<IMyWordDocument>();

            var webHostEnvironment = WebHostEnvironmentHelper.GetMock("root");
            var sut = new EngineerReplacer(webHostEnvironment.Object);

            // act

            await sut.ReplaceSecondNameAndSignature(document.Object, engineer, typeOfEngineer);

            // assert

            document.Verify(x => x.Replace("%PESN%", ""), Times.Once);
            document.Verify(x => x.ReplaceTextWithImage("%PES%", signaturePath), Times.Never);
            document.Verify(x => x.Replace("%PES%", ""), Times.Once);
        }
    }
}