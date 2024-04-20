namespace TrackMS.Domain.Entities;

public class LocationStamp
{
    public string DeviceId { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
