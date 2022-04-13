using POS.Infrastructure.Services.DocumentServices.WordService.Base;
using POS.Infrastructure.Services.DocumentServices.WordService.Format;
using Spire.Doc.Documents;
using Spire.Doc.Interface;

namespace POS.Infrastructure.Services.DocumentServices.WordService
{
    public class MyTextRange : IMyTextRange
    {
        private readonly ITextRange _textRange;
        public string Text => _textRange.Text;

        public MyTextRange(ITextRange textRange)
        {
            _textRange = textRange;
        }

        public IMyTextRange ApplyFormat(MyCharacterFormat format)
        {
            if (format.UnderlineStyle is not null)
            {
                _textRange.CharacterFormat.UnderlineStyle = format.UnderlineStyle switch
                {
                    MyUnderlineStyle.Empty => UnderlineStyle.None,
                    MyUnderlineStyle.Single => UnderlineStyle.Single,
                    MyUnderlineStyle.Words => UnderlineStyle.Words,
                    MyUnderlineStyle.Double => UnderlineStyle.Double,
                    MyUnderlineStyle.DotDot => UnderlineStyle.DotDot,
                    MyUnderlineStyle.Dotted => UnderlineStyle.Dotted,
                    MyUnderlineStyle.Thick => UnderlineStyle.Thick,
                    MyUnderlineStyle.Dash => UnderlineStyle.Dash,
                    _ => throw new ArgumentOutOfRangeException(nameof(format.UnderlineStyle), format.UnderlineStyle, null)
                };
            }

            if (format.FontSize is not null)
            {
                _textRange.CharacterFormat.FontSize = format.FontSize.Value;
            }

            return this;
        }
    }
}