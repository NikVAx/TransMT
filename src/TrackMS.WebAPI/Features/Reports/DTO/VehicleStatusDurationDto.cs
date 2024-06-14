namespace TrackMS.WebAPI.Features.Reports.DTO;

public class VehicleStatusDurationDto
{
    public string VehicleId { get; set; } = null!;
    public string VehicleStatus { get; set; } = null!;
    public long TotalDuration { get; set; }
}
