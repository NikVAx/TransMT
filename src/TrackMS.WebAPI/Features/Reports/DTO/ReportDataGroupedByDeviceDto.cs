using TrackMS.WebAPI.Features.Tracking;

namespace TrackMS.WebAPI.Features.Reports.DTO;

class ReportDataGroupedByDeviceDto
{
    public string DeviceId { get; set; }
    public double TotalDuration { get; set; }
    public List<DurationInStatusDto> GroupsByStatus { get; set; }
}
