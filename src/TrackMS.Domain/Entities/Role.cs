using Microsoft.AspNetCore.Identity;

namespace TrackMS.Domain.Entities;

public class Role : IdentityRole
{
    public ICollection<Permission> Permissions { get; set; }
}
