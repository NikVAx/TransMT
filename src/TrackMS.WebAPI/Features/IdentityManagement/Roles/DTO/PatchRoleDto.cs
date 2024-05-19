namespace TrackMS.WebAPI.Features.IdentityManagement.Roles.DTO;

public class PatchRoleDto
{
    public string? Name { get; set; }
    public string? Description { get; set; } = "";
    public IEnumerable<string>? Permissions { get; set; }
}
