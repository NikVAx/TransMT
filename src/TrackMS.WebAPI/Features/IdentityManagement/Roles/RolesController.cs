using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.IdentityManagement.Roles.DTO;
using TrackMS.WebAPI.Features.Roles.IdentityManagement.DTO;
using TrackMS.WebAPI.Shared.DTO;
using Swashbuckle.AspNetCore.Annotations;

namespace TrackMS.WebAPI.Features.IdentityManagement.Roles;

[SwaggerTag]
[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RolesService _rolesService;

    public RolesController(SignInManager<User> signInManager,
        UserManager<User> userManager, RoleManager<Role> roleManager, RolesService rolesService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _rolesService = rolesService;
    }


    [HttpPost]
    public async Task<ActionResult<GetRoleWithShortPermissionsDto>> CreateUserRole(CreateRoleDto createRoleDto)
    {
        return await _rolesService.CreateRoleAsync(createRoleDto);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteManyRoles(DeleteManyDto<string> deleteManyDto)
    {
        await _rolesService.DeleteManyRolesAsync(deleteManyDto);
        
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<PageResponseDto<GetRoleDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        return await _rolesService.GetRolesPageAsync(getPageDto.PageSize, getPageDto.PageIndex);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetRoleWithShortPermissionsDto>> Get(string id)
    {
        return Ok(await _rolesService.GetRoleByIdAsync(id));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        await _rolesService.DeleteRoleByIdAsync(id);

        return NoContent();
    }
}
