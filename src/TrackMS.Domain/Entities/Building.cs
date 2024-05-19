using NetTopologySuite.Geometries;
using TrackMS.Domain.Interfaces;

namespace TrackMS.Domain.Entities;

public class Building : IEntity<string>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Address { get; set; }
    public Point Location { get; set; }
}
