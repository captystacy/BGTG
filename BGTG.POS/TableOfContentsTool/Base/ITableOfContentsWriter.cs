namespace BGTG.POS.TableOfContentsTool.Base
{
    public interface ITableOfContentsWriter
    {
        void Write(string objectCipher, string templatePath, string savePath);
    }
}
