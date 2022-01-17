using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BGTGWeb.Services.Interfaces
{
    public interface IEnergyAndWaterService
    {
        void WriteEnergyAndWater(IEnumerable<IFormFile> estimateFiles, string userFullName);
        string GetEnergyAndWatersPath();
        string GetEnergyAndWaterFileName(string userFullName);
        string GetDownloadEnergyAndWaterFileName();
    }
}
