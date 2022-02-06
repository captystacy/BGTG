using BGTG.Entities;
using BGTG.Web.Infrastructure.Mappers.Base;
using BGTG.Web.ViewModels.ConstructionObjectViewModels;
using Calabonga.UnitOfWork;

namespace BGTG.Web.Infrastructure.Mappers
{
    public class ConstructionObjectMapperConfiguration : MapperConfigurationBase
    {
        public ConstructionObjectMapperConfiguration()
        {
            CreateMap<ConstructionObjectEntity, ConstructionObjectViewModel>()
                .ForMember(x => x.POSViewModel, o => o.MapFrom(x => x.POS))
                .ForMember(x => x.UpdatedAt, o => o.MapFrom(x => x.UpdatedAt.HasValue ? x.UpdatedAt.Value.ToLocalTime() : x.UpdatedAt));

            CreateMap<ConstructionObjectCreateViewModel, ConstructionObjectEntity>()
                .ForAllMembers(x => x.Ignore());

            CreateMap<ConstructionObjectUpdateViewModel, ConstructionObjectEntity>()
                .ForAllMembers(x => x.Ignore());

            CreateMap<ConstructionObjectEntity, ConstructionObjectUpdateViewModel>();

            CreateMap<IPagedList<ConstructionObjectEntity>, IPagedList<ConstructionObjectViewModel>>()
                .ConvertUsing<PagedListConverter<ConstructionObjectEntity, ConstructionObjectViewModel>>();
        }
    }
}