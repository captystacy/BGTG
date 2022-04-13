using System.Collections.Generic;
using System.Linq;
using Moq;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;

namespace POS.Tests.Helpers.Services.DocumentServices.WordService
{
    public static class MyTableHelper
    {
        public static Mock<IMyTable> GetMock(IDictionary<int, Mock<IMyRow>> rows)
        {
            var table = new Mock<IMyTable>();

            foreach (var row in rows)
            {
                table.SetupGet(x => x.Rows[row.Key]).Returns(row.Value.Object);
            }

            return table;
        }

        public static Mock<IMyTable> GetMock(IReadOnlyList<Mock<IMyRow>> rows)
        {
            var table = new Mock<IMyTable>();

            table.SetupGet(x => x.Rows).Returns(rows.Select(x => x.Object).ToList());

            for (int i = 0; i < rows.Count; i++)
            {
                table.SetupGet(x => x.Rows[i]).Returns(rows[i].Object);
            }

            return table;
        }

        public static Mock<IMyTable> GetMock()
        {
            var table = new Mock<IMyTable>();

            var row = MyRowHelper.GetMock().Object;
            table.SetupGet(x => x.Rows[It.IsAny<int>()]).Returns(row);

            return table;
        }
    }
}