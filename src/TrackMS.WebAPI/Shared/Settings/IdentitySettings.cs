using Microsoft.AspNetCore.Identity;

namespace TrackMS.WebAPI.Shared.Settings;

public class IdentitySettings
{
    public static Action<IdentityOptions> Default { get; } = options =>
    {
        options.SignIn = new SignInDefaultSettings();
        options.Password = new PasswordDefaultSettings();
        options.ClaimsIdentity = new ClaimsIdentitySettings();
    };
}
