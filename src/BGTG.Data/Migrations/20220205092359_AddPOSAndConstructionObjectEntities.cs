using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BGTG.POS.Data.Migrations
{
    public partial class AddPOSAndConstructionObjectEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropColumn(
                name: "ObjectCipher",
                table: "StepwiseExtrapolationDurationByTCPs");

            migrationBuilder.DropColumn(
                name: "ObjectCipher",
                table: "InterpolationDurationByTCPs");

            migrationBuilder.DropColumn(
                name: "ObjectCipher",
                table: "ExtrapolationDurationByTCPs");

            migrationBuilder.DropColumn(
                name: "ObjectCipher",
                table: "EnergyAndWaters");

            migrationBuilder.DropColumn(
                name: "ObjectCipher",
                table: "DurationByLCs");

            migrationBuilder.DropColumn(
                name: "ObjectCipher",
                table: "CalendarPlans");

            migrationBuilder.AddColumn<Guid>(
                name: "POSId",
                table: "StepwiseExtrapolationDurationByTCPs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "POSId",
                table: "InterpolationDurationByTCPs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "POSId",
                table: "ExtrapolationDurationByTCPs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "POSId",
                table: "EnergyAndWaters",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "POSId",
                table: "DurationByLCs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "POSId",
                table: "CalendarPlans",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ConstructionObjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Cipher = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionObjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "POSes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConstructionObjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POSes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POSes_ConstructionObjects_ConstructionObjectId",
                        column: x => x.ConstructionObjectId,
                        principalTable: "ConstructionObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StepwiseExtrapolationDurationByTCPs_POSId",
                table: "StepwiseExtrapolationDurationByTCPs",
                column: "POSId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterpolationDurationByTCPs_POSId",
                table: "InterpolationDurationByTCPs",
                column: "POSId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExtrapolationDurationByTCPs_POSId",
                table: "ExtrapolationDurationByTCPs",
                column: "POSId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnergyAndWaters_POSId",
                table: "EnergyAndWaters",
                column: "POSId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DurationByLCs_POSId",
                table: "DurationByLCs",
                column: "POSId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalendarPlans_POSId",
                table: "CalendarPlans",
                column: "POSId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_POSes_ConstructionObjectId",
                table: "POSes",
                column: "ConstructionObjectId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarPlans_POSes_POSId",
                table: "CalendarPlans",
                column: "POSId",
                principalTable: "POSes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DurationByLCs_POSes_POSId",
                table: "DurationByLCs",
                column: "POSId",
                principalTable: "POSes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnergyAndWaters_POSes_POSId",
                table: "EnergyAndWaters",
                column: "POSId",
                principalTable: "POSes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtrapolationDurationByTCPs_POSes_POSId",
                table: "ExtrapolationDurationByTCPs",
                column: "POSId",
                principalTable: "POSes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterpolationDurationByTCPs_POSes_POSId",
                table: "InterpolationDurationByTCPs",
                column: "POSId",
                principalTable: "POSes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StepwiseExtrapolationDurationByTCPs_POSes_POSId",
                table: "StepwiseExtrapolationDurationByTCPs",
                column: "POSId",
                principalTable: "POSes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarPlans_POSes_POSId",
                table: "CalendarPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_DurationByLCs_POSes_POSId",
                table: "DurationByLCs");

            migrationBuilder.DropForeignKey(
                name: "FK_EnergyAndWaters_POSes_POSId",
                table: "EnergyAndWaters");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtrapolationDurationByTCPs_POSes_POSId",
                table: "ExtrapolationDurationByTCPs");

            migrationBuilder.DropForeignKey(
                name: "FK_InterpolationDurationByTCPs_POSes_POSId",
                table: "InterpolationDurationByTCPs");

            migrationBuilder.DropForeignKey(
                name: "FK_StepwiseExtrapolationDurationByTCPs_POSes_POSId",
                table: "StepwiseExtrapolationDurationByTCPs");

            migrationBuilder.DropTable(
                name: "POSes");

            migrationBuilder.DropTable(
                name: "ConstructionObjects");

            migrationBuilder.DropIndex(
                name: "IX_StepwiseExtrapolationDurationByTCPs_POSId",
                table: "StepwiseExtrapolationDurationByTCPs");

            migrationBuilder.DropIndex(
                name: "IX_InterpolationDurationByTCPs_POSId",
                table: "InterpolationDurationByTCPs");

            migrationBuilder.DropIndex(
                name: "IX_ExtrapolationDurationByTCPs_POSId",
                table: "ExtrapolationDurationByTCPs");

            migrationBuilder.DropIndex(
                name: "IX_EnergyAndWaters_POSId",
                table: "EnergyAndWaters");

            migrationBuilder.DropIndex(
                name: "IX_DurationByLCs_POSId",
                table: "DurationByLCs");

            migrationBuilder.DropIndex(
                name: "IX_CalendarPlans_POSId",
                table: "CalendarPlans");

            migrationBuilder.DropColumn(
                name: "POSId",
                table: "StepwiseExtrapolationDurationByTCPs");

            migrationBuilder.DropColumn(
                name: "POSId",
                table: "InterpolationDurationByTCPs");

            migrationBuilder.DropColumn(
                name: "POSId",
                table: "ExtrapolationDurationByTCPs");

            migrationBuilder.DropColumn(
                name: "POSId",
                table: "EnergyAndWaters");

            migrationBuilder.DropColumn(
                name: "POSId",
                table: "DurationByLCs");

            migrationBuilder.DropColumn(
                name: "POSId",
                table: "CalendarPlans");

            migrationBuilder.AddColumn<string>(
                name: "ObjectCipher",
                table: "StepwiseExtrapolationDurationByTCPs",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ObjectCipher",
                table: "InterpolationDurationByTCPs",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ObjectCipher",
                table: "ExtrapolationDurationByTCPs",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ObjectCipher",
                table: "EnergyAndWaters",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ObjectCipher",
                table: "DurationByLCs",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ObjectCipher",
                table: "CalendarPlans",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Logger = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    ThreadId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });
        }
    }
}
