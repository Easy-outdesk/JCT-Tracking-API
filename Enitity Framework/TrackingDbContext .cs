using JCT_Tracking_Api.Models;
using Microsoft.EntityFrameworkCore;
using Vessel_Tracking_Api.Models;

namespace Vessel_Tracking_Api.Enitity_Framework
{
public class TrackingDbContext : DbContext
    {
        public TrackingDbContext(DbContextOptions<TrackingDbContext> options) : base(options) { }

        public DbSet<VesselSchedule> VesselSchedules { get; set; }

        public DbSet<BlDetail> BlDetails { get; set; }
        public DbSet<ContainerDetail> ContainerDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VesselSchedule>()
                .HasNoKey()
                .ToView("NCT_VIEW_WEB_VESSEL_INFO", "tos_usr");

            modelBuilder.Entity<BlDetail>()
                 .HasKey(b => b.BL_KEY);

            modelBuilder.Entity<ContainerDetail>()
                .HasKey(c => new { c.CONTAINER_NBR, c.BL_KEY });

            modelBuilder.Entity<BlDetail>()
                .HasMany(b => b.Containers)
                .WithOne(c => c.BLDetail)
                .HasForeignKey(c => c.BL_KEY);
        }
    }
}
