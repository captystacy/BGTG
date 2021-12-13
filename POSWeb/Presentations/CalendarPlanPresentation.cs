using AutoMapper;
using Microsoft.AspNetCore.Http;
using POSCore.CalendarPlanLogic;
using POSWeb.Models;
using POSWeb.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

        public void WriteCalendarPlan(IEnumerable<IFormFile> estimateFiles, List<UserWork> userWorks, string userFullName)
        {
            _calendarPlanService.WriteCalendarPlan(estimateFiles, userWorks, userFullName);
        }

        public CalendarPlanVM GetCalendarPlanVM(IEnumerable<IFormFile> estimateFiles)
        {
            var estimate = _calendarPlanService.GetEstimate(estimateFiles);

            var calendarPlanVM = _mapper.Map<CalendarPlanVM>(estimate);
            calendarPlanVM.UserWorks.Add(new UserWork()
            {
                WorkName = CalendarWorkCreator.MainOtherExpensesWorkName,
                Chapter = 9
            });

            return calendarPlanVM;
        }

        public IEnumerable<decimal> GetTotalPercentages(IEnumerable<IFormFile> estimateFiles, List<UserWork> userWorks)
        {
            return _calendarPlanService.GetTotalPercentages(estimateFiles, userWorks);
        }

        public string GetCalendarPlanFileName(string userFullName)
        {
            return _calendarPlanService.GetCalendarPlanFileName(userFullName);
        }

        public string GetDownloadCalendarPlanFileName()
        {
            return _calendarPlanService.GetDownloadCalendarPlanFileName();
        }

        public string GetCalendarPlansPath()
        {
            return _calendarPlanService.GetCalendarPlansPath();
        }
    }
}
