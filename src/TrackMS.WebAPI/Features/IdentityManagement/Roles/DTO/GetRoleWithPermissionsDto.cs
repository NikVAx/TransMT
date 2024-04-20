using TrackMS.WebAPI.Features.IdentityManagement.Permissions.DTO;

namespace TrackMS.WebAPI.Features.IdentityManagement.DTO;

public class GetRoleWithPermissionsDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<GetPermissionDto> Permissions { get; set; }
}
