using System;
using Calabonga.OperationResults;
using Moq;
using POS.Infrastructure.Services;
using POS.Infrastructure.Services.Base;
using POS.Models.EstimateModels;
using POS.ViewModels;

namespace POS.Tests.Helpers.Services
{
    public static class EstimateServiceHelper
    {
        public static Mock<IEstimateService> GetMock()
        {
            return new Mock<IEstimateService>();
        }

        public static Mock<IEstimateService> GetMock(EnergyAndWaterViewModel viewModel, EstimateWork totalEstimateWork, DateTime constructionStartDate)
        {
            var estimateService = GetMock();

            estimateService.Setup(x => x.GetTotalEstimateWork(viewModel.EstimateFiles, viewModel.TotalWorkChapter))
                .ReturnsAsync(new OperationResult<EstimateWork> { Result = totalEstimateWork });

            estimateService
                .Setup(x => x.GetConstructionStartDate(viewModel.EstimateFiles[0]))
                .ReturnsAsync(new OperationResult<DateTime> { Result = constructionStartDate });

            return estimateService;
        }
    }
}
