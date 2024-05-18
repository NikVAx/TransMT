using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.ValueTypes;
using TrackMS.WebAPI.Features.IdentityManagement.Roles;
using TrackMS.WebAPI.Features.Roles.IdentityManagement.DTO;
using TrackMS.WebAPI.Features.Users;
using TrackMS.WebAPI.Shared.Models;
using TrackMS.WebAPI.Shared.Utils;

namespace TrackMS.WebAPI.Features.Development;

[Route("api/[controller]")]
[ApiController]    
public class DevelopmentController : ControllerBase
{
    private readonly AuthDbContext _authDbContext;
    private readonly ApplicationDbContext _appDbContext;
    private readonly UsersService _usersService;
    private readonly RolesService _rolesService;

    public DevelopmentController(
        AuthDbContext authDbContext, 
        ApplicationDbContext appDbContext,
        UsersService usersService,
        RolesService rolesService)
    {
        _authDbContext = authDbContext;
        _appDbContext = appDbContext;
        _usersService = usersService;
        _rolesService = rolesService;
    }

    [HttpPost("rst")]
    public async Task<ActionResult> Setup(bool forData = true, bool forAuth = true)
    {
        if (forData)
        {
            _appDbContext.Database.EnsureDeleted();
            _appDbContext.Database.EnsureCreated();
            _appDbContext.GeoZones.AddRange(
                new GeoZone
                {
                    Id = Guid.NewGuid().ToString(),
                    Color = "#FFFFFF",
                    Name = "GeoZone_1",
                    Points = GeoUtil.CreatePolygon(
                        [
                        new GeoPoint(10,10),
                        new GeoPoint(10,20),
                        new GeoPoint(5,20),
                        new GeoPoint(20,20),
                    ])
                },
                new GeoZone
                {
                    Id = Guid.NewGuid().ToString(),
                    Color = "#001133",
                    Name = "GeoZone2",
                    Points = GeoUtil.CreatePolygon(
                        [
                        new GeoPoint(0,0),
                        new GeoPoint(0,1),
                        new GeoPoint(1,1),
                        ])
                }
            );

            var building = new Building
            {
                Id = Guid.NewGuid().ToString(),
                Address = "default",
                Location = new NetTopologySuite.Geometries.Point(0, 0),
                Name = "default",
                Type = "default",
            };

            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                Number = "00112233",
                OperatingStatus = "active",
                StorageAreaId = building.Id,
                Type = "default"
            };

            var device = new Device
            {
                Id = "0000-AAAA",
                VehicleId = vehicle.Id,
            };

            _appDbContext.Buildings.Add(building);
            _appDbContext.Vehicles.Add(vehicle);
            _appDbContext.Devices.Add(device);

            _appDbContext.SaveChanges();
        }

        if (forAuth)
        {
            _authDbContext.Database.EnsureDeleted();
            _authDbContext.Database.EnsureCreated();

            var permissions = Permissions.GetPermissions();
                
            var identityPerms = permissions
                .Where(x => x.Name.Contains("USER") || x.Name.Contains("ROLE"))
                .ToList();

            _authDbContext.Permissions.AddRange(permissions);
            _authDbContext.SaveChanges();
            _authDbContext.ChangeTracker.Clear();

            var superUserRole = await _rolesService.CreateRoleAsync(new CreateRoleDto
            {
                Name = "superuser",
                Description = "Пользователь со всеми разрешениями доступными в системе",
                Permissions = permissions.Select(permission => permission.Id)
            });

            var identityManagerRole = await _rolesService.CreateRoleAsync(new CreateRoleDto
            {
                Name = "identity-admin",
                Permissions = identityPerms.Select(permission => permission.Id)
            });

            _authDbContext.ChangeTracker.Clear();

            await _usersService.CreateUserAsync(
                new Users.DTO.CreateUserDto
                {
                    Email = "admin@vkusnuts.online",
                    Username = "admin",
                    Password = "admin",
                    Roles =
                    [
                        superUserRole.Name
                    ]
                });

            await _usersService.CreateUserAsync(
                new Users.DTO.CreateUserDto
                {
                    Email = "staff.manager@vkusnuts.online",
                    Username = "mng-auth",
                    Password = "mng-auth",
                    Roles =
                    [
                        identityManagerRole.Name,
                    ]
                });
        }
        
        return Ok();
    }
}
