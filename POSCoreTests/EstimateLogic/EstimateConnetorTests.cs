using NUnit.Framework;
using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;

namespace POSCoreTests.EstimateLogic
{
    public class EstimateConnetorTests
    {
        private EstimateAsserter _estimateAsserter;

        [SetUp]
        public void SetUp()
        {
            _estimateAsserter = new EstimateAsserter();
        }

        private EstimateConnector CreateDefaultEstimateConnector()
        {
            return new EstimateConnector();
        }

        [Test]
        public void Connect_OneEstimate_SameEstimate()
        {
            var constructionStartDate = new DateTime(2021, 9, 1);
            var constructionDuration = 1;
            var objectCipher = "5.5-20.548";

            var estimateVatFree = new Estimate(new List<EstimateWork>
            {
                new EstimateWork("РЕКУЛЬТИВАЦИЯ", 0, 0, 0.005M, 1, new List<decimal> { 1 }),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, 0.592M, 8, new List<decimal> { 1 }),
            },
            new List<EstimateWork>
            {
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 50.02M, 2),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, 0.004M, 0.092M, 7),
                new EstimateWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", 0, 11.519M, 63.689M, 10),
            }, constructionStartDate, constructionDuration, objectCipher, 16);

            var estimateConnector = CreateDefaultEstimateConnector();
            var estimate = estimateConnector.Connect(new List<Estimate> { estimateVatFree });

            _estimateAsserter.AssertEstimate(estimateVatFree, estimate);
        }

        [Test]
        public void Connect_TwoEstimates_CorrectOneEstimate()
        {
            var constructionStartDate = new DateTime(2021, 9, 1);
            var constructionDuration = 1;
            var objectCipher = "5.5-20.548";

            var estimateVat = new Estimate(new List<EstimateWork>
            {
                new EstimateWork("РЕКУЛЬТИВАЦИЯ", 0, 0, 0.001M, 1, new List<decimal> { 1 }),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, 0.253M, 8, new List<decimal> { 1 }),
            },
            new List<EstimateWork>
            {
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 19.308M, 2),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, 0.002M, 0.038M, 7),
                new EstimateWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", 0, 9.911M, 30.085M, 10),
            }, constructionStartDate, constructionDuration, objectCipher, 16);

            var estimateVatFree = new Estimate(new List<EstimateWork>
            {
                new EstimateWork("РЕКУЛЬТИВАЦИЯ", 0, 0, 0.005M, 1, new List<decimal> { 1 }),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, 0.592M, 8, new List<decimal> { 1 }),
            }, 
            new List<EstimateWork>
            {
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 50.02M, 2),
                new EstimateWork("ШРП", 0, 0.014M, 0.192M, 3),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, 0.004M, 0.092M, 7),
                new EstimateWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", 0, 11.519M, 63.689M, 10),
            }, constructionStartDate, constructionDuration, objectCipher, 17);

            var expectedEstimate = new Estimate(new List<EstimateWork>
            {
                new EstimateWork("РЕКУЛЬТИВАЦИЯ", 0, 0, 0.006M, 1, new List<decimal> { 1 }),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, 0.845M, 8, new List<decimal> { 1 }),
            }, 
            new List<EstimateWork>
            {
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", 0, 0, 69.328M, 2),
                new EstimateWork("ШРП", 0, 0.014M, 0.192M, 3),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, 0.006M, 0.13M, 7),
                new EstimateWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", 0, 21.43M, 93.774M, 10),
            }, constructionStartDate, constructionDuration, objectCipher, 33);

            var estimateConnector = CreateDefaultEstimateConnector();
            var estimate = estimateConnector.Connect(new List<Estimate> { estimateVat, estimateVatFree });

            _estimateAsserter.AssertEstimate(expectedEstimate, estimate);
        }
    }
}
