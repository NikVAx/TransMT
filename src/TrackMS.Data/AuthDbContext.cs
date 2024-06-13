using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackMS.Domain.Entities;

namespace TrackMS.Data;

public class AuthDbContext 
    : IdentityDbContext<User, Role, string>
{
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Entity<User>(u => 
        {
            u.HasMany<Session>().WithOne().HasForeignKey(x => x.UserId);
        });

        builder.Entity<Role>(r =>
        {
            r.HasMany<User>().WithMany(x => x.Roles).UsingEntity<IdentityUserRole<string>>();
            r.HasIndex(x => x.Name).IsUnique();
            r.HasMany(x => x.Permissions).WithMany();
        });

        base.OnModelCreating(builder);
    }
}
