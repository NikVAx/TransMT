using Microsoft.AspNetCore.Mvc;
using TrackMS.Domain.Abstractions;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.DTO;

namespace TrackMS.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleOperatorController : ControllerBase
{
    private readonly ICrudService<VehicleOperator, string> _vehicleOperatorService;

    public VehicleOperatorController(ICrudService<VehicleOperator, string> vehicleOperatorService)
    {
        _vehicleOperatorService = vehicleOperatorService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetVehicleOperatorDto>> Get(string id)
    {
        try
        {
            var vehicleOperator = await _vehicleOperatorService.GetByIdAsync(id);

            return Ok(
                new GetVehicleOperatorDto
                {
                    Id = vehicleOperator.Id,
                });

        }
        catch(ApplicationException ex)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateVehicleOperatorDto vehicleOperatorDto)
    {
        var vehicleOperator = new VehicleOperator();

        await _vehicleOperatorService.CreateAsync(vehicleOperator);

        return Created("api/vehicleOperators/{id}", vehicleOperator);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(string id, [FromBody] PatchVehicleOperatorDto vehicleOperatorDto)
    {
        try
        {
            var vehicleOperator = await _vehicleOperatorService.GetByIdAsync(id);

            await _vehicleOperatorService.UpdateAsync(vehicleOperator);
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
            var vehicleOperator = await _vehicleOperatorService.GetByIdAsync(id);
        }
        catch(ApplicationException ex)
        {
            return NotFound();
        }

        return NoContent();
    }
}
