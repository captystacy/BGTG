using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BGTG.POS.Data.Migrations
{
    public partial class AddCalendarPlanToolEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowId = table.Column<string>(maxLength: 50, nullable: false),
                    TableName = table.Column<string>(maxLength: 128, nullable: false),
                    Changed = table.Column<string>(nullable: true),
                    Kind = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    ObjectCipher = table.Column<string>(maxLength: 32, nullable: false),
                    ConstructionStartDate = table.Column<DateTime>(nullable: false),
                    ConstructionDuration = table.Column<decimal>(type: "money", nullable: false),
                    ConstructionDurationCeiling = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Logger = table.Column<string>(maxLength: 255, nullable: false),
                    Level = table.Column<string>(maxLength: 50, nullable: false),
                    Message = table.Column<string>(maxLength: 4000, nullable: false),
                    ThreadId = table.Column<string>(maxLength: 255, nullable: true),
                    ExceptionMessage = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MainCalendarWorks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CalendarPlanId = table.Column<Guid>(nullable: false),
                    WorkName = table.Column<string>(maxLength: 128, nullable: false),
                    TotalCost = table.Column<decimal>(type: "money", nullable: false),
                    TotalCostIncludingCAIW = table.Column<decimal>(type: "money", nullable: false),
                    EstimateChapter = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainCalendarWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainCalendarWorks_CalendarPlans_CalendarPlanId",
                        column: x => x.CalendarPlanId,
                        principalTable: "CalendarPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreparatoryCalendarWorks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CalendarPlanId = table.Column<Guid>(nullable: false),
                    WorkName = table.Column<string>(maxLength: 128, nullable: false),
                    TotalCost = table.Column<decimal>(type: "money", nullable: false),
                    TotalCostIncludingCAIW = table.Column<decimal>(type: "money", nullable: false),
                    EstimateChapter = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreparatoryCalendarWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreparatoryCalendarWorks_CalendarPlans_CalendarPlanId",
                        column: x => x.CalendarPlanId,
                        principalTable: "CalendarPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MainConstructionMonths",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    InvestmentVolume = table.Column<decimal>(type: "money", nullable: false),
                    VolumeCAIW = table.Column<decimal>(type: "money", nullable: false),
                    PercentPart = table.Column<decimal>(type: "money", nullable: false),
                    CreationIndex = table.Column<int>(nullable: false),
                    MainCalendarWorkId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainConstructionMonths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainConstructionMonths_MainCalendarWorks_MainCalendarWorkId",
                        column: x => x.MainCalendarWorkId,
                        principalTable: "MainCalendarWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreparatoryConstructionMonths",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    InvestmentVolume = table.Column<decimal>(type: "money", nullable: false),
                    VolumeCAIW = table.Column<decimal>(type: "money", nullable: false),
                    PercentPart = table.Column<decimal>(type: "money", nullable: false),
                    CreationIndex = table.Column<int>(nullable: false),
                    PreparatoryCalendarWorkId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreparatoryConstructionMonths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreparatoryConstructionMonths_PreparatoryCalendarWorks_PreparatoryCalendarWorkId",
                        column: x => x.PreparatoryCalendarWorkId,
                        principalTable: "PreparatoryCalendarWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainCalendarWorks_CalendarPlanId",
                table: "MainCalendarWorks",
                column: "CalendarPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_MainConstructionMonths_MainCalendarWorkId",
                table: "MainConstructionMonths",
                column: "MainCalendarWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_PreparatoryCalendarWorks_CalendarPlanId",
                table: "PreparatoryCalendarWorks",
                column: "CalendarPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PreparatoryConstructionMonths_PreparatoryCalendarWorkId",
                table: "PreparatoryConstructionMonths",
                column: "PreparatoryCalendarWorkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoHistory");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "MainConstructionMonths");

            migrationBuilder.DropTable(
                name: "PreparatoryConstructionMonths");

            migrationBuilder.DropTable(
                name: "MainCalendarWorks");

            migrationBuilder.DropTable(
                name: "PreparatoryCalendarWorks");

            migrationBuilder.DropTable(
                name: "CalendarPlans");
        }
    }
}
