namespace POS.Infrastructure.Services.Base;

public interface IDocumentService
{
    public int DocumentIndex { get; set; }
    public int TableIndex { get; set; }

    void Load(string path);
    void ReplaceText(int rowIndex, string searchValue, string newValue);
    int GetRowCount();
    int GetColumnCount();
    int GetParagraphsCount(int rowIndex);
    string GetParagraphText(int rowIndex, int paragraphIndex);
    void RemoveText(int rowIndex, int paragraphIndex);
    void MergeCellsInColumn(int columnIndex, int startRowIndex, int endRowIndex);
    void Dispose();
    void InsertDocument(int toDocumentIndex,int fromDocumentIndex);
    void SaveAs(Stream stream, int documentIndex);
    void InsertTemplateRow(int templateRowIndex, int insertionIndex);
    void RemoveRow(int rowIndex);
    void Append(string text, int rowIndex, int cellIndex, int paragraphIndex, int fontSize = 12);
}