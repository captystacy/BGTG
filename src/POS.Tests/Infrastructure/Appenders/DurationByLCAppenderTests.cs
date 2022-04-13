using System.Collections.Generic;
using AutoFixture;
using Moq;
using POS.Infrastructure.AppConstants;
using POS.Infrastructure.Appenders;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using POS.Models;
using POS.Tests.Helpers.Services.DocumentServices.WordService;
using Xunit;

namespace POS.Tests.Infrastructure.Appenders
{
    public class DurationByLCAppenderTests
    {
        [Fact]
        public void ItShould_set_correct_formula_table()
        {
            // arrange
            
            var fixture = new Fixture();

            var durationByLC = fixture.Create<DurationByLC>();

            var firstRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var secondRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var formulaTable = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(firstRowCells),
                MyRowHelper.GetMock(secondRowCells),
            });

            var section = MySectionHelper.GetMock(formulaTable, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            section
                .Setup(x => x.AddTable(3, 2, Constants.TableHorizontalCentered, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(formulaTable.Object);

            section
                .Setup(x => x.AddTable(4, 5, null!, null!, null!, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            var sut = new DurationByLCAppender();

            // act

            sut.AppendAsync(section.Object, durationByLC);

            // assert

            firstRowCells[0].Verify(x => x.AddParagraph("Т ="), Times.Once);
            firstRowCells[1].Verify(x => x.AddParagraph(durationByLC.TotalLaborCosts.ToString()), Times.Once);
            firstRowCells[2].Verify(x => x.AddParagraph($"= {durationByLC.Duration} мес."), Times.Once);

            secondRowCells[1].Verify(x => x.AddParagraph($"{durationByLC.NumberOfWorkingDays}х{durationByLC.WorkingDayDuration}х{durationByLC.Shift}х{durationByLC.NumberOfEmployees}"), Times.Once);
        }

        [Fact]
        public void ItShould_set_correct_description_table()
        {
            // arrange

            var fixture = new Fixture();

            var durationByLC = fixture
                .Build<DurationByLC>()
                .With(x => x.TotalLaborCosts, 123)
                .Create();

            var firstRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var secondRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var thirdRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var fourthRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var fifthRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var descriptionTable = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(firstRowCells),
                MyRowHelper.GetMock(secondRowCells),
                MyRowHelper.GetMock(thirdRowCells),
                MyRowHelper.GetMock(fourthRowCells),
                MyRowHelper.GetMock(fifthRowCells),
            });

            var section = MySectionHelper.GetMock(descriptionTable, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            section
                .Setup(x => x.AddTable(3, 2, Constants.TableHorizontalCentered, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            section
                .Setup(x => x.AddTable(4, 5, null!, null!, null!, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(descriptionTable.Object);

            var sut = new DurationByLCAppender();

            // act

            sut.AppendAsync(section.Object, durationByLC);

            // assert

            firstRowCells[0].Verify(x => x.AddParagraph("где"), Times.Once);
            firstRowCells[1].Verify(x => x.AddParagraph(durationByLC.TotalLaborCosts.ToString()), Times.Once);
            firstRowCells[2].Verify(x => x.AddParagraph(Constants.DashSymbolStr), Times.Once);
            firstRowCells[3].Verify(x => x.AddParagraph("нормативные трудозатраты (трудозатраты по сметам и трудозатраты по технологической карте), человеко/часов;"), Times.Once);

            secondRowCells[1].Verify(x => x.AddParagraph(durationByLC.WorkingDayDuration.ToString()), Times.Once());
            secondRowCells[2].Verify(x => x.AddParagraph(Constants.DashSymbolStr), Times.Once);
            secondRowCells[3].Verify(x => x.AddParagraph("продолжительность рабочего дня, часов;"), Times.Once());

            thirdRowCells[1].Verify(x => x.AddParagraph(durationByLC.Shift.ToString()), Times.Once());
            thirdRowCells[2].Verify(x => x.AddParagraph(Constants.DashSymbolStr), Times.Once);
            thirdRowCells[3].Verify(x => x.AddParagraph("сменность;"), Times.Once());

            fourthRowCells[1].Verify(x => x.AddParagraph(durationByLC.NumberOfWorkingDays.ToString()), Times.Once());
            fourthRowCells[2].Verify(x => x.AddParagraph(Constants.DashSymbolStr), Times.Once);
            fourthRowCells[3].Verify(x => x.AddParagraph("количество рабочих дней в месяце;"), Times.Once());

            fifthRowCells[1].Verify(x => x.AddParagraph(durationByLC.NumberOfEmployees.ToString()), Times.Once());
            fifthRowCells[2].Verify(x => x.AddParagraph(Constants.DashSymbolStr), Times.Once);
            fifthRowCells[3].Verify(x => x.AddParagraph("количество работающих в бригаде в соответствии с технологией производства ремонтно-строительных работ."), Times.Once());
        }


        [Fact]
        public void ItShould_set_correct_labor_costs_description_text_when_technological_labor_costs_is_zero()
        {
            // arrange

            var fixture = new Fixture();

            var durationByLC = fixture
                .Build<DurationByLC>()
                .With(x => x.TechnologicalLaborCosts, 0)
                .Create();

            var firstRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var secondRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var thirdRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var fourthRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var fifthRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var descriptionTable = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(firstRowCells),
                MyRowHelper.GetMock(secondRowCells),
                MyRowHelper.GetMock(thirdRowCells),
                MyRowHelper.GetMock(fourthRowCells),
                MyRowHelper.GetMock(fifthRowCells),
            });

            var section = MySectionHelper.GetMock(descriptionTable, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            section
                .Setup(x => x.AddTable(3, 2, Constants.TableHorizontalCentered, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            section
                .Setup(x => x.AddTable(4, 5, null!, null!, null!, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(descriptionTable.Object);

            var sut = new DurationByLCAppender();

            // act

            sut.AppendAsync(section.Object, durationByLC);

            // assert

            firstRowCells[3].Verify(x => x.AddParagraph("нормативные трудозатраты, человеко/часов;"), Times.Once);
        }

        [Fact]
        public void ItShould_set_correct_labor_costs_description_text_when_technological_labor_costs_is_above_zero()
        {
            // arrange

            var fixture = new Fixture();

            var durationByLC = fixture
                .Build<DurationByLC>()
                .With(x => x.TechnologicalLaborCosts, 1)
                .Create();

            var firstRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var secondRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var thirdRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var fourthRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var fifthRowCells = new List<Mock<IMyCell>>
            {
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
                MyCellHelper.GetMock(),
            };

            var descriptionTable = MyTableHelper.GetMock(new List<Mock<IMyRow>>
            {
                MyRowHelper.GetMock(firstRowCells),
                MyRowHelper.GetMock(secondRowCells),
                MyRowHelper.GetMock(thirdRowCells),
                MyRowHelper.GetMock(fourthRowCells),
                MyRowHelper.GetMock(fifthRowCells),
            });

            var section = MySectionHelper.GetMock(descriptionTable, null!, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, null!, MyTableStyle.TableGrid);

            section
                .Setup(x => x.AddTable(3, 2, Constants.TableHorizontalCentered, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            section
                .Setup(x => x.AddTable(4, 5, null!, null!, null!, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(descriptionTable.Object);

            var sut = new DurationByLCAppender();

            // act

            sut.AppendAsync(section.Object, durationByLC);

            // assert

            firstRowCells[3].Verify(x => x.AddParagraph("нормативные трудозатраты (трудозатраты по сметам и трудозатраты по технологической карте), человеко/часов;"), Times.Once);
        }

        [Fact]
        public void ItShould_add_correct_penultimate_paragraph_when_rounding_is_included_and_acceptance_time_is_included()
        {
            // arrange

            var fixture = new Fixture();

            var durationByLC = fixture
                .Build<DurationByLC>()
                .With(x => x.RoundingIncluded, true)
                .With(x => x.AcceptanceTimeIncluded, true)
                .Create();

            var section = MySectionHelper.GetMock();

            section
                .Setup(x => x.AddTable(3, 2, Constants.TableHorizontalCentered, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            section
                .Setup(x => x.AddTable(4, 5, null!, null!, null!, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            var sut = new DurationByLCAppender();

            // act

            sut.AppendAsync(section.Object, durationByLC);

            // assert

            section.Verify(x=>x.AddParagraph(
                $"С учетом округления в соответствии с 4.36 ТКП 45-1.03-122-2015 нормативная продолжительность строительства составит {durationByLC.RoundedDuration} мес, " +
                "с учетом времени на приемку объекта в эксплуатацию и утверждения акта приемки объекта в эксплуатацию согласно ТКП 45-1.03-122-2015 " +
                $"п. 4.22 общая продолжительность строительства составит – Tобщ = {durationByLC.RoundedDuration} + {durationByLC.AcceptanceTime} = {durationByLC.TotalDuration} мес."), Times.Once);
        }

        [Fact]
        public void ItShould_add_correct_penultimate_paragraph_when_rounding_is_included_and_acceptance_time_is_not_included()
        {
            // arrange

            var fixture = new Fixture();

            var durationByLC = fixture
                .Build<DurationByLC>()
                .With(x => x.RoundingIncluded, true)
                .With(x => x.AcceptanceTimeIncluded, false)
                .Create();

            var section = MySectionHelper.GetMock();

            section
                .Setup(x => x.AddTable(3, 2, Constants.TableHorizontalCentered, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            section
                .Setup(x => x.AddTable(4, 5, null!, null!, null!, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            var sut = new DurationByLCAppender();

            // act

            sut.AppendAsync(section.Object, durationByLC);

            // assert

            section.Verify(x => x.AddParagraph(
                $"С учетом округления в соответствии с 4.36 ТКП 45-1.03-122-2015 нормативная продолжительность строительства составит {durationByLC.RoundedDuration} мес."), Times.Once);
        }

        [Fact]
        public void ItShould_add_correct_penultimate_paragraph_when_rounding_is_not_included_and_acceptance_time_is_included()
        {
            // arrange

            var fixture = new Fixture();

            var durationByLC = fixture
                .Build<DurationByLC>()
                .With(x => x.RoundingIncluded, false)
                .With(x => x.AcceptanceTimeIncluded, true)
                .Create();

            var section = MySectionHelper.GetMock();

            section
                .Setup(x => x.AddTable(3, 2, Constants.TableHorizontalCentered, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            section
                .Setup(x => x.AddTable(4, 5, null!, null!, null!, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            var sut = new DurationByLCAppender();

            // act

            sut.AppendAsync(section.Object, durationByLC);

            // assert

            section.Verify(x => x.AddParagraph(
                "Нормативная продолжительность строительства с учетом времени на приемку объекта в эксплуатацию и утверждения акта приемки объекта в эксплуатацию согласно ТКП 45-1.03-122-2015 " +
                $"п. 4.22 общая продолжительность строительства составит – Tобщ = {durationByLC.RoundedDuration} + {durationByLC.AcceptanceTime} = {durationByLC.TotalDuration} мес."), Times.Once);
        }

        [Fact]
        public void ItShould_do_not_add_penultimate_paragraph_when_rounding_is_not_included_and_acceptance_time_is_not_included()
        {
            // arrange

            var fixture = new Fixture();

            var durationByLC = fixture
                .Build<DurationByLC>()
                .With(x => x.RoundingIncluded, false)
                .With(x => x.AcceptanceTimeIncluded, false)
                .Create();

            var section = MySectionHelper.GetMock();

            section
                .Setup(x => x.AddTable(3, 2, Constants.TableHorizontalCentered, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            section
                .Setup(x => x.AddTable(4, 5, null!, null!, null!, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            var sut = new DurationByLCAppender();

            // act

            sut.AppendAsync(section.Object, durationByLC);

            // assert

            section.Verify(x => x.AddParagraph(
                $"С учетом округления в соответствии с 4.36 ТКП 45-1.03-122-2015 нормативная продолжительность строительства составит {durationByLC.RoundedDuration} мес, " +
                "с учетом времени на приемку объекта в эксплуатацию и утверждения акта приемки объекта в эксплуатацию согласно ТКП 45-1.03-122-2015 " +
                $"п. 4.22 общая продолжительность строительства составит – Tобщ = {durationByLC.RoundedDuration} + {durationByLC.AcceptanceTime} = {durationByLC.TotalDuration} мес."), Times.Never);
            section.Verify(x => x.AddParagraph(
                $"С учетом округления в соответствии с 4.36 ТКП 45-1.03-122-2015 нормативная продолжительность строительства составит {durationByLC.RoundedDuration} мес."), Times.Never);
            section.Verify(x => x.AddParagraph(
                "Нормативная продолжительность строительства с учетом времени на приемку объекта в эксплуатацию и утверждения акта приемки объекта в эксплуатацию согласно ТКП 45-1.03-122-2015 " +
                $"п. 4.22 общая продолжительность строительства составит – Tобщ = {durationByLC.RoundedDuration} + {durationByLC.AcceptanceTime} = {durationByLC.TotalDuration} мес."), Times.Never);
        }

        [Fact]
        public void ItShould_add_correct_last_paragraph_when_acceptance_time_is_included()
        {
            // arrange

            var fixture = new Fixture();

            var durationByLC = fixture
                .Build<DurationByLC>()
                .With(x => x.AcceptanceTimeIncluded, true)
                .Create();

            var section = MySectionHelper.GetMock();

            section
                .Setup(x => x.AddTable(3, 2, Constants.TableHorizontalCentered, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            section
                .Setup(x => x.AddTable(4, 5, null!, null!, null!, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            var sut = new DurationByLCAppender();

            // act

            sut.AppendAsync(section.Object, durationByLC);

            // assert

            section.Verify(x => x.AddParagraph(
                $"Принимаем продолжительность строительства равную {durationByLC.TotalDuration} мес, в том числе подготовительный период – {durationByLC.PreparatoryPeriod} мес, " +
                $"приемка объекта в эксплуатацию – {durationByLC.AcceptanceTime} мес."), Times.Once);
        }

        [Fact]
        public void ItShould_add_correct_last_paragraph_when_acceptance_time_is_not_included()
        {
            // arrange

            var fixture = new Fixture();

            var durationByLC = fixture
                .Build<DurationByLC>()
                .With(x => x.AcceptanceTimeIncluded, false)
                .Create();

            var section = MySectionHelper.GetMock();

            section
                .Setup(x => x.AddTable(3, 2, Constants.TableHorizontalCentered, Constants.VerticalCentered, Constants.ParagraphHorizontalCentered, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            section
                .Setup(x => x.AddTable(4, 5, null!, null!, null!, Constants.DefaultFontSize, MyTableStyle.TableCleared))
                .Returns(MyTableHelper.GetMock().Object);

            var sut = new DurationByLCAppender();

            // act

            sut.AppendAsync(section.Object, durationByLC);

            // assert

            section.Verify(x => x.AddParagraph(
                $"Принимаем продолжительность строительства равную {durationByLC.RoundedDuration} мес, в том числе подготовительный период – {durationByLC.PreparatoryPeriod} мес."), Times.Once);
        }
    }
}