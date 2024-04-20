using Microsoft.EntityFrameworkCore;
using TrackMS.Domain.Entities;
using TrackMS.Domain.ValueTypes;

namespace TrackMS.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Building> Buildings { get; set; }
    public DbSet<GeoZone> GeoZones { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<VehicleOperator> VehicleOperators { get; set; }
    public DbSet<LocationStamp> LocationStamps { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
     
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Building>(building =>
        {
            building.ComplexProperty(y => y.Location);
            building.HasMany<Vehicle>()
                    .WithOne(x => x.StorageArea)
                    .HasForeignKey(x => x.StorageAreaId);
        });

        modelBuilder.Entity<LocationStamp>(locationStamp =>
        {
            locationStamp.HasKey(stamp => new { stamp.DeviceId, stamp.Timestamp });
        });

        modelBuilder.Entity<Device>(device =>
        {
            device.HasOne<Vehicle>()
                  .WithMany()
                  .HasForeignKey(x => x.VehicleId);

            device.HasMany<LocationStamp>()
                  .WithOne()
                  .HasForeignKey(x => x.DeviceId);
        });

        modelBuilder.Entity<GeoZone>(geoZone =>
        {
            geoZone.OwnsMany(point => point.Points, ownedNavigationBuildier =>
            {
                ownedNavigationBuildier.ToJson();
            });
        });

        base.OnModelCreating(modelBuilder);
    }
}
