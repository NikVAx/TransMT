using TrackMS.Domain.Interfaces;

namespace TrackMS.Domain.Entities;

public class Device : IEntity<string>
{
    public string Id { get; set; } = null!;
    public string VehicleId { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
}
