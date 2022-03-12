using AutoMapper;
using POS.Infrastructure.Tools.CalendarPlanTool.Models;
using POS.Infrastructure.Tools.EstimateTool.Models;
using POS.ViewModels;

namespace POS.Infrastructure.Mappers;

public class CalendarPlanMapperConfiguration : Profile
{
    public CalendarPlanMapperConfiguration()
    {
        CreateMap<Estimate, CalendarPlanViewModel>()
            .ForMember(d => d.CalendarWorks, o => o.MapFrom(s => s.MainEstimateWorks))
            .ForMember(d => d.TotalWorkChapter, o => o.Ignore())
            .ForMember(d => d.EstimateFiles, o => o.Ignore());
        CreateMap<EstimateWork, CalendarWorkViewModel>();
        CreateMap<CalendarWork, CalendarWorkViewModel>()
            .ForMember(d => d.Percentages, o => o.MapFrom(s => s.ConstructionMonths.Select(x => x.PercentPart)))
            .ForMember(d => d.Chapter, o => o.MapFrom(x => x.EstimateChapter));
    }
}