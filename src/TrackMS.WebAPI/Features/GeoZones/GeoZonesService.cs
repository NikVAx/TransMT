using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.GeoZones.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Extensions;

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
            Points = createDto.Points,
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
            throw new Exception("Not Found");
        }

        geoZone.Name = patchDto.Name ?? geoZone.Name;
        geoZone.Points = patchDto.Points ?? geoZone.Points;
        geoZone.Color = patchDto.Color ?? geoZone.Color;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<GetGeoZoneDto>(geoZone);
    }
}
