using Calabonga.OperationResults;
using Microsoft.AspNetCore.Http;
using Moq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Services;
using POS.Models.EstimateModels;
using POS.Tests.Helpers;
using POS.Tests.Helpers.Appenders;
using POS.Tests.Helpers.Calculators;
using POS.Tests.Helpers.Factories;
using POS.Tests.Helpers.Services;
using POS.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POS.Models.CalendarPlanModels;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Services
{
    public class CalendarPlanServiceTests
    {
        [Fact]
        public async Task ItShould_be_able_to_give_correct_calendar_plan_view_model()
        {
            // arrange

            var estimateStream1 = StreamHelper.GetMock();
            var estimateStream2 = StreamHelper.GetMock();

            var calendarPlanCreateViewModel = new CalendarPlanCreateViewModel
            {
                EstimateFiles = new FormFileCollection
                {
                    FormFileHelper.GetMock(estimateStream1).Object,
                    FormFileHelper.GetMock(estimateStream2).Object,
                },
                TotalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter
            };

            var calendarPlanAppender = CalendarPlanAppenderHelper.GetMock();
            var calendarPlanCalculator = CalendarPlanCalculatorHelper.GetMock();
            var estimateService = EstimateServiceHelper.GetMock();

            var estimate = new Estimate();
            estimateService
                .Setup(x => x.GetEstimate(calendarPlanCreateViewModel.EstimateFiles,
                    calendarPlanCreateViewModel.TotalWorkChapter))
                .ReturnsAsync(new OperationResult<Estimate> { Result = estimate });

            var mapper = MapperHelper.GetMock();
            var calendarPlanViewModel = new CalendarPlanViewModel
            {
                PreparatoryCalendarWorks = new List<CalendarWorkViewModel>
                {
                    new()
                    {
                        Chapter = 1,
                    }
                },
                MainCalendarWorks = new List<CalendarWorkViewModel>
                {
                    new()
                    {
                        Chapter = (int)calendarPlanCreateViewModel.TotalWorkChapter
                    }
                }
            };
            mapper.Setup(x => x.Map<CalendarPlanViewModel>(estimate)).Returns(calendarPlanViewModel);

            var documentFactory = MyWordDocumentFactoryHelper.GetMock();
            var sut = new CalendarPlanService(documentFactory.Object, estimateService.Object,
                calendarPlanCalculator.Object, calendarPlanAppender.Object, mapper.Object);

            // act

            var getCalendarPlanViewModelOperation = await sut.GetCalendarPlanViewModel(calendarPlanCreateViewModel);

            // assert

            Assert.True(getCalendarPlanViewModelOperation.Ok);

            Assert.Contains(getCalendarPlanViewModelOperation.Result.PreparatoryCalendarWorks,
                x => x.Equals(Constants.PreparatoryCalendarWork));

            Assert.Contains(getCalendarPlanViewModelOperation.Result.PreparatoryCalendarWorks,
                x => x.Equals(Constants.TemporaryBuildingsCalendarWork));

            Assert.DoesNotContain(getCalendarPlanViewModelOperation.Result.MainCalendarWorks,
                x => x.Chapter == (int)calendarPlanViewModel.TotalWorkChapter);

            Assert.Contains(getCalendarPlanViewModelOperation.Result.MainCalendarWorks,
                x => x.Equals(Constants.OtherExpensesCalendarWork));
        }

        [Fact]
        public async Task ItShould_be_able_to_give_correct_total_percentages()
        {
            // arrange

            var estimateStream1 = StreamHelper.GetMock();
            var estimateStream2 = StreamHelper.GetMock();

            var viewModel = new CalendarPlanViewModel
            {
                EstimateFiles = new FormFileCollection
                {
                    FormFileHelper.GetMock(estimateStream1).Object,
                    FormFileHelper.GetMock(estimateStream2).Object,
                },
                PreparatoryCalendarWorks = new List<CalendarWorkViewModel>
                {
                    new()
                    {
                        WorkName = Constants.PreparatoryWorkName,
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                    new()
                    {
                        WorkName = Constants.PreparatoryTemporaryBuildingsWorkName,
                        Percentages = new List<decimal> { 0.4M, 0.6M }
                    },
                },
                MainCalendarWorks = new List<CalendarWorkViewModel>
                {
                    new()
                    {
                        WorkName = "Main work 1",
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                    new()
                    {
                        WorkName = "Main work 2",
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                    new()
                    {
                        WorkName = Constants.MainOtherExpensesWorkName,
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                },
                TotalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter,
            };

            var calendarPlanAppender = CalendarPlanAppenderHelper.GetMock();
            var estimateService = EstimateServiceHelper.GetMock();

            var estimate = new Estimate
            {
                PreparatoryEstimateWorks = new List<EstimateWork>
                {
                    new() { Chapter = Constants.PreparatoryWorkChapter },
                    new() { Chapter = Constants.PreparatoryWorkChapter },
                    new() { Chapter = Constants.PreparatoryTemporaryBuildingsWorkChapter },
                    new() { Chapter = Constants.PreparatoryTemporaryBuildingsWorkChapter },
                },
                MainEstimateWorks = new List<EstimateWork>
                {
                    new()
                    {
                        WorkName = "Main work 1",
                        Percentages = new List<decimal>()
                    },
                    new()
                    {
                        WorkName = "Main work 2",
                        Percentages = new List<decimal>()
                    },
                }
            };
            estimateService.Setup(x => x.GetEstimate(viewModel.EstimateFiles, viewModel.TotalWorkChapter))
                .ReturnsAsync(new OperationResult<Estimate> { Result = estimate });

            var mapper = MapperHelper.GetMock();
            mapper.Setup(x => x.Map<CalendarPlanViewModel>(estimate)).Returns(viewModel);

            var estimateAfterPreparing = new Estimate
            {
                PreparatoryEstimateWorks = new List<EstimateWork>
                {
                    new()
                    {
                        Chapter = Constants.PreparatoryWorkChapter,
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                    new()
                    {
                        Chapter = Constants.PreparatoryWorkChapter,
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                    new()
                    {
                        Chapter = Constants.PreparatoryTemporaryBuildingsWorkChapter,
                        Percentages = new List<decimal> { 0.4M, 0.6M }
                    },
                    new()
                    {
                        Chapter = Constants.PreparatoryTemporaryBuildingsWorkChapter,
                        Percentages = new List<decimal> { 0.4M, 0.6M }
                    },
                },
                MainEstimateWorks = new List<EstimateWork>
                {
                    new()
                    {
                        WorkName = "Main work 1",
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                    new()
                    {
                        WorkName = "Main work 2",
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                }
            };
            var calendarPlanCalculator = CalendarPlanCalculatorHelper.GetMock();

            var totalWork = new CalendarWork()
            {
                WorkName = Constants.TotalWorkName
            };
            var preparatoryCalendarPlan = new CalendarPlan
            {
                CalendarWorks = new List<CalendarWork>
                {
                    totalWork
                }
            };
            calendarPlanCalculator
                .Setup(
                    x => x.CalculatePreparatory(estimateAfterPreparing, new List<decimal> { 0.6M, 0.4M }, new List<decimal> { 0.4M, 0.6M }))
                .ReturnsAsync(new OperationResult<CalendarPlan> { Result = preparatoryCalendarPlan });

            var expectedTotalPercentages = new List<decimal> { 0.56M, 0.44M };
            var mainCalendarPlan = new CalendarPlan
            {
                CalendarWorks = new List<CalendarWork>
                {
                    new()
                    {
                        EstimateChapter = (int)viewModel.TotalWorkChapter,
                        ConstructionMonths = new List<ConstructionMonth>
                        {
                            new()
                            {
                                PercentPart = 0.56M,
                            },
                            new()
                            {
                                PercentPart = 0.44M,
                            },
                        }
                    }
                }
            };
            calendarPlanCalculator
                .Setup(x => x.CalculateMain(estimate, totalWork, new List<decimal> { 0.6M, 0.4M }))
                .ReturnsAsync(new OperationResult<CalendarPlan> { Result = mainCalendarPlan });

            var documentFactory = MyWordDocumentFactoryHelper.GetMock();
            var sut = new CalendarPlanService(documentFactory.Object, estimateService.Object,
                calendarPlanCalculator.Object, calendarPlanAppender.Object, mapper.Object);

            // act

            var getTotalPercentagesOperation = await sut.GetTotalPercentages(viewModel);

            // assert

            Assert.True(getTotalPercentagesOperation.Ok);

            Assert.True(expectedTotalPercentages.SequenceEqual(getTotalPercentagesOperation.Result));
        }

        [Fact]
        public async Task ItShould_be_able_to_give_correct_calendar_plan_stream()
        {
            // arrange

            var estimateStream1 = StreamHelper.GetMock();
            var estimateStream2 = StreamHelper.GetMock();

            var viewModel = new CalendarPlanViewModel
            {
                EstimateFiles = new FormFileCollection
                {
                    FormFileHelper.GetMock(estimateStream1).Object,
                    FormFileHelper.GetMock(estimateStream2).Object,
                },
                PreparatoryCalendarWorks = new List<CalendarWorkViewModel>
                {
                    new()
                    {
                        WorkName = Constants.PreparatoryWorkName,
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                    new()
                    {
                        WorkName = Constants.PreparatoryTemporaryBuildingsWorkName,
                        Percentages = new List<decimal> { 0.4M, 0.6M }
                    },
                },
                MainCalendarWorks = new List<CalendarWorkViewModel>
                {
                    new()
                    {
                        WorkName = "Main work 1",
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                    new()
                    {
                        WorkName = "Main work 2",
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                    new()
                    {
                        WorkName = Constants.MainOtherExpensesWorkName,
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                },
                TotalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter,
            };

            var calendarPlanAppender = CalendarPlanAppenderHelper.GetMock();
            var estimateService = EstimateServiceHelper.GetMock();

            var estimate = new Estimate
            {
                PreparatoryEstimateWorks = new List<EstimateWork>
                {
                    new() { Chapter = Constants.PreparatoryWorkChapter },
                    new() { Chapter = Constants.PreparatoryWorkChapter },
                    new() { Chapter = Constants.PreparatoryTemporaryBuildingsWorkChapter },
                    new() { Chapter = Constants.PreparatoryTemporaryBuildingsWorkChapter },
                },
                MainEstimateWorks = new List<EstimateWork>
                {
                    new()
                    {
                        WorkName = "Main work 1",
                        Percentages = new List<decimal>()
                    },
                    new()
                    {
                        WorkName = "Main work 2",
                        Percentages = new List<decimal>()
                    },
                }
            };
            estimateService.Setup(x => x.GetEstimate(viewModel.EstimateFiles, viewModel.TotalWorkChapter))
                .ReturnsAsync(new OperationResult<Estimate> { Result = estimate });

            var mapper = MapperHelper.GetMock();
            mapper.Setup(x => x.Map<CalendarPlanViewModel>(estimate)).Returns(viewModel);

            var estimateAfterPreparing = new Estimate
            {
                PreparatoryEstimateWorks = new List<EstimateWork>
                {
                    new()
                    {
                        Chapter = Constants.PreparatoryWorkChapter,
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                    new()
                    {
                        Chapter = Constants.PreparatoryWorkChapter,
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                    new()
                    {
                        Chapter = Constants.PreparatoryTemporaryBuildingsWorkChapter,
                        Percentages = new List<decimal> { 0.4M, 0.6M }
                    },
                    new()
                    {
                        Chapter = Constants.PreparatoryTemporaryBuildingsWorkChapter,
                        Percentages = new List<decimal> { 0.4M, 0.6M }
                    },
                },
                MainEstimateWorks = new List<EstimateWork>
                {
                    new()
                    {
                        WorkName = "Main work 1",
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                    new()
                    {
                        WorkName = "Main work 2",
                        Percentages = new List<decimal> { 0.6M, 0.4M }
                    },
                }
            };
            var calendarPlanCalculator = CalendarPlanCalculatorHelper.GetMock();

            var totalWork = new CalendarWork()
            {
                WorkName = Constants.TotalWorkName
            };
            var preparatoryCalendarPlan = new CalendarPlan
            {
                CalendarWorks = new List<CalendarWork>
                {
                    totalWork
                }
            };
            calendarPlanCalculator
                .Setup(
                    x => x.CalculatePreparatory(estimateAfterPreparing, new List<decimal> { 0.6M, 0.4M }, new List<decimal> { 0.4M, 0.6M }))
                .ReturnsAsync(new OperationResult<CalendarPlan> { Result = preparatoryCalendarPlan });

            var mainCalendarPlan = new CalendarPlan
            {
                CalendarWorks = new List<CalendarWork>
                {
                    new()
                    {
                        EstimateChapter = (int)viewModel.TotalWorkChapter,
                        ConstructionMonths = new List<ConstructionMonth>
                        {
                            new()
                            {
                                PercentPart = 0.56M,
                            },
                            new()
                            {
                                PercentPart = 0.44M,
                            },
                        }
                    }
                }
            };
            calendarPlanCalculator
                .Setup(x => x.CalculateMain(estimate, totalWork, new List<decimal> { 0.6M, 0.4M }))
                .ReturnsAsync(new OperationResult<CalendarPlan> { Result = mainCalendarPlan });

            var section = MySectionHelper.GetMock();
            var document = MyWordDocumentHelper.GetMock(section);
            var documentFactory = MyWordDocumentFactoryHelper.GetMock(document);
            var sut = new CalendarPlanService(documentFactory.Object, estimateService.Object,
                calendarPlanCalculator.Object, calendarPlanAppender.Object, mapper.Object);

            // act

            var getCalendarPlanStreamOperation = await sut.GetCalendarPlanStream(viewModel);

            // assert

            Assert.True(getCalendarPlanStreamOperation.Ok);

            Assert.NotNull(getCalendarPlanStreamOperation.Result);

            calendarPlanAppender.Verify(x => x.AppendAsync(section.Object, preparatoryCalendarPlan, CalendarPlanType.Preparatory), Times.Once);

            calendarPlanAppender.Verify(x => x.AppendAsync(section.Object, mainCalendarPlan, CalendarPlanType.Main), Times.Once);
        }
    }
}