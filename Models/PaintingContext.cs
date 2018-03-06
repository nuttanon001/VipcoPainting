using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace VipcoPainting.Models
{
    public class PaintingContext : DbContext
    {
        public PaintingContext(DbContextOptions<PaintingContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlastRoom>().ToTable("BlastRoom");
            modelBuilder.Entity<BlastWorkItem>().ToTable("BlastWorkItem");
            modelBuilder.Entity<ColorItem>().ToTable("ColorItem");
            modelBuilder.Entity<ColorMovementStock>().ToTable("ColorMovementStock");
            modelBuilder.Entity<FinishedGoodsMaster>().ToTable("FinishedGoodsMaster");
            modelBuilder.Entity<MovementStockStatus>().ToTable("MovementStockStatus");
            modelBuilder.Entity<PaintTaskDetail>().ToTable("PaintTaskDetail");
            modelBuilder.Entity<PaintTaskMaster>().ToTable("PaintTaskMaster");
            modelBuilder.Entity<PaintTeam>().ToTable("PaintTeam");
            modelBuilder.Entity<PaymentCostHistory>().ToTable("PaymentCostHistory");
            modelBuilder.Entity<PaymentDetail>().ToTable("PaymentDetail");
            modelBuilder.Entity<PaintWorkItem>().ToTable("PaintWorkItem");
            modelBuilder.Entity<Permission>().ToTable("Permission");
            modelBuilder.Entity<ProjectCodeSub>().ToTable("ProjectCodeSub");
            modelBuilder.Entity<RequirePaintingList>().ToTable("RequirePaintingList");
            modelBuilder.Entity<RequirePaintingListHasAttach>().ToTable("RequirePaintingListHasAttach");
            modelBuilder.Entity<RequirePaintingListOption>().ToTable("RequirePaintingListOption");
            modelBuilder.Entity<RequirePaintingMaster>().ToTable("RequirePaintingMaster");
            modelBuilder.Entity<RequisitionMaster>().ToTable("RequisitionMaster");
            modelBuilder.Entity<RequirePaintingMasterHasAttach>().ToTable("RequirePaintingMasterHasAttach");
            modelBuilder.Entity<StandradTime>().ToTable("StandradTime");
            modelBuilder.Entity<SubPaymentDetail>().ToTable("SubPaymentDetail");
            modelBuilder.Entity<SubPaymentMaster>().ToTable("SubPaymentMaster");
            modelBuilder.Entity<SurfaceType>().ToTable("SurfaceType");
        }

        public DbSet<BlastRoom> BlastRooms { get; set; }
        public DbSet<BlastWorkItem> BlastWorkItems { get; set; }
        public DbSet<ColorItem> ColorItems { get; set; }
        public DbSet<ColorMovementStock> ColorMovementStocks { get; set; }
        public DbSet<FinishedGoodsMaster> FinishedGoodsMasters { get; set; }
        public DbSet<MovementStockStatus> MovementStockStatuses { get; set; }
        public DbSet<PaintTaskDetail> PaintTaskDetails { get; set; }
        public DbSet<PaintTaskMaster> PaintTaskMasters { get; set; }
        public DbSet<PaintTeam> PaintTeams { get; set; }
        public DbSet<PaintWorkItem> PaintWorkItems { get; set; }
        public DbSet<PaymentCostHistory> PaymentCostHistorys { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ProjectCodeSub> ProjectCodeSubs { get; set; }
        public DbSet<RequirePaintingList> RequirePaintingLists { get; set; }
        public DbSet<RequirePaintingListHasAttach> RequirePaintingListHasAttachs { get; set; }
        public DbSet<RequirePaintingListOption> RequirePaintingListOptions { get; set; }
        public DbSet<RequirePaintingMaster> RequirePaintingMasters { get; set; }
        public DbSet<RequirePaintingMasterHasAttach> RequirePaintingMasterHasAttachs { get; set; }
        public DbSet<RequisitionMaster> RequisitionMasters { get; set; }
        public DbSet<StandradTime> StandradTimes { get; set; }
        public DbSet<SubPaymentDetail> SubPaymentDetails { get; set; }
        public DbSet<SubPaymentMaster> SubPaymentMasters { get; set; }
        public DbSet<SurfaceType> SurfaceTypes { get; set; }
    }
}
