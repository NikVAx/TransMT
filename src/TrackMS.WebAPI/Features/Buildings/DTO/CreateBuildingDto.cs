using TrackMS.Domain.ValueTypes;

namespace TrackMS.WebAPI.Features.Buildings.DTO;

public class CreateBuildingDto
{
    public string Address { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public GeoPoint Location { get; set; }
}
