using System.IO;
using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Readers.Base;
using POS.Models;

namespace POS.Tests.Helpers.Readers
{
    public static class DurationByLCReaderHelper
    {
        public static Mock<IDurationByLCReader> GetMock()
        {
            return new Mock<IDurationByLCReader>();
        }

        public static Mock<IDurationByLCReader> GetMock(Mock<Stream> durationByLCStream, DurationByLC durationByLC)
        {
            var durationByLCReader = GetMock();

            durationByLCReader
                .Setup(x => x.GetDurationByLC(durationByLCStream.Object))
                .ReturnsAsync(new OperationResult<DurationByLC> { Result = durationByLC });

            return durationByLCReader;
        }
    }
}
