using BGTG.Entities.POS.EnergyAndWaterToolEntities;
using BGTG.POS.EnergyAndWaterTool;
using BGTG.Web.Infrastructure.Mappers.Base;
using BGTG.Web.ViewModels.POS.EnergyAndWaterViewModels;
using Calabonga.UnitOfWork;

namespace BGTG.Web.Infrastructure.Mappers.POS
{
    public class EnergyAndWaterMapperConfiguration : MapperConfigurationBase
    {
        public EnergyAndWaterMapperConfiguration()
        {
            CreateMap<EnergyAndWaterEntity, EnergyAndWaterViewModel>()
                .ForMember(x => x.CreatedAt, o => o.MapFrom(x => x.CreatedAt.ToLocalTime()));

            CreateMap<EnergyAndWaterEntity, EnergyAndWater>();

            CreateMap<IPagedList<EnergyAndWaterEntity>, IPagedList<EnergyAndWaterViewModel>>()
                .ConvertUsing<PagedListConverter<EnergyAndWaterEntity, EnergyAndWaterViewModel>>();

            CreateMap<EnergyAndWater, EnergyAndWaterEntity>()
                .ForMember(x => x.POSId, o => o.Ignore())
                .ForMember(x => x.POS, o => o.Ignore())
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.CreatedAt, o => o.Ignore())
                .ForMember(x => x.CreatedBy, o => o.Ignore())
                .ForMember(x => x.UpdatedAt, o => o.Ignore())
                .ForMember(x => x.UpdatedBy, o => o.Ignore());
        }
    }
}