using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BGTG.POS.Tools.EstimateTool;
using BGTG.POS.Web.ViewModels.CalendarPlanViewModels;
using BGTG.POS.Web.ViewModels.DurationByLCViewModels;
using BGTG.POS.Web.ViewModels.DurationByTCPViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers
{
    public class PosController : Controller
    {
        private readonly HttpClient _httpClient;

        private const string DocxMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        public PosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("POS");
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetCalendarPlanViewModel(IFormFileCollection estimateFiles, TotalWorkChapter totalWorkChapter)
        {
            if (estimateFiles.Count == 0)
            {
                return BadRequest();
            }

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StringContent(totalWorkChapter.ToString()), "totalWorkChapter");
            FillMultipartContent(multipartContent, estimateFiles);

            var response = await _httpClient.PostAsync("/api/Home/get-calendar-plan-view-model", multipartContent);

            var json = await response.Content.ReadAsStringAsync();

            var calendarPlanViewModel = JsonSerializer.Deserialize<CalendarPlanViewModel>(json);

            return Json(calendarPlanViewModel);
        }

        public async Task<IActionResult> GetTotalPercentages(IFormFileCollection estimateFiles, CalendarPlanViewModel calendarPlanViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var multipartContent = new MultipartFormDataContent();
            FillMultipartContent(multipartContent, estimateFiles);
            FillMultipartContent(multipartContent, calendarPlanViewModel);

            var response = await _httpClient.PostAsync("/api/Home/get-total-percentages", multipartContent);

            var totalPercentages = JsonSerializer.Deserialize<IEnumerable<decimal>>(await response.Content.ReadAsStringAsync());

            return Json(totalPercentages);
        }

        public async Task<IActionResult> WriteCalendarPlan(IFormFileCollection estimateFiles, CalendarPlanViewModel calendarPlanViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var multipartContent = new MultipartFormDataContent();
            FillMultipartContent(multipartContent, estimateFiles);
            FillMultipartContent(multipartContent, calendarPlanViewModel);

            multipartContent.Add(new StringContent(User.Identity.Name), "userFullName");

            await _httpClient.PostAsync("/api/Home/write-calendar-plan", multipartContent);

            return Ok();
        }

        private void FillMultipartContent(MultipartFormDataContent multipartContent, CalendarPlanViewModel calendarPlanViewModel)
        {

            multipartContent.Add(new StringContent(calendarPlanViewModel.TotalWorkChapter.ToString()), "TotalWorkChapter");
            multipartContent.Add(new StringContent(calendarPlanViewModel.ConstructionDurationCeiling.ToString()), "ConstructionDurationCeiling");
            multipartContent.Add(new StringContent(calendarPlanViewModel.ConstructionStartDate.ToString()), "ConstructionStartDate");

            for (int i = 0; i < calendarPlanViewModel.UserWorks.Count; i++)
            {
                multipartContent.Add(new StringContent(calendarPlanViewModel.UserWorks[i].WorkName), $"UserWorks[{i}].WorkName");
                multipartContent.Add(new StringContent(calendarPlanViewModel.UserWorks[i].Chapter.ToString()), $"UserWorks[{i}].Chapter");

                for (int j = 0; j < calendarPlanViewModel.UserWorks[i].Percentages.Count; j++)
                {
                    multipartContent.Add(new StringContent(calendarPlanViewModel.UserWorks[i].Percentages[j].ToString()), $"UserWorks[{i}].Percentages[{j}]");
                }
            }
        }

        private void FillMultipartContent(MultipartFormDataContent multipartContent, IFormFileCollection estimateFiles)
        {
            foreach (var file in estimateFiles)
            {
                multipartContent.Add(new StreamContent(file.OpenReadStream()), "estimateFiles", file.FileName);
            }
        }

        public async Task<IActionResult> DownloadCalendarPlan(string objectCipher)
        {
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StringContent(User.Identity.Name), "userFullName");

            var response = await _httpClient.PostAsync("/api/Home/download-calendar-plan", multipartContent);

            var stream = await response.Content.ReadAsStreamAsync();

            return File(stream, DocxMimeType, $"{objectCipher}КП.docx");
        }

        public async Task<IActionResult> DownloadDurationByLC(IFormFileCollection estimateFiles, DurationByLCViewModel durationByLaborLCViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StringContent(User.Identity.Name), "userFullName");
            FillMultipartContent(multipartContent, estimateFiles);
            FillMultipartContent(multipartContent, durationByLaborLCViewModel);

            var response = await _httpClient.PostAsync("/api/Home/download-duration-by-lc", multipartContent);

            var stream = await response.Content.ReadAsStreamAsync();

            return File(stream, DocxMimeType, "Продолжительность по трудозатратам.docx");
        }

        private void FillMultipartContent(MultipartFormDataContent multipartContent, DurationByLCViewModel durationByLaborLCViewModel)
        {
            multipartContent.Add(new StringContent(durationByLaborLCViewModel.TechnologicalLaborCosts.ToString()), "TechnologicalLaborCosts");
            multipartContent.Add(new StringContent(durationByLaborLCViewModel.AcceptanceTimeIncluded.ToString()), "AcceptanceTimeIncluded");
            multipartContent.Add(new StringContent(durationByLaborLCViewModel.NumberOfEmployees.ToString()), "NumberOfEmployees");
            multipartContent.Add(new StringContent(durationByLaborLCViewModel.NumberOfWorkingDays.ToString()), "NumberOfWorkingDays");
            multipartContent.Add(new StringContent(durationByLaborLCViewModel.Shift.ToString()), "Shift");
            multipartContent.Add(new StringContent(durationByLaborLCViewModel.WorkingDayDuration.ToString()), "WorkingDayDuration");
        }

        public async Task<IActionResult> DownloadEnergyAndWater(IFormFileCollection estimateFiles)
        {
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StringContent(User.Identity.Name), "userFullName");
            FillMultipartContent(multipartContent, estimateFiles);

            var response = await _httpClient.PostAsync("/api/Home/download-energy-and-water", multipartContent);

            var stream = await response.Content.ReadAsStreamAsync();

            return File(stream, DocxMimeType, "Энергия и вода.docx");
        }

        public async Task<IActionResult> DownloadDurationByTCP(DurationByTCPViewModel durationByTCPViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            durationByTCPViewModel.UserFullName = User.Identity.Name;

            var response = await _httpClient.PostAsJsonAsync("/api/Home/download-duration-by-tcp", durationByTCPViewModel);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var stream = await response.Content.ReadAsStreamAsync();

            return File(stream, DocxMimeType, "Продолжительность по ТКП.docx");
        }
    }
}
