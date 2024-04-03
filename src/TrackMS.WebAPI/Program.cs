using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Interfaces;
using TrackMS.Domain.ValueTypes;
using TrackMS.WebAPI.Features.Auth;
using TrackMS.WebAPI.Features.Buildings;
using TrackMS.WebAPI.Features.Roles;
using TrackMS.WebAPI.Features.Users;
using TrackMS.WebAPI.Features.Vehicles;
using TrackMS.WebAPI.Services;
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

        Console.WriteLine(JsonSerializer.Serialize(jwtOptions));

        var options = new SettingActions(config, jwtOptions);

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

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddSingleton(jwtOptions);

        builder.Services.AddScoped<JwtService>();
        builder.Services.AddScoped<BuildingsService>();
        builder.Services.AddScoped<VehiclesService>();
        builder.Services.AddScoped<UsersService>();
        builder.Services.AddScoped<RolesService>();
        builder.Services.AddScoped<AuthService>();

        builder.Services.AddScoped<ICrudService<GeoZone, string>, EfCrudService<GeoZone, string>>();
        builder.Services.AddScoped<ICrudService<VehicleOperator, string>, EfCrudService<VehicleOperator, string>>();

        builder.Services.AddRouting(options => 
        {
            options.LowercaseUrls = true;
        });

        builder.Services
            .AddIdentityCore<User>(options =>
            {
                options.SignIn = new SignInSettings();
                options.Password = new PasswordSettings();
                options.ClaimsIdentity = new ClaimsIdentitySettings();
            })
            .AddRoles<Role>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<UserStore<User, Role, AuthDbContext, string>>()
            .AddRoleStore<RoleStore<Role, AuthDbContext, string>>()
            .AddUserManager<UserManager<User>>()
            .AddSignInManager<SignInManager<User>>();

        builder.Services.AddAuthorization();
        builder.Services.AddCors();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options.JwtOptions);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(
            options => 
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TrackMS API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "nik.vasilenko1203@gmail.com",
                        Name = "Nikita Vasilenko Alexsandrovich 2",
                    },
                    Description = ""
                });

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {{
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                Array.Empty<string>()
                }});

                options.DescribeAllParametersInCamelCase();
            }
        );

        var app = builder.Build();

        app.UseCors(options =>
        {
            options.AllowAnyOrigin();
            options.AllowAnyMethod();
            options.AllowAnyHeader();
        });

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthentication();

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

            //var usersService = scope.ServiceProvider.GetRequiredService<UsersService>();
            //
            //usersService.CreateUserAsync(
            //    new Features.Users.DTO.CreateUserDto
            //    {
            //        Email = "admin@vkusnuts.online",
            //        Username = "admin",
            //        Password = "admin"
            //    }).Wait();
            //
            //appDbContext.GeoZones.AddRange(
            //    new GeoZone
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Color = "#FFFFFF",
            //        Name = "GeoZone_1",
            //        Points = new[]
            //        {
            //            new GeoPoint(0,0),
            //            new GeoPoint(0,1),
            //            new GeoPoint(1,1),
            //        }
            //    },
            //    new GeoZone
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Color = "#001133",
            //        Name = "GeoZone2",
            //        Points = new[]
            //        {
            //            new GeoPoint(2,2),
            //            new GeoPoint(2,3),
            //            new GeoPoint(3,3),
            //        }
            //    }
            //);

            //appDbContext.SaveChanges();
        }

        app.Run();
    }

}