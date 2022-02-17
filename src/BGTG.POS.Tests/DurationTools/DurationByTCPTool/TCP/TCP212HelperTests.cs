using BGTG.POS.DurationTools.DurationByTCPTool.TCP;
using NUnit.Framework;

namespace BGTG.POS.Tests.DurationTools.DurationByTCPTool.TCP
{
    public class TCP212HelperTests
    {
        private TCP212Helper _tcp212Helper = null!;

        [SetUp]
        public void SetUp()
        {
            _tcp212Helper = new TCP212Helper();
        }

        [Test]
        public void GetAppendix_AKey_AppendixA()
        {
            var appendixKey = 'A';
            var expectedAppendix = TCP212.AppendixA;

            var actualAppendix = _tcp212Helper.GetAppendix(appendixKey);

            Assert.AreEqual(expectedAppendix, actualAppendix);
        }

        [Test]
        public void GetAppendix_BKey_AppendixB()
        {
            var appendixKey = 'B';
            var expectedAppendix = TCP212.AppendixB;

            var actualAppendix = _tcp212Helper.GetAppendix(appendixKey);

            Assert.AreEqual(expectedAppendix, actualAppendix);
        }

        [Test]
        public void GetPipelineCharacteristic_UrbanSlopesSteelPipelineWith300Diameter_CorrectPipelineCharacteristic()
        {
            var pipelineMaterial = "стальных труб";
            var pipelineDiameter = 300;
            var appendix = TCP212.AppendixA;
            var pipelineCategoryName = "Уличные трубопроводы водо-, газоснабжения и канализации, сооружаемые в траншеях с откосами";

            var expectedPipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 500, "до 500"), new[]
            {
                new PipelineStandard(0.1M, 1M, 0.3M),
                new PipelineStandard(0.5M, 2M, 0.3M),
                new PipelineStandard(1M, 2.5M, 0.3M),
                new PipelineStandard(1.5M, 4M, 0.5M),
            });

            var actualPipelineCharacteristic = _tcp212Helper.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter, pipelineCategoryName);

            Assert.AreEqual(expectedPipelineCharacteristic, actualPipelineCharacteristic);
        }

        [Test]
        public void GetPipelineCharacteristic_UrbanFastenersSteelPipelineWith300Diameter_CorrectPipelineCharacteristic()
        {
            var pipelineMaterial = "стальных труб";
            var pipelineDiameter = 300;
            var appendix = TCP212.AppendixA;
            var pipelineCategoryName = "Уличные трубопроводы водо-, газоснабжения и канализации, сооружаемые в траншеях с креплением стенок";

            var expectedPipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 500, "до 500"), new[]
            {
                new PipelineStandard(0.1M, 1.5M, 0.3M),
                new PipelineStandard(0.5M, 2.5M, 0.3M),
                new PipelineStandard(1M, 3.5M, 0.3M),
                new PipelineStandard(1.5M, 5.5M, 0.5M),
            });

            var actualPipelineCharacteristic = _tcp212Helper.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter, pipelineCategoryName);

            Assert.AreEqual(expectedPipelineCharacteristic, actualPipelineCharacteristic);
        }

        [Test]
        public void GetPipelineCharacteristic_CountrySteelPipelineWith300Diameter_CorrectPipelineCharacteristic()
        {
            var pipelineMaterial = "стальных труб";
            var pipelineDiameter = 300;
            var appendix = TCP212.AppendixB;
            var pipelineCategoryName = "Наружные трубопроводы";

            var expectedPipelineCharacteristic = new PipelineCharacteristic(new DiameterRange(0, 400, "до 400"), new[]
            {
                new PipelineStandard(1M, 2M, 0M),
                new PipelineStandard(2M, 3M, 0M),
                new PipelineStandard(5M, 4M, 0M),
                new PipelineStandard(10M, 6M, 0M),
            });

            var actualPipelineCharacteristic = _tcp212Helper.GetPipelineCharacteristic(appendix, pipelineMaterial, pipelineDiameter, pipelineCategoryName);

            Assert.AreEqual(expectedPipelineCharacteristic, actualPipelineCharacteristic);
        }
    }
}
