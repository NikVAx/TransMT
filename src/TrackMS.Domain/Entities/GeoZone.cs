
using TrackMS.Domain.Interfaces;
using TrackMS.Domain.ValueTypes;

namespace TrackMS.Domain.Entities;

public class GeoZone : IEntity<string>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }

    public ICollection<GeoPoint> Points { get; set; }
}
