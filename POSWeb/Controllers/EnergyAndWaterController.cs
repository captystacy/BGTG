using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSWeb.Services;
using POSWeb.Services.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace POSWeb.Controllers
{
    public class EnergyAndWaterController : Controller
    {
        private readonly IEnergyAndWaterService _energyAndWaterService;

        public EnergyAndWaterController(IEnergyAndWaterService energyAndWaterService)
        {
            _energyAndWaterService = energyAndWaterService;
        }

        public void WriteEnergyAndWater(IEnumerable<IFormFile> estimateFiles)
        {
            _energyAndWaterService.WriteEnergyAndWater(estimateFiles, User.Identity.Name);
        }

        public IActionResult Download()
        {
            var energyAndWatersPath = _energyAndWaterService.GetEnergyAndWatersPath();
            var energyAndWaterFileName = _energyAndWaterService.GetEnergyAndWaterFileName(User.Identity.Name);
            var filePath = Path.Combine(energyAndWatersPath, energyAndWaterFileName);
            var downloadEnergyAndWaterName = _energyAndWaterService.GetDownloadEnergyAndWaterFileName();
            return PhysicalFile(filePath, CalendarPlanService.DocxMimeType, downloadEnergyAndWaterName);
        }
    }
}
