using Moq;
using NUnit.Framework;
using POS.DomainModels.EstimateDomainModels;
using POS.Infrastructure.Readers;
using POS.Infrastructure.Services.Base;
using System.IO;

namespace POS.Tests.Infrastructure.Readers;

public class EstimateReaderTests
{
    private EstimateReader _estimateReader = null!;
    private Mock<IExcelDocumentService> _excelDocumentServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _excelDocumentServiceMock = new Mock<IExcelDocumentService>();
        _estimateReader = new EstimateReader(_excelDocumentServiceMock.Object);
    }

    [Test]
    public void Read()
    {
        _excelDocumentServiceMock.Setup(x => x.WorkSheetName).Returns("4237-C1");

        _excelDocumentServiceMock.Setup(x => x.GetCellText(20, 3)).Returns("август 2022");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(21, 3)).Returns("0,7 мес.");

        _excelDocumentServiceMock.Setup(x => x.GetCellText(27, 1)).Returns("АКТ ОТ 14.05.2021Г.");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(28, 1)).Returns("АКТ ОТ 14.05.2021Г.");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(29, 1)).Returns("НИИ БЕЛГИПРОТОПГАЗ");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(32, 1)).Returns("ОБЪЕКТНАЯ СМЕТА 1");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(35, 1)).Returns("ОБЪЕКТНАЯ СМЕТА 2");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(39, 1)).Returns("ОБЪЕКТНАЯ СМЕТА 3");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(41, 1)).Returns("НРР 8.01.102-2017");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(49, 1)).Returns("НРР 8.01.103-2017");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(70, 1)).Returns("НАЛОГ");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(76, 1)).Returns("ПОДПУНКТ 33.3.2  ИНСТРУКЦИИ");

        _excelDocumentServiceMock.Setup(x => x.GetCellText(46, 2)).Returns("ИТОГО ПО ГЛАВЕ 1-8");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(46, 9)).Returns("0,713\n16");

        _excelDocumentServiceMock.Setup(x => x.GetCellText(79, 2)).Returns("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(79, 7)).Returns("0,041\n0,001");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(79, 8)).Returns("2,536");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(79, 9)).Returns("3,226\n17");

        _excelDocumentServiceMock.Setup(x => x.GetCellText(29, 2)).Returns("ВЫНОС ТРАССЫ В НАТУРУ");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(29, 7)).Returns("-");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(29, 8)).Returns("0,013");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(29, 9)).Returns("0,013\n-");

        _excelDocumentServiceMock.Setup(x => x.GetCellText(32, 2)).Returns("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(32, 7)).Returns("0,04\n0,001");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(32, 8)).Returns("-");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(32, 9)).Returns("0,632\n15");

        _excelDocumentServiceMock.Setup(x => x.GetCellText(35, 2)).Returns("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(35, 7)).Returns("-\n-");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(35, 8)).Returns("-\n");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(35, 9)).Returns("0,02\n-");

        _excelDocumentServiceMock.Setup(x => x.GetCellText(39, 2)).Returns("ОДД НА ПЕРИОД ПРОИЗВОДСТВА РАБОТ");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(39, 7)).Returns("-\n-");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(39, 8)).Returns("-\n");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(39, 9)).Returns("0,005\n-");

        _excelDocumentServiceMock.Setup(x => x.GetCellText(41, 2)).Returns("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(41, 7)).Returns("-\n-");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(41, 8)).Returns("-\n");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(41, 9)).Returns("0,012\n0,745");

        _excelDocumentServiceMock.Setup(x => x.GetCellText(26, 2)).Returns("ГЛАВА 1 ПОДГОТОВКА ТЕРРИТОРИИ СТРОИТЕЛЬСТВА");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(31, 2)).Returns("ГЛАВА 2 ОСНОВНЫЕ ЗДАНИЯ,СООРУЖЕНИЯ");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(34, 2)).Returns("ГЛАВА 7 БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ");
        _excelDocumentServiceMock.Setup(x => x.GetCellText(38, 2)).Returns("ГЛАВА 8 ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ");

        var expectedEstimate = EstimateSource.Estimate548VAT;

        var memoryStream = new MemoryStream();
        var actualEstimate = _estimateReader.Read(memoryStream, TotalWorkChapter.TotalWork1To12Chapter);

        _excelDocumentServiceMock.Verify(x => x.Load(memoryStream), Times.Once());
        _excelDocumentServiceMock.Verify(x => x.Dispose(), Times.Once());
        Assert.AreEqual(expectedEstimate, actualEstimate);
    }
}