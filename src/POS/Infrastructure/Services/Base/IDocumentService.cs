namespace POS.Infrastructure.Services.Base;

public interface IWordDocumentService
{
    int BaseDocumentIndex { get; set; }
    bool ReplaceInBaseDocumentMode { get; set; }
    int CurrentDocumentIndex { get; set; }
    int TableIndex { get; set; }
    int RowIndex { get; set; }
    int CellIndex { get; set; }
    int ParagraphIndex { get; set; }
    int RowCount { get; }
    int ColumnCountInTable { get; }
    int ParagraphsCountInRow { get; }
    int ParagraphsCountInTable { get; }
    int ParagraphsCountInDocument { get; }
    string ParagraphTextInRow { get; }
    string ParagraphTextInTable { get; }
    string ParagraphTextInDocument { get; }
    void Load(string path);
    void Load(Stream stream);
    void ResetIndexesBesidesDocumentIndex();
    void ResetAllIndexes();
    void ReplaceTextInRow(string searchValue, string newValue);
    void ReplaceTextInDocument(string searchValue, string newValue);
    void ReplaceTextWithTable(string searchValue);
    void InsertTemplateRow(int templateRowIndex, int insertionIndex);
    void RemoveRow();
    void EmptyParagraphInRow();
    void MergeCellsInColumn(int columnIndex, int startRowIndex, int endRowIndex);
    void AppendInRow(string text, int fontSize = 12);
    void InsertDocument(int toDocumentIndex, int fromDocumentIndex);
    void SaveAs(Stream stream, int documentIndex = 0);
    void DisposeLastDocument();
    void DisposeAllDocuments();
}