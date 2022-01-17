using NUnit.Framework;
using POSCore.EstimateLogic;
using System.IO;

namespace POSCoreTests.EstimateLogic
{
    public class EstimateReaderTests
    {
        private EstimateReader _estmateReader;

        private const string _estmateSourceFilesDirectory = @"..\..\..\EstimateLogic\EstimateSourceFiles";

        [SetUp]
        public void SetUp()
        {
            _estmateReader = new EstimateReader();
        }

        [Test]
        public void Read_EstimateSourceFile548_CorrectEstimate()
        {
            var expectedEstimate = EstimateSource.Estimate548VAT;

            var estimatePath = Path.Combine(_estmateSourceFilesDirectory, "5.5-20.548VAT.xlsx");

            var actualEstimate = default(Estimate);
            using (var fileStream = new FileStream(estimatePath, FileMode.Open))
            {
                actualEstimate = _estmateReader.Read(fileStream, TotalWorkChapter.TotalWork1To12Chapter);
            }

            Assert.AreEqual(expectedEstimate, actualEstimate);
        }

        [Test]
        public void Read_EstimateSourceFile158VAT_CorrectEstimate()
        {
            var expectedEstimate = EstimateSource.Estimate158VAT;

            var estimatePath = Path.Combine(_estmateSourceFilesDirectory, "5.4-18.158VAT.xlsx");

            var actualEstimate = default(Estimate);
            using (var fileStream = new FileStream(estimatePath, FileMode.Open))
            {
                actualEstimate = _estmateReader.Read(fileStream, TotalWorkChapter.TotalWork1To12Chapter);
            }

            Assert.AreEqual(expectedEstimate, actualEstimate);
        }
    }
}
