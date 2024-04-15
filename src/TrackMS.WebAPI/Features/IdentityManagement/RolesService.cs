using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.IdentityManagement.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Extensions;

namespace TrackMS.WebAPI.Features.IdentityManagement;

public class RolesService
{
    private readonly IMapper _mapper;
    private readonly AuthDbContext _authDbContext;
    private readonly RoleManager<Role> _roleManager;

    public RolesService(IMapper mapper, RoleManager<Role> roleManager, AuthDbContext authDbContext)
    {
        _mapper = mapper;
        _roleManager = roleManager;
        _authDbContext = authDbContext;
    }

    public async Task<GetRoleWithPermissionsDto> CreateRoleAsync(CreateRoleDto createDto)
    {
        var role = new Role
        {
            Name = createDto.Name,
            Permissions = _authDbContext.Permissions
                .Where(x => createDto.Permissions.Contains(x.Id)).ToList(),
        };

        var actionResult = await _roleManager.CreateAsync(role);

        if (actionResult.Succeeded)
        {
            return _mapper.Map<GetRoleWithPermissionsDto>(role);
        }

        var message = string.Join("; ", actionResult.Errors.Select(x => x.Description));

        throw new Exception("Invalid Role " + message);
    }

    public async Task<PageResponseDto<GetRoleDto>> GetRolesPageAsync(int pageSize, int pageIndex, 
        CancellationToken cancellationToken = default)
    {
        var items = await _roleManager.Roles
            .OrderBy(x => x.Id)
            .GetPage(pageSize, pageIndex)
            .Select(role => _mapper.Map<GetRoleDto>(role))
            .ToListAsync(cancellationToken);

        var count = await _roleManager.Roles.CountAsync(cancellationToken);

        return new PageResponseDto<GetRoleDto>(items, pageSize, pageIndex, count);
    }

    public async Task<IList<Role>> GetRoleModelsByKeysAsync(IEnumerable<string> keys,
        CancellationToken cancellationToken = default)
    {
        var items = await _roleManager.Roles
            .Include(x => x.Permissions)
            .Where(x => keys.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return items;
    }

    public async Task<IList<Role>> GetRoleModelsByNamesAsync(IEnumerable<string> keys,
        CancellationToken cancellationToken = default)
    {
        var items = await _roleManager.Roles
            .Include(x => x.Permissions)
            .Where(x => keys.Contains(x.Name))
            .ToListAsync(cancellationToken);

        return items;
    }

}
