using NUnit.Framework;
using POSCore.EstimateLogic;
using POSCore.EstimateLogic.Interfaces;
using System.Linq;

namespace POSCoreTests.EstimateReaderTests
{
    public class ReadShould
    {
        private IEstimateReader _estimateReader;

        private readonly string _estimate1 = @"C:\Users\kss\source\repos\POS\POSCoreTests\EstimateReaderTests\estimate1.xlsx";
        private readonly string _estimate2 = @"C:\Users\kss\source\repos\POS\POSCoreTests\EstimateReaderTests\estimate2.xlsx";

        [SetUp]
        public void SetUp()
        {
            _estimateReader = new EstimateReader();
        }

        // STARTS WITH: ОБЪЕКТНАЯ СМЕТА
        [Test]
        public void ReturnEstimateWorks_ThatStartsWithObektnayaSmeta()
        {
            var estimateWorks = _estimateReader.Read(_estimate1);

            var estimateWorksList = estimateWorks.ToList();

            var recultivationEstimateWorkExistance = estimateWorksList.Exists(x => x.WorkName == "РЕКУЛЬТИВАЦИЯ" && x.EquipmentCost == 0 && x.OtherProductsCost == 0 && x.TotalCost == 0.001);

            Assert.IsTrue(recultivationEstimateWorkExistance);
        }

        // STARTS WITH: НИИ БЕЛГИПРОТОПГАЗ 
        [Test]
        public void ReturnEstimateWorks_ThatStartsWithNiiBelgiprotopgaz()
        {
            var estimateWorks = _estimateReader.Read(_estimate2);

            var estimateWorksList = estimateWorks.ToList();

            var channelTracingWorkExistance = estimateWorksList.Exists(x => x.WorkName == "ТРАССИРОВКА КАНАЛОВ (8,04 КМ)" && x.EquipmentCost == 0 && x.OtherProductsCost == 2.163 && x.TotalCost == 2.163);

            Assert.IsTrue(channelTracingWorkExistance);
        }

        // EQUAL: ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%
        [Test]
        public void ReturnEstimateWorks_ThatContainsTemporaryBuildingsWork()
        {
            var estimateWorks = _estimateReader.Read(_estimate1);

            var estimateWorksList = estimateWorks.ToList();

            var temporaryBuildingsWorkExistance = estimateWorksList.Exists(x => x.WorkName == "ВРЕМЕННЫЕ ЗДАНИЯ И СООРУЖЕНИЯ 8,56Х0,93 - 7,961%" && x.EquipmentCost == 0 && x.OtherProductsCost == 0 && x.TotalCost == 0.253);

            Assert.IsTrue(temporaryBuildingsWorkExistance);
        }

        // EQUAL: ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ
        [Test]
        public void ReturnEstimateWorks_ThatContainsTotalWork()
        {
            var estimateWorks = _estimateReader.Read(_estimate1);

            var estimateWorksList = estimateWorks.ToList();

            var recultivationEstimateWorkExistance = estimateWorksList.Exists(x => x.WorkName == "ВСЕГО ПО СВОДНОМУ СМЕТНОМУ РАСЧЕТУ" && x.EquipmentCost == 0 && x.OtherProductsCost == 9.911 && x.TotalCost == 30.085);

            Assert.IsTrue(recultivationEstimateWorkExistance);
        }

        [Test]
        public void ReturnEstimateWorks_ThatContainsLandscapingWorkWithRightEquipmentCost()
        {
            {
                var estimateWorks = _estimateReader.Read(_estimate1);

                var estimateWorksList = estimateWorks.ToList();

                var landscapingWork = estimateWorksList.Find(x => x.WorkName == "БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ");

                Assert.AreEqual(0.001, landscapingWork.EquipmentCost);
            }
        }

        [Test]
        public void ReturnEstimateWorks_ThatContainsLandscapingWorkWithRightOtherProductsCost()
        {
            {
                var estimateWorks = _estimateReader.Read(_estimate1);

                var estimateWorksList = estimateWorks.ToList();

                var landscapingWork = estimateWorksList.Find(x => x.WorkName == "БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ");

                Assert.AreEqual(0.002, landscapingWork.OtherProductsCost);
            }
        }

        [Test]
        public void ReturnEstimateWorks_ThatContainsLandscapingWorkWithRightTotalCost()
        {
            {
                var estimateWorks = _estimateReader.Read(_estimate1);

                var estimateWorksList = estimateWorks.ToList();

                var landscapingWork = estimateWorksList.Find(x => x.WorkName == "БЛАГОУСТРОЙСТВО ТЕРРИТОРИИ");

                Assert.AreEqual(0.038, landscapingWork.TotalCost);
            }
        }
    }
}
