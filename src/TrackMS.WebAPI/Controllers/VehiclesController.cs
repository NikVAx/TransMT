using Microsoft.AspNetCore.Mvc;
using TrackMS.Domain.Abstractions;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.DTO;

namespace TrackMS.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController : ControllerBase
{
    private readonly ICrudService<Vehicle, string> _vehicleService;

    public VehiclesController(ICrudService<Vehicle, string> vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetVehicleDto>> Get(string id)
    {
        var result = await _vehicleService.GetByIdAsync(id);

        if (!result.Succeeded)
        {
            return NotFound(result);
        }

        var vehicle = result.Object;

        return Ok(
            new GetVehicleDto
            {
                Id = vehicle.Id,
                OperatingStatus = vehicle.OperatingStatus,
                StorageAreaId = vehicle.StorageAreaId,
                Type = vehicle.Type
            });
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateVehicleDto vehicleDto)
    {
        var vehicle = new Vehicle
        {
            OperatingStatus = vehicleDto.OperatingStatus is null ? "Default" : vehicleDto.OperatingStatus,
            Type = vehicleDto.Type is null ? "Default" : vehicleDto.Type,
            StorageAreaId = vehicleDto.StorageAreaId
        };

        var createResult = await _vehicleService.CreateAsync(vehicle);

        if (!createResult.Succeeded)
        {
            return BadRequest(createResult);
        }

        return Created("api/vehicles/{id}", vehicle);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(string id, [FromBody] PatchVehicleDto vehicleDto)
    {
        var result = await _vehicleService.GetByIdAsync(id);

        if (!result.Succeeded)
        {
            return NotFound(result);
        }

        var vehicle = result.Object;

        vehicle.Type = vehicleDto.Type is null ? vehicle.Type : vehicleDto.Type;
        vehicle.OperatingStatus = vehicleDto.OperatingStatus is null ? vehicle.OperatingStatus : vehicleDto.OperatingStatus;
        vehicle.StorageAreaId = vehicleDto.StorageAreaId is null ? vehicle.StorageAreaId : vehicleDto.StorageAreaId;

        var updateResult = await _vehicleService.UpdateAsync(vehicle);

        if (!updateResult.Succeeded)
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
