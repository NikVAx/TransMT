using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Exceptions;
using TrackMS.WebAPI.DTO;
using TrackMS.WebAPI.Shared.DTO;
using TrackMS.WebAPI.Shared.Extensions;

namespace TrackMS.WebAPI.Features.Operators;

public class OperatorsService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public OperatorsService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetVehicleOperatorDto> CreateOperatorAsync(CreateVehicleOperatorDto createDto)
    {
        var vehicle = new VehicleOperator
        {
            Id = Guid.NewGuid().ToString(),
        };

        _context.VehicleOperators.Add(vehicle);
        await _context.SaveChangesAsync();

        return _mapper.Map<GetVehicleOperatorDto>(vehicle);
    }

    public async Task<GetVehicleOperatorDto> GetOperatorByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _context.VehicleOperators
            .Where(x => x.Id == id)
            .Select(x => _mapper.Map<GetVehicleOperatorDto>(x))
            .FirstOrDefaultAsync(cancellationToken);

        if(vehicle == null)
        {
            throw new NotFoundException();
        }

        return vehicle;
    }

    public async Task<PageResponseDto<GetVehicleOperatorDto>> GetOperatorsPageAsync(int pageSize, int pageIndex,
        CancellationToken cancellationToken = default)
    {
        var count = await _context.VehicleOperators.CountAsync(cancellationToken);

        var vehicles = await _context.VehicleOperators
            .OrderBy(x => x.Id)
            .GetPage(pageSize, pageIndex)
            .Select(x => _mapper.Map<GetVehicleOperatorDto>(x))
            .ToListAsync(cancellationToken);

        return new PageResponseDto<GetVehicleOperatorDto>(vehicles, pageSize, pageIndex, count);
    }

    public async Task DeleteOperatorByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        int count = await _context.VehicleOperators
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if(count == 0)
        {
            throw new NotFoundException();
        }
    }

    public async Task DeleteManyOperatorsAsync(DeleteManyDto<string> deleteDto,
        CancellationToken cancellationToken = default)
    {
        int count = await _context.VehicleOperators
            .Where(x => deleteDto.Keys.Contains(x.Id))
            .ExecuteDeleteAsync(cancellationToken);

        if(count != deleteDto.Keys.Count())
        {
            throw new Exception("Partial Deletion");
        }
    }

    public async Task<GetVehicleOperatorDto> EditOperatorByIdAsync(string id, PatchVehicleOperatorDto patchDto,
        CancellationToken cancellationToken = default)
    {
        var vehicle = await GetOperatorModelByIdAsync(id, cancellationToken);

        _context.Update(vehicle);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GetVehicleOperatorDto>(vehicle);
    }

    public async Task<VehicleOperator> GetOperatorModelByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var vehicle = await _context.VehicleOperators
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if(vehicle == null)
        {
            throw new NotFoundException();
        }

        return vehicle;
    }
}
