using TrackMS.WebAPI.Features.Users.DTO;

namespace TrackMS.WebAPI.Features.Auth.DTO;

public class SignInResponseDto
{
    public GetUserWithRolesDto User { get; set; }
    public string AccessToken { get; set; }
}
