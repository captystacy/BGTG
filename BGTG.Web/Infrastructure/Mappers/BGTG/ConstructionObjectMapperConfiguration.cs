using BGTG.Entities.BGTG;
using BGTG.Web.Infrastructure.Mappers.Base;
using BGTG.Web.ViewModels.BGTG;
using Calabonga.UnitOfWork;

namespace BGTG.Web.Infrastructure.Mappers.BGTG
{
    public class ConstructionObjectMapperConfiguration : MapperConfigurationBase
    {
        public ConstructionObjectMapperConfiguration()
        {
            CreateMap<ConstructionObjectEntity, ConstructionObjectViewModel>()
                .ForMember(x => x.POSViewModel, o => o.MapFrom(x => x.POS))
                .ForMember(x => x.UpdatedAt, o => o.MapFrom(x => x.UpdatedAt!.Value.ToLocalTime()));

            CreateMap<IPagedList<ConstructionObjectEntity>, IPagedList<ConstructionObjectViewModel>>()
                .ConvertUsing<PagedListConverter<ConstructionObjectEntity, ConstructionObjectViewModel>>();
        }
    }
}