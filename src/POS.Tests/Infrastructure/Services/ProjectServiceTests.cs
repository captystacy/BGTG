using AutoFixture;
using Microsoft.AspNetCore.Http;
using Moq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Services;
using POS.Models;
using POS.Tests.Helpers;
using POS.Tests.Helpers.Calculators;
using POS.Tests.Helpers.Readers;
using POS.Tests.Helpers.Writers;
using POS.ViewModels;
using System;
using System.Threading.Tasks;
using Xunit;

namespace POS.Tests.Infrastructure.Services
{
    public class ProjectServiceTests
    {
        [Fact]
        public async Task ItShould_be_able_to_give_correct_project_stream()
        {
            // arrange

            var fixture = new Fixture();

            var durationByLCStream = StreamHelper.GetMock();
            var calendarPlanStream = StreamHelper.GetMock();
            var calculationFiles = new FormFileCollection
            {
                FormFileHelper.GetMock(durationByLCStream, Constants.DurationByLCFileName).Object,
                FormFileHelper.GetMock(calendarPlanStream, Constants.CalendarPlanFileName).Object
            };

            var viewModel = fixture
                .Build<ProjectViewModel>()
                .With(x => x.CalculationFiles, calculationFiles)
                .Create();

            var durationByLC = fixture.Create<DurationByLC>();
            var employeesNeed = fixture.Create<EmployeesNeed>();
            var employeesNeedCalculator = EmployeesNeedCalculatorHelper.GetMock(durationByLC.NumberOfEmployees, durationByLC.Shift, employeesNeed);
            var constructionStartDate = new DateTime(2022, 8, 1);
            var calendarPlanReader = CalendarPlanReaderHelper.GetMock(calendarPlanStream, constructionStartDate);
            var durationByLCReader = DurationByLCReaderHelper.GetMock(durationByLCStream, durationByLC);
            var projectWriter = ProjectWriterHelper.GetMock(viewModel, constructionStartDate, employeesNeed, durationByLC);
            var sut = new ProjectService(projectWriter.Object, durationByLCReader.Object, calendarPlanReader.Object, employeesNeedCalculator.Object);

            // act

            var getProjectStreamOperation = await sut.GetProjectStream(viewModel);

            // assert

            Assert.True(getProjectStreamOperation.Ok);

            Assert.NotNull(getProjectStreamOperation.Result);

            projectWriter.Verify(x => x.GetProjectStream(viewModel, constructionStartDate, employeesNeed, durationByLC), Times.Once);
        }
    }
}
