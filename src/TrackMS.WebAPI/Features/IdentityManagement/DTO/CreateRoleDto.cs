namespace TrackMS.WebAPI.Features.IdentityManagement.DTO;

public class CreateRoleDto
{
    public string Name { get; set; }
    public IEnumerable<string> Permissions { get; set; }
}
