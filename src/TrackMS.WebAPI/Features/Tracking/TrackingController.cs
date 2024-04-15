using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    [HttpPost]
    public async Task<ActionResult> PostGPS(GpsData data)
    {
        if(data == null)
            return BadRequest();

        return Ok(data);
    }
}
