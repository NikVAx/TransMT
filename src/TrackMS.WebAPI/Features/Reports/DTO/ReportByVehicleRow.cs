using TrackMS.WebAPI.Features.Tracking;

namespace TrackMS.WebAPI.Features.Reports.DTO;

class ReportByVehicleRow : StatusDuration
{
    public string DeviceId { get; set; } = null!;
}
