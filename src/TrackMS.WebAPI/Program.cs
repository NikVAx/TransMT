
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

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);    
        });

        builder.Services.AddScoped<ICrudService<Construction, string>, EfCrudService<Construction, string>>();
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
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        app.Run();
    }
}
