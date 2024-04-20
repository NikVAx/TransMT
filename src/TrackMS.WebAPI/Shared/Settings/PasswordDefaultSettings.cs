using Microsoft.AspNetCore.Identity;

namespace TrackMS.WebAPI.Shared.Settings;

public class PasswordDefaultSettings : PasswordOptions
{
    public PasswordDefaultSettings()
    {
        RequiredLength = 5;
        RequireDigit = false;
        RequireNonAlphanumeric = false;
        RequireUppercase = false;
        RequireLowercase = false;
        RequiredUniqueChars = 1;
    }
}
