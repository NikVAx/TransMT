using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.Tracking.DTO;
using TrackMS.Domain.Interfaces;

namespace TrackMS.WebAPI.Features.Tracking;

static class HubMethods
{
    public const string LocationUpdate = "LocationUpdate";
    public const string SetDriveLimits = "SetDriveLimits";
}

public enum LimitType
{
    Begin,
    End
}

public class SetDriveLimitsDto : ILatLng
{
    public LimitType Limit { get; set; }
    public string DeviceId { get; set; } = null!;
    public double Lat { get ; set; }
    public double Lng { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public class TrackingController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<TrackingHub> _hubContext;

    public TrackingController(ApplicationDbContext context, IHubContext<TrackingHub> hubContext)
    {
        _hubContext = hubContext;
        _context = context;
    }

    [HttpPost("limits/{deviceId}")]
    public async Task<ActionResult> UpdateTrackSession(SetDriveLimitsDto dto)
    {
        var device = await _context.Devices
            .FirstOrDefaultAsync(x => x.Id.Equals(dto.DeviceId));

        if(device == null)
        {
            return NotFound();
        }

        var vehicle = _context.Vehicles
            .First(x => x.Id.Equals(device.VehicleId));

        vehicle.OperatingStatus = "Нет";

        var locationStamp = new LocationStamp
        {
            DeviceId = device.Id,
            VehicleId = vehicle.Id,
            VehicleStatus = dto.Limit == LimitType.Begin ? 
                "Начало движения" : 
                "Конец движения",
            Timestamp = DateTime.UtcNow,
            Lat = dto.Lat,
            Lng = dto.Lng,
        };

        _context.LocationStamps.Add(locationStamp);
        _context.Vehicles.Update(vehicle);

        await _context.SaveChangesAsync();
        
        await _hubContext.Clients.All
            .SendAsync(HubMethods.SetDriveLimits, locationStamp);

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> PostGpsData(CreateGpsDataDto data)
    {
        if (data.Status == "Нет")
        {
            return BadRequest();
        }

        var device = await _context.Devices
            .FirstOrDefaultAsync(x => x.Id.Equals(data.DeviceId));

        if (device == null)
        {
            return NotFound();
        }

        var vehicle = _context.Vehicles
            .First(x => x.Id.Equals(device.VehicleId));

        var locationStamp = new LocationStamp
        {
            DeviceId = data.DeviceId,
            VehicleId = device.VehicleId,
            VehicleStatus = data.Status,
            Lat = data.Lat,
            Lng = data.Lng,
            Timestamp = data.Timestamp,
        };
      
        vehicle.OperatingStatus = locationStamp.VehicleStatus;

        _context.LocationStamps.Add(locationStamp);
        _context.Vehicles.Update(vehicle);

        await _context.SaveChangesAsync();
        
        await _hubContext.Clients.All
            .SendAsync(HubMethods.LocationUpdate, locationStamp);

        return Ok();
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
