namespace TrackMS.WebAPI.Features.Users.DTO;

public class PatchUserDto
{
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }

    public IEnumerable<string>? Roles { get; set; }
}
