using TrackMS.Domain.ValueTypes;

namespace TrackMS.WebAPI.Features.GeoZones.DTO;

public class PatchGeoZoneDto
{
    public string? Name { get; set; }
    public string? Color { get; set; }

    public ICollection<GeoPoint>? Points { get; set; }
}
