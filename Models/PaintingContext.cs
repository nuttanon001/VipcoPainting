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
            modelBuilder.Entity<PaintTeam>().ToTable("PaintTeam");
            modelBuilder.Entity<PaintWorkItem>().ToTable("PaintWorkItem");
            modelBuilder.Entity<ProjectCodeSub>().ToTable("ProjectCodeSub");
            modelBuilder.Entity<RequirePaintingList>().ToTable("RequirePaintingList");
            modelBuilder.Entity<RequirePaintingMaster>().ToTable("RequirePaintingMaster");
            modelBuilder.Entity<RequisitionMaster>().ToTable("RequisitionMaster");
            modelBuilder.Entity<StandradTime>().ToTable("StandradTime");
            modelBuilder.Entity<SurfaceType>().ToTable("SurfaceType");
            modelBuilder.Entity<TaskBlastDetail>().ToTable("TaskBlastDetail");
            modelBuilder.Entity<TaskMaster>().ToTable("TaskMaster");
            modelBuilder.Entity<TaskPaintDetail>().ToTable("TaskPaintDetail");
        }

        public DbSet<BlastRoom> BlastRooms { get; set; }
        public DbSet<BlastWorkItem> BlastWorkItems { get; set; }
        public DbSet<ColorItem> ColorItems { get; set; }
        public DbSet<PaintTeam> PaintTeams { get; set; }
        public DbSet<PaintWorkItem> PaintWorkItems { get; set; }
        public DbSet<ProjectCodeSub> ProjectCodeSubs { get; set; }
        public DbSet<RequirePaintingList> RequirePaintingLists { get; set; }
        public DbSet<RequirePaintingMaster> RequirePaintingMasters { get; set; }
        public DbSet<RequisitionMaster> RequisitionMasters { get; set; }
        public DbSet<StandradTime> StandradTimes { get; set; }
        public DbSet<SurfaceType> SurfaceTypes { get; set; }
        public DbSet<TaskBlastDetail> TaskBlastDetails { get; set; }
        public DbSet<TaskMaster> TaskMasters { get; set; }
        public DbSet<TaskPaintDetail> TaskPaintDetails { get; set; }
    }
}
