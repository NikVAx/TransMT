using TrackMS.WebAPI.Features.Roles.DTO;

namespace TrackMS.WebAPI.Features.Users.DTO;

public class GetUserWithRolesDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public IEnumerable<GetRoleDto> Roles { get; set; }
}
