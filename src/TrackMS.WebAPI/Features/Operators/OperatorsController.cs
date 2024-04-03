using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackMS.Domain.Entities;
using TrackMS.Domain.Enums;
using TrackMS.Domain.Interfaces;
using TrackMS.WebAPI.DTO;
using TrackMS.WebAPI.Shared.DTO;

namespace TrackMS.WebAPI.Features.Operators;

[Route("api/[controller]")]
[ApiController]
public class OperatorsController : ControllerBase
{
    private readonly ICrudService<VehicleOperator, string> _vehicleOperatorService;

    public OperatorsController(ICrudService<VehicleOperator, string> vehicleOperatorService)
    {
        _vehicleOperatorService = vehicleOperatorService;
    }

    [HttpGet]
    public async Task<ActionResult<PageResponseDto<GetVehicleOperatorDto>>> GetPage([FromQuery] PageRequestDto getPageDto)
    {
        var query = _vehicleOperatorService.GetEntities();

        var items = await query
            .Skip(getPageDto.PageSize * getPageDto.PageIndex)
            .Take(getPageDto.PageSize)
            .Select(@operator => new GetVehicleOperatorDto
            {
                Id = @operator.Id,
            })
            .ToListAsync();

        var count = await _vehicleOperatorService.GetEntities()
            .CountAsync();

        return Ok(new PageResponseDto<GetVehicleOperatorDto>(items, getPageDto.PageSize, getPageDto.PageIndex, count));
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
        var vehicleOperator = new VehicleOperator()
        {
            Id = Guid.NewGuid().ToString(),
        };

        var result = await _vehicleOperatorService.CreateAsync(vehicleOperator);

        if(!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Created("api/operators/{id}", vehicleOperator);
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
