using Xceed.Words.NET;

namespace POS.Infrastructure.Tools.TableOfContentsTool;

public class TableOfContentsWriter : ITableOfContentsWriter
{
    private const string CipherPattern = "%CIPHER%";
    private const string DatePattern = "%DATE%";

    public void Write(string objectCipher, string templatePath)
    {
        using var document = DocX.Load(templatePath);

        document.ReplaceText(CipherPattern, objectCipher);
        document.ReplaceText(DatePattern, DateTime.Now.ToString(AppData.DateTimeMonthAndYearShortFormat));
    }
}