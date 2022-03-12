namespace POS.Infrastructure.Tools.TableOfContentsTool;

public interface ITableOfContentsWriter
{
    MemoryStream Write(string objectCipher, string templatePath);
}