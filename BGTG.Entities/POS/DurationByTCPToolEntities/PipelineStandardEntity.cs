using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Entities.POS.DurationByTCPToolEntities
{
    public abstract class PipelineStandardEntity : Identity
    {
        public decimal PipelineLength { get; set; }
        public decimal Duration { get; set; }
        public decimal PreparatoryPeriod { get; set; }
    }
}
