using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Interfaces;
using TrackMS.WebAPI.Features.Auth;
using TrackMS.WebAPI.Features.Buildings;
using TrackMS.WebAPI.Features.GeoZones;
using TrackMS.WebAPI.Features.IdentityManagement;
using TrackMS.WebAPI.Features.Users;
using TrackMS.WebAPI.Features.Vehicles;
using TrackMS.WebAPI.Filters;
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
        builder.Services.AddScoped<PermissionsService>();
        builder.Services.AddScoped<RolesService>();
        builder.Services.AddScoped<UsersService>();
        builder.Services.AddScoped<AuthService>();

        builder.Services.AddScoped<BuildingsService>();
        builder.Services.AddScoped<VehiclesService>();
        builder.Services.AddScoped<GeoZonesService>();

        builder.Services.AddScoped<ICrudService<VehicleOperator, string>,
            EfCrudService<VehicleOperator, string>>();

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<DomainExceptionFilterAttribute>();
        });

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
                        Name = "Nikita Vasilenko",
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


        app.Run();
    }

}