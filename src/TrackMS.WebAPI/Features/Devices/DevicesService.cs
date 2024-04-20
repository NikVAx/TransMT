using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Exceptions;
using TrackMS.WebAPI.Features.Devices.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Extensions;

namespace TrackMS.WebAPI.Features.Devices;

public class DevicesService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DevicesService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetDeviceDto> CreateDeviceAsync(CreateDeviceDto createDto)
    {
        var vehicle = new Device
        {
            Id = Guid.NewGuid().ToString(),
            VehicleId = createDto.VehicleId,
        };

        _context.Devices.Add(vehicle);
        await _context.SaveChangesAsync();

        return _mapper.Map<GetDeviceDto>(vehicle);
    }

    public async Task<GetDeviceDto> GetDeviceByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _context.Devices
            .Where(x => x.Id == id)
            .Select(x => _mapper.Map<GetDeviceDto>(x))
            .FirstOrDefaultAsync(cancellationToken);

        if(vehicle == null)
        {
            throw new NotFoundException();
        }

        return vehicle;
    }

    public async Task<PageResponseDto<GetDeviceDto>> GetDevicesPageAsync(int pageSize, int pageIndex,
        CancellationToken cancellationToken = default)
    {
        var count = await _context.Devices.CountAsync(cancellationToken);

        var vehicles = await _context.Devices
            .OrderBy(x => x.Id)
            .GetPage(pageSize, pageIndex)
            .Select(x => _mapper.Map<GetDeviceDto>(x))
            .ToListAsync(cancellationToken);

        return new PageResponseDto<GetDeviceDto>(vehicles, pageSize, pageIndex, count);
    }

    public async Task DeleteDeviceByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        int count = await _context.Devices
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if(count == 0)
        {
            throw new NotFoundException();
        }
    }

    public async Task DeleteManyDevicesAsync(DeleteManyDto<string> deleteDto,
        CancellationToken cancellationToken = default)
    {
        int count = await _context.Devices
            .Where(x => deleteDto.Keys.Contains(x.Id))
            .ExecuteDeleteAsync(cancellationToken);

        if(count != deleteDto.Keys.Count())
        {
            throw new Exception("Partial Deletion");
        }
    }

    public async Task<GetDeviceDto> EditDeviceByIdAsync(string id, PatchDeviceDto patchDto,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await GetDeviceModelByIdAsync(id, cancellationToken);

        _context.Update(vehicle);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GetDeviceDto>(vehicle);
    }

    public async Task<Device> GetDeviceModelByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _context.Devices
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if(vehicle == null)
        {
            throw new NotFoundException();
        }

        return vehicle;
    }
}
