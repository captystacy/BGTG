﻿// <auto-generated />
using System;
using BGTG.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BGTG.POS.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BGTG.Entities.BGTG.ConstructionObjectEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cipher")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Cipher");

                    b.ToTable("ConstructionObjects", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.CalendarPlanEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ConstructionDuration")
                        .HasColumnType("money");

                    b.Property<int>("ConstructionDurationCeiling")
                        .HasColumnType("int");

                    b.Property<DateTime>("ConstructionStartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<Guid>("POSId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("POSId")
                        .IsUnique();

                    b.ToTable("CalendarPlans", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.MainCalendarWorkEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CalendarPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EstimateChapter")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("money");

                    b.Property<decimal>("TotalCostIncludingCAIW")
                        .HasColumnType("money");

                    b.Property<string>("WorkName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("CalendarPlanId");

                    b.ToTable("MainCalendarWorks", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.MainConstructionMonthEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CreationIndex")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("InvestmentVolume")
                        .HasColumnType("money");

                    b.Property<Guid>("MainCalendarWorkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("PercentPart")
                        .HasColumnType("money");

                    b.Property<decimal>("VolumeCAIW")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("MainCalendarWorkId");

                    b.ToTable("MainConstructionMonths", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.PreparatoryCalendarWorkEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CalendarPlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EstimateChapter")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("money");

                    b.Property<decimal>("TotalCostIncludingCAIW")
                        .HasColumnType("money");

                    b.Property<string>("WorkName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("CalendarPlanId");

                    b.ToTable("PreparatoryCalendarWorks", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.PreparatoryConstructionMonthEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CreationIndex")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("InvestmentVolume")
                        .HasColumnType("money");

                    b.Property<decimal>("PercentPart")
                        .HasColumnType("money");

                    b.Property<Guid>("PreparatoryCalendarWorkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("VolumeCAIW")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("PreparatoryCalendarWorkId");

                    b.ToTable("PreparatoryConstructionMonths", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByLCToolEntities.DurationByLCEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("AcceptanceTime")
                        .HasColumnType("money");

                    b.Property<bool>("AcceptanceTimeIncluded")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal>("Duration")
                        .HasColumnType("money");

                    b.Property<decimal>("EstimateLaborCosts")
                        .HasColumnType("money");

                    b.Property<int>("NumberOfEmployees")
                        .HasColumnType("int");

                    b.Property<decimal>("NumberOfWorkingDays")
                        .HasColumnType("money");

                    b.Property<Guid>("POSId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("PreparatoryPeriod")
                        .HasColumnType("money");

                    b.Property<decimal>("RoundedDuration")
                        .HasColumnType("money");

                    b.Property<bool>("RoundingIncluded")
                        .HasColumnType("bit");

                    b.Property<decimal>("Shift")
                        .HasColumnType("money");

                    b.Property<decimal>("TechnologicalLaborCosts")
                        .HasColumnType("money");

                    b.Property<decimal>("TotalDuration")
                        .HasColumnType("money");

                    b.Property<decimal>("TotalLaborCosts")
                        .HasColumnType("money");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal>("WorkingDayDuration")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("POSId")
                        .IsUnique();

                    b.ToTable("DurationByLCs", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.ExtrapolationDurationByTCPEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AppendixKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("AppendixPage")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal>("Duration")
                        .HasColumnType("money");

                    b.Property<int>("DurationCalculationType")
                        .HasColumnType("int");

                    b.Property<Guid>("POSId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PipelineDiameter")
                        .HasColumnType("int");

                    b.Property<string>("PipelineDiameterPresentation")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<decimal>("PipelineLength")
                        .HasColumnType("money");

                    b.Property<string>("PipelineMaterial")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<decimal>("PreparatoryPeriod")
                        .HasColumnType("money");

                    b.Property<decimal>("RoundedDuration")
                        .HasColumnType("money");

                    b.Property<decimal>("StandardChangePercent")
                        .HasColumnType("money");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal>("VolumeChangePercent")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("POSId")
                        .IsUnique();

                    b.ToTable("ExtrapolationDurationByTCPs", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.ExtrapolationPipelineStandardEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Duration")
                        .HasColumnType("money");

                    b.Property<Guid>("ExtrapolationDurationByTCPId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("PipelineLength")
                        .HasColumnType("money");

                    b.Property<decimal>("PreparatoryPeriod")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("ExtrapolationDurationByTCPId");

                    b.ToTable("ExtrapolationPipelineStandards", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.InterpolationDurationByTCPEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AppendixKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("AppendixPage")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal>("Duration")
                        .HasColumnType("money");

                    b.Property<int>("DurationCalculationType")
                        .HasColumnType("int");

                    b.Property<decimal>("DurationChange")
                        .HasColumnType("money");

                    b.Property<Guid>("POSId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PipelineDiameter")
                        .HasColumnType("int");

                    b.Property<string>("PipelineDiameterPresentation")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<decimal>("PipelineLength")
                        .HasColumnType("money");

                    b.Property<string>("PipelineMaterial")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<decimal>("PreparatoryPeriod")
                        .HasColumnType("money");

                    b.Property<decimal>("RoundedDuration")
                        .HasColumnType("money");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal>("VolumeChange")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("POSId")
                        .IsUnique();

                    b.ToTable("InterpolationDurationByTCPs", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.InterpolationPipelineStandardEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Duration")
                        .HasColumnType("money");

                    b.Property<Guid>("InterpolationDurationByTCPId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("PipelineLength")
                        .HasColumnType("money");

                    b.Property<decimal>("PreparatoryPeriod")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("InterpolationDurationByTCPId");

                    b.ToTable("InterpolationPipelineStandards", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.StepwiseExtrapolationDurationByTCPEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AppendixKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("AppendixPage")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal>("Duration")
                        .HasColumnType("money");

                    b.Property<int>("DurationCalculationType")
                        .HasColumnType("int");

                    b.Property<Guid>("POSId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PipelineDiameter")
                        .HasColumnType("int");

                    b.Property<string>("PipelineDiameterPresentation")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<decimal>("PipelineLength")
                        .HasColumnType("money");

                    b.Property<string>("PipelineMaterial")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<decimal>("PreparatoryPeriod")
                        .HasColumnType("money");

                    b.Property<decimal>("RoundedDuration")
                        .HasColumnType("money");

                    b.Property<decimal>("StandardChangePercent")
                        .HasColumnType("money");

                    b.Property<decimal>("StepwiseDuration")
                        .HasColumnType("money");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal>("VolumeChangePercent")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("POSId")
                        .IsUnique();

                    b.ToTable("StepwiseExtrapolationDurationByTCPs", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.StepwiseExtrapolationPipelineStandardEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Duration")
                        .HasColumnType("money");

                    b.Property<decimal>("PipelineLength")
                        .HasColumnType("money");

                    b.Property<decimal>("PreparatoryPeriod")
                        .HasColumnType("money");

                    b.Property<Guid>("StepwiseExtrapolationDurationByTCPId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StepwiseExtrapolationDurationByTCPId");

                    b.ToTable("StepwiseExtrapolationPipelineStandards", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.StepwisePipelineStandardEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Duration")
                        .HasColumnType("money");

                    b.Property<decimal>("PipelineLength")
                        .HasColumnType("money");

                    b.Property<decimal>("PreparatoryPeriod")
                        .HasColumnType("money");

                    b.Property<Guid>("StepwiseExtrapolationDurationByTCPId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StepwiseExtrapolationDurationByTCPId")
                        .IsUnique();

                    b.ToTable("StepwisePipelineStandards", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.EnergyAndWaterToolEntities.EnergyAndWaterEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("CompressedAir")
                        .HasColumnType("money");

                    b.Property<int>("ConstructionYear")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal>("Energy")
                        .HasColumnType("money");

                    b.Property<decimal>("Oxygen")
                        .HasColumnType("money");

                    b.Property<Guid>("POSId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<decimal>("VolumeCAIW")
                        .HasColumnType("money");

                    b.Property<decimal>("Water")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("POSId")
                        .IsUnique();

                    b.ToTable("EnergyAndWaters", (string)null);
                });

            modelBuilder.Entity("BGTG.Entities.POS.POSEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ConstructionObjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ConstructionObjectId")
                        .IsUnique();

                    b.ToTable("POSes", (string)null);
                });

            modelBuilder.Entity("Microsoft.EntityFrameworkCore.AutoHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Changed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("Kind")
                        .HasColumnType("int");

                    b.Property<string>("RowId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("AutoHistory");
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.CalendarPlanEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.POSEntity", "POS")
                        .WithOne("CalendarPlan")
                        .HasForeignKey("BGTG.Entities.POS.CalendarPlanToolEntities.CalendarPlanEntity", "POSId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("POS");
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.MainCalendarWorkEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.CalendarPlanToolEntities.CalendarPlanEntity", "CalendarPlan")
                        .WithMany("MainCalendarWorks")
                        .HasForeignKey("CalendarPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CalendarPlan");
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.MainConstructionMonthEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.CalendarPlanToolEntities.MainCalendarWorkEntity", "MainCalendarWork")
                        .WithMany("ConstructionMonths")
                        .HasForeignKey("MainCalendarWorkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MainCalendarWork");
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.PreparatoryCalendarWorkEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.CalendarPlanToolEntities.CalendarPlanEntity", "CalendarPlan")
                        .WithMany("PreparatoryCalendarWorks")
                        .HasForeignKey("CalendarPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CalendarPlan");
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.PreparatoryConstructionMonthEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.CalendarPlanToolEntities.PreparatoryCalendarWorkEntity", "PreparatoryCalendarWork")
                        .WithMany("ConstructionMonths")
                        .HasForeignKey("PreparatoryCalendarWorkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PreparatoryCalendarWork");
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByLCToolEntities.DurationByLCEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.POSEntity", "POS")
                        .WithOne("DurationByLC")
                        .HasForeignKey("BGTG.Entities.POS.DurationByLCToolEntities.DurationByLCEntity", "POSId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("POS");
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.ExtrapolationDurationByTCPEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.POSEntity", "POS")
                        .WithOne("ExtrapolationDurationByTCP")
                        .HasForeignKey("BGTG.Entities.POS.DurationByTCPToolEntities.ExtrapolationDurationByTCPEntity", "POSId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("POS");
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.ExtrapolationPipelineStandardEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.DurationByTCPToolEntities.ExtrapolationDurationByTCPEntity", "ExtrapolationDurationByTCP")
                        .WithMany("CalculationPipelineStandards")
                        .HasForeignKey("ExtrapolationDurationByTCPId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExtrapolationDurationByTCP");
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.InterpolationDurationByTCPEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.POSEntity", "POS")
                        .WithOne("InterpolationDurationByTCP")
                        .HasForeignKey("BGTG.Entities.POS.DurationByTCPToolEntities.InterpolationDurationByTCPEntity", "POSId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("POS");
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.InterpolationPipelineStandardEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.DurationByTCPToolEntities.InterpolationDurationByTCPEntity", "InterpolationDurationByTCP")
                        .WithMany("CalculationPipelineStandards")
                        .HasForeignKey("InterpolationDurationByTCPId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InterpolationDurationByTCP");
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.StepwiseExtrapolationDurationByTCPEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.POSEntity", "POS")
                        .WithOne("StepwiseExtrapolationDurationByTCP")
                        .HasForeignKey("BGTG.Entities.POS.DurationByTCPToolEntities.StepwiseExtrapolationDurationByTCPEntity", "POSId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("POS");
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.StepwiseExtrapolationPipelineStandardEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.DurationByTCPToolEntities.StepwiseExtrapolationDurationByTCPEntity", "StepwiseExtrapolationDurationByTCP")
                        .WithMany("CalculationPipelineStandards")
                        .HasForeignKey("StepwiseExtrapolationDurationByTCPId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StepwiseExtrapolationDurationByTCP");
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.StepwisePipelineStandardEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.DurationByTCPToolEntities.StepwiseExtrapolationDurationByTCPEntity", "StepwiseExtrapolationDurationByTCP")
                        .WithOne("StepwisePipelineStandard")
                        .HasForeignKey("BGTG.Entities.POS.DurationByTCPToolEntities.StepwisePipelineStandardEntity", "StepwiseExtrapolationDurationByTCPId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StepwiseExtrapolationDurationByTCP");
                });

            modelBuilder.Entity("BGTG.Entities.POS.EnergyAndWaterToolEntities.EnergyAndWaterEntity", b =>
                {
                    b.HasOne("BGTG.Entities.POS.POSEntity", "POS")
                        .WithOne("EnergyAndWater")
                        .HasForeignKey("BGTG.Entities.POS.EnergyAndWaterToolEntities.EnergyAndWaterEntity", "POSId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("POS");
                });

            modelBuilder.Entity("BGTG.Entities.POS.POSEntity", b =>
                {
                    b.HasOne("BGTG.Entities.BGTG.ConstructionObjectEntity", "ConstructionObject")
                        .WithOne("POS")
                        .HasForeignKey("BGTG.Entities.POS.POSEntity", "ConstructionObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ConstructionObject");
                });

            modelBuilder.Entity("BGTG.Entities.BGTG.ConstructionObjectEntity", b =>
                {
                    b.Navigation("POS");
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.CalendarPlanEntity", b =>
                {
                    b.Navigation("MainCalendarWorks");

                    b.Navigation("PreparatoryCalendarWorks");
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.MainCalendarWorkEntity", b =>
                {
                    b.Navigation("ConstructionMonths");
                });

            modelBuilder.Entity("BGTG.Entities.POS.CalendarPlanToolEntities.PreparatoryCalendarWorkEntity", b =>
                {
                    b.Navigation("ConstructionMonths");
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.ExtrapolationDurationByTCPEntity", b =>
                {
                    b.Navigation("CalculationPipelineStandards");
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.InterpolationDurationByTCPEntity", b =>
                {
                    b.Navigation("CalculationPipelineStandards");
                });

            modelBuilder.Entity("BGTG.Entities.POS.DurationByTCPToolEntities.StepwiseExtrapolationDurationByTCPEntity", b =>
                {
                    b.Navigation("CalculationPipelineStandards");

                    b.Navigation("StepwisePipelineStandard")
                        .IsRequired();
                });

            modelBuilder.Entity("BGTG.Entities.POS.POSEntity", b =>
                {
                    b.Navigation("CalendarPlan");

                    b.Navigation("DurationByLC");

                    b.Navigation("EnergyAndWater");

                    b.Navigation("ExtrapolationDurationByTCP");

                    b.Navigation("InterpolationDurationByTCP");

                    b.Navigation("StepwiseExtrapolationDurationByTCP");
                });
#pragma warning restore 612, 618
        }
    }
}