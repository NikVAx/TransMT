using Microsoft.AspNetCore.Mvc;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.ValueTypes;
using TrackMS.WebAPI.Features.IdentityManagement;
using TrackMS.WebAPI.Features.IdentityManagement.DTO;
using TrackMS.WebAPI.Features.Users;
using TrackMS.WebAPI.Shared.Models;

namespace TrackMS.WebAPI.Features.Development
{
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
                        Points =
                        [
                            new GeoPoint(0,0),
                            new GeoPoint(0,1),
                            new GeoPoint(1,1),
                        ]
                    },
                    new GeoZone
                    {
                        Id = Guid.NewGuid().ToString(),
                        Color = "#001133",
                        Name = "GeoZone2",
                        Points =
                        [
                            new GeoPoint(2,2),
                            new GeoPoint(2,3),
                            new GeoPoint(3,3),
                        ]
                    }
                );

                _appDbContext.SaveChanges();
            }

            if (forAuth)
            {
                _authDbContext.Database.EnsureDeleted();
                _authDbContext.Database.EnsureCreated();

                var permissions = Enum.GetValues<ApiPermissions>()
                    .Select(permission => Permissions.Create(permission))
                    .ToList();

                var identityPerms = permissions
                    .Where(x => x.Name.EndsWith("User") || x.Name.EndsWith("Role"))
                    .ToList();

                _authDbContext.Permissions.AddRange(permissions);
                _authDbContext.SaveChanges();
                _authDbContext.ChangeTracker.Clear();

                var adminRole = await _rolesService.CreateRoleAsync(new CreateRoleDto
                {
                    Name = "admin",
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
                            adminRole.Name
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
}
