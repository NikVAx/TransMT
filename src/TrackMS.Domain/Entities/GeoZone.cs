using TrackMS.Domain.Interfaces;

namespace TrackMS.Domain.Entities;

public class GeoZone : IEntity<string>
{
    public string Id { get; set; }
    public string Name { get; set; }
}
