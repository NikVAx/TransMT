using Microsoft.AspNetCore.Identity;

namespace TrackMS.WebAPI.Shared.Settings;

public class SignInDefaultSettings : SignInOptions
{
    public SignInDefaultSettings()
    {
        RequireConfirmedPhoneNumber = false;
        RequireConfirmedEmail = false;
        RequireConfirmedAccount = false;
    }
}
