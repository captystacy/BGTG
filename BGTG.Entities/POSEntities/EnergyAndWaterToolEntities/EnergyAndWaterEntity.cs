using System;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Entities.POSEntities.EnergyAndWaterToolEntities
{
    public class EnergyAndWaterEntity : Auditable
    {
        public int ConstructionYear { get; set; }
        public decimal VolumeCAIW { get; set; }
        public decimal Energy { get; set; }
        public decimal Water { get; set; }
        public decimal CompressedAir { get; set; }
        public decimal Oxygen { get; set; }
        public Guid POSId { get; set; }
        public POSEntity POS { get; set; }
    }
}
