using TrackMS.Domain.ValueTypes;

namespace TrackMS.WebAPI.DTO;

public class PatchVehicleOperatorDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
}
