using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TrackMS.Domain.Entities;
using TrackMS.Domain.ValueTypes;

namespace TrackMS.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<GeoZone> GeoZones { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleOperator> VehicleOperators { get; set; }
    public DbSet<Building> Buildings { get; set; }

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

        modelBuilder.Entity<GeoZone>(geoZone =>
        {
            geoZone.OwnsMany<GeoPoint>(point => point.Points, ownedNavigationBuildier =>
            {
                ownedNavigationBuildier.ToJson();
            });
        });


        base.OnModelCreating(modelBuilder);
    }
}
