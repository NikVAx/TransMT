using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.Roles.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Extensions;

namespace TrackMS.WebAPI.Features.Roles;

public static class BuildIn
{
    public static class Role
    {
        public const string SuperAdmin = "super-admin";
        public const string Admin = "admin";
    }
    
    public static readonly string[] Roles = 
    [
        Role.SuperAdmin,
        Role.Admin
    ];
}

public class RolesService
{
    private readonly IMapper _mapper;
    private readonly RoleManager<Role> _roleManager;

    public RolesService(IMapper mapper, RoleManager<Role> roleManager)
    {
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public async Task<GetRoleDto> CreateRoleAsync(CreateRoleDto createDto)
    {
        if(BuildIn.Roles.Contains(createDto.Name))
        {
            throw new Exception("RoleName is Reserved");
        }

        var role = new Role
        {
            Name = createDto.Name,
        };

        var actionResult = await _roleManager.CreateAsync(role);

        if(actionResult.Succeeded)
        {
           return _mapper.Map<GetRoleDto>(role);
        }

        var message = string.Join("; ", actionResult.Errors.Select(x => x.Description));

        throw new Exception("Invalid Role " + message);
    }

    public async Task<PageResponseDto<GetRoleDto>> GetRolesPageAsync(int pageSize, int pageIndex, CancellationToken cancellation = default)
    {
        var items = await _roleManager.Roles
            .OrderBy( x => x.Id)
            .GetPage(pageSize, pageIndex)
            .Select(role => _mapper.Map<GetRoleDto>(role))
            .ToListAsync();

        var count = await _roleManager.Roles.CountAsync();

        return new PageResponseDto<GetRoleDto>(items, pageSize, pageIndex, count);
    }
}
