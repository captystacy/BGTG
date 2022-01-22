using System.Linq;
using BGTG.POS.Tools.CalendarPlanTool;
using BGTG.POS.Tools.EstimateTool;
using BGTG.POS.Web.Infrastructure.Mappers.Base;
using BGTG.POS.Web.ViewModels.CalendarPlanViewModels;

namespace BGTG.POS.Web.Infrastructure.Mappers
{
    public class CalendarPlanMapperConfiguration : MapperConfigurationBase
    {
        public CalendarPlanMapperConfiguration()
        {
            CreateMap<Estimate, CalendarPlanViewModel>()
                .ForMember(d => d.UserWorks, o => o.MapFrom(s => s.MainEstimateWorks))
                .ForMember(d => d.TotalWorkChapter, o => o.Ignore());
            CreateMap<EstimateWork, UserWorkViewModel>();
            CreateMap<CalendarWork, UserWorkViewModel>()
                .ForMember(d => d.Percentages, o => o.MapFrom(s => s.ConstructionMonths.Select(x => x.PercentPart)))
                .ForMember(d => d.Chapter, o => o.MapFrom(x => x.EstimateChapter));
        }
    }
}
