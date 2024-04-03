﻿using Microsoft.AspNetCore.Mvc;
using TrackMS.WebAPI.Features.Users.DTO;
using TrackMS.WebAPI.Shared.DTO;

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
    public async Task<ActionResult<GetUserDto>> CreateUser(CreateUserDto createUserDto)
    {
        return await _usersService.CreateUserAsync(createUserDto);
    }

    [HttpGet]
    public async Task<ActionResult<PageResponseDto<GetUserDto>>> GetUsersPage([FromQuery] PageRequestDto getPageDto)
    {
        return await _usersService.GetUsersPageAsync(getPageDto.PageSize, getPageDto.PageIndex);   
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<GetUserWithRolesDto>> GetUserById(string userId)
    {
        return await _usersService.GetUserById(userId);
    }
}