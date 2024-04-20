using TrackMS.WebAPI.Features.Users.DTO;

namespace TrackMS.WebAPI.Features.Auth.DTO;

public class SignInResponseDto
{
    public GetAuthUserDto User { get; set; }
    public string AccessToken { get; set; }
}
