using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Exceptions;
using TrackMS.WebAPI.Features.Buildings.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Extensions;

namespace TrackMS.WebAPI.Features.Buildings;

public class BuildingsService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public BuildingsService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetBuildingDto> GetBuildingByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var building = await _context.Buildings
            .Where(x => x.Id == id)
            .Select(x => _mapper.Map<GetBuildingDto>(x))
            .FirstOrDefaultAsync(cancellationToken);

        if(building == null)
        {
            throw new NotFoundException();
        }

        return building;
    }

    public async Task<PageResponseDto<GetBuildingDto>> GetBuildingsPageAsync(int pageSize, int pageIndex,
        CancellationToken cancellationToken = default)
    {
        var count = await _context.Buildings.CountAsync(cancellationToken);

        var buildings = await _context.Buildings
            .OrderBy(x => x.Id)
            .GetPage(pageSize, pageIndex)
            .Select(x => _mapper.Map<GetBuildingDto>(x))
            .ToListAsync(cancellationToken);

        return new PageResponseDto<GetBuildingDto>(buildings, pageSize, pageIndex, count);
    }

    public async Task<GetBuildingDto> CreateBuildingAsync(CreateBuildingDto createDto,
        CancellationToken cancellationToken = default)
    {
        var building = new Building
        {
            Id = Guid.NewGuid().ToString(),
            Address = createDto.Address,
            Location = new NetTopologySuite.Geometries.Point(
                createDto.Location.Lng,
                createDto.Location.Lat),
            Type = createDto.Type,
            Name = createDto.Name
        };



        _context.Buildings.Add(building);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GetBuildingDto>(building);
    }

    public async Task DeleteBuildingByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        int count = await _context.Buildings
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if(count == 0)
        {
            throw new NotFoundException();
        }
    }

    public async Task DeleteManyBuidlingsAsync(DeleteManyDto<string> deleteDto,
        CancellationToken cancellationToken = default)
    {
        int count = await _context.Buildings
            .Where(x => deleteDto.Keys.Contains(x.Id))
            .ExecuteDeleteAsync(cancellationToken);

        if(count != deleteDto.Keys.Count())
        {
            throw new Exception("Partial Deletion");
        }
    }

    public async Task<GetBuildingDto> EditBuildingByIdAsync(string id, PatchBuildingDto patchDto,
        CancellationToken cancellationToken = default)
    {
        var building = await GetBuildingModelByIdAsync(id, cancellationToken);

        if(building == null)
        {
            throw new NotFoundException();
        }

        building.Address = patchDto.Address is null ? building.Address : patchDto.Address;
        building.Location = patchDto.Location is null ? building.Location : 
            new NetTopologySuite.Geometries.Point(patchDto.Location.Lat, patchDto.Location.Lng);
        building.Type = patchDto.Type is null ? building.Type : patchDto.Type;
        building.Name = patchDto.Name is null ? building.Name : patchDto.Name;

        _context.Buildings.Update(building);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GetBuildingDto>(building);
    }

    public async Task<Building> GetBuildingModelByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var building = await _context.Buildings
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if(building == null)
        {
            throw new NotFoundException();
        }

        return building;
    }
}
