using BGTGWeb.ViewModels;

namespace BGTGWeb.Services.Interfaces
{
    public interface IDurationByTCPService : ISavable
    {
        bool Write(DurationByTCPViewModel durationByTCPViewModel, string userFullName);
    }
}