using TrackMS.Domain.Interfaces;

namespace TrackMS.Domain.Entities;

public class VehicleOperator : IEntity<string>
{
    public string Id { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
}
