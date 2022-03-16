namespace POS.Infrastructure.Writers.Base;

public interface ITableOfContentsWriter
{
    MemoryStream Write(string objectCipher, string templatePath);
}