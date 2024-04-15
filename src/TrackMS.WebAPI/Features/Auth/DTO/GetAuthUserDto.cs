using TrackMS.WebAPI.Features.IdentityManagement.DTO;

namespace TrackMS.WebAPI.Features.Auth.DTO;

public class GetAuthUserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public IEnumerable<GetRoleWithShortPermissionsDto> Roles { get; set; }
}
