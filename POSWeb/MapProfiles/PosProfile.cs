using System.Linq;
using AutoMapper;
using BGTGWeb.Models;
using POS.CalendarPlanLogic;
using POS.EstimateLogic;

namespace BGTGWeb.MapProfiles
{
    public class PosProfile : Profile
    {
        public PosProfile()
        {
            CreateMap<Estimate, CalendarPlanVM>()
                .ForMember(d => d.UserWorks, o => o.MapFrom(s => s.MainEstimateWorks));
            CreateMap<EstimateWork, UserWorkVM>();
            CreateMap<CalendarWork, UserWorkVM>()
                .ForMember(d => d.Percentages, o => o.MapFrom(s => s.ConstructionMonths.Select(x => x.PercentPart)));
        }
    }
}
