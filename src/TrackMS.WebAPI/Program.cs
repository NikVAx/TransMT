using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackMS.AuthService.Entities;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Interfaces;
using TrackMS.WebAPI.Services;
using TrackMS.WebAPI.Shared.Mapping;

namespace TrackMS.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration;

        builder.Services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(
                config.GetConnectionString("DefaultAuthConnection"));
            options.EnableDetailedErrors();
        });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                config.GetConnectionString("DefaultAppConnection"));
            options.EnableDetailedErrors();
        });

        builder.Services.AddAutoMapper(typeof(AppMappingProfile));

        builder.Services.AddScoped<BuildingsService>();
        builder.Services.AddScoped<VehiclesService>();

        builder.Services.AddScoped<ICrudService<GeoZone, string>, EfCrudService<GeoZone, string>>();
        builder.Services.AddScoped<ICrudService<VehicleOperator, string>, EfCrudService<VehicleOperator, string>>();

        builder.Services.AddRouting(options => 
        {
            options.LowercaseUrls = true;
        });

        builder.Services.AddIdentityCore<User>()
                .AddRoles<Role>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserStore<UserStore<User, Role, AuthDbContext, string>>()
                .AddRoleStore<RoleStore<Role, AuthDbContext, string>>()
                .AddUserManager<UserManager<User>>()
                .AddSignInManager<SignInManager<User>>();

        builder.Services.AddAuthentication();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseCors(options =>
        {
            options.AllowAnyOrigin();
            options.AllowAnyMethod();
            options.AllowAnyHeader();
        });

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();

        app.MapControllers();

        using(var scope = app.Services.CreateScope())
        {
            var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

            //appDbContext.Database.EnsureDeleted();
            //authDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();
            authDbContext.Database.EnsureCreated();
        }

        app.Run();
    }
}
