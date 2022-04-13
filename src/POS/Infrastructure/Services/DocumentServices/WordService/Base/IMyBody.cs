using POS.Infrastructure.Services.DocumentServices.WordService.Format;

namespace POS.Infrastructure.Services.DocumentServices.WordService.Base
{
    public interface IMyBody
    {
        IReadOnlyList<IMyParagraph> Paragraphs { get; }
        IMyParagraph AddParagraph();
        IMyParagraph AddParagraph(string? text);
        IMyParagraph AddParagraph(string? text, MyParagraphFormat? paragraphFormat);
        IMyParagraph AddParagraph(string? text, MyCharacterFormat? characterFormat);
        IMyParagraph AddParagraph(string? text, MyParagraphFormat? paragraphFormat, MyCharacterFormat? characterFormat);
    }
}