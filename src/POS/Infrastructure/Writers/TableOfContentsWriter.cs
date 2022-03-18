using POS.DomainModels;
using POS.Infrastructure.Constants;
using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.Writers;

public class TableOfContentsWriter : ITableOfContentsWriter
{
    private readonly IWordDocumentService _wordDocumentService;

    private const string CipherPattern = "%CIPHER%";
    private const string DatePattern = "%DATE%";

    public TableOfContentsWriter(IWordDocumentService wordDocumentService)
    {
        _wordDocumentService = wordDocumentService;
    }

    public MemoryStream Write(string objectCipher, string templatePath)
    {
        _wordDocumentService.Load(templatePath);

        _wordDocumentService.ReplaceTextInDocument(CipherPattern, objectCipher);
        _wordDocumentService.ReplaceTextInDocument(DatePattern, DateTime.Now.ToString(AppConstants.DateTimeMonthAndYearShortFormat));

        var memoryStream = new MemoryStream();
        _wordDocumentService.SaveAs(memoryStream, MyFileFormat.Doc);
        _wordDocumentService.DisposeLastDocument();

        return memoryStream;
    }
}