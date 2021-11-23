using NUnit.Framework;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using System.IO;
using System.Linq;

namespace POSCoreTests.EstimateLogic.EstimateReaderTests
{
    public class ReadShould
    {
        private IEstimateReader _estimateReader;

        private string _estimate1;
        private string _estimate2;
        private string _estimate1WithBrokenTotalCost;
        private string _estimate1WithBrokenChapter;

        [SetUp]
        public void SetUp()
        {
            _estimateReader = new EstimateReader();
            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var currentProjectDirectory = Path.Combine(projectDirectory, @"EstimateLogic\EstimateReaderTests");
            _estimate1 = Path.Combine(currentProjectDirectory, "estimate1.xlsx");
            _estimate2 = Path.Combine(currentProjectDirectory, "estimate2.xlsx");
            _estimate1WithBrokenTotalCost = Path.Combine(currentProjectDirectory, "estimate1WithBrokenTotalCost.xlsx");
            _estimate1WithBrokenChapter = Path.Combine(currentProjectDirectory, "estimate1WithBrokenChapter.xlsx");
        }

        // STARTS WITH: ОБЪЕКТНАЯ СМЕТА
        [Test]
        public void ReturnEstimateWorks_ThatStartsWithObektnayaSmeta()
        {
            var estimate = _estimateReader.Read(_estimate1);

            var estimateWorksList = estimate.EstimateWorks.ToList();

            var recultivationEstimateWorkExistance = estimateWorksList.Exists(x => x.WorkName == "РЕКУЛЬТИВАЦИЯ" && x.EquipmentCost == 0 && x.OtherProductsCost == 0 && x.TotalCost == (decimal)0.001);

            Assert.IsTrue(recultivationEstimateWorkExistance);
        }

        // STARTS WITH: НИИ БЕЛГИПРОТОПГАЗ 
        [Test]
        public void ReturnEstimateWorks_ThatStartsWithNiiBelgiprotopgaz()
        {
            var estimate = _estimateReader.Read(_estimate2);

            var estimateWorks = estimate.EstimateWorks.ToList();

            var channelTracingWorkExistance = estimateWorks.Exists(x => x.WorkName == "ТРАССИРОВКА КАНАЛОВ (8,04 КМ)" && x.EquipmentCost == 0 && x.OtherProductsCost == (decimal)2.163 && x.TotalCost == (decimal)2.163);

            Assert.IsTrue(channelTracingWorkExistance);
        }

        // EQUAL: ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%
        [Test]
        public void ReturnEstimateWorks_ThatContainsTemporaryBuildingsWork()
        {
            var estimate = _estimateReader.Read(_estimate1);

            var estimateWorks = estimate.EstimateWorks.ToList();

            var temporaryBuildingsWorkExistance = estimateWorks.Exists(x => x.WorkName == "ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%" && x.EquipmentCost == 0 && x.OtherProductsCost == 0 && x.TotalCost == (decimal)0.253);

            Assert.IsTrue(temporaryBuildingsWorkExistance);
        }

        // EQUAL: ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ
        [Test]
        public void ReturnEstimateWorks_ThatContainsTotalWork()
        {
            var estimate = _estimateReader.Read(_estimate1);

            var estimateWorks = estimate.EstimateWorks.ToList();

            var recultivationEstimateWorkExistance = estimateWorks.Exists(x => x.WorkName == "ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ" && x.EquipmentCost == 0 && x.OtherProductsCost == (decimal)9.911 && x.TotalCost == (decimal)30.085);

            Assert.IsTrue(recultivationEstimateWorkExistance);
        }

        [Test]
        public void ReturnEstimateWorks_ThatContainsLandscapingWorkWithRightEquipmentCost()
        {
            var estimate = _estimateReader.Read(_estimate1);

            var estimateWorks = estimate.EstimateWorks.ToList();

            var landscapingWork = estimateWorks.Find(x => x.WorkName == "БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ");

            Assert.AreEqual(0.001, landscapingWork.EquipmentCost);
        }

        [Test]
        public void ReturnEstimateWorks_ThatContainsLandscapingWorkWithRightOtherProductsCost()
        {
            var estimate = _estimateReader.Read(_estimate1);

            var estimateWorks = estimate.EstimateWorks.ToList();

            var landscapingWork = estimateWorks.Find(x => x.WorkName == "БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ");

            Assert.AreEqual(0.002, landscapingWork.OtherProductsCost);
        }

        [Test]
        public void ReturnEstimateWorks_ThatContainsLandscapingWorkWithRightTotalCost()
        {
            var estimate = _estimateReader.Read(_estimate1);

            var estimateWorks = estimate.EstimateWorks.ToList();

            var landscapingWork = estimateWorks.Find(x => x.WorkName == "БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ");

            Assert.AreEqual(0.038, landscapingWork.TotalCost);
        }

        [Test]
        public void ReturnEstimateWorks_InWhichSetRightEstimateWorkChapter()
        {
            var estimate = _estimateReader.Read(_estimate1);

            var estimateWorks = estimate.EstimateWorks.ToList();

            var recultivationWork = estimateWorks.Find(x => x.WorkName == "РЕКУЛЬТИВАЦИЯ");
            Assert.AreEqual(1, recultivationWork.Chapter);

            var electrochemicalProtection = estimateWorks.Find(x => x.WorkName == "ЭЛЕКТРОХИМИЧЕСКАЯ ЗАЩИТА"); ;
            Assert.AreEqual(2, electrochemicalProtection.Chapter);

            var landscapingWork = estimateWorks.Find(x => x.WorkName == "БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ");
            Assert.AreEqual(7, landscapingWork.Chapter);

            var temporaryBuildingsWork = estimateWorks.Find(x => x.WorkName == "ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%");
            Assert.AreEqual(8, temporaryBuildingsWork.Chapter);

            var totalWork = estimateWorks.Find(x => x.WorkName == "ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ");
            Assert.AreEqual(10, totalWork.Chapter);
        }

        [Test]
        public void ReturnNull_IfEstimateWorkTotalCostIsBroken()
        {
            var estimate = _estimateReader.Read(_estimate1WithBrokenTotalCost);

            Assert.Null(estimate);
        }

        [Test]
        public void ReturnNull_IfEstimateWorkChapterIsBroken()
        {
            var estimate = _estimateReader.Read(_estimate1WithBrokenChapter);

            Assert.Null(estimate);
        }
    }
}
