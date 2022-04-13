using AutoFixture;
using Microsoft.AspNetCore.Http;
using Moq;
using POS.Infrastructure.Services;
using POS.Models;
using POS.Models.CalendarPlanModels;
using POS.Models.EstimateModels;
using POS.Tests.Helpers;
using POS.Tests.Helpers.Appenders;
using POS.Tests.Helpers.Calculators;
using POS.Tests.Helpers.Factories;
using POS.Tests.Helpers.Services;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using POS.ViewModels;
using System;
using System.Threading.Tasks;
using Xunit;

namespace POS.Tests.Infrastructure.Services
{
    public class EnergyAndWaterServiceTests
    {
        [Fact]
        public async Task ItShould_be_able_to_give_correct_energy_and_water_stream()
        {
            // arrange

            var fixture = new Fixture();

            var viewModel = new EnergyAndWaterViewModel
            {
                EstimateFiles = new FormFileCollection
                {
                    FormFileHelper.GetMock().Object,
                },
                TotalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter
            };

            var section = MySectionHelper.GetMock();
            var document = MyWordDocumentHelper.GetMock(section);
            var documentFactory = MyWordDocumentFactoryHelper.GetMock(document);

            var energyAndWaterAppender = EnergyAndWaterAppenderHelper.GetMock();

            var totalEstimateWork = fixture.Create<EstimateWork>();

            var constructionStartDate = new DateTime(2022, 9, 1);
            var estimateService = EstimateServiceHelper.GetMock(viewModel, totalEstimateWork, constructionStartDate);

            var totalCalendarWork = fixture.Create<CalendarWork>();
            var calendarWorkCalculator = CalendarWorkCalculatorHelper.GetMock(totalEstimateWork, constructionStartDate, totalCalendarWork);

            var energyAndWater = new EnergyAndWater();
            var energyAndWaterCalculator = EnergyAndWaterCalculatorHelper.GetMock(totalCalendarWork.TotalCostIncludingCAIW, constructionStartDate.Year, energyAndWater);

            var sut = new EnergyAndWaterService(documentFactory.Object, energyAndWaterCalculator.Object, energyAndWaterAppender.Object, estimateService.Object, calendarWorkCalculator.Object);

            // act

            var getEnergyAndWaterStreamOperation = await sut.GetEnergyAndWaterStream(viewModel);

            // assert

            Assert.True(getEnergyAndWaterStreamOperation.Ok);

            Assert.NotNull(getEnergyAndWaterStreamOperation.Result);

            energyAndWaterAppender.Verify(x => x.AppendAsync(section.Object, energyAndWater), Times.Once);
        }
    }
}
