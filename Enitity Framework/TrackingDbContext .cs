using JCT_Tracking_Api.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
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
            // VesselSchedule view
            modelBuilder.Entity<VesselSchedule>()
                .HasNoKey()
                .ToView("NCT_VIEW_WEB_VESSEL_INFO", "TOS_USR");

            // ContainerDetail view
            modelBuilder.Entity<ContainerDetail>(entity =>
            {
                entity.ToView("NCT_VIEW_WEB_CONTAINER_INFO", "TOS_USR");

                entity.HasKey(e => new { e.CONTAINER_NUMBER, e.BL_NBR });

                entity.HasOne(c => c.BLDetail)
                      .WithMany(b => b.Containers)
                      .HasForeignKey(c => c.BL_NBR)
                      .HasPrincipalKey(b => b.BL_KEY);
            });

            // BlDetail view
            modelBuilder.Entity<BlDetail>(entity =>
            {
                entity.ToView("NCT_MOBAPP_BL_DETAIL_VW", "TOS_USR");
                entity.HasKey(b => b.BL_KEY);
            });

            modelBuilder.Entity<ContainerDetail>()
            .Property(c => c.TARE_WEIGHT)
            .HasPrecision(18, 3);
        }
    }
}
