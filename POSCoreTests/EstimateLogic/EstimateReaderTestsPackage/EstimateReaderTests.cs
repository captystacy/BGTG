using NUnit.Framework;
using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace POSCoreTests.EstimateLogic.EstimateReaderTestsPackage
{
    public class EstimateReaderTests
    {
        private string _defaultEstimateWorkSheet0;
        private string _defaultEstimateWorkSheet1;
        private string _defaultEstimateWorkSheet0UnappropriateValues;
        private EstimateAsserter _estimateAsserter;

        [SetUp]
        public void SetUp()
        {
            var cultureInfo = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var currentProjectDirectory = Path.Combine(projectDirectory, @"EstimateLogic\EstimateReaderTestsPackage");
            _defaultEstimateWorkSheet0 = Path.Combine(currentProjectDirectory, "defaultEstimateWorkSheet0.xlsx");
            _defaultEstimateWorkSheet0UnappropriateValues = Path.Combine(currentProjectDirectory, "defaultEstimateWorkSheet0UnappropriateValues.xlsx");
            _defaultEstimateWorkSheet1 = Path.Combine(currentProjectDirectory, "defaultEstimateWorkSheet1.xlsx");
            _estimateAsserter = new EstimateAsserter();
        }

        private EstimateReader CreateDefaultEstimateReader()
        {
            return new EstimateReader();
        }

        private Estimate DefaultExpectedEstimate()
        {
            var constructionStartDate = new DateTime(2022, 8, 1);
            var constructionDuration = 0.7M;
            return new Estimate(new List<EstimateWork>
            {
                new EstimateWork("Вынос трассы в натуру", 0, 0.013M, 0.013M, 1, new List<decimal> { 1 }),
                new EstimateWork("Временные здания и сооружения 8,56х0,93 - 7,961%", 0, 0, 0.012M, 8, new List<decimal> { 1 }),
            }, 
            new List<EstimateWork>
            {
                new EstimateWork("Электрохимическая защита", 0.04M, 0, 0.632M, 2),
                new EstimateWork("Благоустройство территории", 0, 0, 0.02M, 7),
                new EstimateWork("Одд на период производства работ", 0, 0, 0.005M, 8),
                new EstimateWork("Всего по сводному сметному расчету", 0.041M, 2.536M, 3.226M, 10),
            },constructionStartDate, constructionDuration, "5.5-20.548", 16);
        }

        [Test]
        public void Read_EstimateFileStreamWorkSheet0_CorrectEstimate()
        {
            var expectedEstimate = DefaultExpectedEstimate();

            var estimate = default(Estimate);
            var estimateReader = CreateDefaultEstimateReader();
            using (var fileStream = new FileStream(_defaultEstimateWorkSheet0, FileMode.Open))
            {
                estimate = estimateReader.Read(fileStream);
            }

            _estimateAsserter.AssertEstimate(expectedEstimate, estimate);
        }

        [Test]
        public void Read_EstimateFileStreamWorkSheet1_CorrectEstimate()
        {
            var expectedEstimate = DefaultExpectedEstimate();

            var estimate = default(Estimate);
            var estimateReader = CreateDefaultEstimateReader();
            using (var fileStream = new FileStream(_defaultEstimateWorkSheet1, FileMode.Open))
            {
                estimate = estimateReader.Read(fileStream);
            }

            _estimateAsserter.AssertEstimate(expectedEstimate, estimate);
        }

        [Test]
        public void Read_UnappropriateValues_Null()
        {
            var estimate = default(Estimate);
            var estimateReader = CreateDefaultEstimateReader();
            using (var fileStream = new FileStream(_defaultEstimateWorkSheet0UnappropriateValues, FileMode.Open))
            {
                estimate = estimateReader.Read(fileStream);
            }

            Assert.Null(estimate);
        }
    }
}
