using TrackMS.Domain.Interfaces;

namespace TrackMS.Domain.Entities;

public class VehicleOperator : IEntity<string>
{
    public string Id { get; set; }
}
