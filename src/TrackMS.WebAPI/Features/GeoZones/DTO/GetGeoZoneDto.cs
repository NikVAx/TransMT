using TrackMS.Domain.ValueTypes;

namespace TrackMS.WebAPI.Features.GeoZones.DTO;

public class GetGeoZoneDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string Type { get; set; } = null!;

    public ICollection<GeoPoint> Points { get; set; } = new List<GeoPoint>();
}
