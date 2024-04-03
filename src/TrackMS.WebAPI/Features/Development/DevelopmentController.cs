using Microsoft.AspNetCore.Mvc;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.ValueTypes;
using TrackMS.WebAPI.Features.Users;

namespace TrackMS.WebAPI.Features.Development
{
    [Route("api/[controller]")]
    [ApiController]    
    public class DevelopmentController : ControllerBase
    {
        private readonly AuthDbContext _authDbContext;
        private readonly ApplicationDbContext _appDbContext;
        private readonly UsersService _usersService;

        public DevelopmentController(
            AuthDbContext authDbContext, 
            ApplicationDbContext appDbContext,
            UsersService usersService)
        {
            _authDbContext = authDbContext;
            _appDbContext = appDbContext;
            _usersService = usersService;
        }

        [HttpPost("rst")]
        public ActionResult Setup(string keypass)
        {
            if (keypass != "31415926")
            {
                return BadRequest();
            }

            _appDbContext.Database.EnsureDeleted();
            _authDbContext.Database.EnsureDeleted();
            _appDbContext.Database.EnsureCreated();
            _authDbContext.Database.EnsureCreated();


            _appDbContext.GeoZones.AddRange(
                new GeoZone
                {
                    Id = Guid.NewGuid().ToString(),
                    Color = "#FFFFFF",
                    Name = "GeoZone_1",
                    Points = new[]
                    {
                        new GeoPoint(0,0),
                        new GeoPoint(0,1),
                        new GeoPoint(1,1),
                    }
                },
                new GeoZone
                {
                    Id = Guid.NewGuid().ToString(),
                    Color = "#001133",
                    Name = "GeoZone2",
                    Points = new[]
                    {
                        new GeoPoint(2,2),
                        new GeoPoint(2,3),
                        new GeoPoint(3,3),
                    }
                }
            );

            _appDbContext.SaveChanges();

            return Ok();
        }
    }
}
