using Microsoft.AspNetCore.Identity;

namespace TrackMS.Domain.Entities;

public class User : IdentityUser
{
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public string? FirstName { get; set; } = null;
    public string? MiddleName { get; set; } = null;
    public string? LastName { get; set; } = null;
}
