using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Exceptions;
using TrackMS.WebAPI.DTO;
using TrackMS.WebAPI.Features.Operators.DTO;
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
        var vehicleOperator = new VehicleOperator
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            MiddleName = createDto.MiddleName,
        };

        _context.VehicleOperators.Add(vehicleOperator);
        await _context.SaveChangesAsync();

        return _mapper.Map<GetVehicleOperatorDto>(vehicleOperator);
    }

    public async Task<GetVehicleOperatorDto> GetOperatorByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var vehicleOperator = await _context.VehicleOperators
            .Where(x => x.Id == id)
            .Select(x => _mapper.Map<GetVehicleOperatorDto>(x))
            .FirstOrDefaultAsync(cancellationToken);

        if(vehicleOperator == null)
        {
            throw new NotFoundException();
        }

        return vehicleOperator;
    }

    public async Task<PageResponseDto<GetVehicleOperatorDto>> GetOperatorsPageAsync(int pageSize, int pageIndex,
        CancellationToken cancellationToken = default)
    {
        var count = await _context.VehicleOperators.CountAsync(cancellationToken);

        var vehicleOperators = await _context.VehicleOperators
            .OrderBy(x => x.Id)
            .GetPage(pageSize, pageIndex)
            .Select(x => _mapper.Map<GetVehicleOperatorDto>(x))
            .ToListAsync(cancellationToken);

        return new PageResponseDto<GetVehicleOperatorDto>(vehicleOperators, pageSize, pageIndex, count);
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
        var vehicleOperator = await GetOperatorModelByIdAsync(id, cancellationToken);
        
        vehicleOperator.FirstName = patchDto.FirstName ?? vehicleOperator.FirstName;
        vehicleOperator.LastName = patchDto.LastName ?? vehicleOperator.LastName; 
        vehicleOperator.MiddleName = patchDto.MiddleName ?? vehicleOperator.MiddleName;

        _context.Update(vehicleOperator);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GetVehicleOperatorDto>(vehicleOperator);
    }

    public async Task<VehicleOperator> GetOperatorModelByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var vehicleOpertor = await _context.VehicleOperators
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if(vehicleOpertor == null)
        {
            throw new NotFoundException();
        }

        return vehicleOpertor;
    }
}
