using TrackMS.Domain.ValueTypes;

namespace TrackMS.WebAPI.Features.Buildings.DTO;

public class GetBuildingDto
{
    public string Id { get; set; }
    public string Address { get; set; }
    public GeoPoint Location { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
}
