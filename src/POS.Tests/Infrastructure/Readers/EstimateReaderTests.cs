using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Readers;
using POS.Models.CalendarPlanModels;
using POS.Models.EstimateModels;
using POS.Tests.Helpers;
using POS.Tests.Helpers.Factories;
using POS.Tests.Helpers.Parsers;
using POS.Tests.Helpers.Services.DocumentServices.ExcelService;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Readers
{
    public class EstimateReaderTests
    {
        [Fact]
        public async Task ItShould_get_estimate()
        {
            // arrange

            var stream = StreamHelper.GetMock();

            var cells = MyExcelRangeHelper.GetMock();

            var workSheet = MyWorkSheetHelper.GetMock(cells, "4238-С1");

            var workBook = MyWorkBookHelper.GetMock(workSheet);

            var excelDocument = MyExcelDocumentHelper.GetMock(workBook);

            var documentFactory = MyExcelDocumentFactoryHelper.GetMock(stream, excelDocument);

            var estimateParser = EstimateParserHelper.GetMock();

            var fixture = new Fixture();

            // setup preparatory estimate work

            cells.Setup(x => x[27, 1]).Returns(MyExcelRangeHelper.GetMock("ОБЪЕКТНАЯ СМЕТА").Object);

            var preparatoryEstimateWork = fixture
                .Build<EstimateWork>()
                .With(x => x.Chapter, 1)
                .Create();

            estimateParser
                .Setup(x => x.GetEstimateWork(cells.Object, 27, 0))
                .ReturnsAsync(new OperationResult<EstimateWork> { Result = preparatoryEstimateWork });

            // setup total estimate work

            cells.Setup(x => x[28, 1]).Returns(MyExcelRangeHelper.GetMock("подпункт 33.3.2  инструкции").Object);

            var totalEstimateWork = fixture
                .Build<EstimateWork>()
                .With(x => x.Chapter, 12)
                .Create();

            var totalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter;

            estimateParser
                .Setup(x => x.GetTotalEstimateWork(cells.Object, 28, totalWorkChapter))
                .ReturnsAsync(new OperationResult<EstimateWork> { Result = totalEstimateWork });

            // setup construction start date

            var constructionStartDateCellStr = "август 2022";
            var constructionStartDate = new DateTime(2022, 8, 1);

            cells.Setup(x => x[20, 3]).Returns(MyExcelRangeHelper.GetMock(constructionStartDateCellStr).Object);

            var constructionParser = ConstructionParserHelper.GetMock();

            constructionParser
                .Setup(x => x.GetConstructionStartDate(constructionStartDateCellStr))
                .ReturnsAsync(new OperationResult<DateTime> { Result = constructionStartDate });

            // setup construction duration

            var constructionDurationCellStr = "0,7 мес";
            var constructionDuration = 0.7M;

            cells.Setup(x => x[21, 3]).Returns(MyExcelRangeHelper.GetMock(constructionDurationCellStr).Object);

            constructionParser
                .Setup(x => x.GetConstructionDuration(constructionDurationCellStr))
                .ReturnsAsync(new OperationResult<decimal> { Result = constructionDuration });

            var expectedEstimate = new Estimate
            {
                PreparatoryEstimateWorks = new List<EstimateWork> { preparatoryEstimateWork },
                MainEstimateWorks = new List<EstimateWork> { totalEstimateWork },
                TotalWorkChapter = totalWorkChapter,
                ConstructionStartDate = constructionStartDate,
                ConstructionDuration = constructionDuration,
                ConstructionDurationCeiling = 1,
            };

            var sut = new EstimateReader(documentFactory.Object, estimateParser.Object, constructionParser.Object);

            // act

            var getEstimateOperation = await sut.GetEstimate(stream.Object, totalWorkChapter);

            var actualEstimate = getEstimateOperation.Result;

            // assert

            Assert.True(getEstimateOperation.Ok);

            Assert.Equal(expectedEstimate.PreparatoryEstimateWorks.First(), actualEstimate.PreparatoryEstimateWorks.First());

            Assert.Equal(expectedEstimate.MainEstimateWorks.First(), actualEstimate.MainEstimateWorks.First());

            Assert.Equal(expectedEstimate, actualEstimate);
        }

        [Fact]
        public async Task ItShould_get_labor_costs()
        {
            // arrange

            var stream = StreamHelper.GetMock();
            var cells = MyExcelRangeHelper.GetMock();

            cells.Setup(x => x[27, 1]).Returns(MyExcelRangeHelper.GetMock("НРР 8.01.103-2017").Object);

            var workSheet = MyWorkSheetHelper.GetMock(cells, "4238-С1");
            var workBook = MyWorkBookHelper.GetMock(workSheet);
            var document = MyExcelDocumentHelper.GetMock(workBook);
            var excelDocumentFactory = MyExcelDocumentFactoryHelper.GetMock(stream, document);
            var estimateParser = EstimateParserHelper.GetMock();

            var expected = 16;
            estimateParser.Setup(x => x.GetLaborCosts(cells.Object, 27)).ReturnsAsync(new OperationResult<int> { Result = 16 });

            var constructionParser = ConstructionParserHelper.GetMock();
            var sut = new EstimateReader(excelDocumentFactory.Object, estimateParser.Object, constructionParser.Object);

            // act

            var getLaborCostsOperation = await sut.GetLaborCosts(stream.Object);

            // assert

            Assert.True(getLaborCostsOperation.Ok);

            Assert.Equal(expected, getLaborCostsOperation.Result);
        }

        [Fact]
        public async Task ItShould_get_total_estimate_work()
        {
            // arrange

            var stream = StreamHelper.GetMock();
            var cells = MyExcelRangeHelper.GetMock();

            cells.Setup(x => x[27, 1]).Returns(MyExcelRangeHelper.GetMock("ПОДПУНКТ 33.3.2  ИНСТРУКЦИИ").Object);

            var workSheet = MyWorkSheetHelper.GetMock(cells, "4238-С1");
            var workBook = MyWorkBookHelper.GetMock(workSheet);
            var document = MyExcelDocumentHelper.GetMock(workBook);
            var excelDocumentFactory = MyExcelDocumentFactoryHelper.GetMock(stream, document);
            var estimateParser = EstimateParserHelper.GetMock();

            var totalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter;

            var expected = new Fixture().Create<EstimateWork>();
            estimateParser.Setup(x => x.GetTotalEstimateWork(cells.Object, 27, totalWorkChapter)).ReturnsAsync(new OperationResult<EstimateWork> { Result = expected });

            var constructionParser = ConstructionParserHelper.GetMock();

            var sut = new EstimateReader(excelDocumentFactory.Object, estimateParser.Object, constructionParser.Object);

            // act

            var getTotalEstimateWorkOperation = await sut.GetTotalEstimateWork(stream.Object, totalWorkChapter);

            // assert

            Assert.True(getTotalEstimateWorkOperation.Ok);

            Assert.Same(expected, getTotalEstimateWorkOperation.Result);
        }

        [Fact]
        public async Task ItShould_get_constructionStartDate()
        {
            // arrange

            var stream = StreamHelper.GetMock();
            var cells = MyExcelRangeHelper.GetMock();

            var constructionStartDateCellStr = "август 2022";
            cells.Setup(x => x[20, 3]).Returns(MyExcelRangeHelper.GetMock(constructionStartDateCellStr).Object);

            var workSheet = MyWorkSheetHelper.GetMock(cells, "4238-С1");
            var workBook = MyWorkBookHelper.GetMock(workSheet);
            var document = MyExcelDocumentHelper.GetMock(workBook);
            var excelDocumentFactory = MyExcelDocumentFactoryHelper.GetMock(stream, document);
            var estimateParser = EstimateParserHelper.GetMock();
            var constructionParser = ConstructionParserHelper.GetMock();

            var expected = new DateTime(2022, 9, 1);
            constructionParser.Setup(x => x.GetConstructionStartDate(constructionStartDateCellStr))
                .ReturnsAsync(new OperationResult<DateTime> { Result = expected });

            var sut = new EstimateReader(excelDocumentFactory.Object, estimateParser.Object, constructionParser.Object);

            // act

            var getConstructionStartDateOperation = await sut.GetConstructionStartDate(stream.Object);

            // assert

            Assert.True(getConstructionStartDateOperation.Ok);

            Assert.Equal(expected, getConstructionStartDateOperation.Result);
        }
    }
}