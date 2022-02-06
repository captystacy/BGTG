using Microsoft.EntityFrameworkCore.Migrations;

namespace BGTG.POS.Data.Migrations
{
    public partial class AddStepwiseExtrapolationMissingProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "StandardChangePercent",
                table: "StepwiseExtrapolationDurationByTCPs",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VolumeChangePercent",
                table: "StepwiseExtrapolationDurationByTCPs",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StandardChangePercent",
                table: "StepwiseExtrapolationDurationByTCPs");

            migrationBuilder.DropColumn(
                name: "VolumeChangePercent",
                table: "StepwiseExtrapolationDurationByTCPs");
        }
    }
}
