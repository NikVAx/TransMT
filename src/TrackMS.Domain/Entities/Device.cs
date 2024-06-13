using TrackMS.Domain.Interfaces;

namespace TrackMS.Domain.Entities;

public class Device : IEntity<string>
{
    public string Id { get; set; }
    public string VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
}
