namespace TrackMS.Domain.Entities;

public class LocationStamp
{
    public string DeviceId { get; set; }
    public string VehicleId { get; set; }
    public string? OperatorId { get; set; }
    public string VehicleStatus {  get; set; } 
    public double Lat { get; set; }
    public double Lng { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
