using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Enums;
using TrackMS.Domain.Interfaces;
using TrackMS.WebAPI.DTO;
using TrackMS.WebAPI.Features.Buildings.DTO;
using TrackMS.WebAPI.Shared.DTO;

namespace TrackMS.WebAPI.Features.Vehicles;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController : ControllerBase
{
    private readonly ICrudService<Vehicle, string> _vehicleService;
    private readonly ICrudService<Building, string> _buildingService;

    public VehiclesController(ICrudService<Vehicle, string> vehicleService, ICrudService<Building, string> buildingService)
    {
        _vehicleService = vehicleService;
        _buildingService = buildingService;

    }

    [HttpGet]
    public async Task<ActionResult<PageResponseDto<GetVehicleDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        var query = _vehicleService.GetEntities();

        if(getPageDto.SortBy != null && getPageDto.SortOrder != SortOrder.Descending)
        {
            query = getPageDto.SortBy switch
            {
                nameof(Vehicle.Type) => query.OrderBy(x => x.Type),
                nameof(Vehicle.OperatingStatus) => query.OrderBy(x => x.OperatingStatus),
                nameof(Vehicle.StorageAreaId) => query.OrderBy(x => x.StorageAreaId),
                _ => query.OrderBy(x => x.Id),
            };
        }

        var items = await query
            .Skip(getPageDto.PageSize * getPageDto.PageIndex)
            .Take(getPageDto.PageSize)
            .Select(vehicle => new GetVehicleDto
            {
                Id = vehicle.Id,
                Number = vehicle.Number,
                OperatingStatus = vehicle.OperatingStatus,
                StorageArea = new GetBuildingDto
                {
                    Id = vehicle.StorageArea!.Id,
                    Name = vehicle.StorageArea.Name,
                    Address = vehicle.StorageArea.Address,
                    Location = vehicle.StorageArea.Location,
                    Type = vehicle.StorageArea.Type,
                },
                Type = vehicle.Type,
            })
            .ToListAsync();

        var count = await _vehicleService.GetEntities()
            .CountAsync();

        return Ok(new PageResponseDto<GetVehicleDto>
        {
            Items = items,
            PageIndex = getPageDto.PageIndex,
            PageSize = getPageDto.PageSize,
            TotalCount = count
        });
    }

    [HttpGet("{id}", Name = $"Get{nameof(Vehicle)}")]
    public async Task<ActionResult<GetVehicleDto>> Get(string id)
    {
        var result = await _vehicleService.GetByIdAsync(id);

        if(!result.Succeeded)
        {
            return NotFound(result);
        }

        var vehicle = result.Object;

        return Ok(
            new GetVehicleDto
            {
                Id = vehicle.Id,
                OperatingStatus = vehicle.OperatingStatus,
                StorageArea = new GetBuildingDto
                {
                    Id = vehicle.StorageArea!.Id,
                    Name = vehicle.StorageArea.Name,
                    Address = vehicle.StorageArea.Address,
                    Location = vehicle.StorageArea.Location,
                    Type = vehicle.StorageArea.Type,
                },
                Type = vehicle.Type
            });
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateVehicleDto vehicleDto)
    {
        var storageArea = await _buildingService.GetByIdAsync(vehicleDto.StorageAreaId);

        if(!storageArea.Succeeded)
        {
            return NotFound(storageArea);
        }

        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid().ToString(),
            Number = vehicleDto.Number,
            OperatingStatus = vehicleDto.OperatingStatus is null ? "Default" : vehicleDto.OperatingStatus,
            Type = vehicleDto.Type is null ? "Default" : vehicleDto.Type,
            StorageArea = storageArea.Object,
            StorageAreaId = storageArea.Object.Id,
        };

        var createResult = await _vehicleService.CreateAsync(vehicle);

        if(!createResult.Succeeded)
        {
            return BadRequest(createResult);
        }

        return CreatedAtRoute($"Get{nameof(Vehicle)}", new { vehicle.Id }, vehicle);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(string id, [FromBody] PatchVehicleDto vehicleDto)
    {
        var result = await _vehicleService.GetByIdAsync(id);

        if(!result.Succeeded)
        {
            return NotFound(result);
        }

        var vehicle = result.Object;

        vehicle.Type = vehicleDto.Type is null ? vehicle.Type : vehicleDto.Type;
        vehicle.OperatingStatus = vehicleDto.OperatingStatus is null ? vehicle.OperatingStatus : vehicleDto.OperatingStatus;
        vehicle.StorageAreaId = vehicleDto.StorageAreaId is null ? vehicle.StorageAreaId : vehicleDto.StorageAreaId;

        var updateResult = await _vehicleService.UpdateAsync(vehicle);

        if(!updateResult.Succeeded)
        {
            return BadRequest(updateResult);
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _vehicleService.GetByIdAsync(id);

        if(!result.Succeeded)
        {
            return NotFound(result);
        }

        return NoContent();
    }
}
