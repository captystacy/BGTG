using POS.Infrastructure.Services.DocumentServices.WordService.Format;

namespace POS.Infrastructure.Services.DocumentServices.WordService.Base
{
    public interface IMySection
    {
        MyCharacterFormat? CharacterFormat { get; set; }
        MyParagraphFormat? ParagraphFormat { get; set; }

        IReadOnlyList<IMyTable> Tables { get; }
        IReadOnlyList<IMyParagraph> Paragraphs { get; }
        IMyTable AddTable();
        IMyTable AddTable(MyTableStyle? myTableStyle);
        IMyTable AddTable(int? columnsNumber, int? rowsNumber, MyTableStyle? myTableStyle);
        IMyTable AddTable(int? columnsNumber, int? rowsNumber, MyTableFormat? tableFormat, MyCellFormat? cellFormat, MyParagraphFormat? paragraphFormat, MyCharacterFormat? characterFormat, MyTableStyle? myTableStyle);
        IMyParagraph AddParagraph();
        IMyParagraph AddParagraph(string? text);
        IMyParagraph AddParagraph(string? text, MyParagraphFormat? paragraphFormat);
        IMyParagraph AddParagraph(string? text, MyCharacterFormat? characterFormat);
        IMyParagraph AddParagraph(string? text, MyParagraphFormat? paragraphFormat, MyCharacterFormat? characterFormat);
    }
}