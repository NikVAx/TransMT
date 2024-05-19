using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Exceptions;
using TrackMS.WebAPI.Features.Buildings.DTO;
using TrackMS.WebAPI.Features.GeoZones.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Extensions;
using TrackMS.WebAPI.Shared.Utils;

namespace TrackMS.WebAPI.Features.GeoZones;

public class GeoZonesService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GeoZonesService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PageResponseDto<GetGeoZoneDto>> GetGeoZonesPageAsync(int size, int index)
    {
        var items = await _dbContext.GeoZones
            .OrderBy(x => x.Name)
            .GetPage(size, index)
            .Select(x => _mapper.Map<GetGeoZoneDto>(x))
            .ToListAsync();

        var count = await _dbContext.GeoZones.CountAsync(); 

        return new PageResponseDto<GetGeoZoneDto>(items, size, index, count);
    }

    public async Task<GetGeoZoneDto> CreateGeoZoneAsync(CreateGeoZoneDto createDto)
    {
        var geoZone = new GeoZone
        {
            Id = Guid.NewGuid().ToString(),
            Color = createDto.Color,
            Name = createDto.Name,
            Points = GeoUtil.CreatePolygon(createDto.Points),
        };

        _dbContext.Add(geoZone);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<GetGeoZoneDto>(geoZone);
    }

    public async Task DeleteManyGeoZonesAsync(DeleteManyDto<string> deleteDto)
    {
        int count = await _dbContext.GeoZones
            .Where(x => deleteDto.Keys.Contains(x.Id))
            .ExecuteDeleteAsync();

        if(count != deleteDto.Keys.Count())
        {
            throw new Exception("Partial Deletion");
        }
    }

    public async Task<GetGeoZoneDto> EditGeoZoneByIdAsync(string id, PatchGeoZoneDto patchDto)
    {
        var geoZone = await _dbContext.GeoZones.FirstOrDefaultAsync(x => x.Id == id);
            
        if (geoZone == null)
        {
            throw new NotFoundException();
        }

        geoZone.Name = patchDto.Name ?? geoZone.Name;

        geoZone.Points = patchDto.Points == null ? 
            geoZone.Points :
            GeoUtil.CreatePolygon(patchDto.Points);

        geoZone.Color = patchDto.Color ?? geoZone.Color;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<GetGeoZoneDto>(geoZone);
    }

    public async Task DeleteGeoZoneByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        int count = await _dbContext.GeoZones
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if(count == 0)
        {
            throw new NotFoundException();
        }
    }

    public async Task<GetGeoZoneDto> GetGeoZoneByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var geoZone = await _dbContext.GeoZones
            .Where(x => x.Id == id)
            .Select(x => _mapper.Map<GetGeoZoneDto>(x))
            .FirstOrDefaultAsync(cancellationToken);

        if(geoZone == null)
        {
            throw new NotFoundException();
        }

        return geoZone;
    }
}
