using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Exceptions;
using TrackMS.WebAPI.DTO;
using TrackMS.WebAPI.Features.Buildings;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Extensions;

namespace TrackMS.WebAPI.Features.Vehicles;

public class VehicleConstants
{
    public const string DefaultOperatingStatus = "Default";
    public const string DefaultType = "Default";
}

public class VehiclesService
{
    private readonly ApplicationDbContext _context;
    private readonly BuildingsService _buildingsService;
    private readonly IMapper _mapper;

    public VehiclesService(ApplicationDbContext context, BuildingsService buildingsService, IMapper mapper)
    {
        _context = context;
        _buildingsService = buildingsService;
        _mapper = mapper;
    }

    public async Task<GetVehicleDto> CreateVehicleAsync(CreateVehicleDto createDto)
    {
        await _buildingsService.GetBuildingByIdAsync(createDto.StorageAreaId);

        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid().ToString(),
            Number = createDto.Number,
            OperatingStatus = createDto.OperatingStatus is null ?
                VehicleConstants.DefaultOperatingStatus : createDto.OperatingStatus,
            Type = createDto.Type is null ?
                VehicleConstants.DefaultType : createDto.Type,
            StorageAreaId = createDto.StorageAreaId,
        };

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return _mapper.Map<GetVehicleDto>(vehicle);
    }

    public async Task<GetVehicleDto> GetVehicleByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _context.Vehicles
            .Where(x => x.Id == id)
            .Include(x => x.StorageArea)
            .Select(x => _mapper.Map<GetVehicleDto>(x))
            .FirstOrDefaultAsync(cancellationToken);

        if(vehicle == null)
        {
            throw new NotFoundException();
        }

        return vehicle;
    }

    public async Task<PageResponseDto<GetVehicleDto>> GetVehiclesPageAsync(int pageSize, int pageIndex,
        CancellationToken cancellationToken = default)
    {
        var count = await _context.Vehicles.CountAsync(cancellationToken);

        var vehicles = await _context.Vehicles
            .OrderBy(x => x.Id)
            .GetPage(pageSize, pageIndex)
            .Include(x => x.StorageArea)
            .Select(x => _mapper.Map<GetVehicleDto>(x))
            .ToListAsync(cancellationToken);

        return new PageResponseDto<GetVehicleDto>(vehicles, pageSize, pageIndex, count);
    }

    public async Task DeleteVehicleByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        int count = await _context.Vehicles
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if(count == 0)
        {
            throw new NotFoundException();
        }
    }

    public async Task DeleteManyVehiclesAsync(DeleteManyDto<string> deleteDto,
        CancellationToken cancellationToken = default)
    {
        int count = await _context.Vehicles
            .Where(x => deleteDto.Keys.Contains(x.Id))
            .ExecuteDeleteAsync(cancellationToken);

        if(count != deleteDto.Keys.Count())
        {
            throw new Exception("Partial Deletion");
        }
    }

    public async Task<GetVehicleDto> EditVehicleByIdAsync(string id, PatchVehicleDto patchDto,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await GetVehicleModelByIdAsync(id, cancellationToken);

        if(patchDto.StorageAreaId != null)
        {
            var storageArea = await _buildingsService.GetBuildingModelByIdAsync(patchDto.StorageAreaId, cancellationToken);

            vehicle.StorageArea = storageArea;
            vehicle.StorageAreaId = storageArea.Id;
        }

        vehicle.Number = patchDto.Number is null ? vehicle.Number : patchDto.Number;
        vehicle.OperatingStatus = patchDto.OperatingStatus is null ? vehicle.OperatingStatus : patchDto.OperatingStatus;
        vehicle.Type = patchDto.Type is null ? vehicle.Type : patchDto.Type;

        _context.Update(vehicle);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GetVehicleDto>(vehicle);
    }

    public async Task<Vehicle> GetVehicleModelByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if(vehicle == null)
        {
            throw new NotFoundException();
        }

        return vehicle;
    }
}
