using TrackMS.Domain.ValueTypes;

namespace TrackMS.WebAPI.DTO;

public class CreateConstructionDto
{
    public string Address { get; set; }
    public GeoPoint Location { get; set; }
}
