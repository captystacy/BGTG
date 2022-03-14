using System;
using System.Collections.Generic;
using NUnit.Framework;
using POS.Infrastructure.Tools.EstimateTool;
using POS.Infrastructure.Tools.EstimateTool.Models;

namespace POS.Tests.Infrastructure.Tools.EstimateTool;

public class EstimateConnectorTests
{
    private EstimateConnector _estimateConnector = null!;

    [SetUp]
    public void SetUp()
    {
        _estimateConnector = new EstimateConnector();
    }

    [Test]
    public void Connect_OneEstimate548_SameEstimate()
    {
        var estimate = _estimateConnector.Connect(new List<Estimate> { EstimateSource.Estimate548VAT });

        Assert.AreEqual(EstimateSource.Estimate548VAT, estimate);
    }

    [Test]
    public void Connect_TwoEstimates158_CorrectOneEstimate()
    {
        var expectedEstimate = new Estimate(new List<EstimateWork>
            {
                new("Демонтажные работы", 0, 11.659M, 109.474M, 1),
                new("Подготовительные работы", 0, 0, 4.094M, 1),
                new("Обследование строительных конструкций с ндс", 0, 18.131M, 18.131M, 1),
                new("Организация дорожного движения на период строительства", 0, 0, 0.063M, 8),
                new("Временные здания и сооружения 3,76х0,86 - 3,234%", 0, 0, 23.598M, 8),
            },
            new List<EstimateWork>
            {
                new("Здание гаражей с блоком бытовых помещений и складами", 112.869M, 2.924M, 1098.187M, 2),
                new("Абк", 277.031M, 3.051M, 1440.621M, 2),
                new("Склад материалов", 0, 0.544M, 100.598M, 2),
                new("Склад баллонов", 68.38M, 0.827M, 277.016M, 2),
                new("Навес для машин на 8 м/мест", 0, 0, 88.493M, 4),
                new("Эстакада", 0, 0, 26.782M, 4),
                new("Внутриплощадочные сети электроснабжения", 26.139M, 0, 52.076M, 4),
                new("Внутриплощадочные сети автоматизации", 0, 0, 7.274M, 4),
                new("Внутриплощадочные сети телемеханизации", 0, 0, 3.928M, 4),
                new("Электроснабжение. подстанции", 3.206M, 0, 3.572M, 4),
                new("Внутриплощадочные сети системы контроля и управления доступом", 6.458M, 0, 16.049M, 5),
                new("Внутриплощадочные сети пожарной сигнализации", 0.254M, 0, 16.021M, 5),
                new("Внутриплощадочные сети видеонаблюдения", 3.8M, 0, 12.013M, 5),
                new("Организация дорожного движения на период эксплуатации", 0, 0, 1.204M, 5),
                new("Внутриплощадочные сети связи", 0, 0, 2.196M, 5),
                new("Системы радиосвязи", 0, 0, 2.319M, 5),
                new("Газоснабжение. наружные газопроводы", 0, 0, 8.803M, 6),
                new("Узел редуцирования", 0.088M, 0, 4.09M, 6),
                new("Наружные сети водоснабжения и канализации", 2.202M, 0.018M, 74.368M, 6),
                new("Тепловые сети", 0.048M, 0, 20.679M, 6),
                new("Благоустройство", 0, 18.42M, 1064.607M, 7),
                new("Всего по сводному сметному расчету", 512.514M, 1900.851M, 6442.849M, 12),

            }, new DateTime(2019, 7, 1), 6, 6, 80110);

        var actualEstimate = _estimateConnector.Connect(new List<Estimate> { EstimateSource.Estimate158VAT, EstimateSource.Estimate158VATFree });

        Assert.AreEqual(expectedEstimate, actualEstimate);
    }
}