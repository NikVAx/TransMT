using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Shared.Models;

namespace TrackMS.WebAPI.Features.Auth;

public class JwtService
{
    private readonly JwtOptions _jwtOptions;
    private SignInManager<User> _signInManager;

    public JwtService(
        JwtOptions jwtOptions,
        SignInManager<User> signInManager)
    {
        _jwtOptions = jwtOptions;
        _signInManager = signInManager;
    }

    public async Task<string> CreateAccessTokenAsync(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var roleNames = await _signInManager.UserManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(AuthClaimTypes.UserId, user.Id.ToString()),
            new Claim(AuthClaimTypes.UserName, user.UserName!),
            new Claim(AuthClaimTypes.Email, user.Email!),
        };

        claims.AddRange(roleNames.Select(roleName => new Claim(ClaimTypes.Role, roleName)));

        var now = DateTime.Now;
        var lifetime = Convert.ToInt64(_jwtOptions.LifetimeInSeconds);

        var token = new JwtSecurityToken(
          issuer: _jwtOptions.Issuer,
          audience: _jwtOptions.Audience,
          claims:  claims,
          notBefore: now,
          expires: now.AddSeconds(lifetime),
          signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
