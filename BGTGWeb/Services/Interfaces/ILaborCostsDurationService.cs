using System.Collections.Generic;
using BGTGWeb.Models;
using Microsoft.AspNetCore.Http;

namespace BGTGWeb.Services.Interfaces
{
    public interface ILaborCostsDurationService
    {
        void WriteLaborCostsDuration(IEnumerable<IFormFile> estimateFiles, LaborCostsDurationVM laborCostsDurationVM, string userFullName);
        string GetLaborCostsDurationsPath();
        string GetLaborCostsDurationFileName(string userFullName);
        string GetDownloadLaborCostsDurationFileName();
    }
}
