using POS.Infrastructure.Services.Base;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.Writers;

public class TitlePageWriter : ITitlePageWriter
{
    private readonly IWordDocumentService _wordDocumentService;
    private const string NamePattern = "%NAME%";
    private const string CipherPattern = "%CIPHER%";
    private const string YearPattern = "%YEAR%";

    public TitlePageWriter(IWordDocumentService wordDocumentService)
    {
        _wordDocumentService = wordDocumentService;
    }

    public MemoryStream Write(string objectCipher, string objectName, string templatePath)
    {
        _wordDocumentService.Load(templatePath);

        _wordDocumentService.ReplaceTextInDocument(NamePattern, objectName);
        _wordDocumentService.ReplaceTextInDocument(CipherPattern, objectCipher);
        _wordDocumentService.ReplaceTextInDocument(YearPattern, DateTime.Now.Year.ToString());

        var memoryStream = new MemoryStream();
        _wordDocumentService.SaveAs(memoryStream);
        _wordDocumentService.DisposeLastDocument();

        return memoryStream;
    }
}