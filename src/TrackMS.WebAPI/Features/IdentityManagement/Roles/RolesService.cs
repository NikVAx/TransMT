using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Exceptions;
using TrackMS.WebAPI.Features.IdentityManagement.Roles.DTO;
using TrackMS.WebAPI.Features.Roles.IdentityManagement.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Extensions;

namespace TrackMS.WebAPI.Features.IdentityManagement.Roles;

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

    public async Task DeleteManyRolesAsync(DeleteManyDto<string> deleteDto,
        CancellationToken cancellationToken = default)
    {
        int count = await _roleManager.Roles
            .Where(x => deleteDto.Keys.Contains(x.Id))
            .ExecuteDeleteAsync(cancellationToken);

        if(count != deleteDto.Keys.Count())
        {
            throw new Exception("Partial Deletion");
        }
    }

    public async Task<GetRoleWithShortPermissionsDto> GetRoleByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var items = await _roleManager.Roles
            .Where(x => x.Id == id)
            .Include(x => x.Permissions)
            .Select(x => _mapper.Map<GetRoleWithShortPermissionsDto>(x))
            .FirstOrDefaultAsync(cancellationToken);

        if(items == null)
        {
            throw new NotFoundException();
        }

        return items;
    }

    public async Task<GetRoleWithShortPermissionsDto> CreateRoleAsync(CreateRoleDto createDto)
    {
        var role = new Role
        {
            Name = createDto.Name,
            Description = createDto.Description,
            Permissions = _authDbContext.Permissions.Where(x => createDto.Permissions.Contains(x.Id)).ToList(),
        };

        var actionResult = await _roleManager.CreateAsync(role);

        if (actionResult.Succeeded)
        {
            return _mapper.Map<GetRoleWithShortPermissionsDto>(role);
        }

        var message = string.Join("; ", actionResult.Errors.Select(x => x.Description));

        throw new ConflictException(message);
    }

    public async Task<PageResponseDto<GetRoleDto>> GetRolesPageAsync(int pageSize, int pageIndex, 
        CancellationToken cancellationToken = default)
    {
        var items = await _roleManager.Roles
            .OrderBy(x => x.Name)
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

    public async Task DeleteRoleByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        int count = await _roleManager.Roles
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if(count == 0)
        {
            throw new NotFoundException();
        }
    }
}
