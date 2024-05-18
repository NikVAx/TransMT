using TrackMS.Domain.Interfaces;
using TrackMS.Domain.ValueTypes;

namespace TrackMS.Domain.Entities;

public class Construction : IEntity<string>
{
    public string Id { get; set; }
    public GeoPoint Location { get; set; }
    public string Address { get; set; }
}
