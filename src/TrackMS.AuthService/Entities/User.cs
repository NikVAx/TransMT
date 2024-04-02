using Microsoft.AspNetCore.Identity;

namespace TrackMS.AuthService.Entities;

public class User : IdentityUser
{
    public ICollection<Role> Roles { get; set; }
}
