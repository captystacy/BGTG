using POS.Infrastructure.Constants;
using POS.Infrastructure.Services;
using POS.Infrastructure.Writers.Base;

namespace POS.Infrastructure.Writers;

public class TableOfContentsWriter : ITableOfContentsWriter
{
    private const string CipherPattern = "%CIPHER%";
    private const string DatePattern = "%DATE%";

    public MemoryStream Write(string objectCipher, string templatePath)
    {
        using var document = DocumentService.Load(templatePath);

        document.ReplaceText(CipherPattern, objectCipher);
        document.ReplaceText(DatePattern,
            DateTime.Now.ToString(AppConstants.DateTimeMonthAndYearShortFormat));

        var memoryStream = new MemoryStream();
        document.SaveAs(memoryStream);

        return memoryStream;
    }
}