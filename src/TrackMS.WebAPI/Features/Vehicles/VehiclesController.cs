using Microsoft.AspNetCore.Mvc;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Interfaces;
using TrackMS.WebAPI.DTO;
using TrackMS.WebAPI.Features.Buildings;
using TrackMS.WebAPI.Shared.DTO;

namespace TrackMS.WebAPI.Features.Vehicles;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController : ControllerBase
{
    private readonly BuildingsService _buildingsService;
    private readonly VehiclesService _vehiclesService;

    public VehiclesController(BuildingsService buildingsService,
        VehiclesService vehiclesService)
    {
        _buildingsService = buildingsService;
        _vehiclesService = vehiclesService;
    }

    [HttpGet]
    public async Task<ActionResult<PageResponseDto<GetVehicleDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        return await _vehiclesService.GetVehiclesPageAsync(getPageDto.PageSize, getPageDto.PageIndex);
    }

    [HttpGet("{id}", Name = $"Get{nameof(Vehicle)}")]
    public async Task<ActionResult<GetVehicleDto>> Get(string id)
    {
        return await _vehiclesService.GetVehicleByIdAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<GetVehicleDto>> Post([FromBody] CreateVehicleDto vehicleDto)
    {
        var vehicle = await _vehiclesService.CreateVehicleAsync(vehicleDto);

        return CreatedAtRoute($"Get{nameof(Vehicle)}", new { vehicle.Id }, vehicle);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<GetVehicleDto>> Patch(string id, [FromBody] PatchVehicleDto vehicleDto)
    {
        return await _vehiclesService.EditVehicleByIdAsync(id, vehicleDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        await _vehiclesService.DeleteVehicleByIdAsync(id);

        return NoContent();
    }
}
