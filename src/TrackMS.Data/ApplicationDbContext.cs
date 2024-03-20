using Microsoft.EntityFrameworkCore;
using TrackMS.Domain.Entities;

namespace TrackMS.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<GeoZone> GeoZones { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleOperator> VehicleOperators { get; set; }
    public DbSet<Construction> Constructions { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
     
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Construction>(x =>
        {
            x.ComplexProperty(y => y.Location);
        });

        modelBuilder
            .Entity<Construction>()
            .HasMany<Vehicle>()
            .WithOne()
            .HasForeignKey(x => x.StorageAreaId);

        base.OnModelCreating(modelBuilder);
    }
}
