using BGTG.Entities.POS;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace BGTG.Entities.BGTG;

public class ConstructionObjectEntity : Auditable
{
    public string Cipher { get; set; } = null!;
    public POSEntity? POS { get; set; }

}