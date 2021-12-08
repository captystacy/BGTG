using Microsoft.AspNetCore.Http;
using POSCore.EstimateLogic;
using System;
using System.Collections.Generic;

namespace POSWeb.Services.Interfaces
{
    public interface IEnergyAndWaterService
    {
        void WriteEnergyAndWater(IEnumerable<IFormFile> estimateFiles, DateTime constructionStartDate, string fileName);
        string GetEnergyAndWatersPath();
        string GetDownloadEnergyAndWaterName(string objectCipher);
        Estimate GetEstimate(IEnumerable<IFormFile> estimateFiles);
    }
}
