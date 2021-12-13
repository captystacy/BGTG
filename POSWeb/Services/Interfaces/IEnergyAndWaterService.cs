using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace POSWeb.Services.Interfaces
{
    public interface IEnergyAndWaterService
    {
        void WriteEnergyAndWater(IEnumerable<IFormFile> estimateFiles, string userFullName);
        string GetEnergyAndWatersPath();
        string GetEnergyAndWaterFileName(string userFullName);
        string GetDownloadEnergyAndWaterFileName();
    }
}
