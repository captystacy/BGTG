using BGTG.Entities.POSEntities.DurationByTCPToolEntities;
using BGTG.POS.DurationTools.DurationByTCPTool;
using BGTG.POS.DurationTools.DurationByTCPTool.TCP;
using BGTG.Web.Infrastructure.Mappers.Base;
using BGTG.Web.ViewModels.POSViewModels.DurationByTCPViewModels;

namespace BGTG.Web.Infrastructure.Mappers.POSMapperConfigurations
{
    public class DurationByTCPMapperConfiguration : MapperConfigurationBase
    {
        public DurationByTCPMapperConfiguration()
        {
            CreateMap<DurationByTCPEntity, DurationByTCPViewModel>()
                .ForMember(x => x.CreatedAt, o => o.MapFrom(x => x.CreatedAt.ToLocalTime()));

            CreateMap<InterpolationDurationByTCPEntity, InterpolationDurationByTCP>();

            CreateMap<InterpolationPipelineStandardEntity, PipelineStandard>();

            CreateMap<ExtrapolationDurationByTCPEntity, ExtrapolationDurationByTCP>();

            CreateMap<ExtrapolationPipelineStandardEntity, PipelineStandard>();

            CreateMap<StepwiseExtrapolationDurationByTCPEntity, StepwiseExtrapolationDurationByTCP>();

            CreateMap<StepwiseExtrapolationPipelineStandardEntity, PipelineStandard>();
            CreateMap<StepwisePipelineStandardEntity, PipelineStandard>();

            CreateMap<InterpolationDurationByTCP, InterpolationDurationByTCPEntity>()
                .ForMember(x => x.POSId, o => o.Ignore())
                .ForMember(x => x.POS, o => o.Ignore())
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.CreatedAt, o => o.Ignore())
                .ForMember(x => x.CreatedBy, o => o.Ignore())
                .ForMember(x => x.UpdatedAt, o => o.Ignore())
                .ForMember(x => x.UpdatedBy, o => o.Ignore());

            CreateMap<PipelineStandard, InterpolationPipelineStandardEntity>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.InterpolationDurationByTCPId, o => o.Ignore())
                .ForMember(x => x.InterpolationDurationByTCP, o => o.Ignore());

            CreateMap<ExtrapolationDurationByTCP, ExtrapolationDurationByTCPEntity>()
                .ForMember(x => x.POSId, o => o.Ignore())
                .ForMember(x => x.POS, o => o.Ignore())
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.CreatedAt, o => o.Ignore())
                .ForMember(x => x.CreatedBy, o => o.Ignore())
                .ForMember(x => x.UpdatedAt, o => o.Ignore())
                .ForMember(x => x.UpdatedBy, o => o.Ignore());

            CreateMap<PipelineStandard, ExtrapolationPipelineStandardEntity>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.ExtrapolationDurationByTCPId, o => o.Ignore())
                .ForMember(x => x.ExtrapolationDurationByTCP, o => o.Ignore());

            CreateMap<StepwiseExtrapolationDurationByTCP, StepwiseExtrapolationDurationByTCPEntity>()
                .ForMember(x => x.POSId, o => o.Ignore())
                .ForMember(x => x.POS, o => o.Ignore())
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.CreatedAt, o => o.Ignore())
                .ForMember(x => x.CreatedBy, o => o.Ignore())
                .ForMember(x => x.UpdatedAt, o => o.Ignore())
                .ForMember(x => x.UpdatedBy, o => o.Ignore());

            CreateMap<PipelineStandard, StepwiseExtrapolationPipelineStandardEntity>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.StepwiseExtrapolationDurationByTCPId, o => o.Ignore())
                .ForMember(x => x.StepwiseExtrapolationDurationByTCP, o => o.Ignore());

            CreateMap<PipelineStandard, StepwisePipelineStandardEntity>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.StepwiseExtrapolationDurationByTCPId, o => o.Ignore())
                .ForMember(x => x.StepwiseExtrapolationDurationByTCP, o => o.Ignore());
        }
    }
}
