using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BGTG.POS.Data.Migrations
{
    public partial class AddDurationByLCToolEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DurationByLCs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    ObjectCipher = table.Column<string>(maxLength: 32, nullable: false),
                    Duration = table.Column<decimal>(type: "money", nullable: false),
                    TotalLaborCosts = table.Column<decimal>(type: "money", nullable: false),
                    EstimateLaborCosts = table.Column<decimal>(type: "money", nullable: false),
                    TechnologicalLaborCosts = table.Column<decimal>(type: "money", nullable: false),
                    WorkingDayDuration = table.Column<decimal>(type: "money", nullable: false),
                    Shift = table.Column<decimal>(type: "money", nullable: false),
                    NumberOfWorkingDays = table.Column<decimal>(type: "money", nullable: false),
                    NumberOfEmployees = table.Column<int>(nullable: false),
                    RoundedDuration = table.Column<decimal>(type: "money", nullable: false),
                    TotalDuration = table.Column<decimal>(type: "money", nullable: false),
                    PreparatoryPeriod = table.Column<decimal>(type: "money", nullable: false),
                    AcceptanceTime = table.Column<decimal>(type: "money", nullable: false),
                    AcceptanceTimeIncluded = table.Column<bool>(nullable: false),
                    RoundingIncluded = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DurationByLCs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DurationByLCs");
        }
    }
}
