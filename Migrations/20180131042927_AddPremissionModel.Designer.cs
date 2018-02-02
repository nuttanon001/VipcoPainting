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
    [Migration("20180131042927_AddPremissionModel")]
    partial class AddPremissionModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<int?>("BlastRoomNumber");

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

            modelBuilder.Entity("VipcoPainting.Models.ColorMovementStock", b =>
                {
                    b.Property<int>("ColortMovementStockId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ColorItemId");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<DateTime>("MovementStockDate");

                    b.Property<int?>("MovementStockStatusId");

                    b.Property<double>("Quantity");

                    b.HasKey("ColortMovementStockId");

                    b.HasIndex("ColorItemId");

                    b.HasIndex("MovementStockStatusId");

                    b.ToTable("ColorMovementStock");
                });

            modelBuilder.Entity("VipcoPainting.Models.FinishedGoodsMaster", b =>
                {
                    b.Property<int>("FinishedGoodsMasterId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ColorItemId");

                    b.Property<int?>("ColorMovementStockId");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime>("FinishedGoodsDate");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<double?>("Quantity");

                    b.Property<string>("ReceiveBy")
                        .HasMaxLength(50);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.HasKey("FinishedGoodsMasterId");

                    b.HasIndex("ColorItemId");

                    b.HasIndex("ColorMovementStockId")
                        .IsUnique()
                        .HasFilter("[ColorMovementStockId] IS NOT NULL");

                    b.ToTable("FinishedGoodsMaster");
                });

            modelBuilder.Entity("VipcoPainting.Models.MovementStockStatus", b =>
                {
                    b.Property<int>("MovementStockStatusId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int>("StatusMovement");

                    b.Property<string>("StatusName")
                        .HasMaxLength(50);

                    b.Property<int>("TypeStatusMovement");

                    b.HasKey("MovementStockStatusId");

                    b.ToTable("MovementStockStatus");
                });

            modelBuilder.Entity("VipcoPainting.Models.PaintTaskDetail", b =>
                {
                    b.Property<int>("PaintTaskDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("ActualEDate");

                    b.Property<DateTime?>("ActualSDate");

                    b.Property<int?>("BlastRoomId");

                    b.Property<int?>("BlastWorkItemId");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("PaintTaskDetailLayer");

                    b.Property<int?>("PaintTaskDetailStatus");

                    b.Property<int?>("PaintTaskDetailType");

                    b.Property<int?>("PaintTaskMasterId");

                    b.Property<int?>("PaintTeamId");

                    b.Property<int?>("PaintWorkItemId");

                    b.Property<int?>("PaymentDetailId");

                    b.Property<DateTime>("PlanEDate");

                    b.Property<DateTime>("PlanSDate");

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<double?>("TaskDetailProgress");

                    b.HasKey("PaintTaskDetailId");

                    b.HasIndex("BlastRoomId");

                    b.HasIndex("BlastWorkItemId");

                    b.HasIndex("PaintTaskMasterId");

                    b.HasIndex("PaintTeamId");

                    b.HasIndex("PaintWorkItemId");

                    b.HasIndex("PaymentDetailId");

                    b.ToTable("PaintTaskDetail");
                });

            modelBuilder.Entity("VipcoPainting.Models.PaintTaskMaster", b =>
                {
                    b.Property<int>("PaintTaskMasterId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AssignBy");

                    b.Property<DateTime?>("AssignDate")
                        .IsRequired();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<double?>("MainProgress");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int>("PaintTaskStatus");

                    b.Property<int?>("RequirePaintingListId");

                    b.Property<string>("TaskPaintNo")
                        .HasMaxLength(50);

                    b.HasKey("PaintTaskMasterId");

                    b.HasIndex("RequirePaintingListId");

                    b.ToTable("PaintTaskMaster");
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

            modelBuilder.Entity("VipcoPainting.Models.PaymentCostHistory", b =>
                {
                    b.Property<int>("PaymentCostHistoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("EndDate");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<double>("PaymentCost");

                    b.Property<int>("PaymentDetailId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("PaymentCostHistoryId");

                    b.HasIndex("PaymentDetailId");

                    b.ToTable("PaymentCostHistory");
                });

            modelBuilder.Entity("VipcoPainting.Models.PaymentDetail", b =>
                {
                    b.Property<int>("PaymentDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<double>("LastCost");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int>("PaymentType");

                    b.HasKey("PaymentDetailId");

                    b.ToTable("PaymentDetail");
                });

            modelBuilder.Entity("VipcoPainting.Models.Permission", b =>
                {
                    b.Property<int>("PermissionId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<int>("LevelPermission");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int>("UserId");

                    b.HasKey("PermissionId");

                    b.ToTable("Permission");
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

                    b.Property<int?>("RequirePaintingListStatus");

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

                    b.Property<int?>("ColorMovementStockId");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("PaintTaskDetailId");

                    b.Property<double?>("Quantity");

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<string>("RequisitionBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("RequisitionDate");

                    b.HasKey("RequisitionMasterId");

                    b.HasIndex("ColorItemId");

                    b.HasIndex("ColorMovementStockId")
                        .IsUnique()
                        .HasFilter("[ColorMovementStockId] IS NOT NULL");

                    b.HasIndex("PaintTaskDetailId");

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

            modelBuilder.Entity("VipcoPainting.Models.SubPaymentDetail", b =>
                {
                    b.Property<int>("SubPaymentDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<double?>("AdditionArea");

                    b.Property<double?>("AdditionCost");

                    b.Property<double?>("AreaWorkLoad");

                    b.Property<double?>("CalcCost");

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("PaymentCostHistoryId");

                    b.Property<DateTime?>("PaymentDate");

                    b.Property<int?>("PaymentDetailId");

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("SubPaymentMasterId");

                    b.HasKey("SubPaymentDetailId");

                    b.HasIndex("PaymentCostHistoryId");

                    b.HasIndex("PaymentDetailId");

                    b.HasIndex("SubPaymentMasterId");

                    b.ToTable("SubPaymentDetail");
                });

            modelBuilder.Entity("VipcoPainting.Models.SubPaymentMaster", b =>
                {
                    b.Property<int>("SubPaymentMasterId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateDate");

                    b.Property<string>("Creator")
                        .HasMaxLength(50);

                    b.Property<string>("EmpApproved1")
                        .HasMaxLength(50);

                    b.Property<string>("EmpApproved2")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime?>("ModifyDate");

                    b.Property<string>("Modifyer")
                        .HasMaxLength(50);

                    b.Property<int?>("PaintTeamId");

                    b.Property<int?>("PrecedingSubPaymentId");

                    b.Property<int?>("ProjectCodeMasterId");

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<DateTime>("StartDate");

                    b.Property<DateTime?>("SubPaymentDate");

                    b.Property<int?>("SubPaymentMasterStatus");

                    b.Property<string>("SubPaymentNo")
                        .HasMaxLength(100);

                    b.HasKey("SubPaymentMasterId");

                    b.HasIndex("PaintTeamId");

                    b.HasIndex("PrecedingSubPaymentId");

                    b.ToTable("SubPaymentMaster");
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

            modelBuilder.Entity("VipcoPainting.Models.ColorMovementStock", b =>
                {
                    b.HasOne("VipcoPainting.Models.ColorItem", "ColorItem")
                        .WithMany()
                        .HasForeignKey("ColorItemId");

                    b.HasOne("VipcoPainting.Models.MovementStockStatus", "MovementStockStatus")
                        .WithMany("ColorMovementStocks")
                        .HasForeignKey("MovementStockStatusId");
                });

            modelBuilder.Entity("VipcoPainting.Models.FinishedGoodsMaster", b =>
                {
                    b.HasOne("VipcoPainting.Models.ColorItem", "ColorItem")
                        .WithMany()
                        .HasForeignKey("ColorItemId");

                    b.HasOne("VipcoPainting.Models.ColorMovementStock", "ColorMovementStock")
                        .WithOne("FinishedGoodsMaster")
                        .HasForeignKey("VipcoPainting.Models.FinishedGoodsMaster", "ColorMovementStockId");
                });

            modelBuilder.Entity("VipcoPainting.Models.PaintTaskDetail", b =>
                {
                    b.HasOne("VipcoPainting.Models.BlastRoom", "BlastRoom")
                        .WithMany("PaintTaskDetails")
                        .HasForeignKey("BlastRoomId");

                    b.HasOne("VipcoPainting.Models.BlastWorkItem", "BlastWorkItem")
                        .WithMany()
                        .HasForeignKey("BlastWorkItemId");

                    b.HasOne("VipcoPainting.Models.PaintTaskMaster", "PaintTaskMaster")
                        .WithMany("PaintTaskDetails")
                        .HasForeignKey("PaintTaskMasterId");

                    b.HasOne("VipcoPainting.Models.PaintTeam", "PaintTeam")
                        .WithMany("PaintTaskDetails")
                        .HasForeignKey("PaintTeamId");

                    b.HasOne("VipcoPainting.Models.PaintWorkItem", "PaintWorkItem")
                        .WithMany()
                        .HasForeignKey("PaintWorkItemId");

                    b.HasOne("VipcoPainting.Models.PaymentDetail", "PaymentDetail")
                        .WithMany("PaintTaskDetails")
                        .HasForeignKey("PaymentDetailId");
                });

            modelBuilder.Entity("VipcoPainting.Models.PaintTaskMaster", b =>
                {
                    b.HasOne("VipcoPainting.Models.RequirePaintingList", "RequirePaintingList")
                        .WithMany()
                        .HasForeignKey("RequirePaintingListId");
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

            modelBuilder.Entity("VipcoPainting.Models.PaymentCostHistory", b =>
                {
                    b.HasOne("VipcoPainting.Models.PaymentDetail", "PaymentDetail")
                        .WithMany("PaymentCostHistorys")
                        .HasForeignKey("PaymentDetailId")
                        .OnDelete(DeleteBehavior.Cascade);
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

                    b.HasOne("VipcoPainting.Models.ColorMovementStock", "ColorMovementStock")
                        .WithOne("RequisitionMaster")
                        .HasForeignKey("VipcoPainting.Models.RequisitionMaster", "ColorMovementStockId");

                    b.HasOne("VipcoPainting.Models.PaintTaskDetail", "PaintTaskDetail")
                        .WithMany("RequisitionMasters")
                        .HasForeignKey("PaintTaskDetailId");
                });

            modelBuilder.Entity("VipcoPainting.Models.SubPaymentDetail", b =>
                {
                    b.HasOne("VipcoPainting.Models.PaymentCostHistory", "PaymentCostHistory")
                        .WithMany("SubPaymentDetails")
                        .HasForeignKey("PaymentCostHistoryId");

                    b.HasOne("VipcoPainting.Models.PaymentDetail", "PaymentDetail")
                        .WithMany("SubPaymentDetails")
                        .HasForeignKey("PaymentDetailId");

                    b.HasOne("VipcoPainting.Models.SubPaymentMaster", "SubPaymentMaster")
                        .WithMany("SubPaymentDetails")
                        .HasForeignKey("SubPaymentMasterId");
                });

            modelBuilder.Entity("VipcoPainting.Models.SubPaymentMaster", b =>
                {
                    b.HasOne("VipcoPainting.Models.PaintTeam", "PaintTeam")
                        .WithMany("SubPaymentMasters")
                        .HasForeignKey("PaintTeamId");

                    b.HasOne("VipcoPainting.Models.SubPaymentMaster", "PrecedingSubPayment")
                        .WithMany()
                        .HasForeignKey("PrecedingSubPaymentId");
                });
#pragma warning restore 612, 618
        }
    }
}
