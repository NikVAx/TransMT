using TrackMS.Domain.Abstractions;

namespace TrackMS.Domain.Entities;

public class Vehicle : IEntity<string>
{
    public string Id { get; set; }
}
