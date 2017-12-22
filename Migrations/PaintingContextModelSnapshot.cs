﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using VipcoPainting.Models;

namespace VipcoPainting.Migrations
{
    [DbContext(typeof(PaintingContext))]
    partial class PaintingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VipcoPainting.Models.BlastRoom", b =>
                {
                    b.Property<int>("BlastRoomId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BlastRoomName")
                        .HasMaxLength(100);

                    b.Property<int?>("BlastRoomNumber")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("PaintTeamId");

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.HasKey("BlastRoomId");

                    b.HasIndex("PaintTeamId");

                    b.ToTable("BlastRoom");
                });

            modelBuilder.Entity("VipcoPainting.Models.BlastWorkItem", b =>
                {
                    b.Property<int>("BlastWorkItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<double?>("ExtArea");

                    b.Property<double?>("ExtCalcStdUsage");

                    b.Property<double?>("IntArea");

                    b.Property<double?>("IntCalcStdUsage");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("RequirePaintingListId");

                    b.Property<int?>("StandradTimeExtId");

                    b.Property<int?>("StandradTimeIntId");

                    b.Property<int?>("SurfaceTypeExtId");

                    b.Property<int?>("SurfaceTypeIntId");

                    b.HasKey("BlastWorkItemId");

                    b.HasIndex("RequirePaintingListId");

                    b.HasIndex("StandradTimeExtId");

                    b.HasIndex("StandradTimeIntId");

                    b.HasIndex("SurfaceTypeExtId");

                    b.HasIndex("SurfaceTypeIntId");

                    b.ToTable("BlastWorkItem");
                });

            modelBuilder.Entity("VipcoPainting.Models.ColorItem", b =>
                {
                    b.Property<int>("ColorItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ColorCode")
                        .HasMaxLength(50);

                    b.Property<string>("ColorName")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<double?>("VolumeSolids");

                    b.HasKey("ColorItemId");

                    b.ToTable("ColorItem");
                });

            modelBuilder.Entity("VipcoPainting.Models.PaintTeam", b =>
                {
                    b.Property<int>("PaintTeamId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<string>("Ramark")
                        .HasMaxLength(200);

                    b.Property<string>("TeamName")
                        .HasMaxLength(100);

                    b.HasKey("PaintTeamId");

                    b.ToTable("PaintTeam");
                });

            modelBuilder.Entity("VipcoPainting.Models.PaintWorkItem", b =>
                {
                    b.Property<int>("PaintWorkItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<double?>("ExtArea");

                    b.Property<double?>("ExtCalcColorUsage");

                    b.Property<double?>("ExtCalcStdUsage");

                    b.Property<int?>("ExtColorItemId");

                    b.Property<double?>("ExtDFTMax");

                    b.Property<double?>("ExtDFTMin");

                    b.Property<double?>("IntArea");

                    b.Property<double?>("IntCalcColorUsage");

                    b.Property<double?>("IntCalcStdUsage");

                    b.Property<int?>("IntColorItemId");

                    b.Property<double?>("IntDFTMax");

                    b.Property<double?>("IntDFTMin");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("PaintLevel");

                    b.Property<int?>("RequirePaintingListId");

                    b.Property<int?>("StandradTimeExtId");

                    b.Property<int?>("StandradTimeIntId");

                    b.HasKey("PaintWorkItemId");

                    b.HasIndex("ExtColorItemId");

                    b.HasIndex("IntColorItemId");

                    b.HasIndex("RequirePaintingListId");

                    b.HasIndex("StandradTimeExtId");

                    b.HasIndex("StandradTimeIntId");

                    b.ToTable("PaintWorkItem");
                });

            modelBuilder.Entity("VipcoPainting.Models.ProjectCodeSub", b =>
                {
                    b.Property<int>("ProjectCodeSubId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.Property<int?>("ProjectCodeMasterId");

                    b.Property<int?>("ProjectSubParentId");

                    b.HasKey("ProjectCodeSubId");

                    b.HasIndex("ProjectSubParentId");

                    b.ToTable("ProjectCodeSub");
                });

            modelBuilder.Entity("VipcoPainting.Models.RequirePaintingList", b =>
                {
                    b.Property<int>("RequirePaintingListId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("DrawingNo")
                        .HasMaxLength(150);

                    b.Property<bool?>("FieldWeld");

                    b.Property<bool?>("ITP");

                    b.Property<bool?>("Insulation");

                    b.Property<string>("MarkNo")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<string>("PartNo")
                        .HasMaxLength(150);

                    b.Property<DateTime?>("PlanEnd");

                    b.Property<DateTime?>("PlanStart");

                    b.Property<double?>("Quantity");

                    b.Property<int?>("RequirePaintingMasterId");

                    b.Property<double?>("SizeH");

                    b.Property<double?>("SizeL");

                    b.Property<double?>("SizeW");

                    b.Property<int?>("UnitNo");

                    b.Property<double?>("Weight");

                    b.HasKey("RequirePaintingListId");

                    b.HasIndex("RequirePaintingMasterId");

                    b.ToTable("RequirePaintingList");
                });

            modelBuilder.Entity("VipcoPainting.Models.RequirePaintingMaster", b =>
                {
                    b.Property<int>("RequirePaintingMasterId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("FinishDate");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<string>("PaintingSchedule")
                        .HasMaxLength(200);

                    b.Property<int?>("ProjectCodeSubId");

                    b.Property<DateTime?>("ReceiveDate");

                    b.Property<string>("ReceiveEmp");

                    b.Property<DateTime?>("RequireDate");

                    b.Property<string>("RequireEmp");

                    b.Property<string>("RequireNo")
                        .HasMaxLength(200);

                    b.Property<int>("RequirePaintingStatus");

                    b.HasKey("RequirePaintingMasterId");

                    b.HasIndex("ProjectCodeSubId");

                    b.ToTable("RequirePaintingMaster");
                });

            modelBuilder.Entity("VipcoPainting.Models.RequisitionMaster", b =>
                {
                    b.Property<int>("RequisitionMasterId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ColorItemId");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<double?>("Quantity");

                    b.Property<string>("RequisitionBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("RequisitionDate");

                    b.Property<int?>("TaskPaintDetailId");

                    b.HasKey("RequisitionMasterId");

                    b.HasIndex("ColorItemId");

                    b.HasIndex("TaskPaintDetailId");

                    b.ToTable("RequisitionMaster");
                });

            modelBuilder.Entity("VipcoPainting.Models.StandradTime", b =>
                {
                    b.Property<int>("StandradTimeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<double?>("PercentLoss");

                    b.Property<double?>("Rate");

                    b.Property<string>("RateUnit")
                        .HasMaxLength(50);

                    b.Property<int?>("TypeStandardTime");

                    b.HasKey("StandradTimeId");

                    b.ToTable("StandradTime");
                });

            modelBuilder.Entity("VipcoPainting.Models.SurfaceType", b =>
                {
                    b.Property<int>("SurfaceTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<string>("SurfaceCode")
                        .HasMaxLength(100);

                    b.Property<string>("SurfaceName")
                        .HasMaxLength(200);

                    b.HasKey("SurfaceTypeId");

                    b.ToTable("SurfaceType");
                });

            modelBuilder.Entity("VipcoPainting.Models.TaskBlastDetail", b =>
                {
                    b.Property<int>("TaskBlastDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BlastRoomId");

                    b.Property<int?>("BlastWorkItemId");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("TaskMasterId");

                    b.HasKey("TaskBlastDetailId");

                    b.HasIndex("BlastRoomId");

                    b.HasIndex("BlastWorkItemId");

                    b.HasIndex("TaskMasterId");

                    b.ToTable("TaskBlastDetail");
                });

            modelBuilder.Entity("VipcoPainting.Models.TaskMaster", b =>
                {
                    b.Property<int>("TaskMasterId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("ActualEDate");

                    b.Property<DateTime?>("ActualSDate");

                    b.Property<string>("AssignBy");

                    b.Property<DateTime?>("AssignDate")
                        .IsRequired();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("RequirePaintingListId");

                    b.Property<string>("TaskNo")
                        .HasMaxLength(50);

                    b.Property<double?>("TaskProgress")
                        .HasMaxLength(100);

                    b.Property<int?>("TaskStatus")
                        .IsRequired();

                    b.HasKey("TaskMasterId");

                    b.HasIndex("RequirePaintingListId");

                    b.ToTable("TaskMaster");
                });

            modelBuilder.Entity("VipcoPainting.Models.TaskPaintDetail", b =>
                {
                    b.Property<int>("TaskPaintDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("PaintTeamId");

                    b.Property<int?>("PaintWorkItemId");

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("TaskMasterId");

                    b.HasKey("TaskPaintDetailId");

                    b.HasIndex("PaintTeamId");

                    b.HasIndex("PaintWorkItemId");

                    b.HasIndex("TaskMasterId");

                    b.ToTable("TaskPaintDetail");
                });

            modelBuilder.Entity("VipcoPainting.Models.BlastRoom", b =>
                {
                    b.HasOne("VipcoPainting.Models.PaintTeam", "PaintTeam")
                        .WithMany("BlastRooms")
                        .HasForeignKey("PaintTeamId");
                });

            modelBuilder.Entity("VipcoPainting.Models.BlastWorkItem", b =>
                {
                    b.HasOne("VipcoPainting.Models.RequirePaintingList", "RequirePaintingList")
                        .WithMany("BlastWorkItems")
                        .HasForeignKey("RequirePaintingListId");

                    b.HasOne("VipcoPainting.Models.StandradTime", "StandradTimeExt")
                        .WithMany()
                        .HasForeignKey("StandradTimeExtId");

                    b.HasOne("VipcoPainting.Models.StandradTime", "StandradTimeInt")
                        .WithMany()
                        .HasForeignKey("StandradTimeIntId");

                    b.HasOne("VipcoPainting.Models.SurfaceType", "SurfaceTypeExt")
                        .WithMany()
                        .HasForeignKey("SurfaceTypeExtId");

                    b.HasOne("VipcoPainting.Models.SurfaceType", "SurfaceTypeInt")
                        .WithMany()
                        .HasForeignKey("SurfaceTypeIntId");
                });

            modelBuilder.Entity("VipcoPainting.Models.PaintWorkItem", b =>
                {
                    b.HasOne("VipcoPainting.Models.ColorItem", "ExtColorItem")
                        .WithMany()
                        .HasForeignKey("ExtColorItemId");

                    b.HasOne("VipcoPainting.Models.ColorItem", "IntColorItem")
                        .WithMany()
                        .HasForeignKey("IntColorItemId");

                    b.HasOne("VipcoPainting.Models.RequirePaintingList", "RequirePaintingList")
                        .WithMany("PaintWorkItems")
                        .HasForeignKey("RequirePaintingListId");

                    b.HasOne("VipcoPainting.Models.StandradTime", "StandradTimeExt")
                        .WithMany()
                        .HasForeignKey("StandradTimeExtId");

                    b.HasOne("VipcoPainting.Models.StandradTime", "StandradTimeInt")
                        .WithMany()
                        .HasForeignKey("StandradTimeIntId");
                });

            modelBuilder.Entity("VipcoPainting.Models.ProjectCodeSub", b =>
                {
                    b.HasOne("VipcoPainting.Models.ProjectCodeSub", "ProjectSubParent")
                        .WithMany()
                        .HasForeignKey("ProjectSubParentId");
                });

            modelBuilder.Entity("VipcoPainting.Models.RequirePaintingList", b =>
                {
                    b.HasOne("VipcoPainting.Models.RequirePaintingMaster", "RequirePaintingMaster")
                        .WithMany("RequirePaintingLists")
                        .HasForeignKey("RequirePaintingMasterId");
                });

            modelBuilder.Entity("VipcoPainting.Models.RequirePaintingMaster", b =>
                {
                    b.HasOne("VipcoPainting.Models.ProjectCodeSub", "ProjectCodeSub")
                        .WithMany()
                        .HasForeignKey("ProjectCodeSubId");
                });

            modelBuilder.Entity("VipcoPainting.Models.RequisitionMaster", b =>
                {
                    b.HasOne("VipcoPainting.Models.ColorItem", "ColorItem")
                        .WithMany("RequisitionMasters")
                        .HasForeignKey("ColorItemId");

                    b.HasOne("VipcoPainting.Models.TaskPaintDetail", "TaskPaintDetail")
                        .WithMany("RequisitionMasters")
                        .HasForeignKey("TaskPaintDetailId");
                });

            modelBuilder.Entity("VipcoPainting.Models.TaskBlastDetail", b =>
                {
                    b.HasOne("VipcoPainting.Models.BlastRoom", "BlastRoom")
                        .WithMany("TaskBlastDetails")
                        .HasForeignKey("BlastRoomId");

                    b.HasOne("VipcoPainting.Models.BlastWorkItem", "BlastWorkItem")
                        .WithMany()
                        .HasForeignKey("BlastWorkItemId");

                    b.HasOne("VipcoPainting.Models.TaskMaster", "TaskMaster")
                        .WithMany("TaskBlastDetails")
                        .HasForeignKey("TaskMasterId");
                });

            modelBuilder.Entity("VipcoPainting.Models.TaskMaster", b =>
                {
                    b.HasOne("VipcoPainting.Models.RequirePaintingList", "RequirePaintingList")
                        .WithMany()
                        .HasForeignKey("RequirePaintingListId");
                });

            modelBuilder.Entity("VipcoPainting.Models.TaskPaintDetail", b =>
                {
                    b.HasOne("VipcoPainting.Models.PaintTeam", "PaintTeam")
                        .WithMany("TaskPaintDetails")
                        .HasForeignKey("PaintTeamId");

                    b.HasOne("VipcoPainting.Models.PaintWorkItem", "PaintWorkItem")
                        .WithMany()
                        .HasForeignKey("PaintWorkItemId");

                    b.HasOne("VipcoPainting.Models.TaskMaster", "TaskMaster")
                        .WithMany("TaskPaintDetails")
                        .HasForeignKey("TaskMasterId");
                });
#pragma warning restore 612, 618
        }
    }
}
