using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackMS.WebAPI.Features.Users.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Models;

namespace TrackMS.WebAPI.Features.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost]
    [Authorize(policy: PermissionKeys.CanCreateUser)]
    public async Task<ActionResult<GetUserWithRolesDto>> CreateUser(CreateUserDto createUserDto)
    {
        return await _usersService.CreateUserAsync(createUserDto);
    }

    [HttpGet]
    [Authorize(policy: PermissionKeys.CanReadUser)]
    public async Task<ActionResult<PageResponseDto<GetUserDto>>> GetUsersPage([FromQuery] PageRequestDto getPageDto)
    {
        return await _usersService.GetUsersPageAsync(getPageDto.PageSize, getPageDto.PageIndex);   
    }

    [HttpGet("{userId}")]
    [Authorize(policy: PermissionKeys.CanReadUser)]
    public async Task<ActionResult<GetUserWithRolesDto>> GetUserById(string userId)
    {
        return await _usersService.GetUserById(userId);
    }

    [HttpPatch("{userId}")]
    public async Task<ActionResult<GetUserWithRolesDto>> EditUserById(string userId, PatchUserDto dto)
    {
        return await _usersService.EditUserByIdAsync(userId, dto);
    }

    [HttpDelete("{id}")]
    [Authorize(policy: PermissionKeys.CanDeleteUser)]
    public async Task<ActionResult> Delete(string id)
    {
        await _usersService.DeleteUserByIdAsync(id);

        return NoContent();
    }

    [HttpDelete]
    [Authorize(policy: PermissionKeys.CanDeleteUser)]
    public async Task<ActionResult> Delete(DeleteManyDto<string> deleteManyDto)
    {
        await _usersService.DeleteManyUsersAsync(deleteManyDto);

        return NoContent();
    }
}
