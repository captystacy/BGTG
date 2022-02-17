using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BGTG.POS.Data.Migrations
{
    public partial class DurationByTCPToolEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExtrapolationDurationByTCPs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    ObjectCipher = table.Column<string>(maxLength: 32, nullable: false),
                    PipelineMaterial = table.Column<string>(maxLength: 64, nullable: false),
                    PipelineDiameter = table.Column<int>(nullable: false),
                    PipelineDiameterPresentation = table.Column<string>(maxLength: 16, nullable: false),
                    PipelineLength = table.Column<decimal>(type: "money", nullable: false),
                    DurationCalculationType = table.Column<int>(nullable: false),
                    Duration = table.Column<decimal>(type: "money", nullable: false),
                    RoundedDuration = table.Column<decimal>(type: "money", nullable: false),
                    PreparatoryPeriod = table.Column<decimal>(type: "money", nullable: false),
                    AppendixKey = table.Column<string>(nullable: false),
                    AppendixPage = table.Column<int>(nullable: false),
                    VolumeChangePercent = table.Column<decimal>(type: "money", nullable: false),
                    StandardChangePercent = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtrapolationDurationByTCPs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterpolationDurationByTCPs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    ObjectCipher = table.Column<string>(maxLength: 32, nullable: false),
                    PipelineMaterial = table.Column<string>(maxLength: 64, nullable: false),
                    PipelineDiameter = table.Column<int>(nullable: false),
                    PipelineDiameterPresentation = table.Column<string>(maxLength: 16, nullable: false),
                    PipelineLength = table.Column<decimal>(type: "money", nullable: false),
                    DurationCalculationType = table.Column<int>(nullable: false),
                    Duration = table.Column<decimal>(type: "money", nullable: false),
                    RoundedDuration = table.Column<decimal>(type: "money", nullable: false),
                    PreparatoryPeriod = table.Column<decimal>(type: "money", nullable: false),
                    AppendixKey = table.Column<string>(nullable: false),
                    AppendixPage = table.Column<int>(nullable: false),
                    DurationChange = table.Column<decimal>(type: "money", nullable: false),
                    VolumeChange = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterpolationDurationByTCPs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StepwiseExtrapolationDurationByTCPs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    ObjectCipher = table.Column<string>(maxLength: 32, nullable: false),
                    PipelineMaterial = table.Column<string>(maxLength: 64, nullable: false),
                    PipelineDiameter = table.Column<int>(nullable: false),
                    PipelineDiameterPresentation = table.Column<string>(maxLength: 16, nullable: false),
                    PipelineLength = table.Column<decimal>(type: "money", nullable: false),
                    DurationCalculationType = table.Column<int>(nullable: false),
                    Duration = table.Column<decimal>(type: "money", nullable: false),
                    RoundedDuration = table.Column<decimal>(type: "money", nullable: false),
                    PreparatoryPeriod = table.Column<decimal>(type: "money", nullable: false),
                    AppendixKey = table.Column<string>(nullable: false),
                    AppendixPage = table.Column<int>(nullable: false),
                    StepwiseDuration = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepwiseExtrapolationDurationByTCPs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtrapolationPipelineStandards",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PipelineLength = table.Column<decimal>(type: "money", nullable: false),
                    Duration = table.Column<decimal>(type: "money", nullable: false),
                    PreparatoryPeriod = table.Column<decimal>(type: "money", nullable: false),
                    ExtrapolationDurationByTCPId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtrapolationPipelineStandards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtrapolationPipelineStandards_ExtrapolationDurationByTCPs_ExtrapolationDurationByTCPId",
                        column: x => x.ExtrapolationDurationByTCPId,
                        principalTable: "ExtrapolationDurationByTCPs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterpolationPipelineStandards",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PipelineLength = table.Column<decimal>(type: "money", nullable: false),
                    Duration = table.Column<decimal>(type: "money", nullable: false),
                    PreparatoryPeriod = table.Column<decimal>(type: "money", nullable: false),
                    InterpolationDurationByTCPId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterpolationPipelineStandards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterpolationPipelineStandards_InterpolationDurationByTCPs_InterpolationDurationByTCPId",
                        column: x => x.InterpolationDurationByTCPId,
                        principalTable: "InterpolationDurationByTCPs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StepwiseExtrapolationPipelineStandards",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PipelineLength = table.Column<decimal>(type: "money", nullable: false),
                    Duration = table.Column<decimal>(type: "money", nullable: false),
                    PreparatoryPeriod = table.Column<decimal>(type: "money", nullable: false),
                    StepwiseExtrapolationDurationByTCPId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepwiseExtrapolationPipelineStandards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepwiseExtrapolationPipelineStandards_StepwiseExtrapolationDurationByTCPs_StepwiseExtrapolationDurationByTCPId",
                        column: x => x.StepwiseExtrapolationDurationByTCPId,
                        principalTable: "StepwiseExtrapolationDurationByTCPs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StepwisePipelineStandards",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PipelineLength = table.Column<decimal>(type: "money", nullable: false),
                    Duration = table.Column<decimal>(type: "money", nullable: false),
                    PreparatoryPeriod = table.Column<decimal>(type: "money", nullable: false),
                    StepwiseExtrapolationDurationByTCPId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepwisePipelineStandards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepwisePipelineStandards_StepwiseExtrapolationDurationByTCPs_StepwiseExtrapolationDurationByTCPId",
                        column: x => x.StepwiseExtrapolationDurationByTCPId,
                        principalTable: "StepwiseExtrapolationDurationByTCPs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtrapolationPipelineStandards_ExtrapolationDurationByTCPId",
                table: "ExtrapolationPipelineStandards",
                column: "ExtrapolationDurationByTCPId");

            migrationBuilder.CreateIndex(
                name: "IX_InterpolationPipelineStandards_InterpolationDurationByTCPId",
                table: "InterpolationPipelineStandards",
                column: "InterpolationDurationByTCPId");

            migrationBuilder.CreateIndex(
                name: "IX_StepwiseExtrapolationPipelineStandards_StepwiseExtrapolationDurationByTCPId",
                table: "StepwiseExtrapolationPipelineStandards",
                column: "StepwiseExtrapolationDurationByTCPId");

            migrationBuilder.CreateIndex(
                name: "IX_StepwisePipelineStandards_StepwiseExtrapolationDurationByTCPId",
                table: "StepwisePipelineStandards",
                column: "StepwiseExtrapolationDurationByTCPId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtrapolationPipelineStandards");

            migrationBuilder.DropTable(
                name: "InterpolationPipelineStandards");

            migrationBuilder.DropTable(
                name: "StepwiseExtrapolationPipelineStandards");

            migrationBuilder.DropTable(
                name: "StepwisePipelineStandards");

            migrationBuilder.DropTable(
                name: "ExtrapolationDurationByTCPs");

            migrationBuilder.DropTable(
                name: "InterpolationDurationByTCPs");

            migrationBuilder.DropTable(
                name: "StepwiseExtrapolationDurationByTCPs");
        }
    }
}
