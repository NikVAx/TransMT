using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.Users.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Extensions;

namespace TrackMS.WebAPI.Features.Users;

[Authorize]
public class UsersService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UsersService(UserManager<User> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<GetUserDto> CreateUserAsync(CreateUserDto createUserDto, 
        CancellationToken cancellationToken = default)
    {
        User user = new User
        {
            Email = createUserDto.Email,
            UserName = createUserDto.Username,
        };

        var signUpResult = await _userManager.CreateAsync(user, createUserDto.Password);

        if(signUpResult.Succeeded)
        {
            return _mapper.Map<GetUserDto>(user);
        }
        else
        {
            throw new Exception("Create account failed");
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
            throw new Exception("Not Found");
        }

        return _mapper.Map<GetUserWithRolesDto>(user);
    }

    public async Task<User> GetUserModelByUserName(string userName)
    {
        var normilizedUserName = _userManager.NormalizeName(userName);

        var user = await _userManager.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.NormalizedUserName == normilizedUserName);

        if(user == null)
        {
            throw new Exception("Not Found");
        }

        return user;
    }
}
