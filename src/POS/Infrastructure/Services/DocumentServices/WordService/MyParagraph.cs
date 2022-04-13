using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using Spire.Doc.Documents;
using Spire.Doc.Interface;

namespace POS.Infrastructure.Services.DocumentServices.WordService
{
    public class MyParagraph : IMyParagraph
    {
        private readonly IParagraph _paragraph;

        public string Text
        {
            get => _paragraph.Text;
            set => _paragraph.Text = value;
        }

        public MyParagraph(IParagraph paragraph)
        {
            _paragraph = paragraph;
        }

        public void AppendBreak(MyBreakType myBreakType)
        {
            var breakType = myBreakType switch
            {
                MyBreakType.PageBreak => BreakType.PageBreak,
                MyBreakType.ColumnBreak => BreakType.ColumnBreak,
                MyBreakType.LineBreak => BreakType.LineBreak,
                _ => throw new ArgumentOutOfRangeException(nameof(myBreakType), myBreakType, null)
            };

            _paragraph.AppendBreak(breakType);
        }

        public IMyTextRange AppendText(string text)
        {
            return AppendText(text, null!);
        }

        public IMyTextRange AppendText(string text, MyCharacterFormat? format)
        {
            var textRange = _paragraph.AppendText(text);

            var myTextRange = new MyTextRange(textRange);

            if (format is not null)
            {
                myTextRange.ApplyFormat(format);
            }

            return myTextRange;
        }

        public IMyParagraph ApplyFormat(MyParagraphFormat format)
        {
            if (format.HorizontalAlignment is not null)
            {
                _paragraph.Format.HorizontalAlignment = format.HorizontalAlignment switch
                {
                    MyParagraphAlignment.Left => HorizontalAlignment.Left,
                    MyParagraphAlignment.Center => HorizontalAlignment.Center,
                    MyParagraphAlignment.Right => HorizontalAlignment.Right,
                    MyParagraphAlignment.Justify => HorizontalAlignment.Justify,
                    _ => throw new ArgumentOutOfRangeException(nameof(format.HorizontalAlignment), format.HorizontalAlignment, null)
                };
            }

            if (format.FirstLineIndent is not null)
            {
                _paragraph.Format.FirstLineIndent = format.FirstLineIndent.Value;
            }

            return this;
        }
    }
}