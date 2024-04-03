using Microsoft.AspNetCore.Identity;

namespace TrackMS.WebAPI.Shared.Settings;

public class SignInSettings : SignInOptions
{
    public SignInSettings()
    {
        RequireConfirmedPhoneNumber = false;
        RequireConfirmedEmail = false;
        RequireConfirmedAccount = false;
    }
}
