using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Readers;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Models;
using POS.Tests.Helpers;
using POS.Tests.Helpers.Factories;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Readers
{
    public class DurationByLCReaderTests
    {
        [Fact]
        public async Task ItShould_get_correct_duration_by_lc_which_has_rounding_false_acceptance_true()
        {
            // arrange

            var stream = StreamHelper.GetMock();

            var formulaTable = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(2, "= 0,02 мес."),
            });

            var descriptionTable = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(1, "25"),
                MyRowHelper.GetMock(1, "8"),
                MyRowHelper.GetMock(1, "1,5"),
                MyRowHelper.GetMock(1, "21,5"),
                MyRowHelper.GetMock(1, "4"),
            });
            var section = MySectionHelper.GetMock(new List<Mock<IMyTable>> { formulaTable, descriptionTable });
            var document = MyWordDocumentHelper.GetMock(section);

            section.Setup(x => x.Paragraphs.Count).Returns(Constants.DurationByLCParagraphCount);

            var penultimateParagraph = "Нормативная продолжительность строительства с учетом времени на приемку объекта в эксплуатацию и утверждения акта приемки объекта в эксплуатацию согласно ТКП 45-1.03-122-2015 п. 4.22 общая продолжительность строительства составит – Tобщ = 0,1 + 0,5 = 0,6 мес.";
            section.Setup(x => x.Paragraphs[Constants.DurationByLCParagraphCount - 2]).Returns(MyParagraphHelper.GetMock(penultimateParagraph).Object);

            var lastParagraph = "Принимаем продолжительность строительства равную 0,6 мес, в том числе подготовительный период – 0,06 мес, приемка объекта в эксплуатацию – 0,5 мес.";
            section.Setup(x => x.Paragraphs[Constants.DurationByLCParagraphCount - 1]).Returns(MyParagraphHelper.GetMock(lastParagraph).Object);

            var expectedDurationByLC = new DurationByLC
            {
                PreparatoryPeriod = 0.06M,
                AcceptanceTime = 0.5M,
                AcceptanceTimeIncluded = true,
                Duration = 0.02M,
                RoundedDuration = 0.1M,
                TotalDuration = 0.6M,
                NumberOfEmployees = 4,
                NumberOfWorkingDays = 21.5M,
                RoundingIncluded = false,
                Shift = 1.5M,
                TotalLaborCosts = 25,
                WorkingDayDuration = 8,
            };

            var documentFactory = MyWordDocumentFactoryHelper.GetMock(stream, document);
            var sut = new DurationByLCReader(documentFactory.Object);

            // act

            var getDurationByLCOperation = await sut.GetDurationByLC(stream.Object);

            // assert

            Assert.True(getDurationByLCOperation.Ok);

            Assert.Equal(expectedDurationByLC, getDurationByLCOperation.Result);
        }

        [Fact]
        public async Task ItShould_get_correct_duration_by_lc_which_has_rounding_true_acceptance_true()
        {
            // arrange

            var stream = StreamHelper.GetMock();

            var formulaTable = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(2, "= 0,02 мес."),
            });

            var descriptionTable = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(1, "25"),
                MyRowHelper.GetMock(1, "8"),
                MyRowHelper.GetMock(1, "1,5"),
                MyRowHelper.GetMock(1, "21,5"),
                MyRowHelper.GetMock(1, "4"),
            });
            var section = MySectionHelper.GetMock(new List<Mock<IMyTable>> { formulaTable, descriptionTable });
            var document = MyWordDocumentHelper.GetMock(section);

            section.Setup(x => x.Paragraphs.Count).Returns(Constants.DurationByLCParagraphCount);

            var penultimateParagraph = "С учетом округления в соответствии с 4.36 ТКП 45-1.03-122-2015 нормативная продолжительность строительства составит 0,1 мес, с учетом времени на приемку объекта в эксплуатацию и утверждения акта приемки объекта в эксплуатацию согласно ТКП 45-1.03-122-2015 п. 4.22 общая продолжительность строительства составит – Tобщ  = 0,1 + 0,5 = 0,6 мес.";
            section.Setup(x => x.Paragraphs[Constants.DurationByLCParagraphCount - 2]).Returns(MyParagraphHelper.GetMock(penultimateParagraph).Object);

            var lastParagraph = "Принимаем продолжительность строительства равную 0,6 мес, в том числе подготовительный период – 0,06 мес, приемка объекта в эксплуатацию – 0,5 мес.";
            section.Setup(x => x.Paragraphs[Constants.DurationByLCParagraphCount - 1]).Returns(MyParagraphHelper.GetMock(lastParagraph).Object);

            var expectedDurationByLC = new DurationByLC
            {
                PreparatoryPeriod = 0.06M,
                AcceptanceTime = 0.5M,
                AcceptanceTimeIncluded = true,
                Duration = 0.02M,
                RoundedDuration = 0.1M,
                TotalDuration = 0.6M,
                NumberOfEmployees = 4,
                NumberOfWorkingDays = 21.5M,
                RoundingIncluded = true,
                Shift = 1.5M,
                TotalLaborCosts = 25,
                WorkingDayDuration = 8,
            };

            var documentFactory = MyWordDocumentFactoryHelper.GetMock(stream, document);
            var sut = new DurationByLCReader(documentFactory.Object);

            // act

            var getDurationByLCOperation = await sut.GetDurationByLC(stream.Object);

            // assert

            Assert.True(getDurationByLCOperation.Ok);

            Assert.Equal(expectedDurationByLC, getDurationByLCOperation.Result);
        }

        [Fact]
        public async Task ItShould_get_correct_duration_by_lc_which_has_rounding_true_acceptance_false()
        {
            // arrange

            var stream = StreamHelper.GetMock();

            var formulaTable = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(2, "= 0,02 мес."),
            });

            var descriptionTable = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(1, "25"),
                MyRowHelper.GetMock(1, "8"),
                MyRowHelper.GetMock(1, "1,5"),
                MyRowHelper.GetMock(1, "21,5"),
                MyRowHelper.GetMock(1, "4"),
            });
            var section = MySectionHelper.GetMock(new List<Mock<IMyTable>> { formulaTable, descriptionTable });
            var document = MyWordDocumentHelper.GetMock(section);

            section.Setup(x => x.Paragraphs.Count).Returns(Constants.DurationByLCParagraphCount);

            var penultimateParagraph = "С учетом округления в соответствии с 4.36 ТКП 45-1.03-122-2015 нормативная продолжительность строительства составит 0,1 мес.";
            section.Setup(x => x.Paragraphs[Constants.DurationByLCParagraphCount - 2]).Returns(MyParagraphHelper.GetMock(penultimateParagraph).Object);

            var lastParagraph = "Принимаем продолжительность строительства равную 0,6 мес, в том числе подготовительный период – 0,06 мес.";
            section.Setup(x => x.Paragraphs[Constants.DurationByLCParagraphCount - 1]).Returns(MyParagraphHelper.GetMock(lastParagraph).Object);

            var expectedDurationByLC = new DurationByLC
            {
                PreparatoryPeriod = 0.06M,
                AcceptanceTimeIncluded = false,
                Duration = 0.02M,
                RoundedDuration = 0.1M,
                TotalDuration = 0.6M,
                NumberOfEmployees = 4,
                NumberOfWorkingDays = 21.5M,
                RoundingIncluded = true,
                Shift = 1.5M,
                TotalLaborCosts = 25,
                WorkingDayDuration = 8,
            };

            var documentFactory = MyWordDocumentFactoryHelper.GetMock(stream, document);
            var sut = new DurationByLCReader(documentFactory.Object);

            // act

            var getDurationByLCOperation = await sut.GetDurationByLC(stream.Object);

            // assert

            Assert.True(getDurationByLCOperation.Ok);

            Assert.Equal(expectedDurationByLC, getDurationByLCOperation.Result);
        }

        [Fact]
        public async Task ItShould_get_correct_duration_by_lc_which_has_rounding_false_acceptance_false()
        {
            // arrange

            var stream = StreamHelper.GetMock();

            var formulaTable = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(2, "= 0,02 мес."),
            });

            var descriptionTable = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(1, "25"),
                MyRowHelper.GetMock(1, "8"),
                MyRowHelper.GetMock(1, "1,5"),
                MyRowHelper.GetMock(1, "21,5"),
                MyRowHelper.GetMock(1, "4"),
            });
            var section = MySectionHelper.GetMock(new List<Mock<IMyTable>> { formulaTable, descriptionTable });
            var document = MyWordDocumentHelper.GetMock(section);

            section.Setup(x => x.Paragraphs.Count).Returns(4);

            var lastParagraph = "Принимаем продолжительность строительства равную 0,6 мес, в том числе подготовительный период – 0,06 мес.";
            section.Setup(x => x.Paragraphs[3]).Returns(MyParagraphHelper.GetMock(lastParagraph).Object);

            var expectedDurationByLC = new DurationByLC
            {
                PreparatoryPeriod = 0.06M,
                AcceptanceTimeIncluded = false,
                Duration = 0.02M,
                TotalDuration = 0.6M,
                NumberOfEmployees = 4,
                NumberOfWorkingDays = 21.5M,
                RoundingIncluded = false,
                Shift = 1.5M,
                TotalLaborCosts = 25,
                WorkingDayDuration = 8,
            };

            var documentFactory = MyWordDocumentFactoryHelper.GetMock(stream, document);
            var sut = new DurationByLCReader(documentFactory.Object);

            // act

            var getDurationByLCOperation = await sut.GetDurationByLC(stream.Object);

            // assert

            Assert.True(getDurationByLCOperation.Ok);

            Assert.Equal(expectedDurationByLC, getDurationByLCOperation.Result);
        }
    }
}
