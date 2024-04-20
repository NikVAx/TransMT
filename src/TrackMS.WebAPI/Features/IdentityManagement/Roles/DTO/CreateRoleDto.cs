namespace TrackMS.WebAPI.Features.Roles.IdentityManagement.DTO;

public class CreateRoleDto
{
    public string Name { get; set; }
    public string Description { get; set; } = "";
    public IEnumerable<string> Permissions { get; set; }
}
