using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.Auth;
using TrackMS.WebAPI.Features.Buildings;
using TrackMS.WebAPI.Features.Devices;
using TrackMS.WebAPI.Features.GeoZones;
using TrackMS.WebAPI.Features.IdentityManagement.Permissions;
using TrackMS.WebAPI.Features.IdentityManagement.Roles;
using TrackMS.WebAPI.Features.Operators;
using TrackMS.WebAPI.Features.Tracking;
using TrackMS.WebAPI.Features.Users;
using TrackMS.WebAPI.Features.Vehicles;
using TrackMS.WebAPI.Filters;
using TrackMS.WebAPI.Shared.Mapping;
using TrackMS.WebAPI.Shared.Models;
using TrackMS.WebAPI.Shared.Settings;

namespace TrackMS.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        var config = builder.Configuration;

        var jwtOptions = builder.Configuration
            .GetSection("Jwt")
            .Get<JwtOptions>();

        if(jwtOptions is null)
        {
            throw new Exception("JwtOptions is not Setup");
        }

        var options = new SettingActions(config, jwtOptions);

        builder.Services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(
                config.GetConnectionString("DefaultAuthConnection"));
            options.EnableDetailedErrors();
        });

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(
            config.GetConnectionString("DefaultAppConnection"));
        dataSourceBuilder.UseNetTopologySuite();
        var dataSource = dataSourceBuilder.Build();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(dataSource, npgsqlOptions => 
                npgsqlOptions.UseNetTopologySuite());
            options.EnableDetailedErrors();
        });

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddSingleton(jwtOptions);

        builder.Services.AddScoped<JwtService>();
        builder.Services.AddScoped<PermissionsService>();
        builder.Services.AddScoped<RolesService>();
        builder.Services.AddScoped<UsersService>();
        builder.Services.AddScoped<AuthService>();

        builder.Services.AddScoped<BuildingsService>();
        builder.Services.AddScoped<VehiclesService>();
        builder.Services.AddScoped<OperatorsService>();
        builder.Services.AddScoped<DevicesService>();
        builder.Services.AddScoped<GeoZonesService>();

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<DomainExceptionFilterAttribute>();
        });

        builder.Services.AddRouting(options => 
        {
            options.LowercaseUrls = true;
        });

        builder.Services
            .AddIdentityCore<User>(IdentitySettings.Default)
            .AddRoles<Role>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<UserStore<User, Role, AuthDbContext, string>>()
            .AddRoleStore<RoleStore<Role, AuthDbContext, string>>()
            .AddUserManager<UserManager<User>>()
            .AddSignInManager<SignInManager<User>>();

        var authorizationBuilder = builder.Services.AddAuthorizationBuilder();

        var permissions =  Permissions.GetPermissions();

        foreach(var permission in permissions)
        {
            authorizationBuilder.AddPolicy(permission.Id, policyBuilder 
                => policyBuilder.RequireClaim(AuthClaimTypes.Permission, permission.Id));
        }

        builder.Services.AddCors();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options.JwtOptions);
        builder.Services.AddSignalR();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(SwaggerGenSettings.Default);

        var app = builder.Build();

        app.MapHub<TrackingHub>("/hubs/tracking");

        app.UseCors(options =>
        {
            options.AllowAnyMethod();
            options.AllowAnyHeader();
            options.SetIsOriginAllowed(origin => true);
            options.AllowCredentials();
        });

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();


        app.Run();
    }

}