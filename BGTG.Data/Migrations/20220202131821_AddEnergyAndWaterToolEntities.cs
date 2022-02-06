using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BGTG.POS.Data.Migrations
{
    public partial class AddEnergyAndWaterToolEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnergyAndWaters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    ObjectCipher = table.Column<string>(maxLength: 32, nullable: false),
                    ConstructionYear = table.Column<int>(nullable: false),
                    VolumeCAIW = table.Column<decimal>(type: "money", nullable: false),
                    Energy = table.Column<decimal>(type: "money", nullable: false),
                    Water = table.Column<decimal>(type: "money", nullable: false),
                    CompressedAir = table.Column<decimal>(type: "money", nullable: false),
                    Oxygen = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyAndWaters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnergyAndWaters");
        }
    }
}
