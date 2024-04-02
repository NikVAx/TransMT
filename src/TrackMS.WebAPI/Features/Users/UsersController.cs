using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackMS.AuthService.Entities;
using TrackMS.WebAPI.Features.Users.DTO;
using TrackMS.WebAPI.Shared.DTO;

namespace TrackMS.WebAPI.Features.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly UsersService _usersService;

    public UsersController(SignInManager<User> signInManager,
        UserManager<User> userManager, RoleManager<Role> roleManager, UsersService usersService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _usersService = usersService;
    }

    [HttpPost]
    public async Task<ActionResult<GetUserDto>> CreateUser(CreateUserDto createUserDto)
    {
        return await _usersService.CreateUserAsync(createUserDto);
    }

    [HttpGet]
    public async Task<ActionResult<PageResponseDto<GetUserDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        return await _usersService.GetUsersPageAsync(getPageDto.PageSize, getPageDto.PageIndex);   
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<GetUserWithRolesDto>> GetUserById(string userId)
    {
        return await _usersService.GetUserById(userId);
    }
}
