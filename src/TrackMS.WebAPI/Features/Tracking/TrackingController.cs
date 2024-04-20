using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Entities;

namespace TrackMS.WebAPI.Features.Tracking;

public class GpsData
{
    public string Id { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public class TrackingController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TrackingController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult> PostGPS(GpsData data)
    {
        _context.LocationStamps.Add(new LocationStamp
        {
            DeviceId = data.Id,
            Lat = data.Lat,
            Lng = data.Lon,
            Timestamp = data.Timestamp,
        });


        await _context.SaveChangesAsync();

        return Ok(data);
    }

    [HttpGet("{deviceId}")]
    public async Task<ActionResult> GetTracking(string deviceId, DateTimeOffset? from, DateTimeOffset? to)
    {
        var items = await _context.LocationStamps.Where(x => x.DeviceId == deviceId)
            .Where(x => (!from.HasValue || from.Value <= x.Timestamp) &&
                (!to.HasValue || to.Value >= x.Timestamp))
            .ToListAsync();

        return Ok(items);
    }
}
