namespace POS.Infrastructure.Tools.TableOfContentsTool;

public interface ITableOfContentsWriter
{
    void Write(string objectCipher, string templatePath);
}