using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POSWeb.Models;
using POSWeb.Presentations;
using POSWeb.Services;
using System.Collections.Generic;
using System.IO;

namespace POSWeb.Controllers
{
    public class EnergyAndWaterController : Controller
    {
        private readonly EnergyAndWaterPresentation _energyAndWaterPresentation;

        public EnergyAndWaterController(EnergyAndWaterPresentation energyAndWaterPresentation)
        {
            _energyAndWaterPresentation = energyAndWaterPresentation;
        }

        public IActionResult WriteAndGetObjectCipher(IEnumerable<IFormFile> estimateFiles, EnergyAndWaterVM energyAndWaterVM)
        {
            _energyAndWaterPresentation.WriteEnergyAndWater(estimateFiles, energyAndWaterVM.ConstructionStartDate, User.Identity.Name);
            return Json(energyAndWaterVM.ObjectCipher);
        }

        public IActionResult Download(string objectCipher)
        {
            var energyAndWatersPath = _energyAndWaterPresentation.GetEnergyAndWatersPath();
            var energyAndWaterFileName = _energyAndWaterPresentation.GetEnergyAndWaterFileName(User.Identity.Name);
            var filePath = Path.Combine(energyAndWatersPath, energyAndWaterFileName);
            var downloadEnergyAndWaterName = _energyAndWaterPresentation.GetDownloadEnergyAndWaterName(objectCipher);
            return PhysicalFile(filePath, CalendarPlanService.DocxMimeType, downloadEnergyAndWaterName);
        }

        public IActionResult GetEnergyAndWaterVM(IEnumerable<IFormFile> estimateFiles)
        {
            var energyAndWaterVM = _energyAndWaterPresentation.GetEnergyAndWaterVM(estimateFiles);
            return Json(energyAndWaterVM);
        }
    }
}
