using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.Auth.DTO;
using TrackMS.WebAPI.Features.IdentityManagement.Roles.DTO;
using TrackMS.WebAPI.Features.Users;

namespace TrackMS.WebAPI.Features.Auth;

public class AuthService
{
    private readonly UsersService _usersService;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly JwtService _jwtService;
    private readonly AuthDbContext _context;

    public AuthService(
        UsersService usersService,
        SignInManager<User> signInManager,
        IMapper mapper,
        JwtService jwtService,
        AuthDbContext context
        )
    {
        _usersService = usersService;
        _signInManager = signInManager;
        _mapper = mapper;
        _jwtService = jwtService;
        _context = context;
    }

    public async Task<SignInResponseDto> SignInByPassword(SignInByPasswordDto signInDto)
    {
        var user = await _usersService.GetUserModelByUserName(signInDto.Username);

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, signInDto.Password, false);

        if(signInResult.Succeeded)
        {
            var session = new Session
            {
                CreatedAt = DateTimeOffset.UtcNow,
                ExpiredAt = DateTimeOffset.UtcNow.AddDays(30),
                IsBlocked = false,
                SessionId = Guid.NewGuid().ToString(),
                UserId = user.Id,
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return new SignInResponseDto
            {
                User = new GetAuthUserDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Username = user.UserName!,
                    Roles = user.Roles.Select(x => new GetRoleWithShortPermissionsDto
                    {
                        Id = x.Id,
                        Name = x.Name!,
                        Permissions = x.Permissions.Select(y => y.Id)
                    })
                }, 
                AccessToken = await _jwtService.CreateAccessTokenAsync(user),
            };
        }

        throw new Exception("Unauthorized");
    }

    public async Task<IEnumerable<Session>> GetUsersSessions(string userId)
    {
        var sessions = await _context.Sessions
            .Where(x => x.UserId == userId)
            .ToListAsync();

        return sessions;
    }
}
