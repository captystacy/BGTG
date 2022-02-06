using BGTG.Entities.POSEntities;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Entities
{
    public class ConstructionObjectEntity : Auditable
    {
        public string Cipher { get; set; }
        public POSEntity POS { get; set; }

    }
}
