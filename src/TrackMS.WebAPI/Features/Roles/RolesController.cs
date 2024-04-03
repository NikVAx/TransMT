using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.Roles.DTO;
using TrackMS.WebAPI.Shared.DTO;

namespace TrackMS.WebAPI.Features.Roles;

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
    public async Task<ActionResult<GetRoleDto>> CreateUserRole(CreateRoleDto createRoleDto)
    {
        return await _rolesService.CreateRoleAsync(createRoleDto);
    }

    [HttpGet]
    public async Task<ActionResult<PageResponseDto<GetRoleDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        return await _rolesService.GetRolesPageAsync(getPageDto.PageSize, getPageDto.PageIndex);
    }
}
