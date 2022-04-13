using POS.Infrastructure.Services.DocumentServices.WordService.Format;

namespace POS.Infrastructure.Services.DocumentServices.WordService.Base
{
    public interface IMyParagraph
    {
        string Text { get; set; }
        IMyTextRange AppendText(string text);
        IMyParagraph ApplyFormat(MyParagraphFormat format);
        IMyTextRange AppendText(string text, MyCharacterFormat format);
        void AppendBreak(MyBreakType myBreakType);
    }
}