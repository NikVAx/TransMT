using Microsoft.AspNetCore.Identity;

namespace TrackMS.Domain.Entities;

public class Role : IdentityRole
{
    public string Description { get; set; } = "";
    public ICollection<Permission> Permissions { get; set; }
}
