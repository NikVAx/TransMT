using Microsoft.AspNetCore.Identity;

namespace TrackMS.WebAPI.Shared.Settings;

public class PasswordSettings : PasswordOptions
{
    public PasswordSettings()
    {
        RequiredLength = 5;
        RequireDigit = false;
        RequireNonAlphanumeric = false;
        RequireUppercase = false;
        RequireLowercase = false;
        RequiredUniqueChars = 1;
    }
}
