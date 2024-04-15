namespace TrackMS.WebAPI.Features.IdentityManagement.DTO;

public class GetRoleWithPermissionsDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<GetPermissionDto> Permissions { get; set; }
}
