using TrackMS.Domain.Interfaces;

namespace TrackMS.Domain.Entities;

public class Vehicle : IEntity<string>
{
    public string Id { get; set; }
    public string Number { get; set; }
    public string Type { get; set; }
    public string OperatingStatus { get; set; }
    public string StorageAreaId { get; set; }
    public Building StorageArea { get; set; }
}
