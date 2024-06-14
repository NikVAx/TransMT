namespace TrackMS.Domain.Entities;

public class LocationStamp
{
    public string DeviceId { get; set; } = null!;
    public string VehicleId { get; set; } = null!;
    public string? OperatorId { get; set; } = null;
    public string VehicleStatus {  get; set; } = null!;
    public double Lat { get; set; }
    public double Lng { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
