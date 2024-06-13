using TrackMS.WebAPI.Features.Tracking;

namespace TrackMS.WebAPI.Features.Reports.DTO;

class DurationInStatusForDeviceDto : DurationInStatusDto
{
    public string DeviceId { get; set; }
}
