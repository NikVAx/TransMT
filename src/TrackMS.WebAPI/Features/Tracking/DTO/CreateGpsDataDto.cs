namespace TrackMS.WebAPI.Features.Tracking.DTO;

public class CreateGpsDataDto
{
    public string DeviceId { get; set; }
    public string Status { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
