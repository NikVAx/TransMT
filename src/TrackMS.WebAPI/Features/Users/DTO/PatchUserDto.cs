namespace TrackMS.WebAPI.Features.Users.DTO;

public class PatchUserDto
{
    public string? Username { get; set; }

    public IEnumerable<string>? Roles { get; set; }
}
