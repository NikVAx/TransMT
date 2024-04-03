using Microsoft.AspNetCore.Identity;

using TrackMS.WebAPI.Shared.Models;

namespace TrackMS.WebAPI.Shared.Settings;

public class ClaimsIdentitySettings : ClaimsIdentityOptions
{
    public ClaimsIdentitySettings()
    {
        UserIdClaimType = AuthClaimTypes.UserId;
        EmailClaimType = AuthClaimTypes.Email;
        UserNameClaimType = AuthClaimTypes.UserName;
        RoleClaimType = AuthClaimTypes.Role;
    }
}
