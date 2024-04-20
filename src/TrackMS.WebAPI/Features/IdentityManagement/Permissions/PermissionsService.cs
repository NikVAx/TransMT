using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.WebAPI.Features.IdentityManagement.Permissions.DTO;

namespace TrackMS.WebAPI.Features.IdentityManagement.Permissions;

public class PermissionsService
{
    private readonly AuthDbContext _authDbContext;
    private readonly IMapper _mapper;

    public PermissionsService(AuthDbContext authDbContext, IMapper mapper)
    {
        _authDbContext = authDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetPermissionDto>> GetPermissionsAsync()
    {
        var permissions = await _authDbContext.Permissions
            .Select(x => _mapper.Map<GetPermissionDto>(x))
            .ToListAsync();

        return permissions;
    }
}
