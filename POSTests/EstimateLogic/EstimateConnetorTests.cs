using System;
using System.Collections.Generic;
using NUnit.Framework;
using POS.EstimateLogic;

namespace POSTests.EstimateLogic
{
    public class EstimateConnetorTests
    {
        private EstimateConnector _estimateConnector;

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
                new EstimateWork("Демонтажные работы", 0, 11.659M, 109.474M, 1),
                new EstimateWork("Подготовительные работы", 0, 0, 4.094M, 1),
                new EstimateWork("Обследование строительных конструкций с ндс", 0, 18.131M, 18.131M, 1),
                new EstimateWork("Организация дорожного движения на период строительства", 0, 0, 0.063M, 8),
                new EstimateWork("Временные здания и сооружения 3,76х0,86 - 3,234%", 0, 0, 23.598M, 8),
            },
            new List<EstimateWork>
            {
                new EstimateWork("Здание гаражей с блоком бытовых помещений и складами", 112.869M, 2.924M, 1098.187M, 2),
                new EstimateWork("Абк", 277.031M, 3.051M, 1440.621M, 2),
                new EstimateWork("Склад материалов", 0, 0.544M, 100.598M, 2),
                new EstimateWork("Склад баллонов", 68.38M, 0.827M, 277.016M, 2),
                new EstimateWork("Навес для машин на 8 м/мест", 0, 0, 88.493M, 4),
                new EstimateWork("Эстакада", 0, 0, 26.782M, 4),
                new EstimateWork("Внутриплощадочные сети электроснабжения", 26.139M, 0, 52.076M, 4),
                new EstimateWork("Внутриплощадочные сети автоматизации", 0, 0, 7.274M, 4),
                new EstimateWork("Внутриплощадочные сети телемеханизации", 0, 0, 3.928M, 4),
                new EstimateWork("Электроснабжение. подстанции", 3.206M, 0, 3.572M, 4),
                new EstimateWork("Внутриплощадочные сети системы контроля и управления доступом", 6.458M, 0, 16.049M, 5),
                new EstimateWork("Внутриплощадочные сети пожарной сигнализации", 0.254M, 0, 16.021M, 5),
                new EstimateWork("Внутриплощадочные сети видеонаблюдения", 3.8M, 0, 12.013M, 5),
                new EstimateWork("Организация дорожного движения на период эксплуатации", 0, 0, 1.204M, 5),
                new EstimateWork("Внутриплощадочные сети связи", 0, 0, 2.196M, 5),
                new EstimateWork("Системы радиосвязи", 0, 0, 2.319M, 5),
                new EstimateWork("Газоснабжение. наружные газопроводы", 0, 0, 8.803M, 6),
                new EstimateWork("Узел редуцирования", 0.088M, 0, 4.09M, 6),
                new EstimateWork("Наружные сети водоснабжения и канализации", 2.202M, 0.018M, 74.368M, 6),
                new EstimateWork("Тепловые сети", 0.048M, 0, 20.679M, 6),
                new EstimateWork("Благоустройство", 0, 18.42M, 1064.607M, 7),
                new EstimateWork("Всего по сводному сметному расчету", 512.514M, 1900.851M, 6442.849M, 12),

            }, new DateTime(2019, 7, 1), 6, 6, "5.4-18.158", 80110);

            var actualEstimate = _estimateConnector.Connect(new List<Estimate> { EstimateSource.Estimate158VAT, EstimateSource.Estimate158VATFree });

            Assert.AreEqual(expectedEstimate, actualEstimate);
        }
    }
}
