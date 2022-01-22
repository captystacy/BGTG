namespace BGTG.POS.Web.Infrastructure.Services.Interfaces
{
    public interface ISavable
    {
        string GetSavePath(string userFullName);
        string GetFileName(string objectCipher = null);
    }
}
