using System.Collections.Generic;
using Moq;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;

namespace POS.Tests.Helpers.Services.DocumentServices.WordService
{
    public static class MySectionHelper
    {
        public static Mock<IMySection> GetMock(Mock<IMyTable> table, MyTableFormat? tableFormat, MyCellFormat? cellFormat, MyParagraphFormat? paragraphFormat, MyCharacterFormat? characterFormat, MyTableStyle? tableStyle)
        {
            var section = new Mock<IMySection>();
            section.Setup(x => x.AddTable(It.IsAny<int>(), It.IsAny<int>(), tableFormat, cellFormat, paragraphFormat, characterFormat, tableStyle)).Returns(table.Object);
            return section;
        }

        public static Mock<IMySection> GetMock()
        {
            var section = new Mock<IMySection>();
            section
                .Setup(x => x.AddTable(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<MyTableFormat>(), It.IsAny<MyCellFormat>(), It.IsAny<MyParagraphFormat>(), It.IsAny<MyCharacterFormat>(), It.IsAny<MyTableStyle>()))
                .Returns(MyTableHelper.GetMock().Object);

            section.Setup(x => x.AddParagraph()).Returns(MyParagraphHelper.GetMock().Object);

            return section;
        }

        public static Mock<IMySection> GetMock(IReadOnlyList<Mock<IMyTable>> tables)
        {
            var section = new Mock<IMySection>();

            for (int i = 0; i < tables.Count; i++)
            {
                section.Setup(x => x.Tables[i]).Returns(tables[i].Object);
            }

            return section;
        }
    }
}