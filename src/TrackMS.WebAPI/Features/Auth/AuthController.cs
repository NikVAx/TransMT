using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.Auth.DTO;
using TrackMS.WebAPI.Shared.Models;

namespace TrackMS.WebAPI.Features.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    [HttpPost("sign-in")]
    public async Task<ActionResult<SignInResponseDto>> SignIn(SignInByPasswordDto signInDto)
    {
        return await _authService.SignInByPassword(signInDto);
    }

    [HttpGet("{userId}/sessions")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Session>>> GetSessions(string userId)
    {
        if(!User.HasClaim(AuthClaimTypes.UserId, userId))
        {
            return Forbid();
        }
        
        var items = await _authService.GetUsersSessions(userId);

        return Ok(items);
    }
}
