using AutoMapper;
using Microsoft.AspNetCore.Http;
using POSWeb.Helpers;
using POSWeb.Models;
using POSWeb.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace POSWeb.Presentations
{
    public class EnergyAndWaterPresentation
    {
        private readonly IEnergyAndWaterService _energyAndWaterService;
        private readonly IMapper _mapper;

        public EnergyAndWaterPresentation(IEnergyAndWaterService energyAndWaterService, IMapper mapper)
        {
            _energyAndWaterService = energyAndWaterService;
            _mapper = mapper;
        }

        public void WriteEnergyAndWater(IEnumerable<IFormFile> estimateFiles, DateTime constructionStartDate, string userFullName)
        {
            var fileName = GetEnergyAndWaterFileName(userFullName);
            _energyAndWaterService.WriteEnergyAndWater(estimateFiles, constructionStartDate, fileName);
        }

        public EnergyAndWaterVM GetEnergyAndWaterVM(IEnumerable<IFormFile> estimateFiles)
        {
            var estimate = _energyAndWaterService.GetEstimate(estimateFiles);
            return _mapper.Map<EnergyAndWaterVM>(estimate);
        }

        public string GetEnergyAndWaterFileName(string userFullName)
        {
            return $"EnergyAndWater{userFullName.RemoveBackslashes()}.docx";
        }

        public string GetDownloadEnergyAndWaterName(string objectCipher)
        {
            return _energyAndWaterService.GetDownloadEnergyAndWaterName(objectCipher);
        }

        public string GetEnergyAndWatersPath()
        {
            return _energyAndWaterService.GetEnergyAndWatersPath();
        }
    }
}
