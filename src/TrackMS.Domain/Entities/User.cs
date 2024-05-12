using Microsoft.AspNetCore.Identity;

namespace TrackMS.Domain.Entities;

public class User : IdentityUser
{
    public ICollection<Role> Roles { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
}
