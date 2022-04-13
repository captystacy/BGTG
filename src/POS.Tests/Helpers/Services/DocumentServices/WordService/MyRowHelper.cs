using System.Collections.Generic;
using Moq;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Tests.Helpers.Services.DocumentServices.WordService
{
    public static class MyRowHelper
    {
        public static Mock<IMyRow> GetMock(IReadOnlyList<Mock<IMyCell>> cells)
        {
            var row = new Mock<IMyRow>();
            var setupSequence = row.SetupSequence(x => x.AddCell());

            for (var i = 0; i < cells.Count; i++)
            {
                var cell = cells[i].Object;
                setupSequence.Returns(cell);
                row.SetupGet(x => x.Cells[i]).Returns(cell);
            }

            return row;
        }

        public static Mock<IMyRow> GetMock()
        {
            var row = new Mock<IMyRow>();

            row.SetupGet(x => x.Cells[It.IsAny<int>()]).Returns(MyCellHelper.GetMock().Object);

            return row;
        }

        public static Mock<IMyRow> GetMock(int cellIndex, string cellStr)
        {
            var row = new Mock<IMyRow>();

            row.SetupGet(x => x.Cells[cellIndex]).Returns(MyCellHelper.GetMock(cellStr).Object);

            return row;
        }
    }
}