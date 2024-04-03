using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TrackMS.WebAPI.Shared.Models;

namespace TrackMS.WebAPI.Shared.Settings;

public class SettingActions
{
    private readonly JwtOptions _jwtOptions;

    public SettingActions(IConfiguration configuration, JwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public Action<JwtBearerOptions> JwtOptions
    {
        get => (options) =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                LifetimeValidator = (DateTime? notBefore, DateTime? expires,
                    SecurityToken token, TokenValidationParameters @params) =>
                {
                    if (expires != null)
                    {
                        return expires > DateTime.UtcNow;
                    }
                    return false;
                },
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key))
            };
        };
    }
}
