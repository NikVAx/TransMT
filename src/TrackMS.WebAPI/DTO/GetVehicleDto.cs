namespace TrackMS.WebAPI.DTO;

public class GetVehicleDto
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string OperatingStatus { get; set; }
    public string? StorageAreaId { get; set; }
}
