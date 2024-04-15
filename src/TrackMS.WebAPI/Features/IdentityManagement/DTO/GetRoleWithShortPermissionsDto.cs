namespace TrackMS.WebAPI.Features.IdentityManagement.DTO;

public class GetRoleWithShortPermissionsDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<string> Permissions { get; set; }
}
