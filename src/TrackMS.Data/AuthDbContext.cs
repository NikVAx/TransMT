using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackMS.AuthService.Entities;

namespace TrackMS.Data;

public class AuthDbContext 
    : IdentityDbContext<User, Role, string>
{
    public DbSet<Session> Sessions { get; set; }

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(u => {
            u.HasMany<Session>().WithOne().HasForeignKey(x => x.UserId);
        });

        builder
            .Entity<Role>()
            .HasMany<User>()
            .WithMany(x => x.Roles)
            .UsingEntity<IdentityUserRole<string>>();
            
        base.OnModelCreating(builder);
    }
}
