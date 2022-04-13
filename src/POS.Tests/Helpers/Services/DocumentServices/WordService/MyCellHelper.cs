using System.Collections.Generic;
using Moq;
using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;

namespace POS.Tests.Helpers.Services.DocumentServices.WordService
{
    public static class MyCellHelper
    {
        public static Mock<IMyCell> GetMock()
        {
            var cell = new Mock<IMyCell>();
            cell.Setup(x => x.ApplyFormat(It.IsAny<MyCellFormat>())).Returns(cell.Object);
            return cell;
        }

        public static Mock<IMyCell> GetMock(string text)
        {
            var cell = new Mock<IMyCell>();

            cell.Setup(x => x.Paragraphs).Returns(new List<IMyParagraph>
            {
                MyParagraphHelper.GetMock(text).Object
            });

            return cell;
        }

        public static Mock<IMyCell> GetMock(Mock<IMyParagraph> paragraph)
        {
            var cell = new Mock<IMyCell>();
            cell.Setup(x => x.AddParagraph(paragraph.Object.Text)).Returns(paragraph.Object);
            cell.Setup(x => x.ApplyFormat(It.IsAny<MyCellFormat>())).Returns(cell.Object);
            return cell;
        }
    }
}