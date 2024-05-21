using TrackMS.Domain.Interfaces;
using NetTopologySuite.Geometries;

namespace TrackMS.Domain.Entities;

public class GeoZone : IEntity<string>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public string Type { get; set; }

    public Polygon Points { get; set; }
}
