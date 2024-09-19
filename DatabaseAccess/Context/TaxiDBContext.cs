using DatabaseAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Context
{
    public class TaxiDBContext : DbContext
    {
        public TaxiDBContext(DbContextOptions options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<DriverEntity> Drivers { get; set; }
        public DbSet<AdminEntity> Admins { get; set; }
        public DbSet<RideEntity> Rides { get; set; }
        public DbSet<RatingEntity> Ratings { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AdminEntity>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<AdminEntity>(a => a.UserId);

            modelBuilder.Entity<ClientEntity>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<ClientEntity>(a => a.UserId);

            modelBuilder.Entity<DriverEntity>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<DriverEntity>(a => a.UserId);

            modelBuilder.Entity<RideEntity>()
                .HasOne(a => a.Client)
                .WithMany(c => c.Rides)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RideEntity>()
                .HasOne(a => a.Driver)
                .WithMany(d => d.Rides)
                .HasForeignKey(a => a.DriverId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            modelBuilder.Entity<RideEntity>()
                .Property(r => r.EstimatedRideEnd)
                .IsRequired(false);

            modelBuilder.Entity<RatingEntity>()
                .HasOne(a => a.Ride)
                .WithOne()
                .HasForeignKey<RatingEntity>(a => a.RideId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

        }
    }
}
