using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BGTGWeb.Services.Interfaces
{
    public interface IEnergyAndWaterService : ISavable
    {
        void Write(IEnumerable<IFormFile> estimateFiles, string userFullName);
    }
}
