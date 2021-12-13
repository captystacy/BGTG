using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSWeb.Models;
using POSWeb.Services;
using POSWeb.Services.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace POSWeb.Controllers
{
    public class LaborCostsDurationController : Controller
    {
        private readonly ILaborCostsDurationService _laborCostsDurationService;

        public LaborCostsDurationController(ILaborCostsDurationService laborCostsDurationService)
        {
            _laborCostsDurationService = laborCostsDurationService;
        }

        public void WriteLaborCostsDuration(IEnumerable<IFormFile> estimateFiles, LaborCostsDurationVM laborCostsDurationVM)
        {
            _laborCostsDurationService.WriteLaborCostsDuration(estimateFiles, laborCostsDurationVM, User.Identity.Name);
        }

        public IActionResult Download()
        {
            var laborCostsDurationsPath = _laborCostsDurationService.GetLaborCostsDurationsPath();
            var laborCostsDurationFileName = _laborCostsDurationService.GetLaborCostsDurationFileName(User.Identity.Name);
            var filePath = Path.Combine(laborCostsDurationsPath, laborCostsDurationFileName);
            var downloadLaborCostsDurationName = _laborCostsDurationService.GetDownloadLaborCostsDurationFileName();
            return PhysicalFile(filePath, CalendarPlanService.DocxMimeType, downloadLaborCostsDurationName);
        }
    }
}
