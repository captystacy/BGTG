using System;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Entities.POSEntities.DurationByTCPToolEntities
{
    public abstract class DurationByTCPEntity : Auditable
    {
        public string PipelineMaterial { get; set; }
        public int PipelineDiameter { get; set; }
        public string PipelineDiameterPresentation { get; set; }
        public decimal PipelineLength { get; set; }
        public DurationCalculationType DurationCalculationType { get; set; }
        public decimal Duration { get; set; }
        public decimal RoundedDuration { get; set; }
        public decimal PreparatoryPeriod { get; set; }
        public char AppendixKey { get; set; }
        public int AppendixPage { get; set; }
        public Guid POSId { get; set; }
        public POSEntity POS { get; set; }
    }
}
