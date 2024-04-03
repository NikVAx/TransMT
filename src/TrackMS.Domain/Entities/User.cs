using Microsoft.AspNetCore.Identity;

namespace TrackMS.Domain.Entities;

public class User : IdentityUser
{
    public ICollection<Role> Roles { get; set; }
}
