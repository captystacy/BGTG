using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using Spire.Doc;
using Spire.Doc.Documents;

namespace POS.Infrastructure.Services.DocumentServices.WordService
{
    public class MyCell : MyBody, IMyCell
    {
        private readonly TableCell _cell;
        protected override Body Body => _cell;

        public MyCell(TableCell cell, IReadOnlyList<IMyParagraph> paragraphs) : base(paragraphs)
        {
            _cell = cell;
        }

        public MyCell(TableCell cell, IReadOnlyList<IMyParagraph> paragraphs, MyParagraphFormat? paragraphFormat, MyCharacterFormat? characterFormat) : base(paragraphs, paragraphFormat, characterFormat)
        {
            _cell = cell;
        }

        public IMyCell ApplyFormat(MyCellFormat format)
        {
            if (format.VerticalAlignment is not null)
            {
                _cell.CellFormat.VerticalAlignment = format.VerticalAlignment switch
                {
                    MyVerticalAlignment.Top => VerticalAlignment.Top,
                    MyVerticalAlignment.Middle => VerticalAlignment.Middle,
                    MyVerticalAlignment.Bottom => VerticalAlignment.Bottom,
                    _ => throw new ArgumentOutOfRangeException(nameof(format.VerticalAlignment), format.VerticalAlignment, null)
                };
            }

            if (format.TextDirection is not null)
            {
                _cell.CellFormat.TextDirection = format.TextDirection switch
                {
                    MyTextDirection.TopToBottom => TextDirection.TopToBottom,
                    MyTextDirection.RightToLeftRotated => TextDirection.RightToLeftRotated,
                    MyTextDirection.LeftToRight => TextDirection.LeftToRight,
                    MyTextDirection.RightToLeft => TextDirection.RightToLeft,
                    MyTextDirection.TopToBottomRotated => TextDirection.TopToBottomRotated,
                    MyTextDirection.LeftToRightRotated => TextDirection.LeftToRightRotated,
                    _ => throw new ArgumentOutOfRangeException(nameof(format.TextDirection), format.TextDirection, null)
                };
            }

            if (format.Border is not null)
            {
                if (format.Border.Bottom is not null)
                {
                    _cell.CellFormat.Borders.Bottom.BorderType = format.Border.Bottom switch
                    {
                        MyBorderStyle.Single => BorderStyle.Single,
                        MyBorderStyle.Thick => BorderStyle.Thick,
                        MyBorderStyle.Double => BorderStyle.Double,
                        MyBorderStyle.Cleared => BorderStyle.Cleared,
                        _ => throw new ArgumentOutOfRangeException(nameof(format.Border.Bottom), format.Border.Bottom, null)
                    };
                }

                if (format.Border.Top is not null)
                {
                    _cell.CellFormat.Borders.Top.BorderType = format.Border.Top switch
                    {
                        MyBorderStyle.Single => BorderStyle.Single,
                        MyBorderStyle.Thick => BorderStyle.Thick,
                        MyBorderStyle.Double => BorderStyle.Double,
                        MyBorderStyle.Cleared => BorderStyle.Cleared,
                        _ => throw new ArgumentOutOfRangeException(nameof(format.Border.Top), format.Border.Top, null)
                    };
                }

                if (format.Border.Left is not null)
                {
                    _cell.CellFormat.Borders.Left.BorderType = format.Border.Left switch
                    {
                        MyBorderStyle.Single => BorderStyle.Single,
                        MyBorderStyle.Thick => BorderStyle.Thick,
                        MyBorderStyle.Double => BorderStyle.Double,
                        MyBorderStyle.Cleared => BorderStyle.Cleared,
                        _ => throw new ArgumentOutOfRangeException(nameof(format.Border.Left), format.Border.Left, null)
                    };
                }

                if (format.Border.Right is not null)
                {
                    _cell.CellFormat.Borders.Right.BorderType = format.Border.Right switch
                    {
                        MyBorderStyle.Single => BorderStyle.Single,
                        MyBorderStyle.Thick => BorderStyle.Thick,
                        MyBorderStyle.Double => BorderStyle.Double,
                        MyBorderStyle.Cleared => BorderStyle.Cleared,
                        _ => throw new ArgumentOutOfRangeException(nameof(format.Border.Right), format.Border.Right, null)
                    };
                }
            }

            return this;
        }
    }
}