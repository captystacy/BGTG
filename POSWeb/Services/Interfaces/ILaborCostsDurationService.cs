using Microsoft.AspNetCore.Http;
using POSWeb.Models;
using System.Collections.Generic;

namespace POSWeb.Services.Interfaces
{
    public interface ILaborCostsDurationService
    {
        void WriteLaborCostsDuration(IEnumerable<IFormFile> estimateFiles, LaborCostsDurationVM laborCostsDurationVM, string userFullName);
        string GetLaborCostsDurationsPath();
        string GetLaborCostsDurationFileName(string userFullName);
        string GetDownloadLaborCostsDurationFileName();
    }
}
