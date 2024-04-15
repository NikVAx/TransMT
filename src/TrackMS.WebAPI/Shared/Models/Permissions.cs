using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.IdentityManagement;

namespace TrackMS.WebAPI.Shared.Models;

public class Permissions
{
    public static Permission Create(ApiPermissions key, string description = "")
    {
        return new Permission(key.ToString(), key.ToString(), description);
    }
}
