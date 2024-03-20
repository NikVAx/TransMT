using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackMS.AuthService.Entities;

namespace TrackMS.Data;

public class AuthDbContext 
    : IdentityDbContext<User, UserRole, string>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasMany<Session>()
            .WithOne()
            .HasForeignKey(x => x.UserId);

        base.OnModelCreating(modelBuilder);
    }
}
