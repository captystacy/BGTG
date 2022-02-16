using System.Linq;
using BGTG.Entities.POS.CalendarPlanToolEntities;
using BGTG.POS.CalendarPlanTool;
using BGTG.POS.EstimateTool;
using BGTG.Web.Infrastructure.Mappers.Base;
using BGTG.Web.ViewModels.POS.CalendarPlanViewModels;
using Calabonga.UnitOfWork;

namespace BGTG.Web.Infrastructure.Mappers.POS
{
    public class CalendarPlanMapperConfiguration : MapperConfigurationBase
    {
        public CalendarPlanMapperConfiguration()
        {
            CreateMap<Estimate, CalendarPlanCreateViewModel>()
                .ForMember(d => d.CalendarWorkViewModels, o => o.MapFrom(s => s.MainEstimateWorks))
                .ForMember(d => d.TotalWorkChapter, o => o.Ignore())
                .ForMember(d => d.EstimateFiles, o => o.Ignore())
                .ForMember(d => d.ObjectCipher, o => o.Ignore());
            CreateMap<EstimateWork, CalendarWorkViewModel>();
            CreateMap<CalendarWork, CalendarWorkViewModel>()
                .ForMember(d => d.Percentages, o => o.MapFrom(s => s.ConstructionMonths.Select(x => x.PercentPart)))
                .ForMember(d => d.Chapter, o => o.MapFrom(x => x.EstimateChapter));

            CreateMap<CalendarPlanEntity, CalendarPlanViewModel>()
                .ForMember(x => x.ConstructionStartDate, o => o.MapFrom(x => x.ConstructionStartDate))
                .ForMember(x => x.CreatedAt, o => o.MapFrom(x => x.CreatedAt.ToLocalTime()));

            CreateMap<IPagedList<CalendarPlanEntity>, IPagedList<CalendarPlanViewModel>>()
                .ConvertUsing<PagedListConverter<CalendarPlanEntity, CalendarPlanViewModel>>();

            CreateMap<CalendarPlan, CalendarPlanEntity>()
                .ForMember(x => x.POSId, o => o.Ignore())
                .ForMember(x => x.POS, o => o.Ignore())
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.CreatedAt, o => o.Ignore())
                .ForMember(x => x.CreatedBy, o => o.Ignore())
                .ForMember(x => x.UpdatedAt, o => o.Ignore())
                .ForMember(x => x.UpdatedBy, o => o.Ignore());
            CreateMap<CalendarPlanEntity, CalendarPlan>();

            CreateMap<CalendarWork, PreparatoryCalendarWorkEntity>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.CalendarPlanId, o => o.Ignore())
                .ForMember(x => x.CalendarPlan, o => o.Ignore());
            CreateMap<PreparatoryCalendarWorkEntity, CalendarWork>();

            CreateMap<CalendarWork, MainCalendarWorkEntity>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.CalendarPlanId, o => o.Ignore())
                .ForMember(x => x.CalendarPlan, o => o.Ignore());
            CreateMap<MainCalendarWorkEntity, CalendarWork>();

            CreateMap<ConstructionMonth, PreparatoryConstructionMonthEntity>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.PreparatoryCalendarWorkId, o => o.Ignore())
                .ForMember(x => x.PreparatoryCalendarWork, o => o.Ignore());
            CreateMap<PreparatoryConstructionMonthEntity, ConstructionMonth>();

            CreateMap<ConstructionMonth, MainConstructionMonthEntity>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.MainCalendarWorkId, o => o.Ignore())
                .ForMember(x => x.MainCalendarWork, o => o.Ignore());
            CreateMap<MainConstructionMonthEntity, ConstructionMonth>();
        }
    }
}
