﻿using NUnit.Framework;
using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;
using System.IO;

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
            var constructionDuration = (decimal)0.7;
            return new Estimate(new List<EstimateWork>
            {
                new EstimateWork("ВЫНОС ТРАССЫ В НАТУРУ", 0, (decimal)0.013, (decimal)0.013, 1),
                new EstimateWork("ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА", (decimal)0.04, 0, (decimal)0.632, 2),
                new EstimateWork("БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ", 0, 0, (decimal)0.02, 7),
                new EstimateWork("ОДД НА ПЕРИОД ПРОИЗВОДСТВА РАБОТ", 0, 0, (decimal)0.005, 8),
                new EstimateWork("ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%", 0, 0, (decimal)0.012, 8),
                new EstimateWork("ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ", (decimal)0.041, (decimal)2.536, (decimal)3.226, 10),
            }, constructionStartDate, constructionDuration, "5.5-20.548");
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
