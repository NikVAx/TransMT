using Microsoft.AspNetCore.Mvc;
using TrackMS.Domain.Abstractions;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.DTO;

namespace TrackMS.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleOperatorsController : ControllerBase
{
    private readonly ICrudService<VehicleOperator, string> _vehicleOperatorService;

    public VehicleOperatorsController(ICrudService<VehicleOperator, string> vehicleOperatorService)
    {
        _vehicleOperatorService = vehicleOperatorService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetVehicleOperatorDto>> Get(string id)
    {
        var result = await _vehicleOperatorService.GetByIdAsync(id);

        if(!result.Succeeded)
        {
            return NotFound(result);           
        }

        return Ok(
            new GetVehicleOperatorDto
            {
                Id = result.Object.Id,
            });
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateVehicleOperatorDto vehicleOperatorDto)
    {
        var vehicleOperator = new VehicleOperator();

        var result = await _vehicleOperatorService.CreateAsync(vehicleOperator);

        if (!result.Succeeded)
        {
            return BadRequest(result);
            
        }

        return Created("api/vehicleOperators/{id}", vehicleOperator);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> Patch(string id, [FromBody] PatchVehicleOperatorDto vehicleOperatorDto)
    {
        var result = await _vehicleOperatorService.GetByIdAsync(id);

        if(!result.Succeeded)
        {
            return NotFound(result);
        }

        var vehicleOperator = result.Object;

        var updateResult = await _vehicleOperatorService.UpdateAsync(vehicleOperator);

        if(!updateResult.Succeeded)
        {
            return BadRequest(updateResult);
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _vehicleOperatorService.GetByIdAsync(id);

        if(!result.Succeeded)
        {
            return NotFound(result);
        }

        return NoContent();
    }
}
