using BGTGWeb.Models;

namespace BGTGWeb.Services.Interfaces
{
    public interface IDurationByTCPService : ISavable
    {
        bool Write(DurationByTCPVM durationByTCPVM, string userFullName);
    }
}