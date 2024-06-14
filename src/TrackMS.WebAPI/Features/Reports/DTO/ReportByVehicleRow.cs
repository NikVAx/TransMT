using TrackMS.WebAPI.Features.Tracking;

namespace TrackMS.WebAPI.Features.Reports.DTO;

class ReportByVehicleRow : StatusDuration
{
    public string DeviceId { get; set; } = null!;
    public string VehicleType { get; set; } = null!;
    public string VehicleNumber { get; set; } = null!;
    public string VehicleId { get; set; } = null!;
}
