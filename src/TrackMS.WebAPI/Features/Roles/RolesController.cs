using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackMS.AuthService.Entities;
using TrackMS.WebAPI.Features.Roles.DTO;
using TrackMS.WebAPI.Shared.DTO;

namespace TrackMS.WebAPI.Features.Roles;



[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;


    public RolesController(SignInManager<User> signInManager,
        UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    
    [HttpPost]
    public async Task<ActionResult<GetRoleDto>> CreateUserRole(CreateRoleDto dto)
    {
        
    }

    [HttpGet]
    public async Task<ActionResult<PageResponseDto<GetRoleDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        var query = _roleManager.Roles;

        var items = await query
            .Skip(getPageDto.PageSize * getPageDto.PageIndex)
            .Take(getPageDto.PageSize)
            .Select(userRole => new GetRoleDto
            {
                Id = userRole.Id,
                Name = userRole.Name!,
            })
            .ToListAsync();

        var count = await _userManager.Users.CountAsync();

        return new PageResponseDto<GetRoleDto>(items, getPageDto.PageSize, getPageDto.PageIndex, count);
    }
}
