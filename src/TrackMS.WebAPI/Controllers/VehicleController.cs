using Microsoft.AspNetCore.Mvc;
using TrackMS.Domain.Abstractions;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.DTO;

namespace TrackMS.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleController : ControllerBase
{
    private readonly ICrudService<Vehicle, string> _vehicleService;

    public VehicleController(ICrudService<Vehicle, string> vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetVehicleDto>> Get(string id)
    {
        try
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);

            return Ok(
                new GetVehicleDto
                { 
                    Id = vehicle.Id,
                    OperatingStatus = vehicle.OperatingStatus,
                    StorageAreaId = vehicle.StorageAreaId,
                    Type = vehicle.Type   
                });

        }
        catch(ApplicationException ex)
        {
            return NotFound();
        }
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

        await _vehicleService.CreateAsync(vehicle);

        return Created("api/vehicles/{id}", vehicle);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(string id, [FromBody] PatchVehicleDto vehicleDto)
    {
        try
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);

            vehicle.Type = vehicleDto.Type is null ? vehicle.Type : vehicleDto.Type;
            vehicle.OperatingStatus = vehicleDto.OperatingStatus is null ? vehicle.OperatingStatus : vehicleDto.OperatingStatus;
            vehicle.StorageAreaId = vehicleDto.StorageAreaId is null ? vehicle.StorageAreaId : vehicleDto.StorageAreaId;

            await _vehicleService.UpdateAsync(vehicle);
        }
        catch(ApplicationException ex)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        try
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
        }
        catch (ApplicationException ex) 
        {
            return NotFound();
        }

        return NoContent();
    }
}
