using System.Linq;
using AutoMapper;
using BGTGWeb.ViewModels;
using POS.CalendarPlanLogic;
using POS.EstimateLogic;

namespace BGTGWeb.MapProfiles
{
    public class PosProfile : Profile
    {
        public PosProfile()
        {
            CreateMap<Estimate, CalendarPlanViewModel>()
                .ForMember(d => d.UserWorks, o => o.MapFrom(s => s.MainEstimateWorks));
            CreateMap<EstimateWork, UserWorkViewModel>();
            CreateMap<CalendarWork, UserWorkViewModel>()
                .ForMember(d => d.Percentages, o => o.MapFrom(s => s.ConstructionMonths.Select(x => x.PercentPart)));
        }
    }
}
