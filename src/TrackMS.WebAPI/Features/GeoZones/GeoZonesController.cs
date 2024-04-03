using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Shared.DTO;

namespace TrackMS.WebAPI.Features.GeoZones;

[Route("api/[controller]")]
[ApiController]
public class GeoZonesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GeoZonesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> GetGeoZones()
    {
        var items = await _context.GeoZones.ToListAsync();

        return Ok(new PageResponseDto<GeoZone>(items, items.Count, 0, items.Count));
    }
}
