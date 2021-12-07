using AutoMapper;
using Microsoft.AspNetCore.Http;
using POSCore.CalendarPlanLogic;
using POSWeb.Models;
using POSWeb.Services.Interfaces;
using System.Collections.Generic;

namespace POSWeb.Presentations
{
    public class CalendarPlanPresentation
    {
        private readonly ICalendarPlanService _calendarPlanService;
        private readonly IMapper _mapper;

        public CalendarPlanPresentation(ICalendarPlanService calendarPlanService, IMapper mapper)
        {
            _calendarPlanService = calendarPlanService;
            _mapper = mapper;
        }

        public CalendarPlanVM GetCalendarPlanVM(IEnumerable<IFormFile> estimateFiles)
        {
            var estimate = _calendarPlanService.GetEstimate(estimateFiles);

            var calendarPlanVM = _mapper.Map<CalendarPlanVM>(estimate);
            calendarPlanVM.UserWorks.RemoveAll(x =>
                x.Chapter == 1
                || x.WorkName.StartsWith(CalendarPlanSeparator.TemporaryBuildingsSearchPattern)
                || x.Chapter == 10);
            calendarPlanVM.UserWorks.Add(new UserWork()
            {
                WorkName = CalendarPlanSeparator.MainOtherExpensesWork,
                Chapter = 9
            });

            return calendarPlanVM;
        }

        public void WriteCalendarPlan(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM, string userFullName)
        {
            var fileName = GetCalendarPlanFileName(userFullName);
            _calendarPlanService.WriteCalendarPlan(estimateFiles, calendarPlanVM, fileName);
        }

        public UserWork GetMainTotalWork(IEnumerable<IFormFile> estimateFiles, CalendarPlanVM calendarPlanVM)
        {
            var mainTotalWork = _calendarPlanService.GetMainTotalWork(estimateFiles, calendarPlanVM);
            return _mapper.Map<UserWork>(mainTotalWork);
        }

        public string GetCalendarPlanFileName(string userFullName)
        {
            var userName = userFullName.Replace(@"\", string.Empty);
            return $"CalendarPlan{userName}.docx";
        }

        public string GetDownloadCalendarPlanName(string objectCipher)
        {
            return _calendarPlanService.GetDownloadCalendarPlanName(objectCipher);
        }

        public string GetCalendarPlansPath()
        {
            return _calendarPlanService.GetCalendarPlansPath();
        }
    }
}
