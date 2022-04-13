using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Calculators;
using POS.Models.CalendarPlanModels;
using POS.Models.EstimateModels;
using POS.Tests.Helpers.Calculators;
using Xunit;

namespace POS.Tests.Infrastructure.Calculators
{
    public class CalendarPlanCalculatorTests
    {
        [Fact]
        public async Task ItShould_create_correct_preparatory_calendar_plan()
        {
            //arrange

            var estimate = new Estimate()
            {
                ConstructionDuration = 2.5M,
                ConstructionDurationCeiling = 3,
                ConstructionStartDate = new DateTime(1999, 9, 21),
                PreparatoryEstimateWorks = new List<EstimateWork>
                {
                    new()
                    {
                        WorkName = "Демонтажные работы",
                        TotalCost = 1.4M,
                        EquipmentCost = 0.4M,
                        OtherProductsCost = 0.5M,
                        Chapter = 1,
                        Percentages = new List<decimal> {0.4M, 0, 0.6M},
                    },
                    new()
                    {
                        WorkName = "Вынос трассы в натуру",
                        TotalCost = 1.3M,
                        EquipmentCost = 0.3M,
                        OtherProductsCost = 0.4M,
                        Chapter = 1,
                        Percentages = new List<decimal> {0.4M, 0, 0.6M}
                    },
                    new()
                    {
                        WorkName = "Одд на период производства работ",
                        TotalCost = 1.2M,
                        EquipmentCost = 0.2M,
                        OtherProductsCost = 0.3M,
                        Chapter = 8,
                        Percentages = new List<decimal> {0.4M, 0, 0.6M}
                    },
                    new()
                    {
                        WorkName = "Временные здания и сооружения 8,56х0,93 - 7,961",
                        TotalCost = 1.1M,
                        EquipmentCost = 0.1M,
                        OtherProductsCost = 0.2M,
                        Chapter = 8,
                        Percentages = new List<decimal> {0.4M, 0, 0.6M}
                    },
                }
            };

            var operations = new List<OperationResult<CalendarWork>>
            {
                new()
                {
                    Result = new CalendarWork
                    {
                        WorkName = "Демонтажные работы",
                        TotalCost = 1.4M,
                        TotalCostIncludingCAIW = 0.5M,
                        EstimateChapter = 1,
                        ConstructionMonths = new List<ConstructionMonth>()
                        {
                            new()
                            {
                                CreationIndex = 0,
                                InvestmentVolume = 0.56M,
                                VolumeCAIW = 0.2M,
                                Date = new DateTime(1999, 9, 21),
                                PercentPart = 0.4M
                            },
                            new()
                            {
                                CreationIndex = 2,
                                InvestmentVolume = 0.84M,
                                VolumeCAIW = 0.3M,
                                Date = new DateTime(1999, 10, 21),
                                PercentPart = 0.6M
                            },
                        }
                    }
                },
                new()
                {
                    Result = new CalendarWork
                    {
                        WorkName = "Вынос трассы в натуру",
                        TotalCost = 1.3M,
                        TotalCostIncludingCAIW = 0.6M,
                        EstimateChapter = 1,
                        ConstructionMonths = new List<ConstructionMonth>()
                        {
                            new()
                            {
                                CreationIndex = 0,
                                InvestmentVolume = 0.52M,
                                VolumeCAIW = 0.24M,
                                Date = new DateTime(1999, 9, 21),
                                PercentPart = 0.4M
                            },
                            new()
                            {
                                CreationIndex = 2,
                                InvestmentVolume = 0.78M,
                                VolumeCAIW = 0.36M,
                                Date = new DateTime(1999, 10, 21),
                                PercentPart = 0.6M
                            },
                        }
                    }
                },
                new()
                {
                    Result = new CalendarWork
                    {
                        WorkName = "Одд на период производства работ",
                        TotalCost = 1.2M,
                        TotalCostIncludingCAIW = 0.7M,
                        EstimateChapter = 8,
                        ConstructionMonths = new List<ConstructionMonth>()
                        {
                            new()
                            {
                                CreationIndex = 0,
                                InvestmentVolume = 0.48M,
                                VolumeCAIW = 0.28M,
                                Date = new DateTime(1999, 9, 21),
                                PercentPart = 0.4M
                            },
                            new()
                            {
                                CreationIndex = 2,
                                InvestmentVolume = 0.72M,
                                VolumeCAIW = 0.42M,
                                Date = new DateTime(1999, 10, 21),
                                PercentPart = 0.6M
                            },
                        }
                    }
                },
                new()
                {
                    Result = new CalendarWork
                    {
                        WorkName = "Временные здания и сооружения 8,56х0,93 - 7,961",
                        TotalCost = 1.1M,
                        TotalCostIncludingCAIW = 0.8M,
                        EstimateChapter = 8,
                        ConstructionMonths = new List<ConstructionMonth>()
                        {
                            new()
                            {
                                CreationIndex = 0,
                                InvestmentVolume = 0.44M,
                                VolumeCAIW = 0.32M,
                                Date = new DateTime(1999, 9, 21),
                                PercentPart = 0.4M
                            },
                            new()
                            {
                                CreationIndex = 2,
                                InvestmentVolume = 0.66M,
                                VolumeCAIW = 0.48M,
                                Date = new DateTime(1999, 10, 21),
                                PercentPart = 0.6M
                            },
                        }
                    }
                },
            };

            var preparatoryWork = new CalendarWork
            {
                WorkName = "Подготовка территории строительства",
                TotalCost = 2.7M,
                TotalCostIncludingCAIW = 1.1M,
                ConstructionMonths = new List<ConstructionMonth>
                {
                    new()
                    {
                        Date = new DateTime(1999, 9, 21),
                        CreationIndex = 0,
                        InvestmentVolume = 1.08M,
                        VolumeCAIW = 0.44M,
                        PercentPart = 0.4M
                    },
                    new()
                    {
                        Date = new DateTime(1999, 10, 21),
                        CreationIndex = 2,
                        InvestmentVolume = 1.62M,
                        VolumeCAIW = 0.66M,
                        PercentPart = 0.6M
                    },
                },
                EstimateChapter = 1,
            };

            var temporaryBuildingsWork = new CalendarWork
            {
                WorkName = "Временные здания и сооружения",
                TotalCost = 2.3M,
                TotalCostIncludingCAIW = 1.5M,
                ConstructionMonths = new List<ConstructionMonth>
                {
                    new()
                    {
                        Date = new DateTime(1999, 9, 21),
                        CreationIndex = 0,
                        InvestmentVolume = 0.92M,
                        VolumeCAIW = 0.6M,
                        PercentPart = 0.4M
                    },
                    new()
                    {
                        Date = new DateTime(1999, 10, 21),
                        CreationIndex = 2,
                        InvestmentVolume = 1.38M,
                        VolumeCAIW = 0.9M,
                        PercentPart = 0.6M
                    },
                },
                EstimateChapter = 8,
            };

            var expectedCalendarPlan = new CalendarPlan
            {
                ConstructionStartDate = new DateTime(1999, 9, 21),
                ConstructionDuration = 2.5M,
                ConstructionDurationCeiling = 3,
                CalendarWorks = new List<CalendarWork>()
                {
                    preparatoryWork,
                    temporaryBuildingsWork,
                    new()
                    {
                        WorkName = "Итого:",
                        TotalCost = 5,
                        TotalCostIncludingCAIW = 2.6M,
                        ConstructionMonths = new List<ConstructionMonth>
                        {
                            new()
                            {
                                Date = new DateTime(1999, 9, 21),
                                CreationIndex = 0,
                                InvestmentVolume = 2M,
                                VolumeCAIW = 1.04M,
                                PercentPart = 0.4M
                            },
                            new()
                            {
                                Date = new DateTime(1999, 10, 21),
                                CreationIndex = 2,
                                InvestmentVolume = 3,
                                VolumeCAIW = 1.56M,
                                PercentPart = 0.6M
                            },
                        },
                        EstimateChapter = 1,
                    },
                }
            };

            var calendarWorkCalculator = CalendarWorkCalculatorHelper.GetMock(operations);


            var preparatoryPercentages = new List<decimal> { 0.4M, 0, 0.6M };

            var temporaryBuildingsPercentages = new List<decimal> { 0.4M, 0, 0.6M };

            calendarWorkCalculator
                .Setup(x => x.Calculate(Constants.PreparatoryWorkName, 2.7M, 1.1M, estimate.ConstructionStartDate,
                    preparatoryPercentages, 1))
                .ReturnsAsync(new OperationResult<CalendarWork>
                {
                    Result = preparatoryWork
                });

            calendarWorkCalculator
                .Setup(x => x.Calculate(Constants.PreparatoryTemporaryBuildingsWorkName, 2.3M, 1.5M,
                    estimate.ConstructionStartDate, temporaryBuildingsPercentages,
                    Constants.PreparatoryTemporaryBuildingsWorkChapter))
                .ReturnsAsync(new OperationResult<CalendarWork>
                {
                    Result = temporaryBuildingsWork
                });


            var sut = new CalendarPlanCalculator(calendarWorkCalculator.Object);

            // act

            var operation =
                await sut.CalculatePreparatory(estimate, preparatoryPercentages, temporaryBuildingsPercentages);

            // assert

            Assert.True(operation.Ok);

            var expectedCalendarWorks = expectedCalendarPlan.CalendarWorks.ToList();

            var actualCalendarWorks = operation.Result.CalendarWorks.ToList();

            Assert.Equal(expectedCalendarWorks.Count, actualCalendarWorks.Count);

            for (int i = 0; i < expectedCalendarWorks.Count; i++)
            {
                var expectedConstructionMonths = expectedCalendarWorks[i].ConstructionMonths.ToList();

                var actualConstructionMonths = actualCalendarWorks[i].ConstructionMonths.ToList();

                Assert.Equal(expectedConstructionMonths.Count, actualConstructionMonths.Count);

                for (int j = 0; j < expectedConstructionMonths.Count; j++)
                {
                    Assert.Equal(expectedConstructionMonths[j], actualConstructionMonths[j]);
                }

                Assert.Equal(expectedCalendarWorks[i], actualCalendarWorks[i]);
            }

            Assert.Equal(expectedCalendarPlan, operation.Result);
        }

        [Fact]
        public async Task ItShould_create_correct_main_calendar_plan()
        {
            // arrange

            var estimate = new Estimate
            {
                ConstructionDuration = 2.5M,
                ConstructionDurationCeiling = 3,
                ConstructionStartDate = new DateTime(1999, 9, 21),
                TotalWorkChapter = TotalWorkChapter.TotalWork1To12Chapter,
                MainEstimateWorks = new List<EstimateWork>
                {
                    new()
                    {
                        WorkName = "Демонтажные работы",
                        TotalCost = 1.4M,
                        EquipmentCost = 0.4M,
                        OtherProductsCost = 0.5M,
                        Chapter = 2,
                        Percentages = new List<decimal> {0.4M, 0, 0.6M}
                    },
                    new()
                    {
                        WorkName = "Вынос трассы в натуру",
                        TotalCost = 1.3M,
                        EquipmentCost = 0.3M,
                        OtherProductsCost = 0.4M,
                        Chapter = 3,
                        Percentages = new List<decimal> {0.4M, 0, 0.6M}
                    },
                    new()
                    {
                        WorkName = "Одд на период производства работ",
                        TotalCost = 1.2M,
                        EquipmentCost = 0.2M,
                        OtherProductsCost = 0.3M,
                        Chapter = 4,
                        Percentages = new List<decimal> {0.4M, 0, 0.6M}
                    },
                    new()
                    {
                        WorkName = "Временные здания и сооружения 8,56х0,93 - 7,961",
                        TotalCost = 1.1M,
                        EquipmentCost = 0.1M,
                        OtherProductsCost = 0.2M,
                        Chapter = 5,
                        Percentages = new List<decimal> {0.4M, 0, 0.6M}
                    },
                    new()
                    {
                        WorkName = "ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ",
                        TotalCost = 20.1M,
                        EquipmentCost = 5.4M,
                        OtherProductsCost = 5.4M,
                        Chapter = 12,
                        Percentages = new List<decimal> {0.4M, 0, 0.6M}
                    },
                }
            };


            var operationsCalendarWorks = new List<CalendarWork>
            {
                new()
                {
                    WorkName = "Демонтажные работы",
                    TotalCost = 1.4M,
                    TotalCostIncludingCAIW = 0.5M,
                    EstimateChapter = 2,
                    ConstructionMonths = new List<ConstructionMonth>()
                    {
                        new()
                        {
                            CreationIndex = 0,
                            InvestmentVolume = 0.56M,
                            VolumeCAIW = 0.2M,
                            Date = new DateTime(1999, 9, 21),
                            PercentPart = 0.4M
                        },
                        new()
                        {
                            CreationIndex = 2,
                            InvestmentVolume = 0.84M,
                            VolumeCAIW = 0.3M,
                            Date = new DateTime(1999, 10, 21),
                            PercentPart = 0.6M
                        },
                    },
                },
                new()
                {
                    WorkName = "Вынос трассы в натуру",
                    TotalCost = 1.3M,
                    TotalCostIncludingCAIW = 0.6M,
                    EstimateChapter = 3,
                    ConstructionMonths = new List<ConstructionMonth>()
                    {
                        new()
                        {
                            CreationIndex = 0,
                            InvestmentVolume = 0.52M,
                            VolumeCAIW = 0.24M,
                            Date = new DateTime(1999, 9, 21),
                            PercentPart = 0.4M
                        },
                        new()
                        {
                            CreationIndex = 2,
                            InvestmentVolume = 0.78M,
                            VolumeCAIW = 0.36M,
                            Date = new DateTime(1999, 10, 21),
                            PercentPart = 0.6M
                        },
                    }
                },
                new()
                {
                    WorkName = "Одд на период производства работ",
                    TotalCost = 1.2M,
                    TotalCostIncludingCAIW = 0.7M,
                    EstimateChapter = 4,
                    ConstructionMonths = new List<ConstructionMonth>()
                    {
                        new()
                        {
                            CreationIndex = 0,
                            InvestmentVolume = 0.48M,
                            VolumeCAIW = 0.28M,
                            Date = new DateTime(1999, 9, 21),
                            PercentPart = 0.4M
                        },
                        new()
                        {
                            CreationIndex = 2,
                            InvestmentVolume = 0.72M,
                            VolumeCAIW = 0.42M,
                            Date = new DateTime(1999, 10, 21),
                            PercentPart = 0.6M
                        },
                    }
                },
                new()
                {
                    WorkName = "Временные здания и сооружения 8,56х0,93 - 7,961",
                    TotalCost = 1.1M,
                    TotalCostIncludingCAIW = 0.8M,
                    EstimateChapter = 5,
                    ConstructionMonths = new List<ConstructionMonth>()
                    {
                        new()
                        {
                            CreationIndex = 0,
                            InvestmentVolume = 0.44M,
                            VolumeCAIW = 0.32M,
                            Date = new DateTime(1999, 9, 21),
                            PercentPart = 0.4M
                        },
                        new()
                        {
                            CreationIndex = 2,
                            InvestmentVolume = 0.66M,
                            VolumeCAIW = 0.48M,
                            Date = new DateTime(1999, 10, 21),
                            PercentPart = 0.6M
                        },
                    }
                },
                new()
                {
                    WorkName = "ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ",
                    TotalCost = 20.1M,
                    TotalCostIncludingCAIW = 10.8M,
                    EstimateChapter = 12,
                    ConstructionMonths = new List<ConstructionMonth>()
                    {
                        new()
                        {
                            CreationIndex = 0,
                            InvestmentVolume = 8.04M,
                            VolumeCAIW = 3.72M,
                            Date = new DateTime(1999, 9, 21),
                            PercentPart = 0.4M
                        },
                        new()
                        {
                            CreationIndex = 2,
                            InvestmentVolume = 12.06M,
                            VolumeCAIW = 5.58M,
                            Date = new DateTime(1999, 10, 21),
                            PercentPart = 0.6M
                        },
                    }
                },
            };

            var operations = new List<OperationResult<CalendarWork>>
            {
                new()
                {
                    Result = operationsCalendarWorks[0],
                },
                new()
                {
                    Result = operationsCalendarWorks[1],
                },
                new()
                {
                    Result = operationsCalendarWorks[2],
                },
                new()
                {
                    Result = operationsCalendarWorks[3],
                },
                new()
                {
                    Result = operationsCalendarWorks[4],
                },
            };

            var preparatoryTotalWork = new CalendarWork
            {
                WorkName = "Итого:",
                TotalCost = 5,
                TotalCostIncludingCAIW = 2.6M,
                ConstructionMonths = new List<ConstructionMonth>
                {
                    new()
                    {
                        Date = new DateTime(1999, 9, 21),
                        CreationIndex = 0,
                        InvestmentVolume = 2M,
                        VolumeCAIW = 1.04M,
                        PercentPart = 0.4M
                    },
                    new()
                    {
                        Date = new DateTime(1999, 10, 21),
                        CreationIndex = 2,
                        InvestmentVolume = 3,
                        VolumeCAIW = 1.56M,
                        PercentPart = 0.6M
                    },
                },
                EstimateChapter = 1,
            };

            var otherExpensesWork = new CalendarWork()
            {
                WorkName = Constants.MainOtherExpensesWorkName,
                EstimateChapter = Constants.MainOtherExpensesWorkChapter,
                TotalCost = 10.1M,
                TotalCostIncludingCAIW = 5.6M,
                ConstructionMonths = new List<ConstructionMonth>()
                {
                    new()
                    {
                        CreationIndex = 0,
                        InvestmentVolume = 4.04M,
                        VolumeCAIW = 2.24M,
                        Date = new DateTime(1999, 9, 21),
                        PercentPart = 0.4M
                    },
                    new()
                    {
                        CreationIndex = 2,
                        InvestmentVolume = 6.06M,
                        VolumeCAIW = 3.36M,
                        Date = new DateTime(1999, 10, 21),
                        PercentPart = 0.6M
                    },
                }
            };

            var calendarWorks = new List<CalendarWork>
            {
                new()
                {
                    WorkName = Constants.MainOverallPreparatoryWorkName,
                    TotalCost = 5,
                    TotalCostIncludingCAIW = 2.6M,
                    ConstructionMonths = new List<ConstructionMonth>
                    {
                        new()
                        {
                            Date = new DateTime(1999, 9, 21),
                            CreationIndex = 0,
                            InvestmentVolume = 2M,
                            VolumeCAIW = 1.04M,
                            PercentPart = 0.4M
                        },
                        new()
                        {
                            Date = new DateTime(1999, 10, 21),
                            CreationIndex = 2,
                            InvestmentVolume = 3,
                            VolumeCAIW = 1.56M,
                            PercentPart = 0.6M
                        },
                    },
                    EstimateChapter = 2,
                },
                operationsCalendarWorks[0],
                operationsCalendarWorks[1],
                operationsCalendarWorks[2],
                operationsCalendarWorks[3],
                otherExpensesWork,
                new()
                {
                    WorkName = "Итого:",
                    TotalCost = 20.1M,
                    TotalCostIncludingCAIW = 10.8M,
                    EstimateChapter = 12,
                    ConstructionMonths = new List<ConstructionMonth>()
                    {
                        new()
                        {
                            CreationIndex = 0,
                            InvestmentVolume = 8.04M,
                            VolumeCAIW = 4.32M,
                            Date = new DateTime(1999, 9, 21),
                            PercentPart = 0.4M
                        },
                        new()
                        {
                            CreationIndex = 2,
                            InvestmentVolume = 12.06M,
                            VolumeCAIW = 6.48M,
                            Date = new DateTime(1999, 10, 21),
                            PercentPart = 0.6M
                        },
                    }
                },
            };

            var expectedCalendarPlan = new CalendarPlan
            {
                ConstructionStartDate = new DateTime(1999, 9, 21),
                ConstructionDuration = 2.5M,
                ConstructionDurationCeiling = 3,
                CalendarWorks = calendarWorks,
            };

            var otherExpensesPercentages = new List<decimal> { 0.4M, 0, 0.6M };

            var calendarWorkCalculator = CalendarWorkCalculatorHelper.GetMock(operations);

            calendarWorkCalculator
                .Setup(x => x.Calculate(Constants.MainOtherExpensesWorkName, 10.1M, 5.6M,
                        estimate.ConstructionStartDate, otherExpensesPercentages,
                        Constants.MainOtherExpensesWorkChapter))
                    .ReturnsAsync(new OperationResult<CalendarWork>
                    {
                        Result = otherExpensesWork
                    });

            var sut = new CalendarPlanCalculator(calendarWorkCalculator.Object);

            // act

            var operation = await sut.CalculateMain(estimate, preparatoryTotalWork, otherExpensesPercentages);

            // assert

            Assert.True(operation.Ok);

            var expectedCalendarWorks = expectedCalendarPlan.CalendarWorks.ToList();

            var actualCalendarWorks = operation.Result.CalendarWorks.ToList();

            Assert.Equal(expectedCalendarWorks.Count, actualCalendarWorks.Count);

            for (int i = 0; i < expectedCalendarWorks.Count; i++)
            {
                var expectedConstructionMonths = expectedCalendarWorks[i].ConstructionMonths.ToList();

                var actualConstructionMonths = actualCalendarWorks[i].ConstructionMonths.ToList();

                Assert.Equal(expectedConstructionMonths.Count, actualConstructionMonths.Count);

                for (int j = 0; j < expectedConstructionMonths.Count; j++)
                {
                    Assert.Equal(expectedConstructionMonths[j], actualConstructionMonths[j]);
                }

                Assert.Equal(expectedCalendarWorks[i], actualCalendarWorks[i]);
            }

            Assert.Equal(expectedCalendarPlan, operation.Result);
        }
    }
}