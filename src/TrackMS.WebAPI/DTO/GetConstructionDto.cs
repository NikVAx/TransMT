using TrackMS.Domain.ValueTypes;

namespace TrackMS.WebAPI.DTO;

public class GetConstructionDto
{
    public string Id { get; set; }
    public string Address { get; set; }
    public GeoPoint Location { get; set; }
}
