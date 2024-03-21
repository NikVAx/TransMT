
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Abstractions;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Services;

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
        });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                config.GetConnectionString("DefaultAppConnection"));    
        });

        builder.Services.AddScoped<ICrudService<Building, string>, EfCrudService<Building, string>>();
        builder.Services.AddScoped<ICrudService<GeoZone, string>, EfCrudService<GeoZone, string>>();
        builder.Services.AddScoped<ICrudService<Vehicle, string>, EfCrudService<Vehicle, string>>();
        builder.Services.AddScoped<ICrudService<VehicleOperator, string>, EfCrudService<VehicleOperator, string>>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();

        app.MapControllers();

        using(var scope = app.Services.CreateScope())
        {
            var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

            appDbContext.Database.EnsureDeleted();
            authDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();
            authDbContext.Database.EnsureCreated();
        }

        app.Run();
    }
}
