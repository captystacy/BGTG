namespace BGTG.POS.TableOfContentsTool.Interfaces
{
    public interface ITableOfContentsWriter
    {
        void Write(string objectCipher, string templatePath, string savePath);
    }
}
