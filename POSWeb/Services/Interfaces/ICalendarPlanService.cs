using Microsoft.AspNetCore.Http;
using POSCore.EstimateLogic;
using POSWeb.Models;
using System.Collections.Generic;

namespace POSWeb.Services.Interfaces
{
    public interface ICalendarPlanService
    {
        void WriteCalendarPlan(IEnumerable<IFormFile> estimateFiles, List<UserWork> userWorks, string userFullName);
        IEnumerable<decimal> GetTotalPercentages(IEnumerable<IFormFile> estimateFiles, List<UserWork> userWorks);
        Estimate GetEstimate(IEnumerable<IFormFile> estimateFiles);
        string GetCalendarPlansPath();
        string GetCalendarPlanFileName(string userFullName);
        string GetDownloadCalendarPlanFileName();
    }
}