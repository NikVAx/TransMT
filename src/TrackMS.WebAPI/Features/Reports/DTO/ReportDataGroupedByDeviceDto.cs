using TrackMS.WebAPI.Features.Tracking;

namespace TrackMS.WebAPI.Features.Reports.DTO;

class ReportDataGroupedByDeviceDto
{
    public string DeviceId { get; set; } = null!;
    public string VehicleId { get; set; } = null!;
    public string VehicleType { get; set; } = null!;
    public string VehicleNumber { get; set; } = null!;
    public double Duration { get; set; }
    public List<StatusDuration> Statuses { get; set; } = new List<StatusDuration>();
}
