using BGTG.POS.Web.ViewModels.DurationByLCViewModels;
using Microsoft.AspNetCore.Http;

namespace BGTG.POS.Web.Infrastructure.Services.Interfaces
{
    public interface IDurationByLCService : ISavable
    {
        void Write(IFormFileCollection estimateFiles, DurationByLCViewModel durationByLCViewModel, string userFullName);
    }
}
