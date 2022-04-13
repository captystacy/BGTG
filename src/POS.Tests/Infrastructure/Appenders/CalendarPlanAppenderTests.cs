using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Appenders;
using POS.Infrastructure.Extensions;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using POS.Models.CalendarPlanModels;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Appenders
{
    public class CalendarPlanAppenderTests
    {
        public CalendarPlanAppenderTests()
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        }

        [Fact]
        public async Task ItShould_set_correct_header()
        {
            // arrange

            var fixture = new Fixture();

            var calendarPlan = fixture
                .Build<CalendarPlan>()
                .With(x => x.ConstructionStartDate, DateTime.Now)
                .With(x => x.ConstructionDurationCeiling, 3)
                .With(x => x.CalendarWorks, new List<CalendarWork>
                {
                    fixture
                        .Build<CalendarWork>()
                        .With(x => x.WorkName, "Итого:")
                        .Create(),
                })
                .Create();

            var firstRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock()
            };

            var secondRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var table = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(firstRowCells),
                MyRowHelper.GetMock(secondRowCells),
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(),
            });

            var section = MySectionHelper.GetMock(table, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            var sut = new CalendarPlanAppender();

            // act

            await sut.AppendAsync(section.Object, calendarPlan, CalendarPlanType.Main);

            // assert

            firstRowCells[0].Verify(x => x.AddParagraph("Наименование отдельных зданий, сооружений и видов работ"),
                Times.Once);
            firstRowCells[1].Verify(x => x.AddParagraph("Сметная стоимость, тыс. руб."), Times.Once);
            firstRowCells[3].Verify(
                x => x.AddParagraph("Распределение кап. вложений и объемов СМР по месяцам строительства, тыс. руб."),
                Times.Once);

            secondRowCells[1].Verify(x => x.AddParagraph("всего"), Times.Once);
            secondRowCells[2].Verify(x => x.AddParagraph("в т.ч. СМР"), Times.Once);
            secondRowCells[3].Verify(x => x.AddParagraph(DateTime.Now.ToString("MMMM yyyy").MakeFirstLetterUppercase()), Times.Once);
            secondRowCells[4].Verify(x => x.AddParagraph(DateTime.Now.AddMonths(1).ToString("MMMM yyyy").MakeFirstLetterUppercase()), Times.Once);
            secondRowCells[5].Verify(x => x.AddParagraph(DateTime.Now.AddMonths(2).ToString("MMMM yyyy").MakeFirstLetterUppercase()), Times.Once);
        }

        [Fact]
        public async Task ItShould_set_correct_calendar_works_values()
        {
            // arrange

            var fixture = new Fixture();

            var constructionMonths = new List<ConstructionMonth>
            {
                fixture
                    .Build<ConstructionMonth>()
                    .With(x => x.CreationIndex, 0)
                    .Create(),
                fixture
                    .Build<ConstructionMonth>()
                    .With(x => x.CreationIndex, 2)
                    .Create(),
            };

            var calendarWork = fixture
                .Build<CalendarWork>()
                .With(x => x.WorkName, "Итого:")
                .With(x => x.ConstructionMonths, constructionMonths)
                .Create();

            var calendarPlan = fixture
                .Build<CalendarPlan>()
                .With(x => x.ConstructionDurationCeiling, 3)
                .With(x => x.CalendarWorks, new List<CalendarWork>
                {
                    calendarWork
                })
                .Create();

            var thirdRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };
            var thirdRow = MyRowHelper.GetMock(thirdRowCells);

            var fourthRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };
            var fourthRow = MyRowHelper.GetMock(fourthRowCells);

            var table = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(),
                thirdRow,
                fourthRow,
                MyRowHelper.GetMock(),
            });

            var section = MySectionHelper.GetMock(table, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            var sut = new CalendarPlanAppender();

            // act

            await sut.AppendAsync(section.Object, calendarPlan, CalendarPlanType.Main);

            // assert

            thirdRowCells[0].Verify(x => x.AddParagraph(calendarWork.WorkName), Times.Once);
            thirdRowCells[1].Verify(x => x.AddParagraph(calendarWork.TotalCost.ToString(Constants.DecimalThreePlacesFormat)), Times.Once);
            thirdRowCells[2].Verify(x => x.AddParagraph(calendarWork.TotalCostIncludingCAIW.ToString(Constants.DecimalThreePlacesFormat)), Times.Once);
            thirdRowCells[3].Verify(x => x.AddParagraph(constructionMonths[0].InvestmentVolume.ToString(Constants.DecimalThreePlacesFormat), Constants.Underlined), Times.Once);
            thirdRowCells[4].Verify(x => x.AddParagraph("-"), Times.Once);
            thirdRowCells[5].Verify(x => x.AddParagraph(constructionMonths[1].InvestmentVolume.ToString(Constants.DecimalThreePlacesFormat), Constants.Underlined), Times.Once);

            fourthRowCells[3].Verify(x => x.AddParagraph(constructionMonths[0].VolumeCAIW.ToString(Constants.DecimalThreePlacesFormat)), Times.Once);
            fourthRowCells[5].Verify(x => x.AddParagraph(constructionMonths[1].VolumeCAIW.ToString(Constants.DecimalThreePlacesFormat)), Times.Once);
        }

        [Fact]
        public async Task ItShould_set_correct_footer_for_main_calendar_plan()
        {
            // arrange

            var fixture = new Fixture();

            var constructionMonths = new List<ConstructionMonth>
            {
                fixture
                    .Build<ConstructionMonth>()
                    .With(x => x.CreationIndex, 0)
                    .Create(),
                fixture
                    .Build<ConstructionMonth>()
                    .With(x => x.CreationIndex, 1)
                    .Create(),
            };

            var calendarPlan = fixture
                .Build<CalendarPlan>()
                .With(x => x.ConstructionStartDate, DateTime.Now)
                .With(x => x.ConstructionDurationCeiling, 3)
                .With(x => x.CalendarWorks, new List<CalendarWork>
                {
                    fixture
                        .Build<CalendarWork>()
                        .With(x => x.WorkName, "Итого:")
                        .With(x => x.ConstructionMonths, constructionMonths)
                        .Create(),
                })
                .Create();

            var firstRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock()
            };

            var secondRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var lastRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var table = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(firstRowCells),
                MyRowHelper.GetMock(secondRowCells),
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(lastRowCells),
            });

            var section = MySectionHelper.GetMock(table, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            var sut = new CalendarPlanAppender();

            // act

            await sut.AppendAsync(section.Object, calendarPlan, CalendarPlanType.Main);

            // assert

            lastRowCells[0]
                .Verify(x => x.AddParagraph("Примечание: в числителе – объем капвложений, в знаменателе – объем СМР."),
                    Times.Once);
            lastRowCells[1].Verify(x => x.AddParagraph("Задел, %"), Times.Once);

            lastRowCells[3].Verify(x => x.AddParagraph(constructionMonths[0].PercentPart.ToString("P2")), Times.Once);
            lastRowCells[4].Verify(x => x.AddParagraph(constructionMonths[1].PercentPart.ToString("P2")), Times.Once);
        }

        [Fact]
        public async Task ItShould_set_correct_footer_for_preparatory_calendar_plan()
        {
            // arrange

            var fixture = new Fixture();

            var constructionMonths = new List<ConstructionMonth>
            {
                fixture
                    .Build<ConstructionMonth>()
                    .With(x => x.CreationIndex, 0)
                    .Create(),
                fixture
                    .Build<ConstructionMonth>()
                    .With(x => x.CreationIndex, 1)
                    .Create(),
            };

            var calendarPlan = fixture
                .Build<CalendarPlan>()
                .With(x => x.ConstructionStartDate, DateTime.Now)
                .With(x => x.ConstructionDurationCeiling, 3)
                .With(x => x.CalendarWorks, new List<CalendarWork>
                {
                    fixture
                        .Build<CalendarWork>()
                        .With(x => x.WorkName, "Итого:")
                        .With(x => x.ConstructionMonths, constructionMonths)
                        .Create(),
                })
                .Create();

            var firstRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock()
            };

            var secondRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var lastRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var table = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(firstRowCells),
                MyRowHelper.GetMock(secondRowCells),
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(lastRowCells),
            });

            var section = MySectionHelper.GetMock(table, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            var sut = new CalendarPlanAppender();

            // act

            await sut.AppendAsync(section.Object, calendarPlan, CalendarPlanType.Preparatory);

            // assert

            lastRowCells[0]
                .Verify(x => x.AddParagraph("Примечание: в числителе – объем капвложений, в знаменателе – объем СМР."),
                    Times.Once);
            lastRowCells[1].Verify(x => x.AddParagraph("Задел, %"), Times.Never);

            lastRowCells[3].Verify(x => x.AddParagraph(constructionMonths[0].PercentPart.ToString("P2")), Times.Never);
            lastRowCells[4].Verify(x => x.AddParagraph(constructionMonths[1].PercentPart.ToString("P2")), Times.Never);
        }

        [Fact]
        public async Task ItShould_set_acceptance_time_cell_text()
        {
            // arrange

            var fixture = new Fixture();

            var constructionMonths = new List<ConstructionMonth>
            {
                fixture
                    .Build<ConstructionMonth>()
                    .With(x => x.CreationIndex, 0)
                    .Create(),
                fixture
                    .Build<ConstructionMonth>()
                    .With(x => x.CreationIndex, 1)
                    .Create(),
            };

            var calendarPlan = fixture
                .Build<CalendarPlan>()
                .With(x => x.ConstructionStartDate, DateTime.Now)
                .With(x => x.ConstructionDurationCeiling, 2)
                .With(x => x.CalendarWorks, new List<CalendarWork>
                {
                    fixture
                        .Build<CalendarWork>()
                        .With(x => x.WorkName, "Итого:")
                        .With(x => x.ConstructionMonths, constructionMonths)
                        .Create(),
                })
                .Create();

            var thirdRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var table = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(thirdRowCells),
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(),
            });

            var section = MySectionHelper.GetMock(table, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            var sut = new CalendarPlanAppender();

            // act

            await sut.AppendAsync(section.Object, calendarPlan, CalendarPlanType.Main);

            // assert

            thirdRowCells[^1].Verify(x => x.AddParagraph("Приемка объекта в эксплуатацию"), Times.Once);
        }

        [Fact]
        public async Task ItShould_do_not_set_acceptance_time_cell_text()
        {
            // arrange

            var fixture = new Fixture();

            var constructionMonths = new List<ConstructionMonth>
            {
                fixture
                    .Build<ConstructionMonth>()
                    .With(x => x.CreationIndex, 0)
                    .Create(),
                fixture
                    .Build<ConstructionMonth>()
                    .With(x => x.CreationIndex, 1)
                    .Create(),
            };

            var calendarPlan = fixture
                .Build<CalendarPlan>()
                .With(x => x.ConstructionStartDate, DateTime.Now)
                .With(x => x.ConstructionDurationCeiling, 1)
                .With(x => x.CalendarWorks, new List<CalendarWork>
                {
                    fixture
                        .Build<CalendarWork>()
                        .With(x => x.WorkName, "Итого:")
                        .With(x => x.ConstructionMonths, constructionMonths)
                        .Create(),
                })
                .Create();

            var thirdRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var table = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(thirdRowCells),
                MyRowHelper.GetMock(),
                MyRowHelper.GetMock(),
            });

            var section = MySectionHelper.GetMock(table, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            var sut = new CalendarPlanAppender();

            // act

            await sut.AppendAsync(section.Object, calendarPlan, CalendarPlanType.Main);

            // assert

            thirdRowCells[^1].Verify(x => x.AddParagraph((string)"Приемка объекта в эксплуатацию"), Times.Never);
        }
    }
}