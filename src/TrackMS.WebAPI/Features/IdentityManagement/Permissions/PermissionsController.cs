using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMS.WebAPI.Features.IdentityManagement.Permissions;
using TrackMS.WebAPI.Features.IdentityManagement.Permissions.DTO;

namespace TrackMS.WebAPI.Features.IdentityManagement;

[Route("api/[controller]")]
[ApiController]
public class PermissionsController : ControllerBase
{
    private readonly PermissionsService _permissionsService;

    public PermissionsController(PermissionsService permissionsService)
    {
        _permissionsService = permissionsService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<GetPermissionDto>>> GetAllPermissions()
    {
        return Ok(await _permissionsService.GetPermissionsAsync());
    }
}
