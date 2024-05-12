using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Exceptions;
using TrackMS.WebAPI.Features.Devices;
using TrackMS.WebAPI.Features.IdentityManagement.Roles;
using TrackMS.WebAPI.Features.Users.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Extensions;

namespace TrackMS.WebAPI.Features.Users;

[Authorize]
public class UsersService
{
    private readonly UserManager<User> _userManager;
    private readonly RolesService _rolesService;
    private readonly IMapper _mapper;

    public UsersService(UserManager<User> userManager,
        RolesService rolesService,
        IMapper mapper)
    {
        _userManager = userManager;
        _rolesService = rolesService;
        _mapper = mapper;
    }

    public async Task<GetUserWithRolesDto> EditUserByIdAsync(string id, PatchUserDto patchDto,
        CancellationToken cancellation = default)
    {
        var userWithRoles = await _userManager.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (userWithRoles == null)
        {
            throw new NotFoundException();
        }

        userWithRoles.UserName = patchDto.Username ?? userWithRoles.UserName;

        if (patchDto.Roles != null)
        {
            userWithRoles.Roles = patchDto.Roles
                .Select(name => new Role { Name = name }).ToList();
        };

        return _mapper.Map<GetUserWithRolesDto>(userWithRoles);
    }

    public async Task<GetUserWithRolesDto> CreateUserAsync(CreateUserDto createUserDto, 
        CancellationToken cancellationToken = default)
    {
        var roleModels = await _rolesService.GetRoleModelsByNamesAsync(createUserDto.Roles);

        User user = new User
        {
            Email = createUserDto.Email,
            UserName = createUserDto.Username,
            Roles =  roleModels,
        };

        var signUpResult = await _userManager.CreateAsync(user, createUserDto.Password);

        if(signUpResult.Succeeded)
        {
            return _mapper.Map<GetUserWithRolesDto>(user);
        }
        else
        {
            throw new Exception("Create account failed: " + JsonSerializer.Serialize(signUpResult));
        }
    }

    public async Task<PageResponseDto<GetUserDto>> GetUsersPageAsync(int pageSize, int pageIndex, 
        CancellationToken cancellationToken = default)
    {
        var count = await _userManager.Users.CountAsync();

        var users = await _userManager.Users
            .OrderBy(x => x.Id)
            .GetPage(pageSize, pageIndex)
            .Select(user => _mapper.Map<GetUserDto>(user))
            .ToListAsync();

        return new PageResponseDto<GetUserDto>(users, pageSize, pageIndex, count);
    }

    public async Task<GetUserWithRolesDto> GetUserById(string id)
    {
        var user = await _userManager.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id == id);

        if(user == null)
        {
            throw new NotFoundException();
        }

        return _mapper.Map<GetUserWithRolesDto>(user);
    }

    public async Task<User> GetUserModelByUserName(string userName)
    {
        var normilizedUserName = _userManager.NormalizeName(userName);

        var user = await _userManager.Users
            .Include(x => x.Roles)
                .ThenInclude(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.NormalizedUserName == normilizedUserName);

        if(user == null)
        {
            throw new NotFoundException();
        }

        return user;
    }

    public async Task DeleteUserByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        int count = await _userManager.Users
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if(count == 0)
        {
            throw new NotFoundException();
        }
    }

    public async Task DeleteManyUsersAsync(DeleteManyDto<string> deleteDto,
        CancellationToken cancellationToken = default)
    {
        int count = await _userManager.Users
            .Where(x => deleteDto.Keys.Contains(x.Id))
            .ExecuteDeleteAsync(cancellationToken);

        if(count != deleteDto.Keys.Count())
        {
            throw new Exception("Partial Deletion");
        }
    }
}
