using POS.DomainModels;

namespace POS.Infrastructure.Services.Base;

public interface IWordDocumentService
{
    int BaseDocumentIndex { get; set; }
    bool ReplaceInBaseDocumentMode { get; set; }
    int DocumentIndex { get; set; }
    int SectionIndex { get; set; }
    int TableIndex { get; set; }
    int RowIndex { get; set; }
    int CellIndex { get; set; }
    int ParagraphIndex { get; set; }
    int RowCount { get; }
    int CellsCountInRow { get; }
    int ParagraphsCountInCell { get; }
    int ParagraphsCountInDocument { get; }
    string ParagraphTextInDocument { get; }
    string ParagraphTextInCell { get; set; }
    void Load(string path);
    void Load(Stream stream);
    void ReplaceTextInCell(string searchValue, string newValue);
    void ReplaceTextInDocument(string searchValue, string newValue);
    void ReplaceTextWithTable(string searchValue);
    void InsertTemplateRow(int templateRowIndex, int insertionIndex);
    void RemoveRow();
    void RemoveParagraphInCell();
    void ApplyVerticalMerge(int columnIndex, int startRowIndex, int endRowIndex);
    void DisposeLastDocument();
    void DisposeAllDocuments();
    void AddParagraph(string text, int fontSize = 12);
    void SaveAs(Stream stream, MyFileFormat myFileFormat, int documentIndex = 0);
    void ReplaceTextWithImage(string searchValue, string imagePath);
}