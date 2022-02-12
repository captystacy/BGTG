using System.IO;
using BGTG.POS.EstimateTool;
using NUnit.Framework;

namespace BGTG.POS.Tests.EstimateTool
{
    public class EstimateReaderTests
    {
        private EstimateReader _estimateReader;

        private const string EstimateSourceFilesDirectory = @"..\..\..\EstimateTool\EstimateSourceFiles";

        [SetUp]
        public void SetUp()
        {
            _estimateReader = new EstimateReader();
        }

        [Test]
        public void Read_EstimateSourceFile548_CorrectEstimate()
        {
            var expectedEstimate = EstimateSource.Estimate548VAT;

            var estimatePath = Path.Combine(EstimateSourceFilesDirectory, "5.5-20.548VAT.xlsx");

            Estimate actualEstimate;
            using (var fileStream = new FileStream(estimatePath, FileMode.Open))
            {
                actualEstimate = _estimateReader.Read(fileStream, TotalWorkChapter.TotalWork1To12Chapter);
            }

            Assert.AreEqual(expectedEstimate, actualEstimate);
        }

        [Test]
        public void Read_EstimateSourceFile158VAT_CorrectEstimate()
        {
            var expectedEstimate = EstimateSource.Estimate158VAT;

            var estimatePath = Path.Combine(EstimateSourceFilesDirectory, "5.4-18.158VAT.xlsx");

            Estimate actualEstimate;
            using (var fileStream = new FileStream(estimatePath, FileMode.Open))
            {
                actualEstimate = _estimateReader.Read(fileStream, TotalWorkChapter.TotalWork1To12Chapter);
            }

            Assert.AreEqual(expectedEstimate, actualEstimate);
        }
    }
}
