using Microsoft.AspNetCore.Mvc;
using TrackMS.WebAPI.Features.IdentityManagement.Roles.DTO;
using TrackMS.WebAPI.Features.Roles.IdentityManagement.DTO;
using TrackMS.WebAPI.Shared.DTO;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using TrackMS.WebAPI.Shared.Models;

namespace TrackMS.WebAPI.Features.IdentityManagement.Roles;

[SwaggerTag]
[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly RolesService _rolesService;

    public RolesController(RolesService rolesService)
    {
        _rolesService = rolesService;
    }


    [HttpPost]
    [Authorize(policy: PermissionKeys.CanCreateRole)]
    public async Task<ActionResult<GetRoleWithShortPermissionsDto>> CreateUserRole(CreateRoleDto createRoleDto)
    {
        return await _rolesService.CreateRoleAsync(createRoleDto);
    }

    [HttpDelete]
    [Authorize(policy: PermissionKeys.CanDeleteRole)]
    public async Task<ActionResult> DeleteManyRoles(DeleteManyDto<string> deleteManyDto)
    {
        await _rolesService.DeleteManyRolesAsync(deleteManyDto);
        
        return NoContent();
    }

    [HttpGet]
    [Authorize(policy: PermissionKeys.CanReadRole)]
    public async Task<ActionResult<PageResponseDto<GetRoleDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        return await _rolesService.GetRolesPageAsync(getPageDto.PageSize, getPageDto.PageIndex);
    }

    [HttpGet("{id}")]
    [Authorize(policy: PermissionKeys.CanReadRole)]
    public async Task<ActionResult<GetRoleWithShortPermissionsDto>> Get(string id)
    {
        return Ok(await _rolesService.GetRoleByIdAsync(id));
    }

    [HttpDelete("{id}")]
    [Authorize(policy: PermissionKeys.CanDeleteRole)]
    public async Task<ActionResult> Delete(string id)
    {
        await _rolesService.DeleteRoleByIdAsync(id);

        return NoContent();
    }

    [HttpPatch("{id}")]
    [Authorize(policy: PermissionKeys.CanUpdateRole)]
    public async Task<ActionResult<GetRoleDto>> Patch(string id, [FromBody] PatchRoleDto dto)
    {
        await _rolesService.EditRoleByIdAsync(id, dto);
        
        return Ok(await _rolesService.GetRoleByIdAsync(id));
    }

}
