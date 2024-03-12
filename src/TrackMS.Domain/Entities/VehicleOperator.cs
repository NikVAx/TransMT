using TrackMS.Domain.Abstractions;

namespace TrackMS.Domain.Entities;

public class VehicleOperator : IEntity<string>
{
    public string Id { get; set; }
}
