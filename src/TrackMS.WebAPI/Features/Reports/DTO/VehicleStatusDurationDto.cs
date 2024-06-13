namespace TrackMS.WebAPI.Features.Reports.DTO;

public class VehicleStatusDurationDto
{
    public string VehicleId { get; set; }
    public string VehicleStatus { get; set; }
    public long TotalDuration { get; set; }
}
